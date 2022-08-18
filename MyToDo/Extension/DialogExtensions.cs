using MyToDo.Common;
using MyToDo.Common.Event;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace MyToDo.Extension
{
    /// <summary>
    /// 在 view.cs 里面 注册和等待消息
    /// </summary>
    public static class DialogExtensions
    {
        /// <summary>
        /// 询问窗口
        /// </summary>
        /// <param name="dialogHost">指定DialogHost会话主机</param>
        /// <param name="title">标题</param>
        /// <param name="content">询问内容</param>
        /// <param name="dialogHostName">会话主机名称(唯一)</param>
        /// <returns></returns>
        public static async Task<IDialogResult> QuestionAsync(this IDialogHostService dialogHost,
            string title, string content, string dialogHostName = "Root")
        {
            DialogParameters param = new DialogParameters();
            param.Add("Title", title);
            param.Add("Content", content);
            param.Add("dialogHostName", dialogHostName);

            var result = await dialogHost.ShowDialog("MsgView", param, dialogHostName);
            return result;
        }
        /// <summary>
        /// 推送等待消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="model"></param>
        public static void UpdateLoading(this IEventAggregator aggregator, UpdateModel model)
        {
            aggregator.GetEvent<UpdateLodingEvent>().Publish(model);
        }
        /// <summary>
        /// 注册等待消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void Register(this IEventAggregator aggregator, Action<UpdateModel> action)
        {
            aggregator.GetEvent<UpdateLodingEvent>().Subscribe(action);
        }
    }
}
