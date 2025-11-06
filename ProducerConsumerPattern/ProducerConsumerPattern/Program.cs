using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class ProducerConsumerDemo
{
    private static ConcurrentQueue<int> numberQueue = new ConcurrentQueue<int>();
    private const int MaxQueueSize = 10;
    private static bool isRunning = true;
    private static Random random = new Random();
    private static object randomLock = new object();
    private static ManualResetEvent stopEvent = new ManualResetEvent(false);

    static void Main(string[] args)
    {
        Console.WriteLine("Producer-Consumer Pattern Demonstration");
        Console.WriteLine("--------------------------------------");

        Task[] producers = new Task[2];
        for (int i = 0; i < producers.Length; i++)
        {
            int producerId = i + 1;
            producers[i] = Task.Run(() => ProducerTask(producerId));
        }

        Task[] consumers = new Task[3];
        for (int i = 0; i < consumers.Length; i++)
        {
            int consumerId = i + 1;
            consumers[i] = Task.Run(() => ConsumerTask(consumerId));
        }

        Console.WriteLine("Press any key to stop...");
        Console.ReadKey();

        isRunning = false;
        stopEvent.Set();

        Task.WaitAll(producers);
        Task.WaitAll(consumers);

        Console.WriteLine("All producers and consumers have stopped.");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static void ProducerTask(int producerId)
    {
        Console.WriteLine($"Producer {producerId} started");

        while (!stopEvent.WaitOne(0)) 
        {
            int randomNumber;
            lock (randomLock)
            {
                randomNumber = random.Next(1, 101);
            }

            numberQueue.Enqueue(randomNumber);

            Console.WriteLine($"Producer {producerId} added: {randomNumber} (Queue size: {numberQueue.Count})");

            if (numberQueue.Count >= MaxQueueSize)
            {
                Console.WriteLine($"Queue is full. Producer {producerId} waiting...");
                Thread.Sleep(500); 
            }
            else
            {
                Thread.Sleep(100); 
            }
        }

        Console.WriteLine($"Producer {producerId} finished");
    }

    static void ConsumerTask(int consumerId)
    {
        Console.WriteLine($"Consumer {consumerId} started");

        while (!stopEvent.WaitOne(0) || !numberQueue.IsEmpty) 
        {
            if (numberQueue.TryDequeue(out int number))
            {
                Console.WriteLine($"Consumer {consumerId} consumed: {number} (Queue size: {numberQueue.Count})");

                Thread.Sleep(200);
            }
            else
            {
                Thread.Sleep(50);
            }
        }

        while (numberQueue.TryDequeue(out int number))
        {
            Console.WriteLine($"Consumer {consumerId} consumed (cleanup): {number}");
        }

        Console.WriteLine($"Consumer {consumerId} finished");
    }
}
