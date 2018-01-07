using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab3zad12
{
    class Program
    {
        public struct TResultDataStructure
        {
            public int var1 { get; set; }
            public int var2 { get; set; }

            public TResultDataStructure(int var1, int var2)
            {
                this.var1 = var1;
                this.var2 = var2;
            }
        }

        public static Task<TResultDataStructure> OperationTask(byte[] buffer)
        {
            TaskCompletionSource<TResultDataStructure> taskCompletionSource = new TaskCompletionSource<TResultDataStructure>();
            Task.Run(() =>
            {
                taskCompletionSource.SetResult(new TResultDataStructure(buffer[0], buffer[1]));
            });
            return taskCompletionSource.Task;
        }

        static void Main(string[] args)
        {
            int sleeper = 2000;
            byte[] buffer = { 111, 222 };
            Task task = OperationTask(buffer);
            Thread.Sleep(sleeper);
            TResultDataStructure result = ((Task<TResultDataStructure>)task).Result;
            Console.WriteLine(result.var1);
            Console.WriteLine(result.var2);
        }

    }
}
