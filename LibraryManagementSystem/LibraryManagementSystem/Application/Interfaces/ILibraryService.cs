using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface ILibraryService
    {
        void AddBook(Book book);
        bool RemoveBook(string isbn);
        List<Book> SearchByTitle(string title);
        List<Book> SearchByAuthor(string author);
        Book? SearchByISBN(string isbn);
        List<Book> GetAllBooks();
    }
}

