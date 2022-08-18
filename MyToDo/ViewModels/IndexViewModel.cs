using MyToDo.Api.Models;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Share.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyToDo.ViewModels
{
    internal class IndexViewModel : NavigationViewModel
    {
        public IndexViewModel(IContainerProvider provider, IDialogHostService dialogService) : base(provider)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            ExcuteCommand = new DelegateCommand<string>(Excute);
            ToDoCompletedCommand = new DelegateCommand<ToDoDto>(ToDoCompleted);
            CreateTaskBars();
            CreateTestData();
            this.provider = provider;
            this.dialogService = dialogService;
            toDoService = provider.Resolve<IToDoService>();
            memoService = provider.Resolve<IMemoService>();
        }

        private async void ToDoCompleted(ToDoDto todo)
        {
            try
            {
                UpdateLoading(true);
                var updateResult = await toDoService.UpdateAsync(todo);
                if (updateResult.Status)
                {
                    var todoModel = ToDoDtos.FirstOrDefault(t => t.Id == todo.Id);
                    if (todoModel != null)
                    {
                        ToDoDtos.Remove(todoModel);
                    }
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

        private void Excute(string obj)
        {
            switch (obj)
            {
                case "newToDo": AddToDo(null); break;
                case "newMemo": AddMemo(null); break;
            }
        }
        /// <summary>
        /// 添加ToDo
        /// </summary>
        async void AddToDo(ToDoDto model)
        {
            try
            {
                DialogParameters param = new DialogParameters();

                if (model is not null)
                    param.Add("Value", model);

                var dialogResult = await dialogService.ShowDialog("AddToDoView", param);
                UpdateLoading(true);
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var todo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                    // 更新
                    if (todo.Id > 0)
                    {
                        var updateResult = await toDoService.UpdateAsync(todo);
                        if (updateResult.Status)
                        {
                            var todoModel = ToDoDtos.FirstOrDefault(t => t.Id == todo.Id);
                            if (todoModel != null)
                            {
                                todoModel.Title = todo.Title;
                                todoModel.Content = todo.Content;
                            }
                        }
                    }
                    // 添加
                    else
                    {
                        var addResult = await toDoService.AddAsync(todo);
                        if (addResult.Status)
                        {
                            ToDoDtos.Add(addResult.Result);
                        }
                    }
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
        /// <summary>
        /// 添加Memo
        /// </summary>
        async void AddMemo(MemoDto model)
        {
            try
            {
                DialogParameters param = new DialogParameters();

                if (model is not null)
                    param.Add("Value", model);

                var dialogResult = await dialogService.ShowDialog("AddMemoView", param);
                UpdateLoading(true);
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");
                    // 更新
                    if (memo.Id > 0)
                    {
                        var updateResult = await memoService.UpdateAsync(memo);
                        if (updateResult.Status)
                        {
                            var todoModel = MemoDtos.FirstOrDefault(t => t.Id == memo.Id);
                            if (todoModel is not null)
                            {
                                todoModel.Title = memo.Title;
                                todoModel.Content = memo.Content;
                            }
                        }
                    }
                    // 修改
                    else
                    {
                        var addResult = await memoService.AddAsync(memo);
                        if (addResult.Status)
                        {
                            MemoDtos.Add(addResult.Result);
                        }
                    }
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

        public DelegateCommand<string> ExcuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
        public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }
        public DelegateCommand<ToDoDto> ToDoCompletedCommand { get; private set; }

        private ObservableCollection<TaskBar> taskBars;
        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ToDoDto> toDoDtos;
        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<MemoDto> memoDtos;
        private readonly IContainerProvider provider;
        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        private readonly IDialogHostService dialogService;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Color = "#FF0CA0FF", Target = "ToDoView", Content = "0" });
            TaskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF1ECA3A", Target = "ToDoView", Content = "0" });
            TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Color = "#FF02C6DC", Target = "", Content = "0" });
            TaskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Color = "#FFFFA000", Target = "MemoView", Content = "0" });
        }
        void CreateTestData()
        {
        }
    }
}
