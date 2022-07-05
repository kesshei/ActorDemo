using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorDemo.Core
{
    /// <summary>
    /// Actor抽象
    /// </summary>
    public abstract class Actor : IDisposable, IActor
    {
        public Actor(string name)
        {
            Name = name;
            MailBox = new BlockingCollection<object>();
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Active { get; private set; }
        /// <summary>
        /// 是否长时间运行。长时间运行任务使用独立线程，默认true
        /// </summary>
        public bool LongRunning { get; set; } = true;
        /// <summary>
        /// 处理的消息邮箱
        /// </summary>
        public BlockingCollection<object> MailBox { get; set; }
        /// <summary>
        /// 内置任务
        /// </summary>
        private Task _task;

        public virtual Task Start()
        {
            if (Active) return _task;
            Active = true;
            // 启动异步
            if (_task == null)
            {
                lock (this)
                {
                    if (_task == null)
                    {
                        _task = Task.Factory.StartNew(DoActorWork, LongRunning ? TaskCreationOptions.LongRunning : TaskCreationOptions.None);
                    }
                }
            }
            return _task;
        }

        public virtual bool Stop(int WatingTimeout = 100)
        {
            MailBox?.CompleteAdding();
            Active = false;
            if (WatingTimeout == 0 || _task == null) return true;

            return _task.Wait(WatingTimeout);
        }
        public virtual bool AddMsg(object message)
        {
            // 自动开始
            if (!Active)
            {
                Start();
            }

            if (!Active)
            {
                return false;
            }
            MailBox.Add(message);
            return true;
        }
        /// <summary>
        /// 循环消费消息
        /// </summary>
        private void DoActorWork()
        {
            while (!MailBox.IsCompleted)
            {
                try
                {
                    var ctx = MailBox.Take();
                    var task = ProcessAsync(ctx);
                    if (task != null)
                    {
                        task.Wait();
                    }
                }
                catch (InvalidOperationException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"DoActorWork Error : {ex.Message}");
                }
            }

            Active = false;
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <returns></returns>
        public abstract Task ProcessAsync(object msg);
        public void Dispose()
        {
            try
            {
                Stop(100);
            }
            catch (Exception)
            {
            }
            while (MailBox?.TryTake(out _) == true) { }
            MailBox = null;
        }
    }
}
