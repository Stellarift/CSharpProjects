using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncGreetingClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync("127.0.0.1", 8890);
                using (NetworkStream stream = client.GetStream())
                {
                    string message = "Привет, сервер! (async)";
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    var remoteIp = client.Client.RemoteEndPoint?.ToString()?.Split(':')[0];
                    Console.WriteLine($"В {DateTime.Now:HH:mm} от [{remoteIp}] получена строка: {response}");
                }
            }
        }
    }
}
