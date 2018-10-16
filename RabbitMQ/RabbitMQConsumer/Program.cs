using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.UserName = "lex";
            factory.Password = "lex851225";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "writeLog",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        ExcuateWriteFile(message);
                        Console.WriteLine(" Receiver Received {0}", message);
                    };
                    channel.BasicConsume(queue: "writeLog",
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }


            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare("hello", false, false, false, null);

            //        var consumer = new QueueingBasicConsumer(channel);
            //        channel.BasicConsume("hello", true, consumer);

            //        Console.WriteLine(" waiting for message.");
            //        while (true)
            //        {
            //            var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

            //            var body = ea.Body;
            //            var message = Encoding.UTF8.GetString(body);
            //            Console.WriteLine("Received {0}", message);

            //        }
            //    }
            //}

            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare("hello", false, false, false, null);

            //        var consumer = new QueueingBasicConsumer(channel);
            //        channel.BasicConsume("hello", true, consumer);

            //        while (true)
            //        {
            //            var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

            //            var body = ea.Body;
            //            var message = Encoding.UTF8.GetString(body);

            //            int dots = message.Split('.').Length - 1;
            //            Thread.Sleep(dots * 1000);

            //            Console.WriteLine("Received {0}", message);
            //            Console.WriteLine("Done");
            //        }
            //    }
            //}
        }

        public static void ExcuateWriteFile(string i)
        {
            using (FileStream fs = new FileStream(@"d:\\test.txt", FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                    sw.Write(i);
                }
            }
        }
    }
}
