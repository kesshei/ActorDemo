using ActorDemo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorDemo
{
    /// <summary>
    /// 输出
    /// </summary>
    public class WriteActor : Actor
    {
        public WriteActor() : base(nameof(WriteActor))
        {
        }
        /// <summary>
        /// 处理信息
        /// </summary>
        /// <returns></returns>
        public override Task ProcessAsync(object msg)
        {
            try
            {
                Console.WriteLine($"输出 {this.Name} :{msg}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"业务处理异常:{e.Message}");
            }
            return Task.CompletedTask;
        }
    }
}
