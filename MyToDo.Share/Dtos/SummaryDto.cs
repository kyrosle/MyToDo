using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Share.Dtos
{
    public class SummaryDto : BaseDto
    {
        int sum;
        int completedCount;
        int memoCount;
        string completedRatio;
        ObservableCollection<ToDoDto> toDoList;
        ObservableCollection<MemoDto> memoList;

        public int Sum
        {
            get { return sum; }
            set { sum = value; OnPropertyCHanged(); }
        }
        public int CompletedCount
        {
            get { return completedCount; }
            set { completedCount = value; OnPropertyCHanged(); }
        }
        public int MemoCount
        {
            get { return memoCount; }
            set { memoCount = value; OnPropertyCHanged(); }
        }
        public string CompleteRatio
        {
            get { return completedRatio; }
            set { completedRatio = value; OnPropertyCHanged(); }
        }

        public ObservableCollection<ToDoDto> ToDoList
        {
            get { return toDoList; }
            set { toDoList = value; OnPropertyCHanged(); }
        }
        public ObservableCollection<MemoDto> MemoList
        {
            get { return memoList; }
            set { memoList = value; OnPropertyCHanged(); }
        }
    }
}
