using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuiChat
{
    public partial class MainWindow : Window
    {
        private TcpListener? _server;
        private TcpClient? _client;
        private NetworkStream? _stream;
        private bool _isServer = false;

        private readonly string[] _serverBotPhrases = {
            "Привет! Я серверный бот.",
            "Я тебя услышал.",
            "Это очень интересно!",
            "Расскажи еще что-нибудь.",
            "Я работаю на .NET."
        };

        private readonly string[] _clientBotPhrases = {
            "Привет, я бот-клиент!",
            "Понял тебя, сервер.",
            "Хорошая погода сегодня, не так ли?",
            "Я просто программа.",
            "Давай дружить!"
        };

        private Random _random = new Random();

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
            if (!int.TryParse(TxtPort.Text, out int port)) return;
            try
            {
                _isServer = true;
                _server = new TcpListener(IPAddress.Any, port);
                _server.Start();
                Log("Сервер запущен. Ожидание клиента...");
                
                BtnStartServer.IsEnabled = false;
                BtnConnect.IsEnabled = false;
                BtnDisconnect.IsEnabled = true;

                _client = await _server.AcceptTcpClientAsync();
                _stream = _client.GetStream();
                Log("Клиент подключился!");
                
                BtnSend.IsEnabled = true;
                _ = ReadMessagesAsync();
            }
            catch (Exception ex) { Log($"Ошибка сервера: {ex.Message}"); }
        }

        private async void BtnConnect_Click(object? sender, RoutedEventArgs e)
        {
            string ip = TxtIp.Text ?? "127.0.0.1";
            if (!int.TryParse(TxtPort.Text, out int port)) return;
            try
            {
                _isServer = false;
                _client = new TcpClient();
                await _client.ConnectAsync(ip, port);
                _stream = _client.GetStream();
                Log("Успешно подключено!");
                
                BtnStartServer.IsEnabled = false;
                BtnConnect.IsEnabled = false;
                BtnDisconnect.IsEnabled = true;
                BtnSend.IsEnabled = true;

                _ = ReadMessagesAsync();
            }
            catch (Exception ex) { Log($"Ошибка подключения: {ex.Message}"); }
        }

        private void BtnDisconnect_Click(object? sender, RoutedEventArgs e)
        {
            CloseConnection();
            Log("Вы отключились.");
        }

        private async void BtnSend_Click(object? sender, RoutedEventArgs e)
        {
            if (_stream == null || string.IsNullOrWhiteSpace(TxtMessage.Text)) return;

            string msg = TxtMessage.Text;
            TxtMessage.Text = ""; 
            
            await SendMessageAsync(msg);
            Log($"[Вы]: {msg}");
            
            if (msg.Trim().ToLower() == "bye")
            {
                Log("Вы попрощались. Соединение закрыто.");
                CloseConnection();
            }
        }

        private async Task SendMessageAsync(string msg)
        {
            if (_stream == null) return;
            byte[] data = Encoding.UTF8.GetBytes(msg);
            await _stream.WriteAsync(data, 0, data.Length);
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

                    string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    
                    if (msg.Trim().ToLower() == "bye")
                    {
                        Log($"[Собеседник]: {msg}");
                        Log("Собеседник попрощался. Соединение закрыто.");
                        CloseConnection();
                        break;
                    }

                    int mode = CmbMode.SelectedIndex;
                    bool shouldReplyWithBot = false;
                    string? botReply = null;

                    if (_isServer && mode == 1)
                    {
                        shouldReplyWithBot = true;
                        botReply = _serverBotPhrases[_random.Next(_serverBotPhrases.Length)];
                    }
                    else if (!_isServer && mode == 2)
                    {
                        Log($"[Собеседник]: {msg}");
                        shouldReplyWithBot = true;
                        botReply = _clientBotPhrases[_random.Next(_clientBotPhrases.Length)];
                    }
                    else if (mode == 3)
                    {
                        Log($"[Собеседник]: {msg}");
                        shouldReplyWithBot = true;
                        botReply = _isServer 
                            ? _serverBotPhrases[_random.Next(_serverBotPhrases.Length)]
                            : _clientBotPhrases[_random.Next(_clientBotPhrases.Length)];
                    }
                    else 
                    {
                        Log($"[Собеседник]: {msg}");
                    }

                    if (shouldReplyWithBot && botReply != null)
                    {
                        await Task.Delay(1000);
                        Log($"[Бот]: {botReply}");
                        await SendMessageAsync(botReply);
                    }
                }
            }
            catch
            {
                CloseConnection();
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

            Dispatcher.Post(() => {
                BtnStartServer.IsEnabled = true;
                BtnConnect.IsEnabled = true;
                BtnDisconnect.IsEnabled = false;
                BtnSend.IsEnabled = false;
            });
        }
    }
}