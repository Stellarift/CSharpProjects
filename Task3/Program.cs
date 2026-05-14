using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int[] originalArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int[] filterArray = { 2, 4, 6, 8, 10 };

        int[] result = FilterArray(originalArray, filterArray);

        Console.WriteLine("Отфильтрованный массив:");
        foreach (int item in result)
        {
            Console.Write(item + " ");
        }
    }

    static int[] FilterArray(int[] originalArray, int[] filterArray)
    {
        List<int> filteredList = new List<int>();

        for (int i = 0; i < originalArray.Length; i++)
        {
            for (int j = 0; j < filterArray.Length; j++)
            {
                if (originalArray[i] == filterArray[j])
                {
                    filteredList.Add(originalArray[i]);
                    break;
                }
            }
        }

        return filteredList.ToArray();
    }
}