using MyToDo.Common.Models;
using MyToDo.Extension;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace MyToDo.ViewModels
{
    internal class SettingViewModel : BindableBase
    {
        private ObservableCollection<MenuBar> menuBars;

        private readonly IRegionManager regionManager;

        public SettingViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            CreateMenuBar();
        }
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        private void Navigate(MenuBar obj)
        {
            if (obj is null || string.IsNullOrWhiteSpace(obj.NameSpace)) return;
            // ContentControl 的导航
            regionManager.Regions[PrismManager.SettingViewRegionName].RequestNavigate(obj.NameSpace);
        }

        private IRegionNavigationJournal journal;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }
        void CreateMenuBar()
        {
            MenuBars = new ObservableCollection<MenuBar>();
            MenuBars.Add(new MenuBar() { Icon = "Palette", Title = "个性化", NameSpace = "SkinView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "系统设置", NameSpace = "" });
            MenuBars.Add(new MenuBar() { Icon = "Information", Title = "关于更多", NameSpace = "AboutView" });
        }
    }
}
