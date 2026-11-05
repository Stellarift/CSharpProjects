using System;
using System.Diagnostics;

namespace ProcessLab
{
    public static class KillTimeout
    {
        public static void Run()
        {
            Console.WriteLine("3) sleep 60 + WaitForExit(3000) + Kill()");

            var psi = new ProcessStartInfo
            {
                FileName        = "/bin/sleep",
                Arguments       = "60",
                UseShellExecute = false,
                CreateNoWindow  = true
            };

            using var proc = Process.Start(psi)!;
            Console.WriteLine($"Процесс запущен (PID={proc.Id}). Ожидаем 3 секунды...");

            if (!proc.WaitForExit(3000))
            {
                Console.WriteLine("Процесс не завершился за 3 с. Принудительное завершение (Kill)...");
                proc.Kill(entireProcessTree: true);
                proc.WaitForExit();
                Console.WriteLine("Процесс принудительно завершён");
            }
            else
            {
                Console.WriteLine("Процесс завершился сам");
            }

            Console.WriteLine($"HasExited={proc.HasExited}, ExitCode={proc.ExitCode}");
        }
    }
}