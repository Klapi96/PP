﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3zad13
{
    class Program
    {
        private bool x = false;

        public bool Z2 { get; set; }

        public async void Zadanie2()
        {
            //ZADANIE 2. ODKOMENTUJ I POPRAW

            await Task.Run(
                () =>
                {
                    Z2 = true;
                });

        }

        static void Main(string[] args)
        {
        }
    }
}
