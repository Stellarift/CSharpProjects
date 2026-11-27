using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTimeServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8891);
            server.Start();
            Console.WriteLine("Асинхронный сервер времени запущен...");

            using (TcpClient client = await server.AcceptTcpClientAsync())
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim().ToLower();

                string response = request switch
                {
                    "time" => DateTime.Now.ToString("HH:mm:ss"),
                    "date" => DateTime.Now.ToString("dd.MM.yyyy"),
                    _ => "Неизвестный запрос"
                };

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
            server.Stop();
            Console.WriteLine("Сервер завершил работу.");
        }
    }
}
