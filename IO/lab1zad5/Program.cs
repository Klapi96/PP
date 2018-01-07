using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace lab1zad5
{
    class Program
    {
        public static int suma;
        public static int[] tab;
        private static void zrobTabele(int[] x)
        {
            Random R = new Random();
            for(int i=0; i<x.Length;i++)
            {
                x[i] = 1;
                //x[i] = R.Next(10);
            }
        }
        static void Main(string[] args)
        {
            suma = 0;
            Console.WriteLine("Jak duza ma byc tablica?");
            int n = Convert.ToInt32(Console.ReadLine());
            tab = new int[n];
            zrobTabele(tab);

            Console.WriteLine("Jakiej wielkosci ma byc fragment?");
            int x = Convert.ToInt32(Console.ReadLine());
            int numberOfFragments=0;
            if (x > n)
            {
                Console.WriteLine("Error");
            }
            else
            {
                if (n % x == 0)
                {
                    numberOfFragments = n / x;
                }
                else
                {
                    numberOfFragments = (n / x) + 1;
                }
            }

            WaitHandle[] waitHandles = new WaitHandle[numberOfFragments];
            for(int k = 0; k < waitHandles.Length; k++)
            {
                waitHandles[k] = new AutoResetEvent(false);
            }

            int start = 0;
            for(int j = 0;j < numberOfFragments; j++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), new object[] { start, ((j+1)* x)-1, waitHandles[j] });
                start += x;
            }

            WaitHandle.WaitAll(waitHandles);
            Console.WriteLine("Suma globalna " + suma);
        }

        static void ThreadProc(Object stateInfo)
        {
            int x = ((int)((object[])stateInfo)[0]);
            int y = ((int)((object[])stateInfo)[1]);
            int sumka = 0;

            for (int i = x; i <= y; i++)
            {
                if (i >= tab.Length)
                {
                    break;
                }
                else
                {
                    sumka += tab[i];
                }
                
            }
            Console.WriteLine("Suma lokalna "+sumka);
            suma += sumka;
            AutoResetEvent waitHandle = (AutoResetEvent)((object[])stateInfo)[2];
            waitHandle.Set();
        }
    }
}
