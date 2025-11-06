using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Application.Services;
using LibraryManagementSystem.Domain.Entities;
using NUnit.Framework;

namespace LibraryManagementSystem.Tests
{
    [TestFixture]
    public class LibraryServiceTests
    {
        private ILibraryService _libraryService;

        [SetUp]
        public void Setup()
        {
            _libraryService = new LibraryService();
        }

        [Test]
        public void AddBook_ValidBook_AddsBookToLibrary()
        {
            // Arrange
            var book = new Book("Test Book", "Test Author", "1234567890", 2024);

            // Act
            _libraryService.AddBook(book);

            // Assert
            var allBooks = _libraryService.GetAllBooks();
            Assert.That(allBooks.Count, Is.EqualTo(1));
            Assert.That(allBooks[0].Title, Is.EqualTo("Test Book"));
        }

        [Test]
        public void AddBook_NullBook_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _libraryService.AddBook(null!));
        }

        [Test]
        public void AddBook_EmptyTitle_ThrowsArgumentException()
        {
            // Arrange
            var book = new Book("", "Author", "1234567890", 2024);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _libraryService.AddBook(book));
        }

        [Test]
        public void AddBook_DuplicateISBN_ThrowsInvalidOperationException()
        {
            // Arrange
            var book1 = new Book("Book 1", "Author 1", "1234567890", 2024);
            var book2 = new Book("Book 2", "Author 2", "1234567890", 2023);

            // Act
            _libraryService.AddBook(book1);

            // Assert
            Assert.Throws<InvalidOperationException>(() => _libraryService.AddBook(book2));
        }

        [Test]
        public void RemoveBook_ExistingISBN_ReturnsTrue()
        {
            // Arrange
            var book = new Book("Test Book", "Test Author", "1234567890", 2024);
            _libraryService.AddBook(book);

            // Act
            var result = _libraryService.RemoveBook("1234567890");

            // Assert
            Assert.That(result, Is.True);
            var allBooks = _libraryService.GetAllBooks();
            Assert.That(allBooks.Count, Is.EqualTo(0));
        }

        [Test]
        public void RemoveBook_NonExistingISBN_ReturnsFalse()
        {
            // Act
            var result = _libraryService.RemoveBook("9999999999");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void SearchByTitle_ExistingTitle_ReturnsMatchingBooks()
        {
            // Arrange
            _libraryService.AddBook(new Book("Python Programming", "Author 1", "1111111111", 2024));
            _libraryService.AddBook(new Book("Java Programming", "Author 2", "2222222222", 2024));
            _libraryService.AddBook(new Book("C# Programming", "Author 3", "3333333333", 2024));

            // Act
            var result = _libraryService.SearchByTitle("Python");

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("Python Programming"));
        }

        [Test]
        public void SearchByTitle_CaseInsensitive_ReturnsMatchingBooks()
        {
            // Arrange
            _libraryService.AddBook(new Book("Python Programming", "Author 1", "1111111111", 2024));

            // Act
            var result = _libraryService.SearchByTitle("python");

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void SearchByAuthor_ExistingAuthor_ReturnsMatchingBooks()
        {
            // Arrange
            _libraryService.AddBook(new Book("Book 1", "John Doe", "1111111111", 2024));
            _libraryService.AddBook(new Book("Book 2", "Jane Smith", "2222222222", 2024));
            _libraryService.AddBook(new Book("Book 3", "John Doe", "3333333333", 2024));

            // Act
            var result = _libraryService.SearchByAuthor("John Doe");

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void SearchByISBN_ExistingISBN_ReturnsBook()
        {
            // Arrange
            var book = new Book("Test Book", "Test Author", "1234567890", 2024);
            _libraryService.AddBook(book);

            // Act
            var result = _libraryService.SearchByISBN("1234567890");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Test Book"));
        }

        [Test]
        public void SearchByISBN_NonExistingISBN_ReturnsNull()
        {
            // Act
            var result = _libraryService.SearchByISBN("9999999999");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetAllBooks_ReturnsAllBooks()
        {
            // Arrange
            _libraryService.AddBook(new Book("Book 1", "Author 1", "1111111111", 2024));
            _libraryService.AddBook(new Book("Book 2", "Author 2", "2222222222", 2024));

            // Act
            var result = _libraryService.GetAllBooks();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void SearchByTitle_EmptyString_ReturnsEmptyList()
        {
            // Arrange
            _libraryService.AddBook(new Book("Book 1", "Author 1", "1111111111", 2024));

            // Act
            var result = _libraryService.SearchByTitle("");

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}

