using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ThreadTasksWinForms
{
    public partial class Form1 : Form
    {
        private TextBox outputBox;
        private Button btn1, btn2, btn3, btn4, btn5, btn6, btn7;

        public Form1()
        {
            Text = "Многопоточность";
            Width = 800;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;

            // Левая панель с кнопками
            FlowLayoutPanel leftPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                Width = 220,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10),
                WrapContents = false
            };

            btn1 = CreateButton("1. Поток 0..50", () => RunTask1());
            btn2 = CreateButton("2. Поток с диапазоном", () => RunTask2());
            btn3 = CreateButton("3. Несколько потоков", () => RunTask3());
            btn4 = CreateButton("4. Поиск Min/Max/Avg", () => RunTask4());
            btn5 = CreateButton("5. Поиск + файл", () => RunTask5());
            btn6 = CreateButton("6. Числа в окне (GUI)", () => RunTask6());
            btn7 = CreateButton("7. Поиск в окне (GUI)", () => RunTask7());

            leftPanel.Controls.AddRange(new Control[] { btn1, btn2, btn3, btn4, btn5, btn6, btn7 });

            // Правая панель с выводом
            outputBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new System.Drawing.Font("Consolas", 12),
                ReadOnly = true
            };

            Controls.Add(outputBox);
            Controls.Add(leftPanel);
        }

        private Button CreateButton(string text, Action onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Width = 180,
                Height = 40,
                Margin = new Padding(0, 0, 0, 10)
            };
            btn.Click += (s, e) => onClick();
            return btn;
        }

        // Обновление UI из другого потока
        private void UpdateOutput(string text)
        {
            if (outputBox.InvokeRequired)
                outputBox.Invoke(new Action(() => outputBox.AppendText(text)));
            else
                outputBox.AppendText(text);
        }

        // Задание 1
        private void RunTask1()
        {
            outputBox.Clear();
            UpdateOutput("Задание 1\n");
            new Thread(() =>
            {
                for (int i = 0; i <= 50; i++)
                {
                    UpdateOutput($"{i} ");
                    Thread.Sleep(30);
                }
                UpdateOutput("\n");
            }).Start();
        }

        // Задание 2
        private void RunTask2()
        {
            outputBox.Clear();
            UpdateOutput("Задание 2 (10..30)\n");
            new Thread(() =>
            {
                for (int i = 10; i <= 30; i++)
                {
                    UpdateOutput($"{i} ");
                    Thread.Sleep(50);
                }
                UpdateOutput("\n");
            }).Start();
        }

        // Задание 3
        private void RunTask3()
        {
            outputBox.Clear();
            UpdateOutput("Задание 3 (3 потока)\n");
            for (int t = 0; t < 3; t++)
            {
                int threadId = t;
                new Thread(() =>
                {
                    int start = threadId * 10 + 1;
                    int end = start + 9;
                    for (int i = start; i <= end; i++)
                    {
                        UpdateOutput($"{i} ");
                        Thread.Sleep(50);
                    }
                    UpdateOutput($"[Поток {threadId + 1} готов]\n");
                }).Start();
            }
        }

        private int[] GenerateArray()
        {
            Random rnd = new Random();
            int[] arr = new int[10000];
            for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next(1, 100000);
            return arr;
        }

        // Задание 4
        private void RunTask4()
        {
            outputBox.Clear();
            UpdateOutput("Задание 4\nГенерация 10000 чисел...\n");
            int[] arr = GenerateArray();

            new Thread(() =>
            {
                int max = arr.Max();
                int min = arr.Min();
                double avg = arr.Average();

                UpdateOutput($"Минимум: {min}\nМаксимум: {max}\nСреднее: {avg:F2}\n");
            }).Start();
        }

        // Задание 5
        private void RunTask5()
        {
            outputBox.Clear();
            UpdateOutput("Задание 5\nГенерация и запись в файл...\n");
            int[] arr = GenerateArray();

            new Thread(() =>
            {
                int max = arr.Max();
                int min = arr.Min();
                double avg = arr.Average();

                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "result_threads.txt");
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine("Массив:");
                    sw.WriteLine(string.Join(", ", arr));
                    sw.WriteLine($"\nMin: {min}\nMax: {max}\nAvg: {avg:F2}");
                }

                UpdateOutput($"Минимум: {min}\nМаксимум: {max}\nСреднее: {avg:F2}\n");
                UpdateOutput($"Файл сохранен на Рабочий стол: result_threads.txt\n");
            }).Start();
        }

        // Задание 6
        private void RunTask6()
        {
            outputBox.Clear();
            UpdateOutput("Задание 6 (GUI)\n");
            new Thread(() =>
            {
                for (int i = 0; i <= 50; i++)
                {
                    UpdateOutput($"{i} ");
                    Thread.Sleep(50);
                }
                UpdateOutput("\n");
            }).Start();
        }

        // Задание 7
        private void RunTask7()
        {
            outputBox.Clear();
            UpdateOutput("Задание 7 (GUI)\nПоиск в 10000 числах...\n");
            int[] arr = GenerateArray();

            new Thread(() =>
            {
                Thread.Sleep(500);
                int max = arr.Max();
                int min = arr.Min();
                double avg = arr.Average();

                UpdateOutput($"Готово!\nМинимум: {min}\nМаксимум: {max}\nСреднее: {avg:F2}\n");
            }).Start();
        }
    }
}