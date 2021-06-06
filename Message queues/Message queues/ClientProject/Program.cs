using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientProject
{
    class Program
    {
        const string MessageQueueName = @".\Private$\MySimpleLocalQueue";

        const string ListeningFolderName = @"D:\ListeningFolder";


        static void Main(string[] args)
        {
            var sdf = new MessageQueue();

            var sentFiles = new List<string>();

            if (!Directory.Exists(ListeningFolderName))
            {
                Directory.CreateDirectory(ListeningFolderName);
            }

            while (true)
            {
                var result = Directory.GetFiles(ListeningFolderName);

                foreach (var file in result)
                {
                    if (!sentFiles.Contains(file))
                    {
                        SendFile(file);
                        sentFiles.Add(file);
                    }
                }

                Thread.Sleep(2000);

            }
           
        }

        private static void SendFile(string path)
        {
            using (var client = new MessageQueue(MessageQueueName))
            {
                var message = new Message();

                using (var fs = new FileStream(path, FileMode.Open))
                {
                    message.BodyStream = fs;
                    message.Label = Path.GetFileName(path);
                    message.Priority = MessagePriority.Normal;
                    client.Send(message);
                }
                
            }
        }
    }
}
