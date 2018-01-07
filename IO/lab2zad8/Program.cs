using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_zad8
{
    class Program
    {
        static int SilniaRekurencyjna(int number)
        {
            if (number < 2)
            {
                return 1;
            }
            return number * SilniaRekurencyjna(number - 1);
        }
        static int SilniaIteracyjna(int number)
        {
            int result = 1;

            for (int i = 1; i <= number; i++)
            {
                result *= i;
            }
            return result;
        }

        static int Fibonacci(int number)
        {
            int pop = 0;
            int next = 1;
            int temp;
            for (int i = 1; i < number; i++)
            {
                temp = next;
                next = pop + next;
                pop = temp;
            }
            return next;
        }

        delegate int DelegateType(int arguments);
        static void Main(string[] args)
        {
            DelegateType D1 = new DelegateType(SilniaRekurencyjna);
            DelegateType D2 = new DelegateType(SilniaIteracyjna);
            DelegateType D3 = new DelegateType(Fibonacci);

            IAsyncResult IAR1 = D1.BeginInvoke(5, null, null);
            IAsyncResult IAR2 = D2.BeginInvoke(5, null, null);
            IAsyncResult IAR3 = D3.BeginInvoke(5, null, null);

            int wynik1 = D1.EndInvoke(IAR1);
            Console.WriteLine(wynik1);
            int wynik2 = D2.EndInvoke(IAR2);
            Console.WriteLine(wynik2);
            int wynik3 = D3.EndInvoke(IAR3);
            Console.WriteLine(wynik3);


        }
    }
}
