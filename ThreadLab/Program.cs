using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadLab
{
    class Program
    {
        // Для Задачи 3
        private static int counter = 0;
        private static readonly object locker = new object();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Многопоточность");
                Console.WriteLine("1 - Задача 1 (Потоки и Join)");
                Console.WriteLine("2 - Задача 2 (Фоновый поток и Таймер)");
                Console.WriteLine("3 - Задача 3 (Гонка данных и Lock)");
                Console.WriteLine("4 - Задача 4 (Игра на реакцию)");
                Console.WriteLine("0 - Выход");
                Console.Write("\nВыберите задачу: ");

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
                    case "0":
                     return;
                    default: Console.WriteLine("Неверный ввод.");
                     break;
                }

                Console.WriteLine("\nНажмите Enter, чтобы вернуться в меню...");
                Console.ReadLine();
            }
        }

        // 1
        static void Task1()
        {
            Console.WriteLine("1) Потоки и Join");
            Thread tA = new Thread(Worker);
            Thread tB = new Thread(Worker);

            tA.Start("Поток А");
            tB.Start("Поток Б");

            // завершения обоих потоков
            tA.Join();
            tB.Join();

            Console.WriteLine("Все потоки завершили работу!");
        }

        static void Worker(object? nameObj)
        {
            string name = (string)nameObj!;
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"[{name}] Шаг: {i}");
                Thread.Sleep(500);
            }
        }

        // 2
        static void Task2()
        {
            Console.WriteLine("2) Таймер и фоновый поток");
            
            // Таймер (запуск через 1 сек, повтор каждые 2 сек)
            Timer timer = new Timer(TimerCallback, null, 1000, 2000);

            // Фоновый поток
            Thread bgThread = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine("Фоновый поток работает...");
                    Thread.Sleep(300);
                }
            });
            bgThread.IsBackground = true; // Делаем фоновым
            bgThread.Start();

            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
        }

        static void TimerCallback(object? state)
        {
            Console.WriteLine($"[Таймер] Текущее время: {DateTime.Now:HH:mm:ss}");
        }

        // 3
        static void Task3()
        {
            Console.WriteLine("3)Гонка данных");
            counter = 0; // Сбрасываем счетчик
            Thread[] threads = new Thread[5];

            // Сначала БЕЗ блокировки
            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 100000; j++)
                    {
                        counter++; //гонка данных
                    }
                });
                threads[i].Start();
            }

            foreach (var t in threads) t.Join();
            Console.WriteLine($"Без lock: Итоговое значение = {counter} (должно быть 500000)");

            // Теперь С блокировкой
            counter = 0;
            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 100000; j++)
                    {
                        lock (locker) // Защита от гонки данных
                        {
                            counter++;
                        }
                    }
                });
                threads[i].Start();
            }

            foreach (var t in threads) t.Join();
            Console.WriteLine($"С lock:  Итоговое значение = {counter} (должно быть 500000)");
        }

        // задача 4
        static void Task4()
        {
            Console.WriteLine("4) Игра 'Успел, не успел'");
            Console.WriteLine("Приготовьтесь...");
            
            Random rnd = new Random();
            int delay = rnd.Next(2000, 5000); // от 2 до 5 секунд
            Thread.Sleep(delay);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ЖМИ!");
            Console.ResetColor();

            Stopwatch sw = Stopwatch.StartNew();

            // фоновый таймер на 3 секунд
            Thread timeoutThread = new Thread(() =>
            {
                Thread.Sleep(3000);
                // клавишу не нажали
                Console.WriteLine("\nВы не успели! Слишком долго");
                Environment.Exit(0);
            });
            timeoutThread.IsBackground = true;
            timeoutThread.Start();

            // Ждем
            Console.ReadKey(true);

            // останавливаем секундомер
            sw.Stop();
            Console.WriteLine($"Ваше время реакции: {sw.ElapsedMilliseconds} мс");
        }
    }
}