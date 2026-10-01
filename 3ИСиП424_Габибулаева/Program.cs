using System;
using System.Collections.Generic;

namespace Пр3
{
    class Statistika
    {
        public string Text { get; set; }
        public int WordsCount { get; set; }
        public int SentencesCount { get; set; }
        public int Kolvoglasny { get; set; }
        public int KolvoSoglasny { get; set; }
        public string SHORTWORD { get; set; }
        public string LONGWORD { get; set; }
        public string ALFAVIT { get; set; }
        public int[] KolvoBukv { get; set; }
    }
    class Program
    {
        static List<Statistika> savedStats = new List<Statistika>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1. Ввести новый текст");
                Console.WriteLine("2. Показать историю");
                Console.WriteLine("3. Выйти");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    string text = "";
                    while (text.Length < 100)
                    {
                        Console.WriteLine("Введите текст не менее сто символов:");
                        text = Console.ReadLine();
                        if (text.Length < 100)
                        {
                            Console.WriteLine("Ошибка!!!111!!!111!!! Текст слишком короткий. Сейчас длина: " + text.Length);
                        }
                    }

                    // Подсчет предложений
                    int KolichestvoPredlozheniy = 0;
                    for (int i = 0; i < text.Length; i++)
                    {
                        char c = text[i];
                        if (c == '.' || c == '!' || c == '?')
                        {
                            KolichestvoPredlozheniy++;
                        }
                    }
                    if (KolichestvoPredlozheniy == 0 && text.Length > 0)
                    {
                        KolichestvoPredlozheniy = 1;
                    }

                    char[] separators = new char[] { ' ', ',', '.', '!', '?', '-', ';', ':', '(', ')' };
                    string[] allWords = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    int Kol_voWords = allWords.Length;
                    string shorter = "";
                    string longest = "";
                    if (allWords.Length > 0)
                    {
                        shorter = allWords[0];
                        longest = allWords[0];

                        for (int i = 1; i < allWords.Length; i++)
                        {
                            if (allWords[i].Length < shorter.Length)
                            {
                                shorter = allWords[i];
                            }
                            if (allWords[i].Length > longest.Length)
                            {
                                longest = allWords[i];
                            }
                        }
                    }

                    // Гласные и согласные
                    int glasnie = 0;
                    int soglasnie = 0;

                    string GLASNIE = "аеёиоуыэюяaeiouy";
                    string SOGLASNIE = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyz";

                    string alfavit = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяabcdefghijklmnopqrstuvwxyz";
                    int[] letter = new int[alfavit.Length];

                    for (int i = 0; i < text.Length; i++)
                    {
                        char symbol = text[i];
                        char lowerSymbol = char.ToLower(symbol);

                        if (GLASNIE.Contains(lowerSymbol.ToString()))
                        {
                            glasnie++;
                        }
                        else if (SOGLASNIE.Contains(lowerSymbol.ToString()))
                        {
                            soglasnie++;
                        }

                        for (int j = 0; j < alfavit.Length; j++)
                        {
                            if (lowerSymbol == alfavit[j])
                            {
                                letter[j]++;
                                break;
                            }
                        }
                    }

                    // Вывод всей Этой вкусняшки
                    Console.WriteLine("\n--- РЕЗУЛЬТАТЫ АНАЛИЗА ---");
                    Console.WriteLine("Слов в тексте: " + Kol_voWords);
                    Console.WriteLine("Предложений: " + KolichestvoPredlozheniy);
                    Console.WriteLine("Гласных букв: " + glasnie);
                    Console.WriteLine("Согласных букв: " + soglasnie);
                    Console.WriteLine("Самое короткое слово: " + shorter);
                    Console.WriteLine("Самое длинное слово: " + longest);

                    Console.WriteLine("Частота букв (которые встретились):");
                    for (int i = 0; i < alfavit.Length; i++)
                    {
                        if (letter[i] > 0)
                        {
                            Console.WriteLine("Буква '" + alfavit[i] + "' встретилась: " + letter[i] + " раз(а)");
                        }
                    }

                    //  объект класса со всеми собранными данными
                    Statistika stats = new Statistika
                    {
                        Text = text,
                        WordsCount = Kol_voWords,
                        SentencesCount = KolichestvoPredlozheniy,
                        Kolvoglasny = glasnie,
                        KolvoSoglasny = soglasnie,
                        SHORTWORD = shorter,
                        LONGWORD = longest,
                        ALFAVIT = alfavit,
                        KolvoBukv = letter
                    };
                   
                    savedStats.Add(stats);

                    Console.WriteLine("\nСтатистика успешно сохранена в историю!");
                }
                else if (choice == "2")
                {
                    if (savedStats.Count == 0)
                    {
                        Console.WriteLine("История пустенькая.");
                    }
                    else
                    {
                        Console.WriteLine("\n--- ИСТОРИЯ ПРОШЛЫХ ТЕКСТОВ ---");
                        for (int i = 0; i < savedStats.Count; i++)
                        {
                            Statistika current = savedStats[i]; // данные текущего текста из списка
                            Console.WriteLine("Текст №" + (i + 1));
                            Console.WriteLine("Начало текста: " + current.Text.Substring(0, Math.Min(30, current.Text.Length)) + "...");
                            Console.WriteLine("Количество слов: " + current.WordsCount);
                            Console.WriteLine("Количество предложений: " + current.SentencesCount);
                            Console.WriteLine("Гласных букв: " + current.Kolvoglasny);
                            Console.WriteLine("Согласных букв: " + current.KolvoSoglasny);
                            Console.WriteLine("Самое короткое слово: " + current.SHORTWORD);
                            Console.WriteLine("Самое длинное слово: " + current.LONGWORD);
                            
                            Console.WriteLine("Частота букв в этом тексте:");
                            for (int j = 0; j < current.ALFAVIT.Length; j++)
                            {
                                if (current.KolvoBukv[j] > 0)
                                {
                                    Console.WriteLine("  Буква '" + current.ALFAVIT[j] + "' встретилась: " + current.KolvoBukv[j] + " раз(а)");
                                }
                            }
                        }
                    }
                }
                else if (choice == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Всего три пункта, а вы что выбрали, а?");
                }
                Console.WriteLine("\nНажмите Ентер, чтобы продолжить...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
