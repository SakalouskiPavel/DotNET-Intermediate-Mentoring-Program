using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ServerProject
{
    public class Program
    {
        const string MessageQueueName = @".\Private$\MySimpleLocalQueue";

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

            Queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(ErrorNotification), typeof(object) });

            while (true)
            {
                var message = Queue.Receive();
                var buffer = new byte[100];
                using (var sr = message.BodyStream)
                {
                    buffer = new byte[sr.Length];
                    var sdf  = sr.Read(buffer,0, (int)sr.Length);
                }

                using (var stream = new FileStream(@"D:\FolderForMessageQueue\" + message.Label, FileMode.OpenOrCreate))
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
                
            }
        }
    }
}
