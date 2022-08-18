using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    internal class MenuBar : BindableBase
    {
        private string icon;

        /// <summary>
        /// Menu Icon
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set { icon = value; RaisePropertyChanged(); }
        }

        private string title;

        /// <summary>
        /// Menu Title
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }
        private string nameSpace;

        /// <summary>
        /// Menu NameSpace
        /// </summary>
        public string NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; RaisePropertyChanged(); }
        }



    }
}
