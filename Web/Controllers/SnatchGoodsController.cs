using Common;
using Newtonsoft.Json;
using Redis.Common;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace MutipleNetDemo.Controllers
{
    [RoutePrefix("api/SnatchGoods")]
    public class SnatchGoodsController : ApiController
    {
        private readonly string ServerName;
        public RedisStackExchangeHelper _redis { get; set; }

        public SnatchGoodsController()
        {
            _redis = new RedisStackExchangeHelper();
            ServerName = "server2";
        }

        [HttpGet]
        [Route("SnatchGood")]
        public Result SnatchGood(int userId)
        {
            var db = _redis.GetDatabase();

            var goodKey = "good_stroe";

            var orderKey = "order";

            //RedisValue token = Environment.MachineName;

            //var lockKey = "LockKey";

            //if (db.LockTake(lockKey, token, TimeSpan.FromSeconds(0.5)))
            //{
            //    try
            //    {
            //        //1. 判断库存
            //        var remain = db.ListLength(goodKey);

            //        if (remain == 0)
            //        {
            //            return Result.FromError(str + "已抢完");
            //        }

            //        //2. 判断是否已经抢购过
            //        if (!db.SetAdd(orderKey, userId))
            //            return Result.FromError(str + "用户" + userId + "已经抢购过");

            //        //3. 下单
            //        db.SetAdd(orderKey, userId);

            //        //4. 减去库存
            //        db.ListLeftPop(goodKey);
            //    }
            //    finally
            //    {
            //        db.LockRelease(lockKey, token);
            //    }
            //}

            var remain = db.ListLength(goodKey);

            if (remain == 0)
            {
                return Result.FromError(ServerName + "已抢完");
            }


            ////评论者2又被赞了两次
            //db.SortedSetIncrement("文章1", userId, 2); //对应的值的score+2
            ////评论者101被赞了4次
            //db.SortedSetIncrement("文章1", "评论者101", 4);  //若不存在该值，则插入一个新的
            //RedisValue[] userStores = db.SortedSetRangeByRank(orderKey, 0, 10, Order.Descending);
            ////if (db.SortedSetScore("文章1", userStores[i]))
            ////    return Result.FromError(str + "用户" + userId + "已经抢购过");
            //for (int i = 0; i < userStores.Length; i++)
            //{
            //    Console.WriteLine(userStores[i] + ":" + db.SortedSetScore("文章1", userStores[i]));
            //}

            //if (userStores.Count() == 5)
            //    return Result.FromError("用户" + userId + "已经到达预购数量5");

            if (!db.SetAdd(orderKey, userId))
                return Result.FromError(ServerName + "用户" + userId + "已经抢购过");

            db.ListLeftPop(goodKey);

            //方法1
            //db.ListRightPush(orderKey, str + "用户："+ userId);

            //db.SortedSetIncrement(orderKey, userId, 2); //对应的值的score+2
            //db.SortedSetIncrement(orderKey, userId, 1);  //若不存在该值，则插入一个新的

            return Result.Ok("恭喜" + ServerName + "用户：" + userId + "用户抢到了");
        }

        /// <summary>
        /// 分布式，
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SnatchGoodWithDistributedServer")]
        public Result SnatchGoodWithDistributedServer(string idNum)
        {
            var db = _redis.GetDatabase();

            var goodKey = "good_stroe";

            var orderKey = "order";

            RedisValue token = Environment.MachineName;

            //var lockKey = "LockKey";

            //if (db.LockTake(idNum, token, TimeSpan.FromMilliseconds(10)))
            //{
            //    try
            //    {
            //        ////1. 判断库存(不行，库存没减去)
            //        //if (string.IsNullOrWhiteSpace(db.ListGetByIndex(goodKey,0)))
            //        //{
            //        //    return Result.FromError(ServerName + "已抢完");
            //        //}
            //        ////1. 判断库存(不行，库存会全部没有？)
            //        //if (string.IsNullOrWhiteSpace(db.ListLeftPop(goodKey)))
            //        //{
            //        //    return Result.FromError(ServerName + "已抢完");
            //        //}

            //        //1. 先要减去库存
            //        var good = db.ListLeftPop(goodKey);
            //        if(string.IsNullOrWhiteSpace(good))
            //            return Result.FromError(ServerName + "已抢完");

            //        ////1. 判断库存
            //        //if (db.ListLength(goodKey) == 0)
            //        //{
            //        //    return Result.FromError(ServerName + "已抢完");
            //        //}

            //        //2. 判断是否已经抢购过
            //        if (!db.SetAdd(orderKey, idNum))
            //            return Result.FromError(ServerName + "用户" + idNum + "已经抢购过");

            //        //4. 下单（在db.SetAdd(orderKey, idNum)中其实已经下单了，不要在这里用db.ListRightPush，这样会多余且导致类型匹配错误,因为orderKey已经在上面定了位set类型了）
            //        //db.ListRightPush(orderKey, idNum);
            //    }
            //    finally
            //    {
            //        db.LockRelease(idNum, token);
            //    }
            //}
            //else
            //{
            //    return Result.FromError(ServerName + "用户" + idNum + "已经抢购过");
            //}

            //1. 先要减去库存
            var good = db.ListLeftPop(goodKey);
            if (string.IsNullOrWhiteSpace(good))
                return Result.FromError(ServerName + "已抢完");

            //2. 判断是否已经抢购过
            if (!db.SetAdd(orderKey, idNum))
                return Result.FromError(ServerName + "用户" + idNum + "已经抢购过");

            return Result.Ok("恭喜" + ServerName + "用户：" + idNum + "用户抢到了");
        }

        /// <summary>
        /// 单机接收,每人只能抢购1个
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SnatchGoodWithSingleServer")]
        public Result SnatchGoodWithSingleServer(int userId)
        {
            var db = _redis.GetDatabase();

            var goodKey = "good_stroe";

            var orderKey = "order";

            //1. 先要减去库存
            var good = db.ListLeftPop(goodKey);
            if (string.IsNullOrWhiteSpace(good))
                return Result.FromError(ServerName + "已抢完");

            //2. 判断是否已经抢购过,没抢购过的自动添加订单数据
            if (!db.SetAdd(orderKey, userId))
                return Result.FromError(ServerName + "用户" + userId + "已经抢购过");

            return Result.Ok("恭喜" + ServerName + "用户：" + userId + "用户抢到了");
        }

        /// <summary>
        /// 单机，有库存和超抢要求
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, Route("SnatchGoodWithUserAndSingleServer")]
        public Result SnatchGoodWithUserAndSingleServer(UserDto user)
        {
            var db = _redis.GetDatabase();

            var goodKey = "good_stroe";

            //var orderIdnumKey = "order-idnum";
            var orderUserKey = "order-user";
            var existUser = "exist-user";

            //1. 
            //用hashset 的话，如果key（orderUserKey）和hashfiled（idnum）一样，则会覆盖value（user）
            //1 if field is a new field in the hash and value was set. 0 if field already exists in the hash and the value was updated.
            //db.HashSet(existUser, user.idnum, 1)
            //if (db.StringIncrement(user.idnum) <= 2)
            //{
            //    if (db.LockTake(user.idnum, ServerName, TimeSpan.FromMilliseconds(50)))
            //    {
            //        try
            //        {
            //            var goodValue = db.ListLeftPop(goodKey);
            //            if (string.IsNullOrWhiteSpace(goodValue))
            //            {
            //                if (db.StringDecrement(user.idnum) == 0)
            //                {
            //                    db.KeyDelete(user.idnum);
            //                    return Result.FromError(ServerName + "已没有库存");
            //                }
            //            }
            //            db.ListRightPush(orderUserKey, JsonConvert.SerializeObject(user));

            //        }
            //        finally
            //        {
            //            db.LockRelease(user.idnum, ServerName);
            //        }
            //        return Result.Ok("恭喜" + ServerName + "用户：" + user.idnum + "用户抢到了");
            //    }
            //    else
            //    {
            //        //获取不到说明该锁说明其他客户端已经获取过该锁了
            //        var taker = db.LockQuery(user.idnum);

            //        return Result.FromError("当前" + user.idnum + "锁已被" + taker + "占用");
            //    }
            //}
            //else
            //    return Result.FromError(ServerName + "用户" + user.idnum + "已经超过规定数量咯，不能再抢");

            //2. 
            //if (db.StringIncrement(user.idnum) <= 2)
            //{
            //    var goodValue = db.ListLeftPop(goodKey);
            //    if (string.IsNullOrWhiteSpace(goodValue))
            //    {
            //        if (db.StringDecrement(user.idnum) == 0)
            //        {
            //            db.KeyDelete(user.idnum);
            //            return Result.FromError(ServerName + "已没有库存");
            //        }
            //    }
            //    db.ListRightPush(orderUserKey, JsonConvert.SerializeObject(user));
            //    return Result.Ok("恭喜" + ServerName + "用户：" + user.idnum + "用户抢到了");
            //}
            //else
            //    return Result.FromError(ServerName + "用户" + user.idnum + "已经超过规定数量咯，不能再抢");

            if (db.SetAdd(existUser, user.idnum))
            {
                var goodValue = db.ListLeftPop(goodKey);
                if (string.IsNullOrWhiteSpace(goodValue))
                {
                    db.HashDelete(existUser, user.idnum);
                    return Result.FromError(ServerName + "已没有库存");
                }
                db.ListRightPush(orderUserKey, JsonConvert.SerializeObject(user));
                return Result.Ok("恭喜" + ServerName + "用户：" + user.idnum + "用户抢到了");
            }
            else
                return Result.FromError(ServerName + "用户" + user.idnum + "已经抢购过");
        }

        /// <summary>
        /// 消费者进程
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("ConsumeGoodsByMutiplThread")]
        public Result ConsumeGoodsByMutiplThread()
        {
            var db = _redis.GetDatabase();
            
            var orderUserKey = "order-user";

            var values = db.HashGetAll(orderUserKey);

            if(values.Count() == 0)
                return Result.Ok("已发送完毕");

            foreach (var item in values)
            {
                return Result.Ok(item.Value);
            }
            return Result.Ok();
        }

        [HttpGet, Route("GenerateStore")]
        public Result GenerateStore()
        {
            var db = _redis.GetDatabase();
            var goodKey = "good_stroe";

            if (db.KeyExists(goodKey))
                db.KeyDelete(goodKey);

            //1. 设置商品库存
            for (var i = 1; i <= 10; i++)
            {
                db.ListRightPush(goodKey, i);
            }
            return Result.Ok("生成完毕");
        }

        private List<HashEntry> ObjectToHashEntryList<T>(string key, T obj) where T : class, new()
        {
            //var people = new UserDto() { idnum = "123456", tickettypeid = "st" };
            List<HashEntry> list = new List<HashEntry>();
            foreach (PropertyInfo p in obj.GetType().GetProperties())
            {
                var name = p.Name.ToString();
                var val = p.GetValue(obj);
                list.Add(new HashEntry(name, JsonConvert.SerializeObject(val)));
            }
            var db = _redis.GetDatabase();
            db.HashSet(key, list.ToArray());
            return list;
        }
    }

    public class UserDto
    {
        /// <summary>
        /// 购买的票种
        /// </summary>
        public string tickettypeid { get; set; }

        /// <summary>
        /// 游客身份证号码
        /// </summary>
        public string idnum { get; set; }
    }
}
