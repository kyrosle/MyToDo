using MyToDo.Common;
using MyToDo.Extension;
using MyToDo.Service;
using MyToDo.Share.Dtos;
using MyToDo.Share.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        public ToDoViewModel(IToDoService service, IContainerProvider provider) : base(provider)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DeleteCommand = new DelegateCommand<ToDoDto>(Delete);
            dialogHost = provider.Resolve<IDialogHostService>();
            this.service = service;
        }

        private int selectIndex;

        public int SeletedIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; RaisePropertyChanged(); }
        }


        private async void Delete(ToDoDto obj)
        {
            try
            {
                var dialogResut = await dialogHost.QuestionAsync("Tips",$"Want Delete {obj.Title} ?");
                if (dialogResut.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

                UpdateLoading(true);
                var deleteResult = await service.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    var model = ToDoDtos.FirstOrDefault(t => t.Id == obj.Id);
                    if (model != null)
                        ToDoDtos.Remove(model);
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        /// <summary>
        /// 根据 param 执行 新增代办 或者 执行查询 
        /// </summary>
        /// <param name="obj"></param>
        private void Excute(string obj)
        {
            switch (obj)
            {
                case "new": Add(); break;
                case "query": GetDataAsync(); break;
                case "save": Save(); break;
            }
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentDto.Title) || string.IsNullOrWhiteSpace(CurrentDto.Content)) return;

            UpdateLoading(true);
            try
            {
                if (CurrentDto.Id > 0)
                {
                    var updateResult = await service.UpdateAsync(CurrentDto);
                    if (updateResult.Status)
                    {
                        var todo = ToDoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                            todo.Status = CurrentDto.Status;
                        }
                    }
                }
                else
                {
                    var addResult = await service.AddAsync(currentDto);
                    if (addResult.Status)
                    {
                        ToDoDtos.Add(addResult.Result);
                    }
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                IsRigthDrawerOpen = false;
                UpdateLoading(false);
            }
        }

        private string search;

        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }


        private async void Selected(ToDoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var toDoResult = await service.GetFirstOrDefaultAsync(obj.Id);
                if (toDoResult.Status)
                {
                    CurrentDto = toDoResult.Result;
                    IsRigthDrawerOpen = true;
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private bool isRigthDrawerOpen;

        /// <summary>
        /// 右侧编辑窗口是否展开
        /// </summary>
        public bool IsRigthDrawerOpen
        {
            get { return isRigthDrawerOpen; }
            set { isRigthDrawerOpen = value; RaisePropertyChanged(); }
        }

        private ToDoDto currentDto;

        /// <summary>
        /// 编辑选中对象/编辑中的对象
        /// </summary>
        public ToDoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }



        /// <summary>
        /// 添加代办
        /// </summary>
        private void Add()
        {
            CurrentDto = new ToDoDto();
            IsRigthDrawerOpen = true;
        }

        private ObservableCollection<ToDoDto> toDoDtos;
        private readonly IToDoService service;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> ExcuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }
        public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }


        /// <summary>
        /// 获取数据
        /// </summary>
        async void GetDataAsync()
        {
            UpdateLoading(true);
            int? Status = SeletedIndex == 0 ? null : SeletedIndex == 2 ? 1 : 0;
            var ToDoResult = await service.GetAllFilterAsync(new TodoParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
                Status = Status
            });
            if (ToDoResult.Status)
            {
                ToDoDtos.Clear();
                foreach (var item in ToDoResult.Result.Items)
                {
                    ToDoDtos.Add(item);
                }

            }
            else
            {
                ToDoDtos.Clear();
            }
            UpdateLoading(false);
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetDataAsync();
        }
    }
}
