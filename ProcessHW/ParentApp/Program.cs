using System;
using System.Diagnostics;
using System.IO;

namespace ParentApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string childPath = Path.Combine(baseDir, "..", "..", "..", "..", "ChildApp", "bin", "Debug", "net10.0", "ChildApp.dll");

            Console.WriteLine("Процессы");
            Console.WriteLine("1 - Запустить дочерний и дождаться (Код завершения)");
            Console.WriteLine("2 - Запустить, выбор: ждать или убить");
            Console.WriteLine("3 - Калькулятор (7, 3, +)");
            Console.WriteLine("4 - Поиск слова в файле");
            Console.Write("\nВыберите задание: ");
            
            string? choice = Console.ReadLine();

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"\"{childPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            switch (choice)
            {
                case "1":
                    psi.Arguments += " test_arg";
                    var p1 = Process.Start(psi);
                    string out1 = p1!.StandardOutput.ReadToEnd();
                    p1.WaitForExit();
                    Console.WriteLine(out1);
                    Console.WriteLine($"Дочерний процесс завершился с кодом: {p1.ExitCode}");
                    break;

                case "2":
                    psi.Arguments += " test_arg";
                    var p2 = Process.Start(psi);
                    Console.WriteLine("1 - Ждать завершения\n2 - Убить процесс");
                    string? action = Console.ReadLine(); 

                    if (action == "1")
                    {
                        string out2 = p2!.StandardOutput.ReadToEnd();
                        p2.WaitForExit();
                        Console.WriteLine(out2);
                        Console.WriteLine($"Код завершения: {p2.ExitCode}");
                    }
                    else
                    {
                        p2!.Kill();
                        Console.WriteLine("Процесс был принудительно завершен (Kill).");
                    }
                    break;

                case "3":
                    Console.Write("Введите первое число: ");
                    string? a = Console.ReadLine(); 
                    Console.Write("Введите второе число: ");
                    string? b = Console.ReadLine(); 
                    Console.Write("Введите операцию (+, -, *, /): ");
                    string? op = Console.ReadLine(); 

                    psi.Arguments += $" {a} {b} {op}";
                    var p3 = Process.Start(psi);
                    string out3 = p3!.StandardOutput.ReadToEnd();
                    p3.WaitForExit();
                    Console.WriteLine(out3);
                    break;

                case "4":
                    Console.Write("Введите полный путь к файлу: ");
                    string? filePath = Console.ReadLine(); 
                    Console.Write("Введите слово для поиска: ");
                    string? word = Console.ReadLine(); 

                    psi.Arguments += $" \"{filePath}\" {word}";
                    var p4 = Process.Start(psi);
                    string out4 = p4!.StandardOutput.ReadToEnd();
                    p4.WaitForExit();
                    Console.WriteLine(out4);
                    break;
            }
        }
    }
}