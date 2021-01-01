

namespace RabbitMq.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RabbitMQ.Client;
    using System.Text;
    using RabbitMQ.Client.Events;
    using Newtonsoft.Json;
    using System.Security.Authentication;
    using System.Linq;

    public class HostUri
    {
        public string GetMessage()
        {
            string queueName = "queue";
            string exchangeName = "exchange";
            string routingKeyIn = "routingKeyIn";
            string message = string.Empty;
            string rpt = string.Empty;
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.ContinuationTimeout = new TimeSpan(10, 0, 0, 0);
                factory.Protocol = Protocols.AMQP_0_9_1;                
                factory.Uri = new Uri("amqps://dev:nnnnnnnnnnnnnnn@test.rmq.cloudamqp.com:5671/dev");                

                using (IConnection connection = factory.CreateConnection())
                {
                    Console.WriteLine(string.Concat("Connection open: ", connection.IsOpen));
                    using (var channel = connection.CreateModel())
                    {
                        Console.WriteLine("Step 1, Channel");                       
                        channel.QueueDeclare(queue: queueName,
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);
                        
                        var data = channel.BasicGet(queue: queueName, autoAck: false);

                        Console.WriteLine("Step 2, Channel");
                        // the message is null if the queue was empty                       

                        if (data != null)
                        {
                            Console.WriteLine("Step 2.1, Channel");
                            Console.WriteLine($"Step 3, Channel {data}");
                            Console.WriteLine(data);
                            message = Encoding.UTF8.GetString(data.Body);
                            Console.WriteLine("Step 4, Channel");
                            channel.BasicAck(data.DeliveryTag, false);
                            Console.WriteLine("Step 5, Channel");
                            rpt = Json(message);
                        }
                        else
                        {
                            rpt = Json("sin respuesta");
                        }
                    }


                }
            }
            catch (RabbitMQ.Client.Exceptions.RabbitMQClientException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rpt;
        }

        private string Json(string p)
        {
            // ar data = JsonConvert.DeserializeObject(p);
            return p;
        }
    }

}
