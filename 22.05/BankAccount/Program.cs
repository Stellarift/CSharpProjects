using System;

// пользовательские исключения
class InvalidAmountException : Exception
{
    public InvalidAmountException() { }
    public InvalidAmountException(string message) : base(message) { }
    public InvalidAmountException(string message, Exception inner) : base(message, inner) { }
}

class InsufficientFundsException : Exception
{
    public InsufficientFundsException() { }
    public InsufficientFundsException(string message) : base(message) { }
    public InsufficientFundsException(string message, Exception inner) : base(message, inner) { }
}

// Класс банковского счёта
class BankAccount
{
    private static int _counter = 0;
    public string AccountNumber { get; private set; }
    public decimal Balance { get; private set; }

    public BankAccount()
    {
        _counter++;
        AccountNumber = DateTime.Now.ToString("yyyyMMdd") + _counter.ToString("D4");
        Balance = 0;
    }

    public BankAccount(decimal initialBalance)
    {
        if (initialBalance < 0)
            throw new InvalidAmountException("Начальный баланс не может быть отрицательным");

        _counter++;
        AccountNumber = DateTime.Now.ToString("yyyyMMdd") + _counter.ToString("D4");
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAmountException("Сумма пополнения должна быть положительной");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAmountException("Сумма снятия должна быть положительной");
        if (amount > Balance)
            throw new InsufficientFundsException("Недостаточно средств на счете");

        Balance -= amount;
    }

    public void ShowBalance()
    {
        Console.WriteLine($"Баланс: {Balance:F2} руб.");
    }
}

class Program
{
    static void Main()
    {
        BankAccount account;

        // Создание счёта с возможностью указать начальный баланс
        try
        {
            Console.Write("Введите начальный баланс (или 0 для создания пустого счёта): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal initial))
                account = new BankAccount(initial);
            else
                account = new BankAccount();

            Console.WriteLine($"Счёт создан. Номер: {account.AccountNumber}");
            account.ShowBalance();
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
            return;
        }

        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1 - Пополнить счёт");
            Console.WriteLine("2 - Снять средства");
            Console.WriteLine("3 - Показать баланс");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");

            string? choice = Console.ReadLine();

            if (choice == "0")
            {
                Console.WriteLine("До свидания!");
                break;
            }

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Введите сумму пополнения: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal deposit))
                            throw new FormatException();
                        account.Deposit(deposit);
                        Console.WriteLine("Пополнение выполнено успешно");
                        break;

                    case "2":
                        Console.Write("Введите сумму снятия: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal withdraw))
                            throw new FormatException();
                        account.Withdraw(withdraw);
                        Console.WriteLine("Снятие выполнено успешно");
                        break;

                    case "3":
                        account.ShowBalance();
                        continue;

                    default:
                        Console.WriteLine("Неверный выбор");
                        continue;
                }

                account.ShowBalance();
            }
            catch (InvalidAmountException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: введите корректное число");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
            }
        }
    }
}