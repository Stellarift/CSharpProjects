namespace HomeworkTasks;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1 - Сжать массив (0 → -1)");
            Console.WriteLine("2 - Сначала отрицательные, потом положительные");
            Console.WriteLine("3 - Сколько раз число встречается в массиве");
            Console.WriteLine("4 - Поменять местами столбцы");
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
                case "0":
                 return;
                default: Console.WriteLine("Неверный ввод");
                 break;
            }
        }
    }

    // задача 1: удалить все 0, справа заполнить -1
    static void Task1()
    {
        int[] arr = { 1, 0, 2, 0, 3, 0, 4, 5, 0, 6 };
        Console.WriteLine("Исходный:");
        PrintArr(arr);

        int[] res = new int[arr.Length];
        int idx = 0;

        for (int i = 0; i < arr.Length; i++)
            if (arr[i] != 0)
                res[idx++] = arr[i];

        for (int i = idx; i < res.Length; i++)
            res[i] = -1;

        Console.WriteLine("Результат:");
        PrintArr(res);
    }

    // задача 2: сначала отрицательные, потом положительные (0 считается положительным)
    static void Task2()
    {
        int[] arr = { 5, -3, 0, -8, 2, -1, 7, -4, 6 };
        Console.WriteLine("Исходный:");
        PrintArr(arr);

        int[] res = new int[arr.Length];
        int idx = 0;

        for (int i = 0; i < arr.Length; i++)
            if (arr[i] < 0)
                res[idx++] = arr[i];

        for (int i = 0; i < arr.Length; i++)
            if (arr[i] >= 0)
                res[idx++] = arr[i];

        Console.WriteLine("Результат:");
        PrintArr(res);
    }

    // задача 3: сколько раз число встречается в массиве
    static void Task3()
    {
        int[] arr = { 3, 7, 2, 3, 8, 3, 1, 3, 9, 3 };
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
            if (arr[i] == num)
                count++;

        Console.WriteLine($"Число {num} встречается {count} раз");
    }

    // задача 4: поменять местами столбцы в двумерном массиве
    static void Task4()
    {
        int[,] matrix = {
            { 1, 2, 3, 4 },
            { 5, 6, 7, 8 },
            { 9, 10, 11, 12 }
        };

        Console.WriteLine("Исходная:");
        PrintMat(matrix);

        Console.Write("Первый столбец: ");
        if (!int.TryParse(Console.ReadLine(), out int col1))
        {
            Console.WriteLine("Ошибка");
            return;
        }

        Console.Write("Второй столбец: ");
        if (!int.TryParse(Console.ReadLine(), out int col2))
        {
            Console.WriteLine("Ошибка");
            return;
        }

        if (col1 < 0 || col1 >= matrix.GetLength(1) || col2 < 0 || col2 >= matrix.GetLength(1))
        {
            Console.WriteLine("Неверный номер");
            return;
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            int temp = matrix[i, col1];
            matrix[i, col1] = matrix[i, col2];
            matrix[i, col2] = temp;
        }

        Console.WriteLine("Результат:");
        PrintMat(matrix);
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
        Console.WriteLine();
    }
}