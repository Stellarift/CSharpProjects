using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadTasksWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Text = "Модуль 3: Многопоточность и Асинхронность";
            Width = 900;
            Height = 700;
            StartPosition = FormStartPosition.CenterScreen;

            TabControl tabControl = new TabControl { Dock = DockStyle.Fill };

            tabControl.TabPages.Add(CreateTask1Tab());
            tabControl.TabPages.Add(CreateTask2And3Tab());
            tabControl.TabPages.Add(CreateTask4And5Tab());
            tabControl.TabPages.Add(CreateTask6Tab());
            tabControl.TabPages.Add(CreateTask7Tab());
            tabControl.TabPages.Add(CreateTask8Tab());

            Controls.Add(tabControl);
        }

        // Потоки с приоритетами
        private TabPage CreateTask1Tab()
        {
            TabPage page = new TabPage("Задание 1: Потоки (Приоритеты)");
            TextBox output = new TextBox { Multiline = true, Dock = DockStyle.Bottom, Height = 400, ScrollBars = ScrollBars.Vertical, ReadOnly = true };

            Button btnStart = new Button { Text = "Запустить 3 потока", Top = 20, Left = 20, Width = 150 };

            // Выбор приоритета
            ComboBox cbNum = new ComboBox { Top = 20, Left = 200, Width = 100 };
            cbNum.Items.AddRange(new object[] { "Lowest", "BelowNormal", "Normal", "AboveNormal", "Highest" });
            cbNum.SelectedIndex = 2;

            ComboBox cbLetter = new ComboBox { Top = 20, Left = 320, Width = 100 };
            cbLetter.Items.AddRange(new object[] { "Lowest", "BelowNormal", "Normal", "AboveNormal", "Highest" });
            cbLetter.SelectedIndex = 2;

            ComboBox cbSym = new ComboBox { Top = 20, Left = 440, Width = 100 };
            cbSym.Items.AddRange(new object[] { "Lowest", "BelowNormal", "Normal", "AboveNormal", "Highest" });
            cbSym.SelectedIndex = 2;

            page.Controls.Add(new Label { Text = "Приоритет чисел:", Top = 25, Left = 200, Width = 100 });
            page.Controls.Add(cbNum);
            page.Controls.Add(new Label { Text = "Приоритет букв:", Top = 25, Left = 310, Width = 100 });
            page.Controls.Add(cbLetter);
            page.Controls.Add(new Label { Text = "Приоритет символов:", Top = 25, Left = 420, Width = 110 });
            page.Controls.Add(cbSym);

            btnStart.Click += (s, e) =>
            {
                output.Clear();
                output.AppendText("Запуск потоков...\n");

                Thread t1 = new Thread(() => { for (int i = 0; i < 10; i++) { AppendText(output, $"Число {i}\n"); Thread.Sleep(50); } }) { Priority = (ThreadPriority)Enum.Parse(typeof(ThreadPriority), cbNum.SelectedItem.ToString()) };
                Thread t2 = new Thread(() => { for (char c = 'A'; c <= 'J'; c++) { AppendText(output, $"Буква {c}\n"); Thread.Sleep(50); } }) { Priority = (ThreadPriority)Enum.Parse(typeof(ThreadPriority), cbLetter.SelectedItem.ToString()) };
                Thread t3 = new Thread(() => { string symbols = "!@#$%^&*()"; foreach (char c in symbols) { AppendText(output, $"Символ {c}\n"); Thread.Sleep(50); } }) { Priority = (ThreadPriority)Enum.Parse(typeof(ThreadPriority), cbSym.SelectedItem.ToString()) };

                t1.Start(); t2.Start(); t3.Start();
            };

            page.Controls.Add(btnStart);
            page.Controls.Add(output);
            return page;
        }

        // Копирование файла с паузой
        private TabPage CreateTask2And3Tab()
        {
            TabPage page = new TabPage("Задания 2-3: Копирование файла");
            TextBox sourceBox = new TextBox { Top = 20, Left = 150, Width = 400 };
            TextBox destBox = new TextBox { Top = 60, Left = 150, Width = 400 };
            Button btnSource = new Button { Text = "Исходный файл", Top = 20, Left = 10, Width = 130 };
            Button btnDest = new Button { Text = "Куда копировать", Top = 60, Left = 10, Width = 130 };

            ProgressBar pb = new ProgressBar { Top = 110, Left = 10, Width = 540, Height = 30 };
            Button btnStart = new Button { Text = "Старт", Top = 150, Left = 10, Width = 100 };
            Button btnPause = new Button { Text = "Пауза", Top = 150, Left = 120, Width = 100, Enabled = false };
            Button btnStop = new Button { Text = "Стоп", Top = 150, Left = 230, Width = 100, Enabled = false };

            bool isPaused = false;
            bool isStopped = false;
            Thread copyThread = null;

            btnSource.Click += (s, e) => { using OpenFileDialog ofd = new OpenFileDialog(); if (ofd.ShowDialog() == DialogResult.OK) sourceBox.Text = ofd.FileName; };
            btnDest.Click += (s, e) => { using FolderBrowserDialog fbd = new FolderBrowserDialog(); if (fbd.ShowDialog() == DialogResult.OK) destBox.Text = fbd.SelectedPath; };

            btnStart.Click += (s, e) =>
            {
                if (!File.Exists(sourceBox.Text)) { MessageBox.Show("Выберите файл!"); return; }
                if (!Directory.Exists(destBox.Text)) { MessageBox.Show("Выберите папку!"); return; }

                isPaused = false;
                isStopped = false;
                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnStop.Enabled = true;
                btnSource.Enabled = false;
                btnDest.Enabled = false;
                pb.Value = 0;

                copyThread = new Thread(() =>
                {
                    try
                    {
                        using FileStream src = new FileStream(sourceBox.Text, FileMode.Open, FileAccess.Read);
                        using FileStream dst = new FileStream(Path.Combine(destBox.Text, Path.GetFileName(sourceBox.Text)), FileMode.Create, FileAccess.Write);

                        byte[] buffer = new byte[1024];
                        int bytesRead;
                        long totalRead = 0;

                        while ((bytesRead = src.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            if (isStopped) break;
                            while (isPaused && !isStopped) Thread.Sleep(100);

                            dst.Write(buffer, 0, bytesRead);
                            totalRead += bytesRead;

                            int progress = (int)((double)totalRead / src.Length * 100);
                            pb.Invoke(new Action(() => pb.Value = progress));
                        }
                    }
                    catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
                    finally
                    {
                        btnStart.Invoke(new Action(() => btnStart.Enabled = true));
                        btnPause.Invoke(new Action(() => btnPause.Enabled = false));
                        btnStop.Invoke(new Action(() => btnStop.Enabled = false));
                        btnSource.Invoke(new Action(() => btnSource.Enabled = true));
                        btnDest.Invoke(new Action(() => btnDest.Enabled = true));
                        if (isStopped) MessageBox.Show("Копирование остановлено.");
                        else MessageBox.Show("Копирование завершено");
                    }
                });
                copyThread.IsBackground = true;
                copyThread.Start();
            };

            btnPause.Click += (s, e) => { isPaused = !isPaused; btnPause.Text = isPaused ? "Продолжить" : "Пауза"; };
            btnStop.Click += (s, e) => { isStopped = true; isPaused = false; };

            page.Controls.AddRange(new Control[] { btnSource, btnDest, sourceBox, destBox, pb, btnStart, btnPause, btnStop });
            return page;
        }

        // Копирование папки
        private TabPage CreateTask4And5Tab()
        {
            TabPage page = new TabPage("Задания 4-5: Копирование папки");
            TextBox sourceBox = new TextBox { Top = 20, Left = 150, Width = 400 };
            TextBox destBox = new TextBox { Top = 60, Left = 150, Width = 400 };
            Button btnSource = new Button { Text = "Исходная папка", Top = 20, Left = 10, Width = 130 };
            Button btnDest = new Button { Text = "Куда копировать", Top = 60, Left = 10, Width = 130 };

            ProgressBar pb = new ProgressBar { Top = 110, Left = 10, Width = 540, Height = 30 };
            Button btnStart = new Button { Text = "Старт", Top = 150, Left = 10, Width = 100 };
            Button btnStop = new Button { Text = "Стоп", Top = 150, Left = 120, Width = 100, Enabled = false };

            bool isStopped = false;

            btnSource.Click += (s, e) => { using FolderBrowserDialog fbd = new FolderBrowserDialog(); if (fbd.ShowDialog() == DialogResult.OK) sourceBox.Text = fbd.SelectedPath; };
            btnDest.Click += (s, e) => { using FolderBrowserDialog fbd = new FolderBrowserDialog(); if (fbd.ShowDialog() == DialogResult.OK) destBox.Text = fbd.SelectedPath; };

            btnStart.Click += (s, e) =>
            {
                if (!Directory.Exists(sourceBox.Text)) { MessageBox.Show("Выберите исходную папку!"); return; }
                if (!Directory.Exists(destBox.Text)) { MessageBox.Show("Выберите папку назначения!"); return; }

                isStopped = false;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                pb.Value = 0;

                Task.Run(() =>
                {
                    try
                    {
                        string[] files = Directory.GetFiles(sourceBox.Text, "*.*", SearchOption.AllDirectories);
                        int totalFiles = files.Length;
                        int copiedFiles = 0;

                        foreach (var file in files)
                        {
                            if (isStopped) break;

                            string relativePath = file.Substring(sourceBox.Text.Length).TrimStart('\\');
                            string destFile = Path.Combine(destBox.Text, relativePath);
                            Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                            File.Copy(file, destFile, true);
                            copiedFiles++;

                            int progress = (int)((double)copiedFiles / totalFiles * 100);
                            pb.Invoke(new Action(() => pb.Value = progress));
                        }
                    }
                    catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
                    finally
                    {
                        btnStart.Invoke(new Action(() => btnStart.Enabled = true));
                        btnStop.Invoke(new Action(() => btnStop.Enabled = false));
                        if (isStopped) MessageBox.Show("Копирование папки остановлено.");
                        else MessageBox.Show("Папка скопирована!");
                    }
                });
            };

            btnStop.Click += (s, e) => { isStopped = true; };

            page.Controls.AddRange(new Control[] { btnSource, btnDest, sourceBox, destBox, pb, btnStart, btnStop });
            return page;
        }

        // Факториал (Асинхронно) 
        private TabPage CreateTask6Tab()
        {
            TabPage page = new TabPage("Задание 6: Факториал");
            TextBox input = new TextBox { Top = 20, Left = 150, Width = 100 };
            Button btnCalc = new Button { Text = "Вычислить", Top = 20, Left = 260, Width = 100 };
            Label result = new Label { Top = 60, Left = 20, Width = 400, Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold) };

            btnCalc.Click += async (s, e) =>
            {
                if (!int.TryParse(input.Text, out int n)) { MessageBox.Show("Введите число!"); return; }
                result.Text = "Вычисление...";
                btnCalc.Enabled = false;

                // Асинхронный расчет в фоне, чтобы окно не зависало
                long res = await Task.Run(() =>
                {
                    long f = 1;
                    for (int i = 1; i <= n; i++)
                    {
                        f *= i;
                        Thread.Sleep(100); // Имитация сложности
                    }
                    return f;
                });

                result.Text = $"Факториал {n}! = {res}";
                btnCalc.Enabled = true;
            };

            page.Controls.Add(new Label { Text = "Введите число:", Top = 25, Left = 20, Width = 120 });
            page.Controls.Add(input);
            page.Controls.Add(btnCalc);
            page.Controls.Add(result);
            return page;
        }

        // Степень числа (Асинхронно)
        private TabPage CreateTask7Tab()
        {
            TabPage page = new TabPage("Задание 7: Степень");
            TextBox numInput = new TextBox { Top = 20, Left = 150, Width = 100 };
            TextBox powInput = new TextBox { Top = 60, Left = 150, Width = 100 };
            Button btnCalc = new Button { Text = "Вычислить", Top = 20, Left = 260, Width = 100 };
            Label result = new Label { Top = 100, Left = 20, Width = 400, Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold) };

            btnCalc.Click += async (s, e) =>
            {
                if (!double.TryParse(numInput.Text, out double num) || !double.TryParse(powInput.Text, out double pow)) { MessageBox.Show("Введите числа!"); return; }
                result.Text = "Вычисление...";
                btnCalc.Enabled = false;

                double res = await Task.Run(() =>
                {
                    Thread.Sleep(2000); // Имитация
                    return Math.Pow(num, pow);
                });

                result.Text = $"{num} в степени {pow} = {res}";
                btnCalc.Enabled = true;
            };

            page.Controls.Add(new Label { Text = "Число:", Top = 25, Left = 20, Width = 120 });
            page.Controls.Add(numInput);
            page.Controls.Add(new Label { Text = "Степень:", Top = 65, Left = 20, Width = 120 });
            page.Controls.Add(powInput);
            page.Controls.Add(btnCalc);
            page.Controls.Add(result);
            return page;
        }

        // Подсчет символов
        private TabPage CreateTask8Tab()
        {
            TabPage page = new TabPage("Задание 8: Текст");
            TextBox input = new TextBox { Multiline = true, Top = 20, Left = 20, Width = 500, Height = 200 };
            Button btnCount = new Button { Text = "Подсчитать", Top = 230, Left = 20, Width = 100 };
            Label result = new Label { Top = 270, Left = 20, Width = 500, Height = 100, Font = new System.Drawing.Font("Arial", 12) };

            btnCount.Click += async (s, e) =>
            {
                string text = input.Text;
                if (string.IsNullOrEmpty(text)) return;
                btnCount.Enabled = false;

                var counts = await Task.Run(() =>
                {
                    int vowels = 0, consonants = 0, symbols = 0;
                    string vowelsStr = "aeiouyаеёиоуыэюя";
                    string consonantsStr = "bcdfghjklmnpqrstvwxzбвгджзйклмнпрстфхцчшщ";

                    foreach (char c in text.ToLower())
                    {
                        if (vowelsStr.Contains(c)) vowels++;
                        else if (consonantsStr.Contains(c)) consonants++;
                        else if (!char.IsWhiteSpace(c)) symbols++;
                    }
                    return (vowels, consonants, symbols);
                });

                result.Text = $"Гласных: {counts.vowels}\nСогласных: {counts.consonants}\nСимволов: {counts.symbols}";
                btnCount.Enabled = true;
            };

            page.Controls.Add(input);
            page.Controls.Add(btnCount);
            page.Controls.Add(result);
            return page;
        }

        // Вспомогательный метод
        private void AppendText(TextBox box, string text)
        {
            if (box.InvokeRequired)
                box.Invoke(new Action(() => box.AppendText(text)));
            else
                box.AppendText(text);
        }
    }
}