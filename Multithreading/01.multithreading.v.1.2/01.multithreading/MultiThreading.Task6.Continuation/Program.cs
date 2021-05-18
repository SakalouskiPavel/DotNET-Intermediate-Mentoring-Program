/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // Criteria "a"

            Console.WriteLine("a:");

            var initialTask = new Task(ExecuteInitialTask);

            var continuationTask = initialTask
                .ContinueWith(task => ExecuteRegardlessResult(), TaskContinuationOptions.None);

            initialTask.Start();
            continuationTask.Wait();

            // Criteria "b"

            Console.WriteLine("b:");

            initialTask = new Task(ExecuteInitialTask);

            continuationTask = initialTask
                .ContinueWith(task => ExecuteWithoutSuccessResult(), TaskContinuationOptions.OnlyOnFaulted);

            initialTask.Start();
            continuationTask.Wait();

            // Criteria "c"

            Console.WriteLine("c:");
            initialTask = new Task(ExecuteInitialTask);

            continuationTask = initialTask
                .ContinueWith(task => ExecuteWithFailResultUsingTheSameThread(), TaskContinuationOptions.ExecuteSynchronously);

            initialTask.Start();
            continuationTask.Wait();

            // Criteria "d"

            Console.WriteLine("d:");

            var cancelTokenSource = new CancellationTokenSource();
            var cancellationToken = cancelTokenSource.Token;
            var scheduler = TaskScheduler.Default;

            initialTask = Task.Run(() =>
            {
                ExecuteToCancel();
                cancellationToken.ThrowIfCancellationRequested();
            }, cancellationToken);

            initialTask.ContinueWith(task => ExecuteOnCancelResult(), new CancellationToken(), TaskContinuationOptions.OnlyOnCanceled, scheduler);

            cancelTokenSource.Cancel();

            Console.ReadLine();
        }

        /// <summary>
        /// This task is used as parent task for criteria a, b and c.
        /// </summary>
        private static void ExecuteInitialTask()
        {
            Console.WriteLine("This is initial task");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
            throw new Exception();
        }

        /// <summary>
        /// The task that should be canceled for criteria d.
        /// </summary>
        private static void ExecuteToCancel()
        {
            Console.WriteLine("This task should be canceled.");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
        }

        /// <summary>
        /// This is a task which should be executed for criteria a.
        /// </summary>
        private static void ExecuteRegardlessResult()
        {
            Console.WriteLine("This task should be executed regardless of the result of the parent task.");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
        }

        /// <summary>
        /// This is a task which should be executed for criteria b.
        /// </summary>
        private static void ExecuteWithoutSuccessResult()
        {
            Console.WriteLine("This task should be executed when the parent task finished without success.");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
        }

        /// <summary>
        /// This is a task which should be executed for criteria c.
        /// </summary>
        private static void ExecuteWithFailResultUsingTheSameThread()
        {
            Console.WriteLine("This task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
        }

        /// <summary>
        /// This is a task which should be executed for criteria d.
        /// </summary>
        private static void ExecuteOnCancelResult()
        {
            Console.WriteLine("This task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
        }
    }
}
