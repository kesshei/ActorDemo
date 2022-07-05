using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorDemo.Core
{
    /// <summary>
    /// 无锁并行编程模型（暂时用来处理串行任务，任务串行执行）
    /// </summary>
    public interface IActor
    {
        /// <summary>
        /// 增加消息
        /// </summary>
        /// <returns></returns>
        bool AddMsg(object message);
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        Task Start();
        /// <summary>
        /// 停止服务运行，等待毫秒数
        /// </summary>
        /// <param name="WatingTimeout"></param>
        /// <returns></returns>
        bool Stop(int WatingTimeout);
    }
}
