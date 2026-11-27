using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SyncTimeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8889);
            server.Start();
            Console.WriteLine("Сервер времени запущен (порт 8889)...");

            using (TcpClient client = server.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim().ToLower();

                string response;
                if (request == "time")
                    response = DateTime.Now.ToString("HH:mm:ss");
                else if (request == "date")
                    response = DateTime.Now.ToString("dd.MM.yyyy");
                else
                    response = "Неизвестный запрос. Используйте 'time' или 'date'.";

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
            server.Stop();
            Console.WriteLine("Сервер разорвал соединение и завершил работу.");
        }
    }
}
