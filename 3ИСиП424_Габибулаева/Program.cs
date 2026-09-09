using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Write("Сколько будет операций (от 2 до 40): ");
        int count = int.Parse(Console.ReadLine());
        if (count < 2 || count > 40)
        {
            Console.WriteLine("Написала же от 2 до 40 >:|. Домой Уолтер.");
            return;
        }
        List<string> names = new List<string>();
        List<double> prices = new List<double>();

        Console.WriteLine("Вводите этот кошмар");
        for (int i = 0; i < count; i++)
        {
            string[] parts = Console.ReadLine().Split(';');
            names.Add(parts[0]);
            prices.Add(double.Parse(parts[1]));
        }

        while (true) //True Adam
        {
            Console.WriteLine("\n1. Вывод | " +
                "2. Статистика | " +
                "3. Пузырек| " +
                "4. Курс валюты | " +
                "5. Поиск | " +
                "0. Выход |");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    for (int i = 0; i < names.Count; i++)
                        Console.WriteLine($"{names[i]} — {prices[i]} руб.");
                    break;

                case "2":
                    double sum = 0, max = prices[0], min = prices[0];
                    foreach (double p in prices)
                    {
                        sum += p;
                        if (p > max) max = p;
                        if (p < min) min = p;
                    }
                    Console.WriteLine($"Сумма: {sum} | Среднее: {sum / count} | Макс: {max} | Мин: {min}");
                    break;

                case "3":
                    for (int i = 0; i < prices.Count - 1; i++)
                        for (int j = 0; j < prices.Count - 1 - i; j++)
                            if (prices[j] > prices[j + 1])
                            {
                                double tempP = prices[j]; prices[j] = prices[j + 1]; prices[j + 1] = tempP;
                                string tempN = names[j]; names[j] = names[j + 1]; names[j + 1] = tempN;
                            }
                    Console.WriteLine("Отсортировано");
                    break;

                case "4":
                    Console.Write("Введите курс (я вот на третьем курсе уже): ");
                    double rate = double.Parse(Console.ReadLine());
                    for (int i = 0; i < names.Count; i++)
                        Console.WriteLine($"{names[i]} — {prices[i] / rate:F2} деняк");
                    break;

                case "5":
                    Console.Write("Что искать, Босс ?: ");
                    string word = Console.ReadLine().ToLower();
                    for (int i = 0; i < names.Count; i++)
                        if (names[i].ToLower().Contains(word))
                            Console.WriteLine($"{names[i]} — {prices[i]} рубуксов");
                    break;

                case "0":
                    Console.Write("Ну все, пока ");
                    return;
            }
        }
    }
}
