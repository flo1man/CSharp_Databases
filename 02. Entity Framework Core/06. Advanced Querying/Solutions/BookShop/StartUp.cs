namespace BookShop
{
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            DbInitializer.ResetDatabase(context);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context.Books
                .Select(x => new { x.Title, x.AgeRestriction })
                .ToList()
                .Where(x => x.AgeRestriction.ToString().ToLower() == command.ToLower())
                .OrderBy(x => x.Title)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Select(x => new { x.Title, x.BookId, x.Copies, x.EditionType })
                .ToList()
                .Where(x => x.Copies < 5000 && x.EditionType.ToString() == "Gold")
                .OrderBy(x => x.BookId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Price > 40)
                .Select(x => new { x.Title, x.Price })
                .OrderByDescending(x => x.Price)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .Select(x => new { x.Title, x.BookId })
                .OrderBy(x => x.BookId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Where(x => x.BookCategories.Any(x => categories.Contains(x.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var targetDate = DateTime
                .ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(x => x.ReleaseDate.Value < targetDate)
                .Select(x => new
                {
                    x.Title,
                    x.Price,
                    x.ReleaseDate,
                    x.EditionType
                })
                .OrderByDescending(x => x.ReleaseDate)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            //r = x.ReleaseDate.Value.ToShortDateString(),
            //s = Convert.ToDateTime(date).ToShortDateString()

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Select(x => new
                {
                    x.Author.FirstName,
                    x.Author.LastName,
                    x.Title,
                    x.BookId
                })
                .Where(x => x.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(x => x.BookId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FirstName} {book.LastName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksCount = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .Count();

            return booksCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context
                .Authors
                .Select(a => new
                {
                    BooksCount = a.Books.Sum(b => b.Copies),
                    FullName = a.FirstName + " " + a.LastName
                })
                .OrderByDescending(a => a.BooksCount)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in authors)
            {
                sb.AppendLine($"{item.FullName} - {item.BooksCount}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Categories
                .Select(x => new
                {
                    CategoryName = x.Name,
                    Profit = x.CategoryBooks.Sum(x => x.Book.Price * x.Book.Copies)
                })
                .OrderByDescending(x => x.Profit)
                .ThenBy(x => x.CategoryName)
                .ToList();

            StringBuilder sb = new StringBuilder();


            foreach (var item in books)
            {
                Console.WriteLine($"{item.CategoryName} ${item.Profit:f2}");
            }

            return sb.ToString().TrimEnd(); ;
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories
                .Select(x => new
                {
                    x.Name,
                    CategoryBooks = x.CategoryBooks.Select(x => new
                    {
                        Title = x.Book.Title,
                        Date = x.Book.ReleaseDate
                    })
                    .OrderByDescending(x => x.Date)
                    .Take(3)
                    .ToList()
                })
                .OrderBy(x => x.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"--{book.Name}");

                foreach (var item in book.CategoryBooks)
                {
                    sb.AppendLine($"{item.Title} ({item.Date.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            //var books = context.Books
            //    .Where(x => x.ReleaseDate.Value.Year < 2010)
            //    .Update(x => new Book() { Price = x.Price + 5});

            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();

            for (int i = 0; i < books.Count; i++)
            {
                books[i].Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Copies < 4200)
                .ToList();

            foreach (var book in books)
            {
                context.Books.Remove(book);
            }

            context.SaveChanges();

            return books.Count;
        }
    }
}
