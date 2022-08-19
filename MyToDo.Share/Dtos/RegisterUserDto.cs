using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Share.Dtos
{
    public class RegisterUserDto : UserDto
    {
		private string newPassWord;

		public string NewPassWord
		{
			get { return newPassWord; }
			set { newPassWord = value; OnPropertyCHanged(); }
		}
	}
}
