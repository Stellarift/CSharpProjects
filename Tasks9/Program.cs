namespace PracticeModule2;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1 - Чётные, нечётные, уникальные");
            Console.WriteLine("2 - Меньше заданного параметра");
            Console.WriteLine("3 - Поиск последовательности из 3 чисел");
            Console.WriteLine("4 - Общие элементы двух массивов без повторений");
            Console.WriteLine("5 - Минимум и максимум в двумерном массиве");
            Console.WriteLine("6 - Количество слов в предложении");
            Console.WriteLine("7 - Перевернуть каждое слово");
            Console.WriteLine("8 - Количество гласных букв");
            Console.WriteLine("9 - Поиск подстроки в строке");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");
            
            string? choice = Console.ReadLine();
            Console.WriteLine();
            
            switch (choice)
            {
                case "1": Task1();
                 break;
                case "2": Task2();
                 break;
                case "3": Task3();
                 break;
                case "4": Task4();
                 break;
                case "5": Task5();
                 break;
                case "6": Task6();
                 break;
                case "7": Task7();
                 break;
                case "8": Task8();
                 break;
                case "9": Task9();
                 break;
                case "0":
                 return;
                default: Console.WriteLine("Неверный ввод");
                 break;
            }
        }
    }

    static void Task1()
    {
        int[] arr = { 1, 2, 2, 3, 4, 4, 5, 6, 6, 7 };
        Console.WriteLine("Массив:");
        PrintArr(arr);

        int even = 0, odd = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] % 2 == 0) even++;
            else odd++;
        }

        int unique = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            bool isUnique = true;
            for (int j = 0; j < arr.Length; j++)
            {
                if (i != j && arr[i] == arr[j])
                {
                    isUnique = false;
                    break;
                }
            }
            if (isUnique) unique++;
        }

        Console.WriteLine($"Чётных: {even}");
        Console.WriteLine($"Нечётных: {odd}");
        Console.WriteLine($"Уникальных: {unique}");
    }

    static void Task2()
    {
        int[] arr = { 3, 7, 1, 9, 2, 8, 4, 6, 5 };
        Console.WriteLine("Массив:");
        PrintArr(arr);

        Console.Write("Введите число: ");
        if (!int.TryParse(Console.ReadLine(), out int num))
        {
            Console.WriteLine("Ошибка");
            return;
        }

        int count = 0;
        for (int i = 0; i < arr.Length; i++)
            if (arr[i] < num)
                count++;

        Console.WriteLine($"Меньше {num}: {count}");
    }

    static void Task3()
{
    int[] arr = { 7, 6, 5, 3, 4, 7, 6, 5, 8, 7, 6, 5 };
    Console.WriteLine("Массив:");
    PrintArr(arr);

    Console.Write("Введите 3 числа через пробел: ");
    string? line = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(line))
    {
        Console.WriteLine("Ошибка ввода");
        return;
    }

    string[] input = line.Split();
    if (input.Length != 3)
    {
        Console.WriteLine("Нужно 3 числа");
        return;
    }

    if (!int.TryParse(input[0], out int a) ||
        !int.TryParse(input[1], out int b) ||
        !int.TryParse(input[2], out int c))
    {
        Console.WriteLine("Ошибка ввода");
        return;
    }

    int count = 0;
    for (int i = 0; i <= arr.Length - 3; i++)
    {
        if (arr[i] == a && arr[i + 1] == b && arr[i + 2] == c)
            count++;
    }

    Console.WriteLine($"Последовательность {a} {b} {c} встречается {count} раз");
}

    static void Task4()
    {
        int[] arr1 = { 1, 2, 3, 4, 5, 6, 7 };
        int[] arr2 = { 5, 6, 7, 8, 9, 10 };
        Console.WriteLine("Первый массив:");
        PrintArr(arr1);
        Console.WriteLine("Второй массив:");
        PrintArr(arr2);

        int[] temp = new int[Math.Min(arr1.Length, arr2.Length)];
        int idx = 0;

        for (int i = 0; i < arr1.Length; i++)
        {
            bool inArr2 = false;
            for (int j = 0; j < arr2.Length; j++)
            {
                if (arr1[i] == arr2[j])
                {
                    inArr2 = true;
                    break;
                }
            }

            if (inArr2)
            {
                bool already = false;
                for (int k = 0; k < idx; k++)
                {
                    if (temp[k] == arr1[i])
                    {
                        already = true;
                        break;
                    }
                }
                if (!already)
                    temp[idx++] = arr1[i];
            }
        }

        int[] result = new int[idx];
        for (int i = 0; i < idx; i++)
            result[i] = temp[i];

        Console.WriteLine("Общие элементы:");
        PrintArr(result);
    }

    static void Task5()
    {
        int[,] matrix = {
            { 3, 8, 1 },
            { 9, 2, 7 },
            { 4, 6, 5 }
        };

        Console.WriteLine("Матрица:");
        PrintMat(matrix);

        int min = matrix[0, 0];
        int max = matrix[0, 0];

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] < min) min = matrix[i, j];
                if (matrix[i, j] > max) max = matrix[i, j];
            }
        }

        Console.WriteLine($"Минимум: {min}");
        Console.WriteLine($"Максимум: {max}");
    }

    static void Task6()
    {
        Console.Write("Введите предложение: ");
        string? text = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("0 слов");
            return;
        }

        string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Console.WriteLine($"Количество слов: {words.Length}");
    }

    static void Task7()
    {
        Console.Write("Введите предложение: ");
        string? text = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Пусто");
            return;
        }

        string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        for (int i = 0; i < words.Length; i++)
        {
            char[] chars = words[i].ToCharArray();
            for (int j = 0, k = chars.Length - 1; j < k; j++, k--)
            {
                char temp = chars[j];
                chars[j] = chars[k];
                chars[k] = temp;
            }
            words[i] = new string(chars);
        }

        Console.WriteLine("Результат: " + string.Join(" ", words));
    }

    static void Task8()
    {
        Console.Write("Введите предложение: ");
        string? text = Console.ReadLine();

        if (string.IsNullOrEmpty(text))
        {
            Console.WriteLine("0");
            return;
        }

        string vowels = "аеёиоуыэюяАЕЁИОУЫЭЮЯaeiouyAEIOUY";
        int count = 0;

        for (int i = 0; i < text.Length; i++)
        {
            for (int j = 0; j < vowels.Length; j++)
            {
                if (text[i] == vowels[j])
                {
                    count++;
                    break;
                }
            }
        }

        Console.WriteLine($"Гласных букв: {count}");
    }

    static void Task9()
    {
        Console.Write("Введите строку: ");
        string? text = Console.ReadLine();
        
        Console.Write("Введите подстроку для поиска: ");
        string? search = Console.ReadLine();

        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(search))
        {
            Console.WriteLine("0");
            return;
        }

        int count = 0;
        int pos = 0;

        while (pos <= text.Length - search.Length)
        {
            bool found = true;
            for (int i = 0; i < search.Length; i++)
            {
                if (text[pos + i] != search[i])
                {
                    found = false;
                    break;
                }
            }
            if (found)
            {
                count++;
                pos += search.Length;
            }
            else
            {
                pos++;
            }
        }

        Console.WriteLine($"Результат поиска: {count}");
    }

    static void PrintArr(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
            Console.Write(arr[i] + " ");
        Console.WriteLine();
    }

    static void PrintMat(int[,] mat)
    {
        for (int i = 0; i < mat.GetLength(0); i++)
        {
            for (int j = 0; j < mat.GetLength(1); j++)
                Console.Write(mat[i, j] + " ");
            Console.WriteLine();
        }
    }
}