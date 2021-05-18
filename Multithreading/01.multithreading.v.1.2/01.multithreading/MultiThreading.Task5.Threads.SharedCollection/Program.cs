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

        private static object locker = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();
           
            for (int i = 0; i < 10; i++)
            {
                var firstTask = new Task(AddElements);
                var secondTask = firstTask.ContinueWith(task => PrintElements());
               
                firstTask.Start();
                secondTask.Wait();
            }

            Console.ReadLine();
        }

        private static void AddElements()
        {
            lock (locker)
            {
                numbers.Add(numbers.Count + 1);
            }
        }

        private static void PrintElements()
        {
            lock (locker)
            {
                foreach (var item in numbers)
                {
                    Console.Write(item + " ");
                }

                Console.WriteLine();
            }
        }
    }
}
