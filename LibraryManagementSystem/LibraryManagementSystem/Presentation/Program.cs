using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Application.Services;
using LibraryManagementSystem.Domain.Entities;

ILibraryService libraryService = new LibraryService();

try
{
    libraryService.AddBook(new Book("Artificial Intelligence and Online Engineering", 
        "Michael E. Auer, Samir A. El-Seoud, and Omar H. Karam", "9783031170904", 1965));
    libraryService.AddBook(new Book("Video Game Level Design", 
        "Michael Salmond", "9781350015722", 1966));
    libraryService.AddBook(new Book("Generative Deep Learning", 
        "David Foster", "9781098134181", 1979));
}
catch (Exception ex)
{
    Console.WriteLine($"Error adding sample books: {ex.Message}");
}

bool running = true;

while (running)
{
    Console.WriteLine("\n===== Library Management System =====");
    Console.WriteLine("1. Add a new book");
    Console.WriteLine("2. Remove a book");
    Console.WriteLine("3. Search by title");
    Console.WriteLine("4. Search by author");
    Console.WriteLine("5. Search by ISBN");
    Console.WriteLine("6. Display all books");
    Console.WriteLine("7. Exit");
    Console.Write("Enter your choice (1-7): ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            try
            {
                Console.Write("Enter title: ");
                string? title = Console.ReadLine();
                Console.Write("Enter author: ");
                string? author = Console.ReadLine();
                Console.Write("Enter ISBN: ");
                string? isbn = Console.ReadLine();
                Console.Write("Enter publication year: ");
                if (int.TryParse(Console.ReadLine(), out int year))
                {
                    var book = new Book(title ?? "", author ?? "", isbn ?? "", year);
                    libraryService.AddBook(book);
                    Console.WriteLine($"Book added: {book.Title}");
                }
                else
                {
                    Console.WriteLine("Invalid year format.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            break;

        case "2":
            Console.Write("Enter ISBN of the book to remove: ");
            string? removeIsbn = Console.ReadLine();
            if (libraryService.RemoveBook(removeIsbn ?? ""))
            {
                Console.WriteLine("Book removed successfully.");
            }
            else
            {
                Console.WriteLine($"Book with ISBN {removeIsbn} not found.");
            }
            break;

        case "3":
            Console.Write("Enter title to search: ");
            string? searchTitle = Console.ReadLine();
            var booksByTitle = libraryService.SearchByTitle(searchTitle ?? "");
            DisplayBooks(booksByTitle);
            break;

        case "4":
            Console.Write("Enter author name to search: ");
            string? searchAuthor = Console.ReadLine();
            var booksByAuthor = libraryService.SearchByAuthor(searchAuthor ?? "");
            DisplayBooks(booksByAuthor);
            break;

        case "5":
            Console.Write("Enter ISBN to search: ");
            string? searchIsbn = Console.ReadLine();
            var bookByISBN = libraryService.SearchByISBN(searchIsbn ?? "");
            if (bookByISBN != null)
            {
                Console.WriteLine(bookByISBN);
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
            break;

        case "6":
            var allBooks = libraryService.GetAllBooks();
            DisplayBooks(allBooks);
            break;

        case "7":
            running = false;
            Console.WriteLine("Thank you for using the Library Management System!");
            break;

        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}

static void DisplayBooks(List<Book> books)
{
    if (books.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    Console.WriteLine("\nBooks found:");
    foreach (var book in books)
    {
        Console.WriteLine(book);
    }
}

