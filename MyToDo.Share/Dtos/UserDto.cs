using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Share.Dtos
{
    public class UserDto : BaseDto
    {
        private string? userName;

        public string? UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyCHanged(); }
        }
        private string account;

        public string Account
        {
            get { return account; }
            set { account = value; OnPropertyCHanged(); }
        }
        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; OnPropertyCHanged(); }
        }

    }
}
