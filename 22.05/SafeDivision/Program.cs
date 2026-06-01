using System;

class Program
{
    static void Main()
    {
        int[] results = new int[3];
        int index = 0;

        while (index < results.Length)
        {
            Console.Write("Введите первое целое число: ");
            string? input1 = Console.ReadLine();
            Console.Write("Введите второе целое число: ");
            string? input2 = Console.ReadLine();

            try
            {
                if (!int.TryParse(input1, out int a) || !int.TryParse(input2, out int b))
                    throw new FormatException();

                int result = a / b;
                results[index] = result;
                Console.WriteLine($"Результат: {a} / {b} = {result}");
                index++;

                if (index < results.Length)
                {
                    Console.Write("Хотите продолжить? (да/нет): ");
                    string? answer = Console.ReadLine();
                    if (answer?.ToLower() != "да")
                        break;
                }
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("деление на ноль невозможно");
            }
            catch (FormatException)
            {
                Console.WriteLine("введите целое число");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Массив результатов заполнен, дальнейшие вычисления невозможны");
                break;
            }
            finally
            {
                Console.WriteLine("Попытка выполнения операции завершена\n");
            }
        }

        Console.WriteLine("\nСохранённые результаты:");
        for (int i = 0; i < index; i++)
            Console.WriteLine($"{i + 1}: {results[i]}");
    }
}