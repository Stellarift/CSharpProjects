using System;
using MyLibrary;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Использование DLL-библиотеки");
                Console.WriteLine("1 - Задание 1 (Сообщение)");
                Console.WriteLine("2 - Задание 2 (Высокосный год)");
                Console.WriteLine("3 - Задание 3 (Max, Min, Sum)");
                Console.WriteLine("4 - Задание 4 (Факториал, Простое, Четность)");
                Console.WriteLine("5 - Задание 5 (Класс Дробь)");
                Console.WriteLine("0 - Выход");
                Console.Write("\nВыберите задание: ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": Task1();
                     break;
                    case "2": Task2();
                     break;
                    case "3": Task3();
                     break;
                    case "4": Task4();
                     break;
                    case "5": Task5();
                     break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод.");
                     break;
                }

                Console.WriteLine("\nНажмите Enter, чтобы вернуться в меню...");
                Console.ReadLine();
            }
        }

        static void Task1()
        {
            Utility.ShowMessage("Привет, это сообщение из DLL!");
        }

        static void Task2()
        {
            Console.Write("Введите год: ");
            int year = int.Parse(Console.ReadLine()!);
            bool isLeap = Utility.IsLeapYear(year);
            Console.WriteLine(isLeap ? $"{year} - высокосный год" : $"{year} - не высокосный год");
        }

        static void Task3()
        {
            Console.WriteLine("Введите три числа:");
            int a = int.Parse(Console.ReadLine()!);
            int b = int.Parse(Console.ReadLine()!);
            int c = int.Parse(Console.ReadLine()!);

            Console.WriteLine($"Максимум: {Utility.Max(a, b, c)}");
            Console.WriteLine($"Минимум: {Utility.Min(a, b, c)}");
            Console.WriteLine($"Сумма: {Utility.Sum(a, b, c)}");
        }

        static void Task4()
        {
            Console.Write("Введите число: ");
            int n = int.Parse(Console.ReadLine()!);

            Console.WriteLine($"Факториал: {Utility.Factorial(n)}");
            Console.WriteLine($"Простое: {(Utility.IsPrime(n) ? "Да" : "Нет")}");
            Console.WriteLine($"Четное: {(Utility.IsEven(n) ? "Да" : "Нет")}");
        }

        static void Task5()
        {
            Console.WriteLine("Работа с дробями (числитель, знаменатель).");
            Console.Write("Дробь 1 (числитель): "); int n1 = int.Parse(Console.ReadLine()!);
            Console.Write("Дробь 1 (знаменатель): "); int d1 = int.Parse(Console.ReadLine()!);
            Console.Write("Дробь 2 (числитель): "); int n2 = int.Parse(Console.ReadLine()!);
            Console.Write("Дробь 2 (знаменатель): "); int d2 = int.Parse(Console.ReadLine()!);

            Fraction f1 = new Fraction(n1, d1);
            Fraction f2 = new Fraction(n2, d2);

            Console.WriteLine($"Дробь 1: {f1}");
            Console.WriteLine($"Дробь 2: {f2}");
            Console.WriteLine($"Сложение: {f1.Add(f2)}");
            Console.WriteLine($"Умножение: {f1.Multiply(f2)}");
        }
    }
}
