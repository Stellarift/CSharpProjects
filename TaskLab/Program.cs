using System;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLab
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Параллельное программирование");
                Console.WriteLine("1 - Задание 1 (Три способа запуска Task)");
                Console.WriteLine("2 - Задание 2 (Простые числа 0..1000)");
                Console.WriteLine("3 - Задание 3 (Простые числа в диапазоне)");
                Console.WriteLine("4 - Задание 4 (Min, Max, Avg, Sum через Task[])");
                Console.WriteLine("5 - Задание 5 (Continuation Tasks)");
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
                    default: Console.WriteLine("Неверный ввод");
                     break;
                }

                Console.WriteLine("\nНажмите Enter, чтобы вернуться в меню...");
                Console.ReadLine();
            }
        }

        // задание 1
        static void Task1()
        {
            Console.WriteLine("1) Три способа запуска Task");

            Console.WriteLine("1) Через метод Start:");
            Task t1 = new Task(() => Console.WriteLine($"[Start] Время: {DateTime.Now}"));
            t1.Start();
            t1.Wait();

            Console.WriteLine("2) Через Task.Factory.StartNew:");
            Task t2 = Task.Factory.StartNew(() => Console.WriteLine($"[Factory] Время: {DateTime.Now}"));
            t2.Wait();

            Console.WriteLine("3) Через Task.Run:");
            Task t3 = Task.Run(() => Console.WriteLine($"[Run] Время: {DateTime.Now}"));
            t3.Wait();
        }

        // задание 2
        static void Task2()
        {
            Console.WriteLine("2) Простые числа от 0 до 1000");

            Task task = Task.Run(() =>
            {
                for (int i = 0; i <= 1000; i++)
                {
                    if (IsPrime(i)) Console.Write($"{i} ");
                }
                Console.WriteLine();
            });

            task.Wait(); // Основной поток ожидает завершения
            Console.WriteLine("Основной поток дождался задачи");
        }

        // задание 3
        static void Task3()
        {
            Console.Write("Введите начало диапазона: ");
            int start = int.Parse(Console.ReadLine()!);
            Console.Write("Введите конец диапазона: ");
            int end = int.Parse(Console.ReadLine()!);

            Task<int> task = Task.Run(() =>
            {
                int count = 0;
                for (int i = start; i <= end; i++)
                {
                    if (IsPrime(i))
                    {
                        Console.Write($"{i} ");
                        count++;
                    }
                }
                Console.WriteLine();
                return count;
            });

            task.Wait(); // Ожидаем завершения
            Console.WriteLine($"\nКоличество простых чисел: {task.Result}");
        }

        // задание 4
        static void Task4()
        {
            Console.WriteLine("4) Min, Max, Avg, Sum через Task[]");
            
            int[] numbers = new int[10000];
            Random rnd = new Random();
            for (int i = 0; i < numbers.Length; i++) numbers[i] = rnd.Next(1, 100000);

            Task<int> tMin = Task.Run(() => numbers.Min());
            Task<int> tMax = Task.Run(() => numbers.Max());
            Task<double> tAvg = Task.Run(() => numbers.Average());
            Task<long> tSum = Task.Run(() => (long)numbers.Sum());
            Task.WaitAll(tMin, tMax, tAvg, tSum);

            Console.WriteLine($"Минимум: {tMin.Result}");
            Console.WriteLine($"Максимум: {tMax.Result}");
            Console.WriteLine($"Среднее: {tAvg.Result:F2}");
            Console.WriteLine($"Сумма: {tSum.Result}");
        }

        // задание 5
        static void Task5()
        {
            Console.WriteLine("5) Continuation Tasks");
            
            int[] numbers = { 5, 2, 9, 2, 5, 7, 9, 1, 1, 8 };
            Console.WriteLine($"Исходный массив: {string.Join(", ", numbers)}");

            int targetValue = 7;
            Console.WriteLine($"Искомое значение: {targetValue}\n");

            // 1. Удаление дублей
            Task<int[]> removeDuplicatesTask = Task.Run(() =>
            {
                int[] unique = numbers.Distinct().ToArray();
                Console.WriteLine($"[Поток 1] Удалены дубли: {string.Join(", ", unique)}");
                return unique;
            });

            // 2. Сортировка
            Task<int[]> sortTask = removeDuplicatesTask.ContinueWith(prevTask =>
            {
                int[] sorted = prevTask.Result.OrderBy(x => x).ToArray();
                Console.WriteLine($"[Поток 2] Массив отсортирован: {string.Join(", ", sorted)}");
                return sorted;
            });

            // 3. Бинарный поиск (запустится после сортировки)
            Task sortTaskEnd = sortTask.ContinueWith(prevTask =>
            {
                int[] sorted = prevTask.Result;
                int index = Array.BinarySearch(sorted, targetValue);
                
                if (index >= 0)
                    Console.WriteLine($"[Поток 3] Значение {targetValue} найдено по индексу {index}");
                else
                    Console.WriteLine($"[Поток 3] Значение {targetValue} не найдено");
            });

            sortTaskEnd.Wait(); // Ждем завершения всей цепочки
        }

        // Вспомогательный метод проверки на простое число
        static bool IsPrime(int n)
        {
            if (n < 2) return false;
            for (int i = 2; i <= Math.Sqrt(n); i++)
                if (n % i == 0) return false;
            return true;
        }
    }
}
