namespace HomeworkTasks;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1 - Прямоугольник и квадраты");
            Console.WriteLine("2 - Банковский вклад");
            Console.WriteLine("3 - Вывод чисел от A до B с повторами");
            Console.WriteLine("4 - Переворот числа справа налево");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");
            
            string? choice = Console.ReadLine();
            Console.WriteLine();
            
            switch (choice)
            {
                case "1": Task1_RectangleAndSquares();
                 break;
                case "2": Task2_BankDeposit(); 
                 break;
                case "3": Task3_PrintNumbersWithRepetitions();
                 break;
                case "4": Task4_ReverseNumber();
                 break;
                case "0":
                 return;
                default: Console.WriteLine("Неверный ввод");
                 break;
            }
        }
    }

    // задача 1
    static void Task1_RectangleAndSquares()
    {
        Console.Write("Введите длину прямоугольника A: ");
        if (!int.TryParse(Console.ReadLine(), out int A) || A <= 0)
        {
            Console.WriteLine("должно быть положительным целым числом");
            return;
        }

        Console.Write("Введите ширину прямоугольника B: ");
        if (!int.TryParse(Console.ReadLine(), out int B) || B <= 0)
        {
            Console.WriteLine("должно быть положительным целым числом");
            return;
        }

        Console.Write("Введите сторону квадрата C: ");
        if (!int.TryParse(Console.ReadLine(), out int C) || C <= 0)
        {
            Console.WriteLine("должно быть положительным целым числом");
            return;
        }

        if (C > A || C > B)
        {
            Console.WriteLine("Служебное сообщение: квадрат со стороной C не помещается в прямоугольник (C больше A или B).");
            Console.WriteLine("Количество квадратов: 0");
            Console.WriteLine($"Площадь незанятой части: {A * B}");
            return;
        }

        int countWidth = A / C;
        int countHeight = B / C;
        int totalSquares = countWidth * countHeight;
        int occupiedArea = totalSquares * C * C;
        int freeArea = A * B - occupiedArea;

        Console.WriteLine($"Количество квадратов: {totalSquares}");
        Console.WriteLine($"Площадь незанятой части: {freeArea}");
    }

    //Банковский вклад
    static void Task2_BankDeposit()
    {
        double initialAmount = 10000;
        double targetAmount = 11000;
        
        Console.Write("Введите ежемесячный процент P (0 < P < 25): ");
        if (!double.TryParse(Console.ReadLine(), out double P) || P <= 0 || P >= 25)
        {
            Console.WriteLine("P должно быть в диапазоне (0, 25)");
            return;
        }

        int months = 0;
        double currentAmount = initialAmount;
        
        while (currentAmount <= targetAmount)
        {
            currentAmount += currentAmount * (P / 100);
            months++;
        }
        
        Console.WriteLine($"Количество месяцев: {months}");
        Console.WriteLine($"Итоговый размер вклада: {currentAmount:F2} руб.");
    }

    //Вывод чисел от A до B, каждое число повторяется значение раз
    static void Task3_PrintNumbersWithRepetitions()
    {
        Console.Write("Введите целое положительное число A: ");
        if (!int.TryParse(Console.ReadLine(), out int A) || A <= 0)
        {
            Console.WriteLine("A должно быть положительным целым числом");
            return;
        }

        Console.Write("Введите целое положительное число B (A < B): ");
        if (!int.TryParse(Console.ReadLine(), out int B) || B <= 0)
        {
            Console.WriteLine("B должно быть положительным целым числом");
            return;
        }

        if (A >= B)
        {
            Console.WriteLine("должно быть A < B");
            return;
        }

        for (int i = A; i <= B; i++)
        {
            for (int j = 0; j < i; j++)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
        }
    }

    //Переворот числа справа налево
    static void Task4_ReverseNumber()
    {
        Console.Write("Введите целое положительное число N: ");
        if (!int.TryParse(Console.ReadLine(), out int N) || N <= 0)
        {
            Console.WriteLine("N должно быть положительным целым числом");
            return;
        }

        int reversed = 0;
        int temp = N;
        
        while (temp > 0)
        {
            int digit = temp % 10;
            reversed = reversed * 10 + digit;
            temp /= 10;
        }
        
        Console.WriteLine($"Исходное число: {N}");
        Console.WriteLine($"Число справа налево: {reversed}");
    }
}