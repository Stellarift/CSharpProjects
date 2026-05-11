namespace MyFirstCSharp;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1 - Задание 1 (Fizz Buzz)");
            Console.WriteLine("2 - Задание 2 (Процент от числа)");
            Console.WriteLine("3 - Задание 3 (Четыре цифры в число)");
            Console.WriteLine("4 - Задание 4 (Обмен цифр в шестизначном числе)");
            Console.WriteLine("5 - Задание 5 (Дата: сезон и день недели)");
            Console.WriteLine("6 - Задание 6 (Чётные числа в диапазоне)");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");
            
            string? choice = Console.ReadLine();
            Console.WriteLine();
            
            switch (choice)
            {
                case "1": Task1_FizzBuzz(); 
                 break;
                case "2": Task2_Percentage();
                 break;
                case "3": Task3_DigitsToNumber(); 
                 break;
                case "4": Task4_SwapDigits();
                 break;
                case "5": Task5_DateInfo();
                 break;
                case "6": Task6_EvenNumbersInRange();
                 break;
                case "0":
                 return;
                default: Console.WriteLine("Неверно");
                 break;
            }
        }
    }

    //Fizz Buzz
    static void Task1_FizzBuzz()
    {
        Console.Write("Введите число от 1 до 100: ");
        if (!int.TryParse(Console.ReadLine(), out int num) || num < 1 || num > 100)
        {
            Console.WriteLine("Число должно быть в диапазоне от 1 до 100");
            return;
        }

        if (num % 3 == 0 && num % 5 == 0)
            Console.WriteLine("Fizz Buzz");
        else if (num % 3 == 0)
            Console.WriteLine("Fizz");
        else if (num % 5 == 0)
            Console.WriteLine("Buzz");
        else
            Console.WriteLine(num);
    }

    //Нахождение процента от числа
    static void Task2_Percentage()
    {
        Console.Write("Введите число: ");
        if (!double.TryParse(Console.ReadLine(), out double number))
        {
            Console.WriteLine("Ошибка ввода числа");
            return;
        }

        Console.Write("Введите процент: ");
        if (!double.TryParse(Console.ReadLine(), out double percent))
        {
            Console.WriteLine("Ошибка ввода процента");
            return;
        }

        double result = number * percent / 100;
        Console.WriteLine($"{percent}% от {number} = {result}");
    }

    //Четыре цифры в одно число
    static void Task3_DigitsToNumber()
    {
        int[] digits = new int[4];
        for (int i = 0; i < 4; i++)
        {
            Console.Write($"Введите цифру {i + 1}: ");
            if (!int.TryParse(Console.ReadLine(), out int digit) || digit < 0 || digit > 9)
            {
                Console.WriteLine("Нужно ввести цифру от 0 до 9");
                return;
            }
            digits[i] = digit;
        }

        int result = digits[0] * 1000 + digits[1] * 100 + digits[2] * 10 + digits[3];
        Console.WriteLine($"Сформированное число: {result}");
    }

    //Обмен цифр в шестизначном числе
    static void Task4_SwapDigits()
    {
        Console.Write("Введите шестизначное число: ");
        if (!int.TryParse(Console.ReadLine(), out int number) || number < 100000 || number > 999999)
        {
            Console.WriteLine("Нужно ввести именно шестизначное число.");
            return;
        }

        Console.Write("Введите первый номер разряда (от 1 до 6, слева направо): ");
        if (!int.TryParse(Console.ReadLine(), out int pos1) || pos1 < 1 || pos1 > 6)
        {
            Console.WriteLine("Номер разряда должен быть от 1 до 6.");
            return;
        }

        Console.Write("Введите второй номер разряда (от 1 до 6): ");
        if (!int.TryParse(Console.ReadLine(), out int pos2) || pos2 < 1 || pos2 > 6)
        {
            Console.WriteLine("Номер разряда должен быть от 1 до 6.");
            return;
        }

        char[] digits = number.ToString().ToCharArray();
        
        (digits[pos1 - 1], digits[pos2 - 1]) = (digits[pos2 - 1], digits[pos1 - 1]);
        
        int result = int.Parse(new string(digits));
        Console.WriteLine($"Результат обмена: {result}");
    }

    //Сезон и день недели по дате
    static void Task5_DateInfo()
    {
        Console.Write("Введите дату (например, 22.12.2021): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
        {
            Console.WriteLine("Неверный формат даты");
            return;
        }

        // определение сезона
        string season = date.Month switch
        {
            12 or 1 or 2 => "Winter",
            3 or 4 or 5 => "Spring",
            6 or 7 or 8 => "Summer",
            9 or 10 or 11 => "Autumn",
            _ => "Unknown"
        };

        // Определение дня недели
        string dayOfWeek = date.DayOfWeek.ToString();
        
        Console.WriteLine($"{season} {dayOfWeek}");
    }

    //Четные числа в диапазоне с нормализацией
    static void Task6_EvenNumbersInRange()
    {
        Console.Write("Введите первое число (начало диапазона): ");
        if (!int.TryParse(Console.ReadLine(), out int first))
        {
            Console.WriteLine("Ошибка ввода числа");
            return;
        }

        Console.Write("Введите второе число (конец диапазона): ");
        if (!int.TryParse(Console.ReadLine(), out int second))
        {
            Console.WriteLine("Ошибка ввода числа");
            return;
        }

        // Нормализация границ
        int start = Math.Min(first, second);
        int end = Math.Max(first, second);

        Console.WriteLine($"Чётные числа в диапазоне от {start} до {end}:");
        
        bool found = false;
        for (int i = start; i <= end; i++)
        {
            if (i % 2 == 0)
            {
                Console.Write(i + " ");
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("В этом диапазоне нет чётных чисел");
        }
        else
        {
            Console.WriteLine();
        }
    }
}