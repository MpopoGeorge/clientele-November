using System.Numerics;
using System.Reflection;
using NUnit.Framework;

namespace FactorialCalculator.Tests
{
    [TestFixture]
    public class FactorialCalculatorTests
    {
        private static BigInteger CalculateFactorialValue(int n)
        {
            if (n <= 1)
                return 1;

            return n * CalculateFactorialValue(n - 1);
        }

        [Test]
        public void CalculateFactorialValue_Zero_ReturnsOne()
        {
            // Act
            var result = CalculateFactorialValue(0);

            // Assert
            Assert.That(result, Is.EqualTo(BigInteger.One));
        }

        [Test]
        public void CalculateFactorialValue_One_ReturnsOne()
        {
            // Act
            var result = CalculateFactorialValue(1);

            // Assert
            Assert.That(result, Is.EqualTo(BigInteger.One));
        }

        [Test]
        public void CalculateFactorialValue_Five_Returns120()
        {
            // Act
            var result = CalculateFactorialValue(5);

            // Assert
            Assert.That(result, Is.EqualTo(new BigInteger(120)));
        }

        [Test]
        public void CalculateFactorialValue_Ten_ReturnsCorrectValue()
        {
            // Act
            var result = CalculateFactorialValue(10);

            // Assert
            Assert.That(result, Is.EqualTo(new BigInteger(3628800)));
        }

        [Test]
        public void CalculateFactorialValue_LargeNumber_ReturnsCorrectValue()
        {
            // Act
            var result = CalculateFactorialValue(20);

            // Assert
            Assert.That(result, Is.GreaterThan(BigInteger.Zero));
            // 20! = 2432902008176640000
            Assert.That(result, Is.EqualTo(new BigInteger(2432902008176640000)));
        }

        [Test]
        public void CalculateFactorialValue_NegativeNumber_ReturnsOne()
        {
            // Act
            var result = CalculateFactorialValue(-5);

            // Assert
            Assert.That(result, Is.EqualTo(BigInteger.One));
        }

        [Test]
        public void CalculateFactorialValue_ThreadSafety_ConcurrentCalculations()
        {
            // Arrange
            var results = new Dictionary<int, BigInteger>();
            var tasks = new List<Task>();
            var lockObject = new object();

            // Act - Calculate factorials concurrently
            for (int i = 0; i < 10; i++)
            {
                int number = i + 1;
                tasks.Add(Task.Run(() =>
                {
                    var result = CalculateFactorialValue(number);
                    lock (lockObject)
                    {
                        results[number] = result;
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            // Assert
            Assert.That(results.Count, Is.EqualTo(10));
            Assert.That(results[1], Is.EqualTo(BigInteger.One)); // 1! = 1
            Assert.That(results[2], Is.EqualTo(new BigInteger(2))); // 2! = 2
            Assert.That(results[3], Is.EqualTo(new BigInteger(6))); // 3! = 6
            Assert.That(results[5], Is.EqualTo(new BigInteger(120))); // 5! = 120
        }
    }
}

