using System;
using System.Diagnostics;

namespace ProcessLab
{
    public static class RedirectOutput
    {
        public static void Run()
        {
            Console.WriteLine("2) ping 8.8.8.8 -c 4 через /bin/bash");

            var psi = new ProcessStartInfo
            {
                FileName               = "/bin/bash",
                Arguments              = "-c \"ping -c 4 8.8.8.8\"",
                UseShellExecute        = false,
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                CreateNoWindow         = true
            };

            using var proc = Process.Start(psi)!;

            string output = proc.StandardOutput.ReadToEnd();
            string error  = proc.StandardError.ReadToEnd();

            Console.WriteLine("stdout");
            Console.WriteLine(output);

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine("stderr");
                Console.WriteLine(error);
            }

            proc.WaitForExit();
            Console.WriteLine($"Код возврата: {proc.ExitCode}");
        }
    }
}