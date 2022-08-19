using MyToDo.Api.Models;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extension;
using MyToDo.Service;
using MyToDo.Share.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyToDo.ViewModels
{
    internal class IndexViewModel : NavigationViewModel
    {
        private readonly IContainerProvider provider;
        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        private readonly IDialogHostService dialogService;
        private readonly IRegionManager regionManager;
        public IndexViewModel(IContainerProvider provider, IDialogHostService dialogService) : base(provider)
        {
            Title = $"Hello Kyros {DateTime.Now.GetDateTimeFormats('D')[1].ToString()}";
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            ExcuteCommand = new DelegateCommand<string>(Excute);
            ToDoCompletedCommand = new DelegateCommand<ToDoDto>(ToDoCompleted);
            NavigateCommand = new DelegateCommand<TaskBar>(Navigate);

            CreateTaskBars();
            this.provider = provider;
            this.dialogService = dialogService;
            regionManager = provider.Resolve<IRegionManager>();
            toDoService = provider.Resolve<IToDoService>();
            memoService = provider.Resolve<IMemoService>();

            SummaryDto = new SummaryDto()
            {
                Sum = 0,
                CompletedCount = 0,
                MemoCount = 0,
                CompleteRatio = "0 %",
                ToDoList = new ObservableCollection<ToDoDto>(),
                MemoList = new ObservableCollection<MemoDto>()
            };
        }

        private void Navigate(TaskBar bar)
        {
            if (string.IsNullOrWhiteSpace(bar.Target)) return;
            NavigationParameters param = new NavigationParameters();
            if (bar.Title == "已完成")
            {
                param.Add("Value", 2);
            }
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(bar.Target, param);
        }

        private async void ToDoCompleted(ToDoDto todo)
        {
            try
            {
                UpdateLoading(true);
                var updateResult = await toDoService.UpdateAsync(todo);
                if (updateResult.Status)
                {
                    var todoModel = SummaryDto.ToDoList.FirstOrDefault(t => t.Id == todo.Id);
                    if (todoModel != null)
                    {
                        SummaryDto.ToDoList.Remove(todoModel);
                        aggregator.SendMessage($"{todoModel.Title} is Completed.");
                        Refresh();
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
                            var todoModel = SummaryDto.ToDoList.FirstOrDefault(t => t.Id == todo.Id);
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
                            SummaryDto.Sum += 1;
                            SummaryDto.ToDoList.Add(addResult.Result);
                            summaryDto.CompleteRatio = (summaryDto.CompletedCount / (double)summaryDto.Sum).ToString("0 %");
                            Refresh();
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
                            var todoModel = SummaryDto.MemoList.FirstOrDefault(t => t.Id == memo.Id);
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
                            SummaryDto.MemoList.Add(addResult.Result);
                            Refresh();
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
        public DelegateCommand<TaskBar> NavigateCommand { get; private set; }

        private ObservableCollection<TaskBar> taskBars;
        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }
        private SummaryDto summaryDto;

        public SummaryDto SummaryDto
        {
            get { return summaryDto; }
            set { summaryDto = value; RaisePropertyChanged(); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }


        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Color = "#FF0CA0FF", Target = "ToDoView" });
            TaskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF1ECA3A", Target = "ToDoView" });
            TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Color = "#FF02C6DC", Target = "" });
            TaskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Color = "#FFFFA000", Target = "MemoView" });
        }
        void Refresh()
        {
            TaskBars[0].Content = summaryDto.Sum.ToString() ?? "0";
            TaskBars[1].Content = (summaryDto.Sum - summaryDto.ToDoList.Count).ToString();
            TaskBars[2].Content = summaryDto.CompleteRatio = ((summaryDto.Sum - summaryDto.ToDoList.Count) / (double)summaryDto.Sum).ToString("0%");
            TaskBars[3].Content = summaryDto.MemoList.Count.ToString();
        }
        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            UpdateLoading(true);
            var result = await toDoService.SummaryAsync();
            if (result.Status)
            {
                SummaryDto = result.Result;
                Refresh();
            }
            base.OnNavigatedTo(navigationContext);
            UpdateLoading(false);
        }
    }
}
