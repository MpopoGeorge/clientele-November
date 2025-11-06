using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static readonly Dictionary<int, BigInteger> FactorialResults = new Dictionary<int, BigInteger>();
    private static readonly object lockObject = new object();

    static void Main(string[] args)
    {
        Console.WriteLine("Multithreaded Factorial Calculator");

        List<int> numbers = new List<int> { 5, 10, 15, 20, 25, 30 };
        List<Thread> threads = new List<Thread>();

        Console.WriteLine("Starting factorial calculations on multiple threads...");

        foreach (int number in numbers)
        {
            Thread thread = new Thread(() => CalculateFactorial(number));
            threads.Add(thread);
            thread.Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("\nAll calculations complete. Results:");
        foreach (var kvp in FactorialResults)
        {
            Console.WriteLine($"Factorial of {kvp.Key} = {kvp.Value}");
        }

        Console.WriteLine("\nCalculating using Tasks:");
        CalculateUsingTasks(numbers);

        Console.WriteLine("\nCalculating using Parallel.ForEach:");
        CalculateUsingParallelForEach(numbers);

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static BigInteger CalculateFactorialValue(int n)
    {
        if (n <= 1)
            return 1;

        return n * CalculateFactorialValue(n - 1);
    }

    static void CalculateFactorial(int number)
    {
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} calculating factorial of {number}");

        BigInteger result = CalculateFactorialValue(number);

        lock (lockObject)
        {
            FactorialResults[number] = result;
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} completed: Factorial of {number} = {result}");
        }
    }

    static void CalculateUsingTasks(List<int> numbers)
    {
        Dictionary<int, BigInteger> taskResults = new Dictionary<int, BigInteger>();
        List<Task> tasks = new List<Task>(); 

        foreach (int number in numbers)
        {
            int n = number;
            tasks.Add(Task.Run(() =>
            {
                BigInteger result = CalculateFactorialValue(n);
                lock (lockObject)
                {
                    taskResults[n] = result;
                    Console.WriteLine($"Task calculated: Factorial of {n} = {result}");
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());
    }

    static void CalculateUsingParallelForEach(List<int> numbers)
    {
        Dictionary<int, BigInteger> parallelResults = new Dictionary<int, BigInteger>();

        Parallel.ForEach(numbers, number =>
        {
            BigInteger result = CalculateFactorialValue(number);
            lock (lockObject)
            {
                parallelResults[number] = result;
                Console.WriteLine($"Parallel.ForEach calculated: Factorial of {number} = {result}");
            }
        });
    }
}