using System;
using System.Collections.Generic;
using System.IO;

namespace Write.Books
{
    class Program
    {
        static void Main()
        {
            new LibraryBook();
        }
    }
    class Book
    {
        private int isbn { get; set; }
        private int quantity;
        private string author { get; set; }
        private string title { get; set; }

        public Book(int ISBN, string Author, string Title, int Quantity)
        {
            this.isbn = ISBN;
            this.author = Author;
            this.title = Title;
            this.quantity = Quantity;
        }
        public string Author
        {
            get => author;
            set => author = value;
        }
        public int Quantity
        {
            get => quantity;
            set => quantity = value;
        }
        public string Title
        {
            get => title;
            set => title = value;
        }
        public int ISBN
        {
            get => isbn;
            set => isbn = value;
        }
        public string ToSaveString()
        {
            return $"{ISBN},{Author},{Title},{Quantity}";
        }
    }

    //I created a book with key ISBN and vale TitleAuthor which meant I could add individual entries to the dictionary as 
    // I chose to use a dictionary with a <key, value> as I wasn't able to use a class in the dictionary.
    class LibraryBook
    {
        readonly SortedDictionary<string, Book> books = new SortedDictionary<string, Book>();
        const string filepath = "book.txt";
        public LibraryBook()
        {
            Menu();
        }
        private void Menu()
        {
            Load();
            string opt;
            do
            {
                Console.Clear();
                Console.WriteLine("Library Menu\n");

                Console.WriteLine("AB. Add Boook.");
                Console.WriteLine("EB. Edit Book.");
                Console.WriteLine("DB. Delete Book.\n");
                Console.WriteLine("SE. Search Catalogue.");
                Console.WriteLine("SO. Show Sorted Catalogue.\n");
                Console.WriteLine("LS. Loan Status.");
                Console.WriteLine("TL. Take Out Loan");
                Console.WriteLine("RL. Return Loan\n");

                Console.WriteLine("X. Exit\n");

                Console.WriteLine("Enter the option you require.");

                opt = Console.ReadLine();

                switch (opt.ToUpper())
                {
                    case "AB":
                        AddBook();
                        break;
                    case "EB":
                        Edit();
                        break;
                    case "DB":
                        Remove();
                        break;
                    case "SE":
                        Search();
                        break;
                    case "SO":
                        SortBooks();
                        break;
                    case "LS":
                        LoanStatus();
                        break;
                    case "TL":
                        Loan();
                        break;
                    case "RL":
                        Return();
                        break;
                }

            } while (opt.ToUpper() != "X");
            Save();
        }
        public void AddBook()
        {
            //Add an item that sets the loan status of the book when it is added and make this the key.

            Console.WriteLine("Who is the author?");
            string Author = Console.ReadLine();
            // The bug fixes for this is when the input is not in the correct format.

            Console.WriteLine("What is the ISBN?");
            int ISBN = Convert.ToInt32(Console.ReadLine());
            // The bug fixes for this is when the input is not in the correct format.

            Console.WriteLine("What is the Title?");
            string Title = Console.ReadLine();

            Console.WriteLine("How many is there?");
            int Quantity = Convert.ToInt32(Console.ReadLine());

            // The bug fixes for this is when the input is not in the correct format.



            books[Title] = new Book(ISBN, Author, Title, Quantity);
            Console.Clear();
            return;
        }
        public void Loan()
        {
            ListBooks();
            Console.WriteLine("What is the tile of the book that you would like to borrow:   ");
            string x = Console.ReadLine();
            if (books.ContainsKey(x))
            {
                Console.WriteLine("Is this the book you would like to borrow [Y/N]:   {0} {1} {2}", books[x].ISBN, books[x].Title, books[x].Author);
                string y = Console.ReadLine();
                if (y == "Y")
                {
                    books[x].Quantity = 0;
                    Console.WriteLine("The book:   {0}, by    {1}, is now out on loan.", books[x].Title, books[x].Author);
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
            }
        }
        public void Return()
        {
            Console.WriteLine("What is the tile of the book that you would like to return:   ");
            string x = Console.ReadLine();
            if (books.ContainsKey(x))
            {
                Console.WriteLine("Is this the book you would like to return [Y/N]:   {0} {1} {2}", books[x].ISBN, books[x].Title, books[x].Author);
                string y = Console.ReadLine();
                if (y == "Y")
                {
                    books[x].Quantity = 1;
                    Console.WriteLine("The book:   {0}, by    {1}, has now been returned.", books[x].Title, books[x].Author);
                    Console.Clear();
                    return;
                }
            }
        }
        public void Edit()
        {
            ListBooks();
            Console.WriteLine("\nEnter the name of the book to edit.");
            string x = Console.ReadLine();
            if (books.ContainsKey(x))
            {
                Console.WriteLine("What is the ISBN?");
                int y = Convert.ToInt32(Console.ReadLine());
                books[x].ISBN = y;

                Console.WriteLine("Who is the Author?");
                string a = Console.ReadLine();
                books[x].Author = a;

                Console.WriteLine("What is the Title?");
                string b = Console.ReadLine();
                books[x].Title = b;
                Console.Clear();
            }
        }
        public void Remove()
        {
            Console.Clear();
            ListBooks();
            Console.WriteLine("What is the title of the book you would to remove:   \n");
            string x = Console.ReadLine();
                
                if (books.ContainsKey(x))
                {
                    Console.WriteLine("Is this the book you would like to delete [Y/N]: {0} {1} {2}\n", books[x].ISBN, books[x].Title, books[x].Author);
                    string y = Console.ReadLine();
                    if (y == "Y")
                    {
                        books.Remove(x);
                    }
                }
            else { Console.WriteLine("This book is not in the library."); };
            Console.Clear();
            return;
        }
        public void LoanStatus()
        {
            Console.Clear();
            Console.WriteLine("BOOKS ON LOAN\n");
            foreach (Book b in books.Values)
            {
                if (b.Quantity == 0)
                {
                    Console.WriteLine("Title:   " + b.Title + "   Author:    " + b.Author + "   ISBN:   " + b.ISBN);
                }
            }
            Console.ReadKey(true);
            Console.Clear();
        }
        public void Save()
        {
            using StreamWriter sw = new StreamWriter(filepath);
            foreach (Book b in books.Values)
            {
                sw.WriteLine(b.ToSaveString());
            }
        }
        public void Load()
        {
            if (File.Exists(filepath))
            {
                using StreamReader sr = new StreamReader(filepath);
                {
                    while (!sr.EndOfStream)
                    //This was neccessary because the books to add would of been otherwise saved over the streamreader
                    {
                        string line = sr.ReadLine();
                        string[] data = line.Split(',');
                        Book b = new Book(Convert.ToInt32(data[0]), data[1], data[2], Convert.ToInt32(data[3]));
                        books[b.Title] = b;
                    }
                }
            }
        }
        public void Search()
        {
            Console.Clear();
            Console.WriteLine("Enter the title of the book you would like to search for:   ");
            string y = Console.ReadLine();
            SearchName(y);
            Console.WriteLine();
            Console.ReadKey();
        }
        void SearchName(string y)
        {
            Book[] bookstosearch = new Book[books.Values.Count];
            books.Values.CopyTo(bookstosearch, 0);
            for (int n = 0; n < bookstosearch.Length; n++)
            {
                if (bookstosearch[n].Title == y)
                {
                    Console.WriteLine("There is book in the catalogue called {0} by {1} and it's availability is {2}.", bookstosearch[n].Title, bookstosearch[n].Author, bookstosearch[n].Quantity);
                }
            }
        }
        public Book[] SortBooks()
        {
            Book[] bookarray = new Book[books.Values.Count];
            books.Values.CopyTo(bookarray, 0);
            List<Book> booklist = new List<Book>(bookarray);
            booklist.Sort((a, b) => { return a.Title.CompareTo(b.Title); });
            Console.WriteLine("Sorted by title.\n");
            foreach ( Book book in bookarray)
            {
                Console.WriteLine("Title: " + book.Title + "    Author: " + book.Author + "   ISBN: " + book.ISBN + "    Quantity: " + book.Quantity);
            }
            Console.ReadKey(true);
            Console.Clear();
            return booklist.ToArray();
        }
        public Book[] ListBooks()
        {
            Book[] bookarray = new Book[books.Values.Count];
            books.Values.CopyTo(bookarray, 0);
            List<Book> booklist = new List<Book>(bookarray);
            booklist.Sort((a, b) => { return a.Title.CompareTo(b.Title); });
            Console.WriteLine("Sorted by title.");
            foreach (Book book in bookarray)
            {
                Console.WriteLine("Title: " + book.Title + "    Author: " + book.Author + "   ISBN: " + book.ISBN + "    Quantity: " + book.Quantity);
            }
            return booklist.ToArray();
        }
    }
}