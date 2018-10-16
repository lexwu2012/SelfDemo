using Redis.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lock
{
    class Program
    {
        static RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        static void Main(string[] args)
        {
            Lock();
        }

        static void Lock()
        {
            Console.WriteLine("Start..........");
            var db = _redis.GetDatabase();
            //RedisValue token = Environment.MachineName;
            var token1 = "机器1";
            var token2 = "机器2";

            var thread1 = new Thread(() =>
            {
                if (db.LockTake("test", token1, TimeSpan.FromSeconds(10)))
                {
                    try
                    {
                        db.StringSet("11", 11, TimeSpan.FromSeconds(10));
                        Console.WriteLine("thread1成功获取test锁");
                    }
                    finally
                    {
                        db.LockRelease("test", token1);
                        Console.WriteLine("thread1成功释放test锁");
                    }
                }
                else
                {
                    var taker = db.LockQuery("test");
                    Console.WriteLine("thread1无法获取test锁，已被" + taker + "机器其他线程占用");
                }
            });

            var thread2 = new Thread(() =>
            {
                bool getTheLock = false;
                while (!getTheLock)
                {
                    if (db.LockTake("test", token2, TimeSpan.FromSeconds(10)))
                    {
                        try
                        {
                            Console.WriteLine("thread2成功获取test锁");
                            getTheLock = true;
                        }
                        finally
                        {
                            db.LockRelease("test", token2);
                            Console.WriteLine("thread2成功释放test锁");
                        }
                    }
                    else
                    {
                        var taker = db.LockQuery("test");
                        Console.WriteLine("thread2无法获取test锁，已被" + taker + "机器其他线程占用");
                    }
                }


            });
            thread1.Start();
            thread2.Start();

            //thread2.Suspend();
            //Thread.Sleep(1000);
            ////线程2继续
            //thread2.Resume();

            //Thread.Sleep(10000);
            Console.ReadKey();
        }
    }
}
