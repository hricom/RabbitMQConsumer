using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq.Domain.Topic
{
    public class MessageReceiver
    {
        public string GetMessage()
        {
            string queueName = "queue";
            string exchange = "Amq.exchange";
            string routingKey = "routingKey";
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.ContinuationTimeout = new TimeSpan(10, 0, 0, 0);
                factory.Uri = new Uri("amqps://dev:nnnnnnnn@test.rmq.cloudamqp.com/dev");

                // Create a connection and open a channel, dispose them when done
                using (IConnection connection = factory.CreateConnection())
                {
                    Console.WriteLine(string.Concat("Connection open: ", connection.IsOpen));
                    using (IModel channel = connection.CreateModel())
                    {

                        Console.WriteLine(string.Concat("Channel open: ", channel.IsOpen));
                        // channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, durable: true, autoDelete: false, arguments: null);                        
                        var queue = channel.QueueDeclare(queue: queueName, durable: true, exclusive: false,  autoDelete: false, arguments: null);
                        channel.QueueBind(queue: queueName, exchange: exchange, routingKey: routingKey, arguments: null);
                        channel.BasicQos(0, 1, false);

                        var consumer = new EventingBasicConsumer(channel);
                        Console.WriteLine("1 escuchando los mensajes");

                        consumer.Received += (model, ea) =>
                        {
                            var bodyy = ea.Body;
                            var messagee = Encoding.UTF8.GetString(bodyy);
                            Console.WriteLine(" [x] Received {0}", messagee);

                            // handle the received message
                            HandleMessage(messagee);
                            channel.BasicAck(ea.DeliveryTag, false);
                        };

                        string consumerTag = channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                        Console.ReadLine();
                    }
                    
                }

            }
            catch (RabbitMQ.Client.Exceptions.RabbitMQClientException e)
            {

                Console.WriteLine("Error: " + e.Message.ToString());
            }
            catch (Exception ex)
            {


            }

            return string.Empty;
        }

        private void HandleMessage(string content)
        {
            // we just print this message 
            Console.WriteLine($"consumer received out {content}");            
        }
    }
}
