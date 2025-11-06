using System.Collections.Concurrent;
using System.Reflection;
using NUnit.Framework;

namespace ProducerConsumerPattern.Tests
{
    [TestFixture]
    public class ProducerConsumerPatternTests
    {
        [Test]
        public void ConcurrentQueue_EnqueueDequeue_WorksCorrectly()
        {
            // Arrange
            var queue = new ConcurrentQueue<int>();

            // Act
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            // Assert
            Assert.That(queue.Count, Is.EqualTo(3));
            Assert.That(queue.TryDequeue(out int result1), Is.True);
            Assert.That(result1, Is.EqualTo(1));
            Assert.That(queue.TryDequeue(out int result2), Is.True);
            Assert.That(result2, Is.EqualTo(2));
        }

        [Test]
        public void ConcurrentQueue_EmptyQueue_DequeueReturnsFalse()
        {
            // Arrange
            var queue = new ConcurrentQueue<int>();

            // Act
            var result = queue.TryDequeue(out int value);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(value, Is.EqualTo(0));
        }

        [Test]
        public void ConcurrentQueue_IsEmpty_ReturnsTrueWhenEmpty()
        {
            // Arrange
            var queue = new ConcurrentQueue<int>();

            // Act & Assert
            Assert.That(queue.IsEmpty, Is.True);

            queue.Enqueue(1);
            Assert.That(queue.IsEmpty, Is.False);

            queue.TryDequeue(out _);
            Assert.That(queue.IsEmpty, Is.True);
        }

        [Test]
        public void ConcurrentQueue_ThreadSafety_MultipleThreads()
        {
            // Arrange
            var queue = new ConcurrentQueue<int>();
            var tasks = new List<Task>();

            // Act - Multiple threads enqueueing
            for (int i = 0; i < 10; i++)
            {
                int value = i;
                tasks.Add(Task.Run(() => queue.Enqueue(value)));
            }

            Task.WaitAll(tasks.ToArray());

            // Assert
            Assert.That(queue.Count, Is.EqualTo(10));
        }

        [Test]
        public void ConcurrentQueue_ThreadSafety_ProducerConsumer()
        {
            // Arrange
            var queue = new ConcurrentQueue<int>();
            var producerTask = Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    queue.Enqueue(i);
                }
            });

            var consumerTask = Task.Run(() =>
            {
                int count = 0;
                while (count < 100)
                {
                    if (queue.TryDequeue(out _))
                    {
                        count++;
                    }
                }
            });

            // Act
            Task.WaitAll(producerTask, consumerTask);

            // Assert
            Assert.That(queue.IsEmpty, Is.True);
        }
    }
}

