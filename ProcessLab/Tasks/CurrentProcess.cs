using System;
using System.Diagnostics;
using System.Linq;

namespace ProcessLab
{
    public static class CurrentProcess
    {
        public static void Run()
        {
            Console.WriteLine("1) текущий процесс");
            Process current = Process.GetCurrentProcess();
            Console.WriteLine($"Имя процесса     : {current.ProcessName}");
            Console.WriteLine($"PID              : {current.Id}");
            Console.WriteLine($"Время запуска    : {current.StartTime}");
            Console.WriteLine($"Физ. память (MB) : {current.WorkingSet64 / 1024 / 1024}");
            Console.WriteLine($"Кол-во потоков   : {current.Threads.Count}");

            Console.WriteLine(new string('-', 60));
            Console.WriteLine("Топ-5 процессов по физической памяти");

            var top5 = Process.GetProcesses()
                .OrderByDescending(p =>
                {
                    try { return p.WorkingSet64; }
                    catch { return 0L; }
                })
                .Take(5);

            foreach (var p in top5)
            {
                try
                {
                    Console.WriteLine($"{p.ProcessName,-30} PID={p.Id,-8} {p.WorkingSet64 / 1024 / 1024} MB");
                }
                catch
                {
                    Console.WriteLine($"{p.ProcessName,-30} PID={p.Id,-8} [нет доступа]");
                }
            }
        }
    }
}