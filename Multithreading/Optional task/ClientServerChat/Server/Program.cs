using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private static TcpListener tcpListener;

        static void Main(string[] args)
        {
            tcpListener = new TcpListener(IPAddress.Any, 8888);

            tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                if (tcpClient != null && tcpClient.Connected)
                {
                    Console.WriteLine("User connected");
                    var sw = tcpClient.GetStream();
                    byte[] data = Encoding.Unicode.GetBytes("Connected");
                    sw.Write(data, 0, data.Length);
                }
            }
        }
    }
}
