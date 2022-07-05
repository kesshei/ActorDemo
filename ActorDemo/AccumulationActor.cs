using ActorDemo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorDemo
{
    /// <summary>
    /// 累加
    /// </summary>
    public class AccumulationActor : Actor
    {
        private int Count = 0;
        private IActor actor;
        public AccumulationActor(IActor actor) : base(nameof(AccumulationActor))
        {
            Count = 0;
            this.actor = actor;
        }   
        /// <summary>
        /// 处理信息
        /// </summary>
        /// <returns></returns>
        public override Task ProcessAsync(object msg)
        {
            try
            {
                var  msgNumber = (int)(msg);
                Count += msgNumber;
                Console.WriteLine($"处理{this.Name} :{msg} ，累积总数:{Count}");

                if (Count >= 100)
                {
                    this.actor.AddMsg(Count);
                    Count = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"业务处理异常:{e.Message}");
            }
            return Task.CompletedTask;
        }
    }
}
