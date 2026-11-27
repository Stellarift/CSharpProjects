using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SyncGreetingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Слушаем на порту 8888
            TcpListener server = new TcpListener(IPAddress.Any, 8888);
            server.Start();
            Console.WriteLine("Сервер запущен. Ожидание клиента...");

            // Синхронное принятие клиента
            using (TcpClient client = server.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            {
                IPEndPoint? remoteIp = (IPEndPoint?)client.Client.RemoteEndPoint;
                
                // Читаем сообщение
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                
                Console.WriteLine($"В {DateTime.Now:HH:mm} от [{remoteIp?.Address}] получена строка: {receivedMessage}");

                // Отправляем ответ
                string response = "Привет, клиент!";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }

            server.Stop();
            Console.WriteLine("Сервер завершил работу.");
        }
    }
}