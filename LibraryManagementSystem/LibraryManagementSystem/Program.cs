using LibraryManagementSystem;
using Library = LibraryManagementSystem.Library;

Library library = new Library();
bool running = true;

// Add some sample books
library.AddBook(new Book("Artificial Intelligence and Online Engineering", "Michael E. Auer, Samir A. El-Seoud, and Omar H. Karam", "9783031170904", 1965));
library.AddBook(new Book("Video Game Level Design", "Michael Salmond", "9781350015722", 1966));
library.AddBook(new Book("Generative Deep Learning", "David Foster", "9781098134181", 1979));

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

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1": // Add a book
            Console.Write("Enter title: ");
            string title = Console.ReadLine();
            Console.Write("Enter author: ");
            string author = Console.ReadLine();
            Console.Write("Enter ISBN: ");
            string isbn = Console.ReadLine();
            Console.Write("Enter publication year: ");
            if (int.TryParse(Console.ReadLine(), out int year))
            {
                library.AddBook(new Book(title, author, isbn, year));
            }
            else
            {
                Console.WriteLine("Invalid year format.");
            }
            break;

        case "2": // Remove a book
            Console.Write("Enter ISBN of the book to remove: ");
            library.RemoveBook(Console.ReadLine());
            break;

        case "3": // Search by title
            Console.Write("Enter title to search: ");
            var booksByTitle = library.SearchByTitle(Console.ReadLine());
            DisplayBooks(booksByTitle);
            break;

        case "4": // Search by author
            Console.Write("Enter author name to search: ");
            var booksByAuthor = library.SearchByAuthor(Console.ReadLine());
            DisplayBooks(booksByAuthor);
            break;

        case "5": // Search by ISBN
            Console.Write("Enter ISBN to search: ");
            var bookByISBN = library.SearchByISBN(Console.ReadLine());
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
            var allBooks = library.GetAllBooks();
            DisplayBooks(allBooks);
            break;

        case "7":
            running = false;
            Console.WriteLine("Thank you for using the Mpopo Library Management System!");
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
