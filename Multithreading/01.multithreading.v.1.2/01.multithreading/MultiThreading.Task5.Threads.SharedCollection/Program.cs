/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> numbers = new List<int>();

        private static ManualResetEvent eventObj = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();
           
            for (int i = 0; i < 10; i++)
            {
                var firstTask = new Task(AddElements);
                var secondTask = new Task(PrintElements);
               
                firstTask.Start();
                secondTask.Start();
                secondTask.Wait();
            }

            Console.ReadLine();
        }

        private static void AddElements()
        {
            numbers.Add(numbers.Count + 1);
            eventObj.Set();
        }

        private static void PrintElements()
        {
            eventObj.WaitOne();
            foreach (var item in numbers)
            {
                Console.Write(item + " ");
            }
            
            Console.WriteLine();
            eventObj.Reset();
        }
    }
}
