using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common
{
    public interface IDialogHostAware
    {
        string DialogHostName { get; set; }
        /// <summary>
        /// 打开过程中执行
        /// </summary>
        /// <param name="parameters"></param>
        void OnDialogOpen(IDialogParameters parameters);
        DelegateCommand SaveCommand { get; set; }
        DelegateCommand CancelCommand { get; set; }
    }
}
