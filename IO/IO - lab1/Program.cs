using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace IO___lab1
{
    class Program
    {
        private static object lockZad4 = new object();
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello world");
            //--------------------------------------------------------------ZAD 1
            //ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 500 });
            //ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 1500 });
            //Thread.Sleep(4000);
            //--------------------------------------------------------------END ZAD 1

            //--------------------------------------------------------------ZAD 2,3,4
            ThreadPool.QueueUserWorkItem(serverr);
            ThreadPool.QueueUserWorkItem(client1);
            ThreadPool.QueueUserWorkItem(client2);
            Thread.Sleep(4000);
            //---------------------------------------------------------------END ZAD 2,3,4
        }

        //------------------------------------------ZAD 1
        static void ThreadProc(Object stateInfo)
        {
            Thread.Sleep((int)((object[])stateInfo)[0]);
            Console.WriteLine("ELO");
        }
        //------------------------------------------END ZAD 1


        //------------------------------------------ZAD 2,3,4

        static void writeConsoleMessage(string message, ConsoleColor color)
        {
            lock (lockZad4)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            
        }

        static void serverr(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                if (client != null)
                {
                    ThreadPool.QueueUserWorkItem(ThreadProcServerHandler, new object[] { client });
                }
                
            }
        }

        static void ThreadProcServerHandler(Object stateInfo)
        {
            
            byte[] buffer = new byte[1024];
            ((TcpClient)((object[])stateInfo)[0]).GetStream().Read(buffer, 0, 1024);
            string wiadd = ASCIIEncoding.ASCII.GetString(buffer);
            writeConsoleMessage(wiadd, ConsoleColor.Red);
            
            ((TcpClient)((object[])stateInfo)[0]).GetStream().Write(buffer, 0, buffer.Length);

        }

        static void client1(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            NetworkStream stream = client.GetStream();
            byte[] message = new byte[1024];
            String wiad = "wiadomosc-1";
            for (int i = 0; i < wiad.Length; i++)
            {
                message[i] = (byte)wiad[i];
            }
            stream.Write(message, 0, message.Length);
    
            while (true)
            {
                byte[] buffer = new byte[1024];

                client.GetStream().Read(buffer, 0, buffer.Length);
                string wiadd = ASCIIEncoding.ASCII.GetString(buffer);
                wiadd = "otrzymalem wiadomosc:" + wiadd;
                writeConsoleMessage(wiadd, ConsoleColor.Green);
            }
        }
        static void client2(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            NetworkStream stream = client.GetStream();
            byte[] message = new byte[1024];
            String wiad = "wiadomosc-2";
            for (int i = 0; i < wiad.Length; i++)
            {
                message[i] = (byte)wiad[i];
            }
            stream.Write(message, 0, message.Length);

            while (true)
            {
                byte[] buffer = new byte[1024];

                client.GetStream().Read(buffer, 0, buffer.Length);
                string wiadd = ASCIIEncoding.ASCII.GetString(buffer);
                wiadd = "otrzymalem wiadomosc:" + wiadd;
                writeConsoleMessage(wiadd, ConsoleColor.Green);
            }
        }
        //------------------------------------------END ZAD 2,3,4
    }
}
