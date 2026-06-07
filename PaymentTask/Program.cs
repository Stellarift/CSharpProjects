using System;
using System.IO;
using System.Text.Json;

namespace HomeworkTask
{
    class PaymentAccount
    {
        private double paymentPerDay;
        private int days;
        private double finePerDay;
        private int delayDays;
        private static bool serializeCalculated = true;
        private double amountNoFine;
        private double fine;
        private double totalAmount;

        public static bool SerializeCalculated
        {
            get { return serializeCalculated; }
            set { serializeCalculated = value; }
        }

        public PaymentAccount(double paymentPerDay, int days, double finePerDay, int delayDays)
        {
            this.paymentPerDay = paymentPerDay;
            this.days = days;
            this.finePerDay = finePerDay;
            this.delayDays = delayDays;
            UpdateCalculatedFields();
        }

        private void UpdateCalculatedFields()
        {
            amountNoFine = paymentPerDay * days;
            fine = finePerDay * delayDays;
            totalAmount = amountNoFine + fine;
        }

        public void Recalculate()
        {
            UpdateCalculatedFields();
        }

        public void Print()
        {
            Console.WriteLine($"Оплата за день: {paymentPerDay} руб.");
            Console.WriteLine($"Количество дней: {days}");
            Console.WriteLine($"Штраф за один день: {finePerDay} руб.");
            Console.WriteLine($"Дней задержки: {delayDays}");
            Console.WriteLine($"Сумма без штрафа: {amountNoFine} руб.");
            Console.WriteLine($"Штраф: {fine} руб.");
            Console.WriteLine($"Общая сумма: {totalAmount} руб.");
        }

        public void Save(string fileName)
        {
            var data = new
            {
                paymentPerDay,
                days,
                finePerDay,
                delayDays,
                amountNoFine = serializeCalculated ? amountNoFine : (double?)null,
                fine = serializeCalculated ? fine : (double?)null,
                totalAmount = serializeCalculated ? totalAmount : (double?)null
            };
            
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }

        public static PaymentAccount? Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Файл не найден");
                return null;
            }

            string json = File.ReadAllText(fileName);
            PaymentAccountData? data = JsonSerializer.Deserialize<PaymentAccountData>(json);
            
            if (data == null)
            {
                Console.WriteLine("не удалось прочитать файл");
                return null;
            }

            var account = new PaymentAccount(data.paymentPerDay, data.days, data.finePerDay, data.delayDays);
            
            if (data.amountNoFine.HasValue)
            {
                if (Math.Abs(account.amountNoFine - data.amountNoFine.Value) > 0.001)
                    Console.WriteLine("сумма без штрафа не совпадает с рассчитанной");
            }
            if (data.fine.HasValue)
            {
                if (Math.Abs(account.fine - data.fine.Value) > 0.001)
                    Console.WriteLine("штраф не совпадает с рассчитанным");
            }
            if (data.totalAmount.HasValue)
            {
                if (Math.Abs(account.totalAmount - data.totalAmount.Value) > 0.001)
                    Console.WriteLine("общая сумма не совпадает с рассчитанной");
            }
            
            return account;
        }

        private class PaymentAccountData
        {
            public double paymentPerDay { get; set; }
            public int days { get; set; }
            public double finePerDay { get; set; }
            public int delayDays { get; set; }
            public double? amountNoFine { get; set; }
            public double? fine { get; set; }
            public double? totalAmount { get; set; }
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Счёт для оплаты\n");

            PaymentAccount account = new PaymentAccount(1000, 30, 200, 5);
            
            Console.WriteLine("Исходные данные:");
            account.Print();

            PaymentAccount.SerializeCalculated = true;
            account.Save("account_with_calc.json");
            Console.WriteLine("\nСохранено в 'account_with_calc.json' (с вычисляемыми полями)");

            PaymentAccount? loaded = PaymentAccount.Load("account_with_calc.json");
            if (loaded != null)
            {
                Console.WriteLine("\nЗагружено из файла (с вычисляемыми полями):");
                loaded.Print();
            }

            PaymentAccount.SerializeCalculated = false;
            account.Save("account_no_calc.json");
            Console.WriteLine("\nСохранено в 'account_no_calc.json' (без вычисляемых полей)");

            PaymentAccount? loaded2 = PaymentAccount.Load("account_no_calc.json");
            if (loaded2 != null)
            {
                Console.WriteLine("\nЗагружено из файла (без вычисляемых полей):");
                loaded2.Print();
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}