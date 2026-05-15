namespace PracticeModule3;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1 - Строка в int (0-9)");
            Console.WriteLine("2 - Бинарная строка в int");
            Console.WriteLine("3 - Кредитная карточка");
            Console.WriteLine("4 - Вычисление выражения (*)");
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

    static void Task1()
    {
        Console.Write("Введите число: ");
        string? input = Console.ReadLine();

        try
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exception("Пустая строка");

            int result = int.Parse(input);
            Console.WriteLine($"Результат: {result}");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Ошибка: число выходит за пределы int");
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: неверный формат (только цифры 0-9)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void Task2()
    {
        Console.Write("Введите двоичное число (0 и 1): ");
        string? input = Console.ReadLine();

        try
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exception("Пустая строка");

            foreach (char c in input)
            {
                if (c != '0' && c != '1')
                    throw new FormatException("Только 0 и 1");
            }

            int result = Convert.ToInt32(input, 2);
            Console.WriteLine($"Результат: {result}");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Ошибка: число выходит за пределы int");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    class CreditCard
    {
        private string _cardNumber = string.Empty;
        private string _ownerName = string.Empty;
        private string _cvc = string.Empty;
        private DateTime _expiryDate;

        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Номер карты не может быть пустым");
                
                string cleaned = value.Replace(" ", "");
                if (cleaned.Length != 16)
                    throw new Exception("Номер карты должен содержать 16 цифр");
                foreach (char c in cleaned)
                    if (!char.IsDigit(c))
                        throw new Exception("Номер карты должен содержать только цифры");
                _cardNumber = cleaned;
            }
        }

        public string OwnerName
        {
            get { return _ownerName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("ФИО владельца не может быть пустым");
                _ownerName = value;
            }
        }

        public string CVC
        {
            get { return _cvc; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("CVC не может быть пустым");
                if (value.Length != 3)
                    throw new Exception("CVC должен содержать 3 цифры");
                foreach (char c in value)
                    if (!char.IsDigit(c))
                        throw new Exception("CVC должен содержать только цифры");
                _cvc = value;
            }
        }

        public DateTime ExpiryDate
        {
            get { return _expiryDate; }
            set
            {
                if (value < DateTime.Now)
                    throw new Exception("Срок действия карты истек");
                _expiryDate = value;
            }
        }

        public void Print()
        {
            Console.WriteLine($"Номер: {_cardNumber}");
            Console.WriteLine($"Владелец: {_ownerName}");
            Console.WriteLine($"CVC: {_cvc}");
            Console.WriteLine($"Действительна до: {_expiryDate:MM/yy}");
        }
    }

    static void Task3()
    {
        try
        {
            CreditCard card = new CreditCard();
            
            Console.Write("Введите номер карты (16 цифр): ");
            string? cardNum = Console.ReadLine();
            if (cardNum != null)
                card.CardNumber = cardNum;
            else
                throw new Exception("Номер карты не введен");
            
            Console.Write("Введите ФИО владельца: ");
            string? owner = Console.ReadLine();
            if (owner != null)
                card.OwnerName = owner;
            else
                throw new Exception("ФИО не введено");
            
            Console.Write("Введите CVC (3 цифры): ");
            string? cvc = Console.ReadLine();
            if (cvc != null)
                card.CVC = cvc;
            else
                throw new Exception("CVC не введен");
            
            Console.Write("Введите дату окончания (гггг-мм-дд): ");
            string? dateStr = Console.ReadLine();
            if (dateStr != null && DateTime.TryParse(dateStr, out DateTime expiry))
                card.ExpiryDate = expiry;
            else
                throw new Exception("Неверный формат даты");
            
            Console.WriteLine("\nКарта создана успешно:");
            card.Print();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void Task4()
    {
        Console.Write("Введите выражение (например, 3*2*1*4): ");
        string? input = Console.ReadLine();

        try
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exception("Пустая строка");

            string[] parts = input.Split('*');
            if (parts.Length < 2)
                throw new Exception("Нужно хотя бы одно умножение");

            long result = 1;
            for (int i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i], out int num))
                    throw new Exception($"'{parts[i]}' не является целым числом");
                result *= num;
            }

            Console.WriteLine($"Результат: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}