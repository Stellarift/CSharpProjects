using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string text = "Вот дом, Который построил Джек. А это пшеница, " +
                      "Которая в темном чулане хранится В доме, " +
                      "Который построил Джек. А это веселая птица-синица, " +
                      "Которая часто ворует пшеницу, Которая в темном чулане хранится " +
                      "В доме, Который построил Джек.";

        string cleanText = text.ToLower();
        char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '\n', '\r', '-', '—' };
        string[] words = cleanText.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        // Подсчитываем частоту слов
        Dictionary<string, int> dictionary = new Dictionary<string, int>();

        foreach (string word in words)
        {
            if (dictionary.ContainsKey(word))
                dictionary[word]++;
            else
                dictionary[word] = 1;
        }

        // Выводим таблицу
        Console.WriteLine("Статистика слов в тексте:\n");
        Console.WriteLine("Слово\t\t\tКоличество");
        Console.WriteLine("---------------------------------");

        foreach (KeyValuePair<string, int> item in dictionary)
        {
            Console.WriteLine($"{item.Key,-20}\t{item.Value}");
        }

        Console.WriteLine($"\nВсего слов: {words.Length}, из них уникальных: {dictionary.Count}");
    }
}