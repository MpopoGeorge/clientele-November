using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Library
    {
        private List<Book> books;

        public Library()
        {
            books = new List<Book>();
        }

        // Add a book to the library
        public void AddBook(Book book)
        {
            books.Add(book);
            Console.WriteLine($"Book added: {book.Title}");
        }

        // Remove a book by ISBN
        public bool RemoveBook(string isbn)
        {
            Book bookToRemove = books.FirstOrDefault(b => b.ISBN == isbn);
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                Console.WriteLine($"Book removed: {bookToRemove.Title}");
                return true;
            }
            Console.WriteLine($"Book with ISBN {isbn} not found.");
            return false;
        }

        // Search books by title (partial match)
        public List<Book> SearchByTitle(string title)
        {
            var result = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            return result;
        }

        // Search books by author (partial match)
        public List<Book> SearchByAuthor(string author)
        {
            var result = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
            return result;
        }

        // Search a book by ISBN (exact match)
        public Book SearchByISBN(string isbn)
        {
            return books.FirstOrDefault(b => b.ISBN == isbn);
        }

        // Get all books in the library
        public List<Book> GetAllBooks()
        {
            return books;
        }
    }
}
