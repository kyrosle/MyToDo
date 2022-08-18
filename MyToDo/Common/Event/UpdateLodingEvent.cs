using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Event
{
    /// <summary>
    /// 定义事件数据载体
    /// </summary>
    public class UpdateModel
    {
        public bool IsOpen { get; set; }
    }
    /// <summary>
    /// 定义事件
    /// </summary>
    public class UpdateLodingEvent : PubSubEvent<UpdateModel>
    {
    }
}
