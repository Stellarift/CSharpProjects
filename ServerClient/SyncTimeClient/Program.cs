using System;
using System.Net.Sockets;
using System.Text;

namespace SyncTimeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Что запросить? (time/date): ");
            string? request = Console.ReadLine()?.Trim().ToLower();

            using (TcpClient client = new TcpClient())
            {
                client.Connect("127.0.0.1", 8889);
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(request ?? "");
                    stream.Write(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Ответ сервера: {response}");
                }
            }
        }
    }
}
