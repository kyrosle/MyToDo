using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Common.Models;
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
    internal class MemoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        public MemoViewModel(IMemoService service, IContainerProvider provider) : base(provider)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            SelectedCommand = new DelegateCommand<MemoDto>(Selected);
            DeleteCommand = new DelegateCommand<MemoDto>(Delete);
            dialogHost = provider.Resolve<IDialogHostService>();
            this.service = service;
        }

        private int selectIndex;

        public int SeletedIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; RaisePropertyChanged(); }
        }


        private async void Delete(MemoDto obj)
        {
            try
            {
                var dialogResut = await dialogHost.QuestionAsync("Tips", $"Want Delete {obj.Title} ?");
                if (dialogResut.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                UpdateLoading(true);
                var deleteResult = await service.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    var model = MemoDtos.FirstOrDefault(t => t.Id == obj.Id);
                    if (model != null)
                        MemoDtos.Remove(model);
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
                        var todo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
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
                        MemoDtos.Add(addResult.Result);
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


        private async void Selected(MemoDto obj)
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

        private MemoDto currentDto;

        /// <summary>
        /// 编辑选中对象/编辑中的对象
        /// </summary>
        public MemoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }



        /// <summary>
        /// 添加代办
        /// </summary>
        private void Add()
        {
            CurrentDto = new MemoDto();
            IsRigthDrawerOpen = true;
        }

        private ObservableCollection<MemoDto> memoDtos;
        private readonly IMemoService service;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> ExcuteCommand { get; private set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; private set; }
        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }


        /// <summary>
        /// 获取数据
        /// </summary>
        async void GetDataAsync()
        {
            UpdateLoading(true);
            var ToDoResult = await service.GetAllAsync(new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
            });
            if (ToDoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in ToDoResult.Result.Items)
                {
                    MemoDtos.Add(item);
                }

            }
            else
            {
                MemoDtos.Clear();
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
