using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Sync
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var tabControl = new TabControl();
            
            // Создаем три вкладки
            tabControl.Items.Add(CreateTask1Tab());
            tabControl.Items.Add(CreateTask2Tab());
            tabControl.Items.Add(CreateTask3Tab());

            // Добавляем вкладки на главный экран
            this.FindControl<Grid>("MainGrid")!.Children.Add(tabControl);
        }

        // Вспомогательный метод для безопасного обновления UI из потоков
        private void AppendText(TextBox box, string text)
        {
            Dispatcher.UIThread.Post(() => box.Text += text);
        }

        // 1)События
        private TabItem CreateTask1Tab()
        {
            var page = new TabItem { Header = "Задание 1: События" };
            
            var output = new TextBox { 
                IsReadOnly = true, 
                AcceptsReturn = true, 
                TextWrapping = TextWrapping.Wrap, 
                FontFamily = new FontFamily("Courier New"),
                Height = 400,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Avalonia.Thickness(0, 50, 0, 0)
            };

            var btnStart = new Button { Content = "Запустить", Width = 150, Height = 40, HorizontalAlignment = HorizontalAlignment.Left };

            btnStart.Click += (s, e) =>
            {
                output.Text = "Генерация чисел началась...\n";
                int[] numbers = new int[1000];
                
                ManualResetEvent generationDone = new ManualResetEvent(false);

                Thread tMax = new Thread(() => { generationDone.WaitOne(); int max = numbers.Max(); AppendText(output, $"[Поток 2] Максимум: {max}\n"); });
                Thread tMin = new Thread(() => { generationDone.WaitOne(); int min = numbers.Min(); AppendText(output, $"[Поток 3] Минимум: {min}\n"); });
                Thread tAvg = new Thread(() => { generationDone.WaitOne(); double avg = numbers.Average(); AppendText(output, $"[Поток 4] Среднее: {avg:F2}\n"); });

                tMax.Start(); tMin.Start(); tAvg.Start();

                Thread generator = new Thread(() =>
                {
                    Random rnd = new Random();
                    for (int i = 0; i < 1000; i++)
                    {
                        numbers[i] = rnd.Next(0, 5001);
                        if (i % 100 == 0) Thread.Sleep(10);
                    }
                    AppendText(output, "[Поток 1] Генерация завершена! Запускаем анализ...\n");
                    generationDone.Set();
                });

                generator.Start();
            };

            var panel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };
            panel.Children.Add(btnStart);
            panel.Children.Add(output);
            page.Content = panel;
            
            return page;
        }

        // 2) Критические секции
        private TabItem CreateTask2Tab()
        {
            var page = new TabItem { Header = "Задание 2: Файл" };
            
            var output = new TextBox { 
                IsReadOnly = true, 
                AcceptsReturn = true, 
                TextWrapping = TextWrapping.Wrap, 
                FontFamily = new FontFamily("Courier New"),
                Height = 400,
                Margin = new Avalonia.Thickness(0, 50, 0, 0)
            };

            var txtPath = new TextBox { Text = "Введите путь к .txt файлу...", Width = 400, HorizontalAlignment = HorizontalAlignment.Left };
            var btnStart = new Button { Content = "Запустить", Width = 150, Height = 40, HorizontalAlignment = HorizontalAlignment.Left };

            btnStart.Click += (s, e) =>
            {
                string filePath = txtPath.Text;
                if (!File.Exists(filePath)) { output.Text = "Файл не найден!"; return; }

                output.Text = "";
                object locker = new object();
                bool isCountingDone = false;

                Thread t1 = new Thread(() =>
                {
                    string text = File.ReadAllText(filePath);
                    int count = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    AppendText(output, $"[Поток 1] Количество предложений: {count}\n");
                    
                    Thread.Sleep(500); // Имитация долгой работы

                    lock (locker)
                    {
                        isCountingDone = true;
                        string modifiedText = text.Replace('!', '#');
                        File.WriteAllText(filePath, modifiedText);
                        AppendText(output, "[Поток 1] Замена '!' на '#' выполнена!\n");
                    }
                });

                Thread t2 = new Thread(() =>
                {
                    lock (locker)
                    {
                        if (!isCountingDone) AppendText(output, "[Поток 2] Ожидает завершения первого потока...\n");
                    }
                    string modifiedText = File.ReadAllText(filePath);
                    AppendText(output, $"[Поток 2] Файл успешно прочитан после изменений.\n");
                });

                t1.Start();
                t2.Start();
            };

            var panel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };
            panel.Children.Add(txtPath);
            panel.Children.Add(btnStart);
            panel.Children.Add(output);
            page.Content = panel;
            
            return page;
        }

        // 3) Критические секции
        private TabItem CreateTask3Tab()
        {
            var page = new TabItem { Header = "Задание 3: Массив" };
            
            var output = new TextBox { 
                IsReadOnly = true, 
                AcceptsReturn = true, 
                TextWrapping = TextWrapping.Wrap, 
                FontFamily = new FontFamily("Courier New"),
                Height = 400,
                Margin = new Avalonia.Thickness(0, 50, 0, 0)
            };

            var txtTarget = new TextBox { Text = "50", Width = 100, HorizontalAlignment = HorizontalAlignment.Left };
            var btnStart = new Button { Content = "Запустить", Width = 150, Height = 40, HorizontalAlignment = HorizontalAlignment.Left };

            btnStart.Click += (s, e) =>
            {
                if (!int.TryParse(txtTarget.Text, out int targetNumber)) { output.Text = "Введите число!"; return; }

                output.Text = "";
                int[] numbers = new int[50];
                Random rnd = new Random();
                for (int i = 0; i < 50; i++) numbers[i] = rnd.Next(0, 100);

                object locker = new object();
                bool isSorted = false;

                Thread t1 = new Thread(() =>
                {
                    AppendText(output, "[Поток 1] Начинаю сортировку...\n");
                    Array.Sort(numbers);
                    Thread.Sleep(1000); // Имитация тяжелой сортировки

                    lock (locker)
                    {
                        isSorted = true;
                        AppendText(output, "[Поток 1] Массив отсортирован!\n");
                    }
                });

                Thread t2 = new Thread(() =>
                {
                    lock (locker)
                    {
                        if (!isSorted) AppendText(output, "[Поток 2] Ожидает сортировки...\n");
                    }

                    bool found = numbers.Contains(targetNumber);
                    AppendText(output, $"[Поток 2] Ищем число {targetNumber}...\n");
                    if (found) AppendText(output, $"  --> ЧИСЛО {targetNumber} НАЙДЕНО!\n");
                    else AppendText(output, $"  --> Число {targetNumber} НЕ найдено.\n");
                    
                    AppendText(output, $"\nМассив: {string.Join(", ", numbers)}\n");
                });

                t1.Start();
                t2.Start();
            };

            var panel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };
            panel.Children.Add(new TextBlock { Text = "Число для поиска:" });
            panel.Children.Add(txtTarget);
            panel.Children.Add(btnStart);
            panel.Children.Add(output);
            page.Content = panel;
            
            return page;
        }
    }
}