using MyToDo.Common.Event;
using MyToDo.Extension;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    /// <summary>
    /// 导航子窗口的基类
    /// 获取数据等待界面
    /// </summary>
    public class NavigationViewModel : BindableBase, INavigationAware
    {
        private readonly IContainerProvider containerProvider;
        public readonly IEventAggregator aggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
            aggregator = containerProvider.Resolve<IEventAggregator>();
        }
        /// <summary>
        /// 是否创建新示例。为true的时候表示不创建新示例，页面还是之前的；如果为false，则创建新的页面
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// 导航到当前页面前, 此处可以传递过来的参数以及是否允许导航等动作的控制
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// 导航离开当前页面前
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
        public void UpdateLoading(bool IsOpen)
        {
            aggregator.UpdateLoading(new UpdateModel() { IsOpen = IsOpen });
        }
    }
}
