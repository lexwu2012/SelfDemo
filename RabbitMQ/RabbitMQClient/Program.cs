using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.UserName = "lex";
            factory.Password = "lex851225";

            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        //声明queue的名字
            //        channel.QueueDeclare("hello", false, false, false, null);
            //        string message = "Hello World";
            //        var body = Encoding.UTF8.GetBytes(message);
            //        channel.BasicPublish("", "hello", null, body);
            //        Console.WriteLine(" set {0}", message);
            //        Console.ReadKey();
            //    }
            //}

            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare("hello", false, false, false, null);
            //        string message = GetMessage(args);
            //        var properties = channel.CreateBasicProperties();
            //        properties.DeliveryMode = 2;

            //        var body = Encoding.UTF8.GetBytes(message);
            //        channel.BasicPublish("", "hello", properties, body);
            //        Console.WriteLine(" set {0}", message);
            //    }
            //}

            //Console.ReadKey();

            new Thread(Write).Start();
            new Thread(Write).Start();
            new Thread(Write).Start();

            Console.ReadKey();
        }

        public static void Write()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.UserName = "lex";
            factory.Password = "lex851225";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "writeLog", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    for (int i = 0; i < 8000; i++)
                    {
                        string message = i.ToString();
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "", routingKey: "writeLog", basicProperties: null, body: body);
                        Console.WriteLine("Program Sent {0}", message);
                    }
                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
