
# Producer-Consumer Pattern Demo

This is a simple demonstration of the Producer-Consumer pattern in C# using multiple producer and consumer tasks. The producers generate random numbers and add them to a thread-safe queue, while the consumers dequeue and process these numbers. The program demonstrates how to manage concurrent access to shared resources and gracefully stop tasks using synchronization primitives.

## Features:
- Multiple producers and consumers run concurrently.
- Producers generate random numbers and add them to a `ConcurrentQueue`.
- Consumers consume numbers from the queue and process them.
- The queue has a maximum size, with producers pausing if the queue gets too large.
- The program allows a graceful shutdown using a `ManualResetEvent` to signal when to stop.

## Prerequisites:
- .NET 5.0 or higher.
- A C# development environment (Visual Studio, Visual Studio Code, or any IDE of your choice).

## How to Run:

1. Clone or download this repository to your local machine.
2. Open the solution in Visual Studio or your preferred IDE.
3. Build and run the application.
4. The console will show the Producer-Consumer demonstration. Press any key to stop the program.

```bash
git clone https://github.com/your-repository/producer-consumer-demo.git
cd producer-consumer-demo
dotnet run
```

## Program Workflow:

1. **Producers**:
   - Each producer generates a random number between 1 and 100.
   - The number is enqueued into the `ConcurrentQueue`.
   - Producers pause if the queue reaches its maximum size (`MaxQueueSize`).

2. **Consumers**:
   - Each consumer dequeues a number and simulates processing it.
   - If the queue is empty, the consumer waits for a short time before checking again.
   - Consumers continue until the stop signal is triggered and the queue is empty.

3. **Graceful Shutdown**:
   - Press any key to signal the program to stop.
   - All producers and consumers gracefully finish their tasks and then the program exits.

## Code Structure:
- **ProducerConsumerPattern**: The main class that handles the producer-consumer logic.
- **ProducerTask**: The task that produces random numbers and enqueues them to the queue.
- **ConsumerTask**: The task that consumes numbers from the queue and processes them.
- **ManualResetEvent**: Used to signal the stop event and allow all tasks to terminate.

## Synchronization:
- **ConcurrentQueue**: A thread-safe queue that ensures correct concurrent access.
- **ManualResetEvent**: Signals when to stop the tasks, ensuring that all tasks can stop gracefully.
- **Lock on Random**: Ensures thread-safe generation of random numbers.

## Sample Output:

```text
Producer-Consumer Pattern Demonstration
--------------------------------------
Producer 1 started
Producer 2 started
Consumer 1 started
Consumer 2 started
Consumer 3 started
Producer 1 added: 58 (Queue size: 1)
Producer 2 added: 77 (Queue size: 2)
Consumer 1 consumed: 58 (Queue size: 1)
Producer 1 added: 23 (Queue size: 2)
...
Press any key to stop...
All producers and consumers have stopped.
Press any key to exit...
```

## License:
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
