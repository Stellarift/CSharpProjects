using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTimeClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Что запросить? (time/date): ");
            string? request = Console.ReadLine()?.Trim().ToLower();

            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync("127.0.0.1", 8891);
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(request ?? "");
                    await stream.WriteAsync(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Ответ: {response}");
                }
            }
        }
    }
}
