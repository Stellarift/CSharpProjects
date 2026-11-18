using System;
using System.Collections.Generic;
using System.Threading;

namespace SyncKernel
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Синхронизация и объекты ядра");
                Console.WriteLine("1 - Задание 1 (Mutex: единственный экземпляр)");
                Console.WriteLine("2 - Задание 2 (Mutex: защита счетчика)");
                Console.WriteLine("3 - Задание 3 (Semaphore: ограничение потоков)");
                Console.WriteLine("4 - Задание 4 (Semaphore + lock: производитель-потребитель)");
                Console.WriteLine("5 - Задание 5 (ManualResetEvent: старт группы)");
                Console.WriteLine("6 - Задание 6 (AutoResetEvent: поочередный вывод)");
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
                    case "6": Task6();
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

        // задание 1
        static void Task1()
        {
            Console.WriteLine("1) Mutex (единственный экземпляр)");
            try
            {
                // Пытаемся открыть существующий мьютекс
                Mutex.OpenExisting("MY_MUTEX");
                Console.WriteLine("Приложение уже запущено!");
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                // Если не удалось открыть — создаем новый
                using Mutex mutex = new Mutex(true, "MY_MUTEX");
                Console.WriteLine("Приложение работает. Нажмите Enter для выхода...");
                Console.ReadLine();
                mutex.ReleaseMutex();
            }
        }

        // задание 2
        static void Task2()
        {
            Console.WriteLine("2) Mutex (защита счетчика)");
            MutexCounter counter = new MutexCounter();
            Thread[] threads = new Thread[5];

            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 1_000_000; j++)
                    {
                        counter.Increment();
                    }
                });
                threads[i].Start();
            }

            foreach (var t in threads) t.Join();
            Console.WriteLine($"Итоговое значение: {counter.Count} (должно быть 5000000)");
        }

        class MutexCounter
        {
            private int count = 0;
            private readonly Mutex mutex = new Mutex();

            public int Count => count;

            public void Increment()
            {
                mutex.WaitOne();
                try { count++; }
                finally { mutex.ReleaseMutex(); }
            }
        }

        // задание 3
        static void Task3()
        {
            Console.WriteLine("3) Semaphore (ограничение потоков)");
            Semaphore sem = new Semaphore(3, 3);

            for (int i = 1; i <= 6; i++)
            {
                int taskId = i;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    while (true)
                    {
                        if (sem.WaitOne(500))
                        {
                            try
                            {
                                Console.WriteLine($"[Задача {taskId}] Начала работу (Поток {Thread.CurrentThread.ManagedThreadId})");
                                Thread.Sleep(2000);
                                Console.WriteLine($"[Задача {taskId}] Закончила работу");
                            }
                            finally
                            {
                                sem.Release();
                            }
                            break; // Успешно выполнили — выходим
                        }
                        else
                        {
                            Console.WriteLine($"[Задача {taskId}] Таймаут, ждем...");
                        }
                    }
                });
            }

            // Ждем завершения работы всех задач
            Thread.Sleep(10000);
            Console.WriteLine("Все задачи завершены.");
        }

        // задание 4
        static void Task4()
        {
            Console.WriteLine("4) Производитель-потребитель (Semaphore + lock)");
            Queue<int> queue = new Queue<int>();
            Semaphore empty = new Semaphore(5, 5);
            Semaphore full = new Semaphore(0, 5);
            object locker = new object();
            bool isProducing = true;

            Thread producer = new Thread(() =>
            {
                int item = 1;
                while (isProducing)
                {
                    empty.WaitOne();
                    lock (locker)
                    {
                        queue.Enqueue(item);
                        Console.WriteLine($"[Производитель] Добавил {item}");
                        item++;
                    }
                    full.Release();
                    Thread.Sleep(500);
                }
            });

            Thread consumer1 = new Thread(() => Consumer(queue, empty, full, locker));
            Thread consumer2 = new Thread(() => Consumer(queue, empty, full, locker));

            producer.Start(); consumer1.Start(); consumer2.Start();

            Thread.Sleep(4000);
            isProducing = false;
            producer.Join();
            Console.WriteLine("Производство завершено.");
        }

        static void Consumer(Queue<int> queue, Semaphore empty, Semaphore full, object locker)
        {
            while (true)
            {
                if (full.WaitOne(1000))
                {
                    lock (locker)
                    {
                        int item = queue.Dequeue();
                        Console.WriteLine($"[Потребитель {Thread.CurrentThread.ManagedThreadId}] Взял {item}");
                    }
                    empty.Release();
                    Thread.Sleep(1000);
                }
                else
                {
                    break; // Если очередь пуста и время вышло — выходим
                }
            }
        }

        // задание 5
        static void Task5()
        {
            Console.WriteLine("5) ManualResetEvent (старт группы)");
            ManualResetEvent startEvent = new ManualResetEvent(false);
            Thread[] threads = new Thread[10];

            for (int i = 0; i < 10; i++)
            {
                int threadId = i + 1;
                threads[i] = new Thread(() =>
                {
                    startEvent.WaitOne();
                    Console.WriteLine($"Поток {threadId} стартовал");
                });
                threads[i].Start();
            }

            Console.WriteLine("Все потоки созданы. Ждем 2 секунды...");
            Thread.Sleep(2000);
            Console.WriteLine("Даем сигнал");
            startEvent.Set();

            foreach (var t in threads) t.Join();
            Console.WriteLine("Все потоки завершены.");
        }

        // задание 6
        static void Task6()
        {
            Console.WriteLine("6) AutoResetEvent (поочередный вывод)");
            AutoResetEvent event1 = new AutoResetEvent(true);
            AutoResetEvent event2 = new AutoResetEvent(false);

            Thread oddThread = new Thread(() =>
            {
                for (int i = 1; i <= 9; i += 2)
                {
                    event1.WaitOne();
                    Console.Write($"{i} ");
                    event2.Set();
                }
            });

            Thread evenThread = new Thread(() =>
            {
                for (int i = 2; i <= 10; i += 2)
                {
                    event2.WaitOne();
                    Console.Write($"{i} ");
                    event1.Set();
                }
            });

            oddThread.Start();
            evenThread.Start();

            oddThread.Join();
            evenThread.Join();
            Console.WriteLine("\nГотово");
        }
    }
}