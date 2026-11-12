using System;
using System.IO;

namespace ChildApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) return;

            // 3) Калькулятор
            if (args.Length == 3 && (args[2] == "+" || args[2] == "-" || args[2] == "*" || args[2] == "/"))
            {
                double a = double.Parse(args[0]);
                double b = double.Parse(args[1]);
                string op = args[2];

                Console.WriteLine($"Получены аргументы: {a}, {b}, {op}");
                
                double result = 0;
                if (op == "+") result = a + b;
                else if (op == "-") result = a - b;
                else if (op == "*") result = a * b;
                else if (op == "/") result = b != 0 ? a / b : 0;

                Console.WriteLine($"Результат: {result}");
                return;
            }

            // 4) Поиск слова в файле
            if (args.Length == 2 && File.Exists(args[0]))
            {
                string filePath = args[0];
                string word = args[1];
                
                string text = File.ReadAllText(filePath);
                int count = 0;
                int index = 0;
                
                while ((index = text.IndexOf(word, index, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    count++;
                    index += word.Length;
                }

                Console.WriteLine($"Слово '{word}' встретилось {count} раз(а).");
                return;
            }

            Console.WriteLine("Дочерний процесс получил аргументы:");
            foreach (var arg in args) Console.WriteLine($" - {arg}");
            Console.WriteLine("Выполнение дочернего процесса успешно завершено.");
        }
    }
}
