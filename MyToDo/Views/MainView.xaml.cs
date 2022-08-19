using MyToDo.Common;
using MyToDo.Extension;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyToDo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// 窗口的最小化，最大化，关闭的事件
    /// menubar 选后自动隐藏事件
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IDialogHostService dialogHost;

        public MainView(IEventAggregator aggregator, IDialogHostService dialogHost)
        {
            InitializeComponent();
            btnMin.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else this.WindowState = WindowState.Maximized;
            };
            btnClose.Click += async (s, e) =>
            {
                var dialogResult = await dialogHost.QuestionAsync("Tips", "退出系统 ？ ");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                this.Close();
            };
            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else this.WindowState = WindowState.Maximized;
            };
            ColorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
            // 注册等待窗口
            aggregator.Register(arg =>
            {
                DialogHost.IsOpen = arg.IsOpen;
                if (DialogHost.IsOpen)
                    DialogHost.DialogContent = new ProgressView();
            });

            // 注册提示消息
            aggregator.RegisterMessage(arg =>
            {
                SnackBar.MessageQueue.Enqueue(arg);
            });

            this.dialogHost = dialogHost;
        }
    }
}
