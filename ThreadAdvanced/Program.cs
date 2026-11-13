using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadAdvanced
{
    class Program
    {
        static int ticketsLeft = 100;
        static int fuelLevel = 0;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Многопоточность и Асинхронность");
                Console.WriteLine("1 - Задание 1 (Диспетчер задач: Thread vs ThreadPool)");
                Console.WriteLine("2 - Задание 2 (Асинхронный калькулятор: APM)");
                Console.WriteLine("3 - Задание 3 (Касса кинотеатра: Синхронизация)");
                Console.WriteLine("4 - Задание 4 (Космический запуск)");
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
            Console.WriteLine("1) Диспетчер задач");

            var cts = new CancellationTokenSource();

            // Фоновый поток для вывода времени
            Thread timeThread = new Thread(() =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    Console.WriteLine($"[Фоновый поток] Текущее время: {DateTime.Now:HH:mm:ss}");
                    Thread.Sleep(1000);
                }
            });
            timeThread.IsBackground = true;
            timeThread.Start();

            for (int i = 1; i <= 3; i++)
            {
                ThreadPool.QueueUserWorkItem(PoolTask, i);
            }

            Console.WriteLine("Главный поток ждет 5 секунд...");
            Thread.Sleep(5000);
            Console.WriteLine("Главный поток завершает работу.");

            // Останавливаем поток времени
            cts.Cancel();
        }

        static void PoolTask(object? state)
        {
            int taskId = (int)state!;
            Console.WriteLine($"[Пул потоков] Задача №{taskId} выполняется в потоке {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);
            Console.WriteLine($"[Пул потоков] Задача №{taskId} завершена.");
        }

        // задание 2
        // Создаем свой класс для реализации паттерна APM
        public class MyAsyncResult : IAsyncResult
        {
            public object? AsyncState { get; set; }
            public WaitHandle AsyncWaitHandle => new ManualResetEvent(false);
            public bool CompletedSynchronously => false;
            public bool IsCompleted { get; set; }
            public double Result { get; set; }
        }

        static void Task2()
        {
            Console.WriteLine("2) Асинхронный калькулятор");

            Func<int, double> heavyMath = CalculateHeavyMath;

            // Имитация APM (BeginInvoke)
            BeginHeavyMath(25, CalculationFinished, heavyMath);

            Console.WriteLine("Расчет запущен. Главный поток свободен.");
        }

        static double CalculateHeavyMath(int n)
        {
            Thread.Sleep(3000);
            return Math.Sqrt(n);
        }

        // Имитация метода BeginInvoke
        static void BeginHeavyMath(int n, AsyncCallback callback, object? state)
        {
            MyAsyncResult asyncResult = new MyAsyncResult { AsyncState = state };

            Task.Run(() =>
            {
                double result = CalculateHeavyMath(n);
                asyncResult.Result = result;
                asyncResult.IsCompleted = true;
                
                // Вызываем Callback, передавая наш IAsyncResult
                callback(asyncResult);
            });
        }

        // Имитация метода EndInvoke
        static double EndHeavyMath(IAsyncResult ar)
        {
            return ((MyAsyncResult)ar).Result;
        }

        static void CalculationFinished(IAsyncResult ar)
        {
            // Получаем результат через свой метод EndInvoke
            double result = EndHeavyMath(ar);

            Console.WriteLine($"\n[Callback] Расчет завершен. Результат: {result}");
            Console.WriteLine($"[Callback] Выполнено в потоке: {Thread.CurrentThread.ManagedThreadId}");
        }

        // задание 3
        static void Task3()
        {
            Console.WriteLine("3) Касса кинотеатра");
            ticketsLeft = 100;

            Console.WriteLine("\n1. ЗАПУСК БЕЗ СИНХРОНИЗАЦИИ (ожидаем гонку данных):");
            RunTicketSales(false);

            Console.WriteLine("\n2. ЗАПУСК С СИНХРОНИЗАЦИЕЙ (Interlocked):");
            ticketsLeft = 100;
            RunTicketSales(true);
        }

        static void RunTicketSales(bool useSync)
        {
            Thread[] cashiers = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                int cashierId = i + 1;
                cashiers[i] = new Thread(() =>
                {
                    while (true)
                    {
                        if (useSync)
                        {
                            // Атомарное уменьшение
                            if (Interlocked.Decrement(ref ticketsLeft) >= 0)
                            {
                                Console.WriteLine($"Кассир {cashierId} продал билет. Осталось: {ticketsLeft}");
                            }
                            else
                            {
                                // Возвращаем счетчик назад, если билетов уже нет
                                Interlocked.Increment(ref ticketsLeft);
                                break;
                            }
                        }
                        else
                        {
                            // Обычное чтение и запись
                            if (ticketsLeft > 0)
                            {
                                Console.WriteLine($"Кассир {cashierId} продал билет. Осталось: {ticketsLeft - 1}");
                                ticketsLeft--;
                            }
                            else break;
                        }
                        Thread.Sleep(10);
                    }
                });
            }

            foreach (var t in cashiers) t.Start();
            foreach (var t in cashiers) t.Join();
            
            Console.WriteLine($"[Итог] Остаток билетов: {ticketsLeft}");
        }

        // задание 4
        static void Task4()
        {
            Console.WriteLine("4) Космический запуск");
            fuelLevel = 0;

            var cts = new CancellationTokenSource();

            // 1. Мониторинг (Thread)
            Thread monitor = new Thread(() =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    Console.WriteLine("[Телеметрия] Все системы в норме...");
                    Thread.Sleep(1000);
                }
            });
            monitor.IsBackground = true;
            monitor.Start();

            // 2. Асинхронный расчет (APM через Task)
            Func<int, double> calcTrajectory = (n) =>
            {
                Thread.Sleep(4000);
                return 42.5;
            };

            Task.Run(() =>
            {
                calcTrajectory(0);
                Console.WriteLine("\n[Траектория] Траектория рассчитана! К запуску готов.");
            });

            // 3. Топливные насосы (ThreadPool)
            ThreadPool.QueueUserWorkItem(PumpFuel);

            // 4. Главный поток ждет 5 секунд
            Thread.Sleep(5000);
            
            // Безопасное чтение
            int currentFuel = Interlocked.CompareExchange(ref fuelLevel, 0, 0);
            Console.WriteLine($"\n[Главный поток] Итоговый уровень топлива: {currentFuel}");
            Console.WriteLine("Запуск завершен");

            // Останавливаем поток телеметрии
            cts.Cancel();
        }

        static void PumpFuel(object? state)
        {
            for (int i = 0; i < 10; i++)
            {
                Interlocked.Add(ref fuelLevel, 10);
                Thread.Sleep(500);
            }
        }
    }
}