using System;

namespace ProcessLab
{
    class Program
    {
        static void Main(string[] args)
        {
            string choice = args.Length > 0 ? args[0].Trim() : ReadFromMenu();
            Console.WriteLine();

            switch (choice)
            {
                case "1": CurrentProcess.Run();
                 break;
                case "2": RedirectOutput.Run();
                 break;
                case "3": KillTimeout.Run();
                 break;
                case "4": AppDomainReport.Run();
                 break;
                default:  Console.WriteLine($"Неизвестный пункт: '{choice}'. Допустимо: 1, 2, 3, 4."); break;
            }

            if (!Console.IsInputRedirected)
            {
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        static string ReadFromMenu()
        {
            Console.WriteLine("Процессы в C#\n");
            Console.WriteLine("1 - Текущий процесс + Топ-5 по памяти");
            Console.WriteLine("2 - Дочерний процесс с перенаправлением вывода (ping)");
            Console.WriteLine("3 - Ожидание с тайм-аутом и Kill()");
            Console.WriteLine("4 - Отчёт по AppDomain (вариант Б)");
            Console.Write("\nвыбор: ");
            return (Console.ReadLine() ?? "").Trim();
        }
    }
}
