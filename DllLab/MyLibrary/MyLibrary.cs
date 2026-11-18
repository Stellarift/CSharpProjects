using System;

namespace MyLibrary
{
    public static class Utility
    {
        // Задание 1
        public static void ShowMessage(string message)
        {
            Console.WriteLine($"[Сообщение] {message}");
        }

        // Задание 2
        public static bool IsLeapYear(int year)
        {
            return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
        }

        // Задание 3
        public static int Max(int a, int b, int c) => Math.Max(a, Math.Max(b, c));
        public static int Min(int a, int b, int c) => Math.Min(a, Math.Min(b, c));
        public static int Sum(int a, int b, int c) => a + b + c;

        // Задание 4
        public static long Factorial(int n)
        {
            if (n < 0) return -1;
            long result = 1;
            for (int i = 1; i <= n; i++) result *= i;
            return result;
        }

        public static bool IsPrime(int n)
        {
            if (n < 2) return false;
            for (int i = 2; i <= Math.Sqrt(n); i++)
                if (n % i == 0) return false;
            return true;
        }

        public static bool IsEven(int n) => n % 2 == 0;
    }

    // Задание 5
    public class Fraction
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0) throw new ArgumentException("Знаменатель не может быть равен нулю");
            Numerator = numerator;
            Denominator = denominator;
        }

        public override string ToString() => $"{Numerator}/{Denominator}";

        public Fraction Add(Fraction other)
        {
            return new Fraction(
                Numerator * other.Denominator + other.Numerator * Denominator,
                Denominator * other.Denominator
            );
        }

        public Fraction Multiply(Fraction other)
        {
            return new Fraction(Numerator * other.Numerator, Denominator * other.Denominator);
        }
    }
}