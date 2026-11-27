using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuiGreeting
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
            // Обновление UI должно происходить в основном потоке
            Dispatcher.Post(() => TxtLog.Text += message + "\n");
        }

        // 1. Запуск сервера
        private async void BtnStartServer_Click(object? sender, RoutedEventArgs e)
        {
            Log("Сервер запущен на порту 8888. Ожидание клиента...");
            
            _server = new TcpListener(IPAddress.Any, 8888);
            _server.Start();

            // Ждем клиента в фоне, чтобы не вешать окно
            _client = await _server.AcceptTcpClientAsync();
            _stream = _client.GetStream();
            
            Log($"Клиент подключился: {_client.Client.RemoteEndPoint}");
            
            // Запускаем чтение сообщений в фоне
            _ = ReadMessagesAsync();
        }

        // 2. Подключение как клиент
        private async void BtnConnect_Click(object? sender, RoutedEventArgs e)
        {
            Log("Подключение к серверу 127.0.0.1:8888...");
            
            _client = new TcpClient();
            try
            {
                await _client.ConnectAsync("127.0.0.1", 8888);
                _stream = _client.GetStream();
                Log("Успешно подключено к серверу!");
                
                // Запускаем чтение сообщений в фоне
                _ = ReadMessagesAsync();
            }
            catch (Exception ex)
            {
                Log($"Ошибка подключения: {ex.Message}");
            }
        }

        // 3. Отправка сообщения
        private async void BtnSend_Click(object? sender, RoutedEventArgs e)
        {
            if (_stream == null || string.IsNullOrWhiteSpace(TxtMessage.Text))
                return;

            string msg = TxtMessage.Text;
            TxtMessage.Text = ""; // Очищаем поле ввода
            
            byte[] data = Encoding.UTF8.GetBytes(msg);
            await _stream.WriteAsync(data, 0, data.Length);
            
            Log($"Вы отправили: {msg}");
            
            if (msg.Trim().ToLower() == "bye")
            {
                Log("Соединение закрыто (вы отправили Bye).");
                CloseConnection();
            }
        }

        // 4. Асинхронное чтение входящих сообщений
        private async Task ReadMessagesAsync()
        {
            if (_stream == null) return;

            byte[] buffer = new byte[1024];
            try
            {
                while (true)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Соединение закрыто

                    string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Log($"Получено: {msg}");

                    if (msg.Trim().ToLower() == "bye")
                    {
                        Log("Собеседник попрощался. Соединение закрыто.");
                        CloseConnection();
                        break;
                    }
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