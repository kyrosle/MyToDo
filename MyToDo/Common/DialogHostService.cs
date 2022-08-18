using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo.Common
{
    /// <summary>
    /// 对话主机服务(自定义)
    /// </summary>
    public class DialogHostService : DialogService, IDialogHostService
    {
        private readonly IContainerExtension containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string diailogHostName = "Root")
        {
            if (parameters == null)
                parameters = new DialogParameters();

            // 从容器当中取出窗口实例
            var content = containerExtension.Resolve<object>(name);
            // 验证实例有效性
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a frameworkElement");

            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (!(dialogContent.DataContext is IDialogHostAware viewModel))
                throw new NullReferenceException("a dialog's ViewModel must be implement the IDialogHostAware interface");

            viewModel.DialogHostName = diailogHostName;

            DialogOpenedEventHandler eventHandler = (sender, eventArgs) => {
                if(viewModel is IDialogHostAware aware)
                {
                    aware.OnDialogOpen(parameters);
                }
                eventArgs.Session.UpdateContent(content);
            };

            return (IDialogResult)await DialogHost.Show(dialogContent, viewModel.DialogHostName, eventHandler);
        }
    }
}
