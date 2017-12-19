using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using System;

namespace Baibaocp.Hangfire.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseRedisStorage("192.168.1.21:6379,password=zf8Mjjo6rLKDzf81,defaultDatabase=9").UseConsole();

            System.Console.WriteLine("Hangfire Server started. Press any key to exit...");
            //var server = new BackgroundJobServer();

            for (int i = 0; i < 1000; i++)
            {
                //支持基于队列的任务处理：任务执行不是同步的，而是放到一个持久化队列中，以便马上把请求控制权返回给调用者。  
                var jobId = BackgroundJob.Enqueue(() => System.Console.WriteLine("{0} ---> Enqueue!", DateTime.Now.ToString("HH:mm:ss")));

                ////延迟任务执行：不是马上调用方法，而是设定一个未来时间点再来执行。  
                //BackgroundJob.Schedule(() => System.Console.WriteLine("{0} ---> Schedule!", DateTime.Now.ToString("HH:mm:ss")), TimeSpan.FromSeconds(5));

                ////循环任务执行：一行代码添加重复执行的任务，其内置了常见的时间循环模式，也可基于CRON表达式来设定复杂的模式。  
                //RecurringJob.AddOrUpdate(() => System.Console.WriteLine("{0} ---> AddOrUpdate!", DateTime.Now.ToString("HH:mm:ss")), Cron.Minutely); //注意最小单位是分钟  

                ////延续性任务执行：类似于.NET中的Task,可以在第一个任务执行完之后紧接着再次执行另外的任务  
                //BackgroundJob.ContinueWith(jobId, () => System.Console.WriteLine("{0} ---> ContinueWith!", DateTime.Now.ToString("HH:mm:ss")));
            }
            
            System.Console.ReadKey();
        }
    }
}
