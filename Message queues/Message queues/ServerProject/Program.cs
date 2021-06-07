using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace ServerProject
{
    public class Program
    {
        const string MessageQueueName = @".\Private$\MySimpleLocalQueue";

        const string FolderForFiles = @"D:\FolderForMessageQueue\";

        private static MessageQueue Queue { get; set; }

        public class ErrorNotification
        {
            public int ErrorId { get; set; }
            public TraceLevel Level { get; set; }
            public string Message { get; set; }

            public override string ToString()
            {
                return ErrorId + " - " + Level + " - " + Message;
            }
        }

        static void Main(string[] args)
        {
            if (!MessageQueue.Exists(MessageQueueName))
            {
                Queue = MessageQueue.Create(MessageQueueName);
            }
            else
            {
                Queue = new MessageQueue(MessageQueueName);
            }

            Queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(ErrorNotification), typeof(FilePart) });

            while (true)
            {
                var message = Queue.Receive();
                var buffer = new byte[100];
                if (message != null && message.Body is FilePart)
                {
                    var part = (FilePart)message.Body;
                    if (part.PartsCount != 1)
                    {
                        if (!Directory.Exists(FolderForFiles + part.Id.ToString()))
                        {
                            Directory.CreateDirectory(FolderForFiles + part.Id.ToString());
                        }

                        var files = Directory.GetFiles(FolderForFiles + part.Id.ToString()).OrderBy(x => x).ToArray();

                        if (files.Length < part.PartsCount - 1)
                        {
                            using (var stream =
                                new FileStream(FolderForFiles + part.Id.ToString() + "\\" + part.PartPosition + ".tmp",
                                    FileMode.OpenOrCreate))
                            {
                                stream.Write(part.Buffer, 0, part.Buffer.Length);
                            }
                        }
                        else
                        {
                            var resultBuffer = new List<byte>();

                            for (int i = 1; i <= part.PartsCount; i++)
                            {
                                if (i == part.PartPosition)
                                {
                                    resultBuffer.AddRange(part.Buffer);
                                }
                                else
                                {
                                    using (var stream = new FileStream(FolderForFiles + part.Id.ToString() + "\\" + i + ".tmp", FileMode.OpenOrCreate))
                                    {
                                        var tempBuffer = new byte[(int)stream.Length];
                                        stream.Read(tempBuffer, 0, (int)stream.Length);

                                        resultBuffer.AddRange(tempBuffer);
                                    }
                                }
                            }

                            using (var stream = new FileStream(FolderForFiles + part.FileName, FileMode.OpenOrCreate))
                            {
                                stream.Write(resultBuffer.ToArray(), 0, resultBuffer.Count);
                            }
                        }
                    }
                    else
                    {
                        using (var stream = new FileStream(FolderForFiles + part.FileName, FileMode.OpenOrCreate))
                        {
                            stream.Write(part.Buffer, 0, part.Buffer.Length);
                        }
                    }
                }
            }
        }
    }
}

