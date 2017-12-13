using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2_zad7
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream FS;
            WaitHandle WH = new AutoResetEvent(false);
            try
            {
                FS = new FileStream("plik.txt", FileMode.Open);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("nie znaleziono pliku");
                return;
            }
            byte[] buffer = new byte[FS.Length];
            IAsyncResult IAR = FS.BeginRead(buffer, 0, (int)FS.Length, null, null);
            FS.EndRead(IAR);
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            Console.WriteLine(s);
            FS.Close();
        }
    }
}
