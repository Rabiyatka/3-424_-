using System;
using System.Collections.Generic;
using System.Linq;
namespace _3ИСИП424
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; } 
        public int Year { get; set; }
        public decimal Price { get; set; }

        public Book(int id, string t, string a, string g, int y, decimal p)
        {
            Id = id;
            if (!string.IsNullOrWhiteSpace(t)) Title = t; else throw new Exception();
            if (!string.IsNullOrWhiteSpace(a)) Author = a; else throw new Exception();
            if (!string.IsNullOrWhiteSpace(g)) Genre = g; else throw new Exception();
            if (y >= 0) Year = y; else throw new Exception();
            if (p >= 0) Price = p; else throw new Exception();
        }
        public override string ToString() => $"[ID: {Id}] \"{Title}\" — {Author} | Жанр: {Genre} | Год: {Year} | Цена: {Price:C}";
    }
    class Program
    {
        static  List<string> DostypGenre = new List<string> { "Худож.лит-ра", "Научное", "Историческое", "Фантастика" };

        static int nextId = 1;

        static List<Book> books = new List<Book>
        {
            new Book(nextId++, "Властелин Колец", "Толкин", "Фантастика", 1954, 1500),
            new Book(nextId++, "Краткая история времени", "Хокинг", "Научное", 1988, 850),
            new Book(nextId++, "Преступление и наказание", "Достоевский", "Худож.лит-ра", 1866, 600),
            new Book(nextId++, "Хоббит", "Толкин", "Фантастика", 1937, 1100),
            new Book(nextId++, "История Древнего Рима", "Моммзен", "Историческое", 1856, 2300)
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
                    "\n6.Мин/Макс цена" +
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
                Console.WriteLine("Доступные жанры: " + string.Join(", ", DostypGenre));
                Console.Write("Введите жанр из списка: ");
                string g = Console.ReadLine()?.Trim();
                string matchedGenre = DostypGenre.FirstOrDefault(gen => gen.Equals(g, StringComparison.OrdinalIgnoreCase));
                if (matchedGenre == null) throw new Exception();
                Console.Write("Год: "); int y = int.Parse(Console.ReadLine());
                Console.Write("Цена: "); decimal p = decimal.Parse(Console.ReadLine());
                books.Add(new Book(nextId++, t, a, matchedGenre, y, p));
                Console.WriteLine("Добавлено.");
            }
            catch { Console.WriteLine("Ошибка ввода! Книга не добавлена."); }
        }
        static void RemoveBook()
        {
            Console.Write("Введите ID: ");
            if (int.TryParse(Console.ReadLine(), out int id) && books.Remove(books.FirstOrDefault(b => b.Id == id)))
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
                "3" => books.Where(b => b.Genre.Contains(q, StringComparison.OrdinalIgnoreCase)),
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
