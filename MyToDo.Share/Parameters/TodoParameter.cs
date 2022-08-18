using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Share.Parameters
{
    public class TodoParameter : QueryParameter
    {
		private int? status;

		public int? Status
		{
			get { return status; }
			set { status = value; }
		}

	}
}
