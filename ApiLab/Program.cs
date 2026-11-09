using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ApiLab
{
    class Program
    {
        // Импорт функций Windows API
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Использование унаследованного кода Windows API");
                Console.WriteLine("1 - MessageBox 'Hello, World!'");
                Console.WriteLine("2 - Игра 'Угадай число'");
                Console.WriteLine("3 - Закрыть Блокнот");
                Console.WriteLine("4 - Выводить время в заголовке Блокнота");
                Console.WriteLine("0 - Выход");
                Console.Write("\nВыберите задание: ");

                string? choice = Console.ReadLine();

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

        // 1 задание
        static void Task1()
        {
            MessageBox(IntPtr.Zero, "Hello, World!", "Задание 1", 0);
        }

        // 2 задание
        static void Task2()
        {
            bool playAgain = true;
            while (playAgain)
            {
                int min = 0, max = 100;
                int guess = 50;
                int attempts = 0;
                bool guessed = false;

                while (!guessed)
                {
                    attempts++;
                    string msg = $"Твое число равно {guess}?\n\n" +
                                 "Нажми 'Да' если угадал.\n" +
                                 "Нажми 'Нет' и укажи в консоли: 1 - больше, 2 - меньше.";

                    // 4 = MB_YESNO (Да - 6, Нет - 7)
                    int result = MessageBox(IntPtr.Zero, msg, $"Попытка {attempts}", 4);

                    if (result == 6) // IDYES
                    {
                        guessed = true;
                        MessageBox(IntPtr.Zero, $"Угадал за {attempts} попыток!", "Победа", 0);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Компьютер предполагает: {guess}");
                        Console.WriteLine("Введите 1, если загаданное число больше");
                        Console.WriteLine("Введите 2, если загаданное число меньше");
                        
                        string? input = Console.ReadLine();
                        if (input == "1") min = guess + 1;
                        else if (input == "2") max = guess - 1;

                        if (min > max)
                        {
                            MessageBox(IntPtr.Zero, "Такого числа не существует.", "Ошибка", 0);
                            break;
                        }
                        guess = (min + max) / 2;
                    }
                }

                int again = MessageBox(IntPtr.Zero, "Сыграем еще раз?", "Повтор", 4);
                playAgain = (again == 6);
            }
        }

        // 3 задание
        static void Task3()
        {
            IntPtr notepadHandle = FindWindow("Notepad", null!);

            if (notepadHandle == IntPtr.Zero)
            {
                Console.WriteLine("Блокнот не запущен Сначала откройте notepad.exe.");
                return;
            }

            Console.WriteLine("Окно Блокнота найдено. Отправляем команду на закрытие...");

            const uint WM_CLOSE = 0x0010;
            SendMessage(notepadHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            Console.WriteLine("Сообщение WM_CLOSE отправлено.");
        }

        // 4 задание
        static void Task4()
        {
            IntPtr notepadHandle = FindWindow("Notepad", null!);

            if (notepadHandle == IntPtr.Zero)
            {
                Console.WriteLine("Блокнот не запущен. Откройте notepad.exe и запустите программу снова.");
                return;
            }

            Console.WriteLine("Окно найдено. Обновляем заголовок каждую секунду (нажмите Ctrl+C для выхода)...");
            const uint WM_SETTEXT = 0x000C;

            while (true)
            {
                string timeString = "Время: " + DateTime.Now.ToString("HH:mm:ss");
                
                SendMessage(notepadHandle, WM_SETTEXT, IntPtr.Zero, timeString);
                
                Thread.Sleep(1000);
            }
        }
    }
}
