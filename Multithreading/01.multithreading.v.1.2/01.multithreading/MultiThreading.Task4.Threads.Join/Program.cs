/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static readonly int threadsCount = 10;

        private static Semaphore semaphore = new Semaphore(10, 10);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            Console.WriteLine("Option a:");
            var thread = new Thread(ThreadProc);
            thread.Start(threadsCount);
            thread.Join();

            Console.WriteLine("Option b:");
            ThreadPool.QueueUserWorkItem(ThreadPoolProc, threadsCount);
            

            Console.ReadLine();
        }

        private static void ThreadProc(object value)
        {
            var integerValue = 0;
            if (!(value is int))
            {
                throw new ArgumentException();
            }

            integerValue = (int)value;

            if (integerValue > 0)
            {
                Print(integerValue.ToString());
                integerValue--;
                var thread = new Thread(ThreadProc);
                thread.Start(integerValue);
                thread.Join();
            }
        }

        private static void ThreadPoolProc(object value)
        {
            var integerValue = 0;
            if (!(value is int))
            {
                throw new ArgumentException();
            }

            integerValue = (int)value;

            if (integerValue > 0)
            {
                semaphore.WaitOne();
                Print(integerValue.ToString());
                integerValue--;
                ThreadPool.QueueUserWorkItem(ThreadPoolProc, integerValue);
                semaphore.Release();
            }
        }

        private static void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
