using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ServiceBus.Samples
{
    using System.Configuration;
    using Microsoft.ServiceBus.Messaging;

    class Program
    {
        #region Fields

        static string eventHubName;
        static string consumerGroupName;
        #endregion

        static void Main(string[] args)
        {
            //Initialize
            ParseArgs(args);
            string connectionString = GetServiceBusConnectionString();
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            var e = Manage.UpdateEventHub(eventHubName, consumerGroupName, namespaceManager);
            var res = e.Result;


            Console.WriteLine("Created ConsumerGroup for eventhub. Press any key to stop worker.");
            Console.ReadLine();
        }

        static void ParseArgs(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("Incorrect number of arguments. Expected 2 args <eventhubname> <consumerGroupName>", args.ToString());
            }
            else
            {
                eventHubName = args[0];
                Console.WriteLine("EventHub Name: " + eventHubName);

                consumerGroupName = args[1];
                Console.WriteLine("ConsumerGroup Name: " + consumerGroupName);

            }
        }

        private static string GetServiceBusConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Did not find Service Bus connections string in appsettings (app.config)");
                return string.Empty;
            }
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(connectionString);
            builder.TransportType = TransportType.Amqp;
            return builder.ToString();
        }
    }
}
