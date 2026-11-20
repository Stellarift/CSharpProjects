using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace ExamTicket11
{
    class Program
    {
        static void Main(string[] args)
        {
            //1)Mutex для синхронизации файла
            Console.WriteLine("1)Синхронизация доступа к файлу");
            
            string filePath = "shared_file.txt";
            
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "Изначальный текст\n");

            // замок мьютекс
            using (Mutex mutex = new Mutex(false, "Global\\FileMutex_Exam11"))
            {
                Console.WriteLine("Ожидаем доступ к файлу (макс. 3 сек)...");
                
                // мьютекс с таймаутом 3000 мс
                if (mutex.WaitOne(3000))
                {
                    try
                    {
                        Console.WriteLine("Мьютекс захвачен Работаем с файлом...");
                        
                        // имитация долгой работы с файлом
                        for (int i = 0; i < 3; i++)
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine($"  ...запись данных ({i + 1}/3)");
                        }

                        File.AppendAllText(filePath, "Данные добавлены потоком\n");
                        Console.WriteLine("Данные успешно записаны в файл");
                    }
                    finally
                    {
                        // освобождение мьютекса
                        mutex.ReleaseMutex();
                        Console.WriteLine("Мьютекс освобожден");
                    }
                }
                else
                {
                    Console.WriteLine("Не удалось получить доступ к файлу за 3 секунды. Файл занят другим процессом");
                }
            }

            Console.WriteLine("\n" + new string('-', 50) + "\n");

            // 2) PLINQ (Фильтрация и сортировка)
            Console.WriteLine("2) PLINQ");

            // массив из 20 случайных чисел от 1 до 100
            Random rnd = new Random();
            int[] numbers = new int[20];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = rnd.Next(1, 100);
            }

            Console.WriteLine("Исходный массив: ");
            Console.WriteLine(string.Join(", ", numbers));

            // PLINQ запрос
            var result = numbers
                .AsParallel()               // Включаем параллелизм
                .Where(n => n % 2 == 0)     // Оставляем только четные
                .OrderBy(n => n)            // Сортируем по возрастанию
                .ToArray();                 // Преобразуем в массив

            Console.WriteLine("\nЧетные числа, отсортированные по возрастанию (PLINQ):");
            Console.WriteLine(string.Join(", ", result));

            Console.WriteLine("\nНажмите Enter для выхода");
            Console.ReadLine();
        }
    }
}
