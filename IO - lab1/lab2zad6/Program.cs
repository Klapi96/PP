using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2
{
    // ZADANIE 6
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
            FS.BeginRead(buffer, 0, (int)FS.Length, new AsyncCallback(Handler), new object[] { buffer, FS, WH });

            WH.WaitOne();

        }

        static void Handler(IAsyncResult IAR)
        {
            byte[] buffer = (byte[])((object[])IAR.AsyncState)[0];
            FileStream FS = (FileStream)((object[])IAR.AsyncState)[1];
            AutoResetEvent ARE = (AutoResetEvent)((object[])IAR.AsyncState)[2];

            string s = ASCIIEncoding.ASCII.GetString(buffer);
            Console.WriteLine(s);
            FS.EndRead(IAR);
            FS.Close();

            ARE.Set();
        }

    }
}
