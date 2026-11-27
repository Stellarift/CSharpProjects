using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuiTime
{
    public partial class MainWindow : Window
    {
        private TcpListener? _server;
        private TcpClient? _client;
        private NetworkStream? _stream;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Log(string message)
        {
            Dispatcher.Post(() => TxtLog.Text += message + "\n");
        }

        private async void BtnStartServer_Click(object? sender, RoutedEventArgs e)
        {
            Log("Сервер запущен на порту 8889. Ожидание клиента...");
            _server = new TcpListener(IPAddress.Any, 8889);
            _server.Start();

            _client = await _server.AcceptTcpClientAsync();
            _stream = _client.GetStream();
            Log($"Клиент подключился: {_client.Client.RemoteEndPoint}");
            
            _ = ReadMessagesAsync();
        }

        private async void BtnConnect_Click(object? sender, RoutedEventArgs e)
        {
            Log("Подключение к серверу 127.0.0.1:8889...");
            _client = new TcpClient();
            try
            {
                await _client.ConnectAsync("127.0.0.1", 8889);
                _stream = _client.GetStream();
                Log("Успешно подключено! Теперь можно запрашивать время/дату.");
                
                BtnTime.IsEnabled = true;
                BtnDate.IsEnabled = true;
                
                _ = ReadMessagesAsync();
            }
            catch (Exception ex)
            {
                Log($"Ошибка подключения: {ex.Message}");
            }
        }

        private async void BtnTime_Click(object? sender, RoutedEventArgs e)
        {
            await SendRequest("time");
        }

        private async void BtnDate_Click(object? sender, RoutedEventArgs e)
        {
            await SendRequest("date");
        }

        private async Task SendRequest(string request)
        {
            if (_stream == null) return;
            byte[] data = Encoding.UTF8.GetBytes(request);
            await _stream.WriteAsync(data, 0, data.Length);
            Log($"Запрос отправлен: {request}");
        }

        private async Task ReadMessagesAsync()
        {
            if (_stream == null) return;

            byte[] buffer = new byte[1024];
            try
            {
                while (true)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    // ИСПРАВЛЕННАЯ ЛОГИКА ЗДЕСЬ
                    string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim().ToLower();
                    string response = request switch
                    {
                        "time" => DateTime.Now.ToString("HH:mm:ss"),
                        "date" => DateTime.Now.ToString("dd.MM.yyyy"),
                        _ => "Неизвестный запрос"
                    };

                    Log($"Получен запрос: {request}. Отправляю ответ: {response}");
                    
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    await _stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    
                    // Сервер разрывает соединение после ответа
                    CloseConnection();
                    break;
                }
            }
            catch
            {
                Log("Соединение прервано.");
            }
        }

        private void CloseConnection()
        {
            _stream?.Close();
            _client?.Close();
            _server?.Stop();
            _stream = null;
            _client = null;
            _server = null;
        }
    }
}