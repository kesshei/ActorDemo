using System;
using System.Threading;

namespace ActorDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Actor Demo by 蓝创精英团队";

            //实现一个加法逻辑
            //a累加到100，就发送消息到 b里，让b 输出。
            var write = new WriteActor();
            var User = new AccumulationActor(write);
            for (int i = 0; i < 20; i++)
            {
                User.AddMsg(i * 30);
            }
            Thread.Sleep(2000);
            write.Stop();
            User.Stop();
            //释放资源
            Console.WriteLine("示例完毕!");
            Console.ReadLine();
        }
    }
}
