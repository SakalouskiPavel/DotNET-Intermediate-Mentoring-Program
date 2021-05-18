/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        private static int[] integerArray;

        private static readonly Random randomizer = new Random(Int32.MaxValue);

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var fillingTask = new Task(FillArray);
            var taskChain = fillingTask.ContinueWith(task => MultiplyArray()).ContinueWith(task => SortArray()).ContinueWith(task => CalculateAverageOfArray());

            fillingTask.Start();

            Console.ReadLine();
        }

        private static void FillArray()
        {
            Print("Filling array");

            integerArray = new int[10];

            for (int i = 0; i < integerArray.Length; i++)
            {
                integerArray[i] = randomizer.Next();
                Print(string.Format("Element # {0} - {1}", i, integerArray[i]));
            }
        }

        private static void MultiplyArray()
        {
            var multiplier = randomizer.Next();
            Print(string.Format("Multiplying array with {0}", multiplier));
            for (int i = 0; i < integerArray.Length; i++)
            {
                integerArray[i] *= multiplier;
                Print(string.Format("Element # {0} - {1}", i, integerArray[i]));
            }
        }

        private static void SortArray()
        {
            Print("Sorting array");

            integerArray = integerArray.OrderBy(value => value).ToArray();

            for (int i = 0; i < integerArray.Length; i++)
            {
                Print(string.Format("Element # {0} - {1}", i, integerArray[i]));
            }
        }

        private static void CalculateAverageOfArray()
        {
            Print("Calculating average of the array");
            int sum = 0;

            for (int i = 0; i < integerArray.Length; i++)
            {
                sum += integerArray[i];
            }

            var result = sum / integerArray.Length;
            Print(string.Format("Average of the array is {0}", result));
        }

        private static void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
