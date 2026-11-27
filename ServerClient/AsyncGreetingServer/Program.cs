using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncGreetingServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8890);
            server.Start();
            Console.WriteLine("Асинхронный сервер запущен...");

            using (TcpClient client = await server.AcceptTcpClientAsync())
            using (NetworkStream stream = client.GetStream())
            {
                IPEndPoint? remoteIp = (IPEndPoint?)client.Client.RemoteEndPoint;
                
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                
                Console.WriteLine($"В {DateTime.Now:HH:mm} от [{remoteIp?.Address}] получена строка: {receivedMessage}");

                string response = "Привет, клиент! (async)";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
            server.Stop();
        }
    }
}
