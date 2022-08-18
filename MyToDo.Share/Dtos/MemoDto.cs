using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Share.Dtos
{
    public class MemoDto : BaseDto
    {
        private string title;
        private string content;
        private int status;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyCHanged(); }
        }
        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyCHanged(); }
        }
        public int Status
        {
            get { return status; }
            set { status = value; OnPropertyCHanged(); }
        }
    }
}
