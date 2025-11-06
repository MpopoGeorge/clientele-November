using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Book(string title, string author, string isbn, int publicationYear)
    {
        public string Title { get; set; } = title;
        public string Author { get; set; } = author;
        public string ISBN { get; set; } = isbn;
        public int PublicationYear { get; set; } = publicationYear;

        public override string ToString()
        {
            return $"Title: {Title}, Author: {Author}, ISBN: {ISBN}, Year: {PublicationYear}";
        }
    }
}
