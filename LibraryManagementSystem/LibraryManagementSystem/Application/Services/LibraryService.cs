using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly List<Book> _books;

        public LibraryService()
        {
            _books = new List<Book>();
        }

        public void AddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Book title is required", nameof(book));

            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("Book author is required", nameof(book));

            if (string.IsNullOrWhiteSpace(book.ISBN))
                throw new ArgumentException("Book ISBN is required", nameof(book));

            // Check if ISBN already exists
            if (_books.Any(b => b.ISBN == book.ISBN))
                throw new InvalidOperationException($"A book with ISBN {book.ISBN} already exists");

            _books.Add(book);
        }

        public bool RemoveBook(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            var bookToRemove = _books.FirstOrDefault(b => b.ISBN == isbn);
            if (bookToRemove != null)
            {
                _books.Remove(bookToRemove);
                return true;
            }

            return false;
        }

        public List<Book> SearchByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return new List<Book>();

            return _books
                .Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Book> SearchByAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
                return new List<Book>();

            return _books
                .Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Book? SearchByISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return null;

            return _books.FirstOrDefault(b => b.ISBN == isbn);
        }

        public List<Book> GetAllBooks()
        {
            return _books.ToList();
        }
    }
}

