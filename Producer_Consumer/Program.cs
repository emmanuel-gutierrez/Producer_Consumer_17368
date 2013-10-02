using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Producer_Consumer
{
    class Program
    {
        static string[] buffer = new string[20];
        static Semaphore mutex = new Semaphore(1, 1);
        static Semaphore empty = new Semaphore(20, 20);
        static Semaphore full = new Semaphore(0, 20);
        static int valorItem = 0;
        static int productosEnBuffer = 0;

        static void Main(string[] args)
        {
            Thread P = new Thread(Producer);
            Thread C = new Thread(Consumer);
            P.Start();
            C.Start();
        }

        static void Producer()
        {
            while (true)
            {
                valorItem++;
                string item = "Se produjo el articulo #" + valorItem;
                empty.WaitOne();
                mutex.WaitOne();
                buffer[productosEnBuffer] = item;
                Console.WriteLine(buffer[productosEnBuffer]);
                productosEnBuffer++;
                mutex.Release();
                full.Release();
                Thread.Sleep(250);
            }
        }

        static void Consumer()
        { 
            while (true)
            {
                full.WaitOne();
                mutex.WaitOne();
                buffer[productosEnBuffer] = "Este espacio esta vacio";
                Console.WriteLine(buffer[productosEnBuffer]);
                productosEnBuffer--;
                mutex.Release();
                empty.Release();
                Thread.Sleep(250);
            }
        }
    }
}
