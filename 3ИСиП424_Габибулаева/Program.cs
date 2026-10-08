using System;
using System.Collections.Generic;
using System.Linq;

namespace CompactLibrary
{
    public enum Genre { Fiction = 1, Science, History, Fantasy }

    public class Book
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; }
        public string Author { get; }
        public Genre Genre { get; }
        public int Year { get; }
        public decimal Price { get; }

        public Book(string t, string a, Genre g, int y, decimal p) =>
            (Title, Author, Genre, Year, Price) =
            (!string.IsNullOrWhiteSpace(t) ? t : throw new Exception(), !string.IsNullOrWhiteSpace(a) ? a : throw new Exception(), g, y >= 0 ? y : throw new Exception(), p >= 0 ? p : throw new Exception());

        public override string ToString() => $"[ID: {Id}] \"{Title}\" — {Author} | Жанр: {Genre} | Год: {Year} | Цена: {Price:C}";
    }

    class Program
    {
        static List<Book> books = new List<Book>
        {
            new Book("Властелин Колец", "Толкин", Genre.Fantasy, 1954, 1500),
            new Book("Краткая история времени", "Хокинг", Genre.Science, 1988, 850),
            new Book("Преступление и наказание", "Достоевский", Genre.Fiction, 1866, 600),
            new Book("Хоббит", "Толкин", Genre.Fantasy, 1937, 1100),
            new Book("История Древнего Рима", "Моммзен", Genre.History, 1856, 2300)
        };

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.Write("1.Все" +
                    "\n2.Добавить" +
                    "\n3.Удалить" +
                    "\n4.Поиск" +
                    "\n5.Сортировка" +
                    "\n6.Мин/Макс цене" +
                    "\n7.Авторы" +
                    "\n0.Выход" +
                    "\nВыбор: ");
                switch (Console.ReadLine())
                {
                    case "1": books.ForEach(Console.WriteLine); break;
                    case "2": AddBook(); break;
                    case "3": RemoveBook(); break;
                    case "4": SearchBooks(); break;
                    case "5": SortBooks(); break;
                    case "6": ShowExtremes(); break;
                    case "7": books.GroupBy(b => b.Author).Select(g => $"{g.Key}: {g.Count()} кн.").ToList().ForEach(Console.WriteLine); break;
                    case "0": return;
                }
            }
        }

        static void AddBook()
        {
            try
            {
                Console.Write("Название: "); string t = Console.ReadLine();
                Console.Write("Автор: "); string a = Console.ReadLine();
                Console.Write("Жанр (1-Fiction, 2-Science, 3-History, 4-Fantasy): "); Genre g = (Genre)int.Parse(Console.ReadLine());
                Console.Write("Год: "); int y = int.Parse(Console.ReadLine());
                Console.Write("Цена: "); decimal p = decimal.Parse(Console.ReadLine());
                books.Add(new Book(t, a, g, y, p));
                Console.WriteLine("Добавлено.");
            }
            catch { Console.WriteLine("Ошибка ввода! Книга не добавлена."); }
        }

        static void RemoveBook()
        {
            Console.Write("Введите ID: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id) && books.Remove(books.FirstOrDefault(b => b.Id == id)))
                Console.WriteLine("Удалено.");
            else Console.WriteLine("Не найдено или неверный ID.");
        }

        static void SearchBooks()
        {
            Console.Write("1.По названию" +
                "\n2.По автору" +
                "\n3.По жанру: ");
            string choice = Console.ReadLine();
            Console.Write("Запрос: "); string q = Console.ReadLine();

            var res = choice switch
            {
                "1" => books.Where(b => b.Title.Contains(q, StringComparison.OrdinalIgnoreCase)),
                "2" => books.Where(b => b.Author.Contains(q, StringComparison.OrdinalIgnoreCase)),
                "3" => books.Where(b => Enum.TryParse(q, true, out Genre g) && b.Genre == g),
                _ => Enumerable.Empty<Book>()
            };
            res.ToList().ForEach(Console.WriteLine);
        }

        static void SortBooks()
        {
            Console.Write("1.По названию" +
                "\n2.По году: ");
            var res = Console.ReadLine() == "1" ? books.OrderBy(b => b.Title) : books.OrderBy(b => b.Year);
            res.ToList().ForEach(Console.WriteLine);
        }

        static void ShowExtremes()
        {
            if (!books.Any()) return;
            Console.WriteLine($"Самая дорогая: {books.OrderByDescending(b => b.Price).First()}");
            Console.WriteLine($"Самая дешевая: {books.OrderBy(b => b.Price).First()}");
        }
    }
}
