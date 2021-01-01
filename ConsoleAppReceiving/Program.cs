/// <summary>
/// 
/// 
/// Prueba de utilizacion de RabbitMQ
/// </summary>

namespace ConsoleAppReceiving
{
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Text;
    using RabbitMq.Domain;
    using RabbitMq.Domain.Fanout;

    class Program
    {
        static void Main(string[] args)
        {
            string rpt = string.Empty;

            //Host getMessage = new Host();
            //getMessage.GetMessage();

            //HostUri getMessage = new HostUri();
            //string rpt = getMessage.GetMessage();

            // rpt = new RabbitMq.Domain.Fanout.MessageReceiver().GetMessage();
            rpt = new RabbitMq.Domain.Topic.MessageReceiver().GetMessage();

            Console.WriteLine(rpt);

            Console.WriteLine("Hello World!");
        }
    }
}
