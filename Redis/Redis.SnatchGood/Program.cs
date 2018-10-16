using Redis.Common;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Redis.SnatchGood
{
    class Program
    {
        static RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        private static Dictionary<int, Thread> activeThreads = new Dictionary<int, Thread>();
        static void Main(string[] args)
        {
            Console.WriteLine("Start..........");


            var db = _redis.GetDatabase();

            //db.ListRightPush();

            var goodKey = "good_stroe";

            var orderKey = "order";

            if (db.KeyExists(goodKey))
                db.KeyDelete(goodKey);

            //1. 设置商品库存
            for (var i = 1; i <= 100; i++)
            {
                db.ListRightPush(goodKey, i);
            }

            //var remain = db.ListLength(goodKey);
            ////db.ListRightPush(goodKey, 100);
            ////db.SetAdd(goodKey, 100);

            //if(remain == 0)
            //    Console.WriteLine("已抢完");

            var userId = new Random().Next(100, 200);

            var threads = new Thread[2];
            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => SnappGood(db, userId, goodKey))
                {
                    IsBackground = true
                };
                //threads[i].Start();
                activeThreads.Add(i, threads[i]);
                //Task.Run(() => IocManager.Instance.Resolve<PostDataJob>().SendData(AliBusinessNotificationMethod.Send));
            }

            foreach (var thread in activeThreads)
            {
                thread.Value.Start();
            }

            var vaule = db.ListLeftPop(goodKey);
            var newValue = Convert.ToInt32(vaule);
            //if (newValue == 0)
            //{
            //    Console.WriteLine("已抢完");
            //}
            //else
            //{
            //    RedisValue[] userStores = db.SortedSetRangeByRank(orderKey, 0, 10, Order.Descending);
            //    //RedisValue[] userStores = db.ListRange("user");
            //    if (userStores.Count() == 5)
            //        Console.WriteLine("用户" + userId + "已经到达预购数量5");

            //    //新数量重新入队
            //    db.ListRightPush(goodKey, newValue - 1);
            //    //保存订单
            //    db.SortedSetIncrement(orderKey, userId, 1);  //若不存在该值，则插入一个新的
            //    //db.ListRightPush(orderKey, userId);

            //    Console.WriteLine("恭喜" + userId + "用户抢到了哦");
            //}


            Console.ReadKey();
        }

        public static void SnappGood(IDatabase db, int userId, string goodKey)
        {
            var orderKey = "order";

            bool strat = true;
            while (strat)
            {

                var remain = db.ListLength(goodKey);

                if (remain == 0)
                {
                    Console.WriteLine("已抢完");
                    strat = false;
                }

                RedisValue[] userStores = db.SortedSetRangeByRank(orderKey, 0, 10, Order.Descending);
                //RedisValue[] userStores = db.ListRange(userId);
                if (userStores.Count() == 5)
                    Console.WriteLine("用户" + userId + "已经到达预购数量5");

                //var usrId = new Random().Next(1, 500);

                var vaule = db.ListLeftPop(goodKey);
                var newValue = Convert.ToInt32(vaule);

                db.SortedSetIncrement(orderKey, userId, 1);  //若不存在该值，则插入一个新的
            }
            //var userId = new Random().Next(100, 200);

           
        }
    }
}
