using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Share.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    internal class IndexViewModel : BindableBase
    {
        public IndexViewModel(IDialogHostService dialogService)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            CreateTaskBars();
            CreateTestData();
            this.dialogService = dialogService;
        }

        private void Excute(string obj)
        {
            switch (obj)
            {
                case "newToDo": AddToDO(); break;
                case "newMemo": AddMemo(); break;
            }
        }
        void AddToDO()
        {
            dialogService.ShowDialog("AddToDoView", null);
        }
        void AddMemo()
        {
            dialogService.ShowDialog("AddMemoView", null);
        }

        public DelegateCommand<string> ExcuteCommand { get; private set; }
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
            for (int i = 0; i < 10; i++)
            {
                ToDoDtos.Add(new ToDoDto() { Title = $"代办事项{i}", Content = $"{i}" });
                MemoDtos.Add(new MemoDto() { Title = $"备忘事项{i}", Content = $"{i}" });
            }
        }
    }
}
