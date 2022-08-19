using MyToDo.Extension;
using MyToDo.Service;
using MyToDo.Share.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    internal class LoginViewModel : BindableBase, IDialogAware
    {
        public LoginViewModel(ILoginService loginService, IEventAggregator aggregator)
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);
            UserDto = new RegisterUserDto();
            this.loginService = loginService;
            this.aggregator = aggregator;
        }

        private void Execute(string arg)
        {
            switch (arg)
            {
                case "Login": Login(); break;
                case "LoginOut": LoginOut(); break;
                case "Go": SelectIndex = 1; break;
                case "Register": Register(); break;
                case "Return": SelectIndex = 0; break;
            }
        }

        private async void Register()
        {
            if (string.IsNullOrWhiteSpace(UserDto.Account)
                || string.IsNullOrWhiteSpace(UserDto.PassWord)
                || string.IsNullOrWhiteSpace(UserDto.NewPassWord)
                || string.IsNullOrWhiteSpace(UserDto.UserName))
            {
                aggregator.SendMessage("Filed can not Empty", "Login");
                return;
            }
            if (UserDto.PassWord != UserDto.NewPassWord)
            {
                aggregator.SendMessage("PassWord not complete same", "Login");
                return;
            }

            var result = await loginService.RegisterAsync(new UserDto
            {
                Account = UserDto.Account,
                UserName = UserDto.UserName,
                PassWord = UserDto.NewPassWord
            });
            if (result != null && result.Status)
            {
                aggregator.SendMessage("Sucessful", "Login");
                SelectIndex = 0;
                return;
            }
            aggregator.SendMessage("UnSucessful", "Login");
            SelectIndex = 0;
        }

        async void Login()
        {
            if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(PassWord)) return;

            var loginResult = await loginService.LoginAsync(new UserDto()
            {
                Account = Account,
                PassWord = PassWord,
            });

            if (loginResult != null && loginResult.Status)
            {
                aggregator.SendMessage("Login Ok", "Login");
                AppSession.UserName = loginResult.Result.UserName;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                aggregator.SendMessage("Login Failed", "Login");
            }
        }

        void LoginOut()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }
        public string Title { get; set; } = "ToDo";
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            LoginOut();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        private string account;

        public string Account
        {
            get { return account; }
            set { account = value; RaisePropertyChanged(); }
        }
        private string passWord;
        private readonly ILoginService loginService;
        private readonly IEventAggregator aggregator;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; RaisePropertyChanged(); }
        }
        private int selectIndex;

        public int SelectIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; RaisePropertyChanged(); }
        }

        private RegisterUserDto userDto;

        public RegisterUserDto UserDto
        {
            get { return userDto; }
            set { userDto = value; }
        }
    }
}
