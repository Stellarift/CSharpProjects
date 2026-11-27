using System;
using System.Net.Sockets;
using System.Text;

namespace SyncGreetingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverIp = "127.0.0.1"; // localhost
            int port = 8888;

            using (TcpClient client = new TcpClient())
            {
                // Синхронное подключение
                client.Connect(serverIp, port);
                
                using (NetworkStream stream = client.GetStream())
                {
                    // Отправляем приветствие
                    string message = "Привет, сервер!";
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    // Читаем ответ
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Получаем IP сервера
                    var remoteIp = client.Client.RemoteEndPoint?.ToString()?.Split(':')[0];
                    Console.WriteLine($"В {DateTime.Now:HH:mm} от [{remoteIp}] получена строка: {response}");
                }
            }
        }
    }
}
