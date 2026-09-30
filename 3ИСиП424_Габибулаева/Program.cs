using System;
using System.Collections.Generic;

namespace Пр3
{
    class Program
    {
        static List<string> savedTexts = new List<string>();
        static List<int> savedKolvoWords = new List<int>();
        static List<int> savedKolichestvoPredlozheniy = new List<int>();

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

                    // 1. Подсчет предложений
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
                        KolichestvoPredlozheniy = 1; // Если знаков препинания нет, но текст есть
                    }

                    // 2. Разбиение на слова ручным способом (через Split)
                    char[] separators = new char[] { ' ', ',', '.', '!', '?', '-', ';', ':', '(', ')' };
                    string[] allWords = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    int Kol_voWords = allWords.Length;

                    // 3. Поиск самого длинного и короткого слова
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

                    // 4. Подсчет гласных, согласных и частоты букв
                    int glasnie = 0;
                    int soglasnie = 0;

                    // Строки для проверки букв (в нижнем регистре)
                    string GLASNIE = "аеёиоуыэюяaeiouy";
                    string SOGLASNIE = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyz";

                    // Массивы для частоты букв: один для алфавита, другой для счетчиков
                    string alfavit = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяabcdefghijklmnopqrstuvwxyz";
                    int[] letter = new int[alfavit.Length];

                    for (int i = 0; i < text.Length; i++)
                    {
                        char symbol = text[i];
                        char lowerSymbol = char.ToLower(symbol);

                        // Проверяем гласная или согласная
                        if (GLASNIE.Contains(lowerSymbol.ToString()))
                        {
                            glasnie++;
                        }
                        else if (SOGLASNIE.Contains(lowerSymbol.ToString()))
                        {
                            soglasnie++;
                        }

                        // Считаем частоту конкретной буквы
                        for (int j = 0; j < alfavit.Length; j++)
                        {
                            if (lowerSymbol == alfavit[j])
                            {
                                letter[j]++;
                                break;
                            }
                        }
                    }

                    // Вывод резов
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

                    // Сохраняем всюэту вкусняшку
                    savedTexts.Add(text);
                    savedKolvoWords.Add(Kol_voWords);
                    savedKolichestvoPredlozheniy.Add(KolichestvoPredlozheniy);

                    Console.WriteLine("\nСтатистика успешно сохранена в историю!");
                }
                else if (choice == "2")
                {
                    if (savedTexts.Count == 0)
                    {
                        Console.WriteLine("История пустенькая.");
                    }
                    else
                    {
                        Console.WriteLine("\n--- ИСТОРИЯ ПРОШЛЫХ ТЕКСТОВ ---");
                        for (int i = 0; i < savedTexts.Count; i++)
                        {
                            Console.WriteLine("Текст №" + (i + 1));
                            Console.WriteLine("Начало текста: " + savedTexts[i].Substring(0, Math.Min(30, savedTexts[i].Length)) + "...");
                            Console.WriteLine("Количествво слов: " + savedKolvoWords[i]);
                            Console.WriteLine("Количество предложений: " + savedKolichestvoPredlozheniy[i]);
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
                Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
