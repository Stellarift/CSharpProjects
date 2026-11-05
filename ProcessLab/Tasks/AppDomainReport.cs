using System;

namespace ProcessLab
{
    public static class AppDomainReport
    {
        public static void Run()
        {
            Console.WriteLine($"Текущий домен: {AppDomain.CurrentDomain.FriendlyName}\n");

            try
            {
                // создать новый домен 
                var domain = AppDomain.CreateDomain("TestDomain");
                Console.WriteLine($"Создан домен: {domain.FriendlyName}");
            }
            catch (PlatformNotSupportedException)
            {
                Console.WriteLine("AppDomain.CreateDomain не поддерживается в современном .NET\n");
                Console.WriteLine("Отчет по заданию 4 (Вариант Б)");
                Console.WriteLine("1. В .NET Framework AppDomain использовались для логической изоляции кода внутри процесса.");
                Console.WriteLine("2. Домен живет внутри процесса, разделяет с ним память и не имеет своего PID. Его создание дешевле, чем процесса.");
                Console.WriteLine("3. На смену пришел AssemblyLoadContext, а для полной изоляции теперь используют отдельные процессы.");
            }
        }
    }
}