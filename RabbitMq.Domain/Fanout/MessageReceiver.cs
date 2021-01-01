using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq.Domain.Fanout
{
    public class MessageReceiver
    {
        public string GetMessage()
        {
            string queueName = "queue";
            string exchange = "Amq.exchange.Fanout";

            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.ContinuationTimeout = new TimeSpan(10, 0, 0, 0);
                // Uri = protocolo, usuario, password, url, vhs
                factory.Uri = new Uri("amqps://dev:nnnnnnnnnnnn@test.rmq.cloudamqp.com/dev");

                // Create a connection and open a channel, dispose them when done
                using (IConnection connection = factory.CreateConnection())
                {
                    Console.WriteLine(string.Concat("Connection open: ", connection.IsOpen));
                    using (IModel channel = connection.CreateModel())
                    {
                        Console.WriteLine(string.Concat("Channel open: ", channel.IsOpen));
                        channel.QueueDeclare(queue: queueName, durable: true,exclusive: false, autoDelete: false, arguments: null);
                        channel.QueueBind(queue: queueName, exchange: exchange, routingKey: "", arguments: null);
                        channel.BasicQos(0, 1, false);                       

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var bodyy = ea.Body;
                            var messagee = Encoding.UTF8.GetString(bodyy);
                            Console.WriteLine("received [x] {0}", messagee);
                        };
                        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);


                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return string.Empty;
        }
    }
}
