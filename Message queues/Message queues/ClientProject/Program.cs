using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace ClientProject
{
    class Program
    {
        private const string MessageQueueName = @".\Private$\MySimpleLocalQueue";

        private const string ListeningFolderName = @"D:\ListeningFolder";

        private const int BytesInMegabyte = 1024 * 1024;

        private const int Limit = 1;


        static void Main(string[] args)
        {
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
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    var buffer = new byte[(int)fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    
                    var size = Limit * BytesInMegabyte;

                    int from = 0;

                    int position = 1;

                    int count = Convert.ToInt32(Math.Ceiling((double)fs.Length / (double)size));

                    Guid fileId = Guid.NewGuid();

                    while (from < buffer.Length)
                    {
                        var message = new Message();
                        message.Label = Path.GetFileName(path);

                        var part = new FilePart()
                        {
                            Id = fileId,
                            Buffer = buffer.Skip(from).Take(size).ToArray(),
                            FileName = Path.GetFileName(path),
                            PartPosition = position,
                            PartsCount = count,
                            FileSize = buffer.Length
                        };

                        message.Body = part;

                        from += size;
                        position++;
                      
                        client.Send(message);
                    }
                }
                
            }
        }
    }
}
