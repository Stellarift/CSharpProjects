namespace HomeworkTasks
{
    // задание 5: перечисление типов товаров
    enum ArticleType
    {
        Food,
        Electronics,
        Clothing,
        Books,
        Furniture
    }

    // задание 6: перечисление важности клиента
    enum ClientType
    {
        Regular,
        Gold,
        Platinum
    }

    // задание 7: перечисление формы оплаты
    enum PayType
    {
        Cash,
        Card,
        Online
    }

    // задание 1: структура Article (добавлено поле ArticleType)
    struct Article
    {
        public int Code;
        public string Name;
        public double Price;
        public ArticleType Type;
    }

    // задание 2: структура Client (добавлено поле ClientType)
    struct Client
    {
        public int Code;
        public string FullName;
        public string Address;
        public string Phone;
        public int OrdersCount;
        public double TotalSum;
        public ClientType Type;
    }

    // задание 3: структура RequestItem
    struct RequestItem
    {
        public Article Product;
        public int Quantity;
    }

    // задание 4: структура Request (добавлено поле PayType)
    struct Request
    {
        public int Code;
        public Client Client;
        public DateTime Date;
        public RequestItem[] Items;
        public PayType Payment;

        public double TotalSum
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < Items.Length; i++)
                    sum += Items[i].Product.Price * Items[i].Quantity;
                return sum;
            }
        }
    }

    // задание 8: класс Студент
class Student
{
    private string _lastName = "";
    private string _firstName = "";
    private string _patronymic = "";
    private string _group = "";
    private int _age;
    private int[][] _grades = new int[3][]; // инициализация

    public void SetLastName(string lastName) { _lastName = lastName; }
    public string GetLastName() { return _lastName; }

    public void SetFirstName(string firstName) { _firstName = firstName; }
    public string GetFirstName() { return _firstName; }

    public void SetPatronymic(string patronymic) { _patronymic = patronymic; }
    public string GetPatronymic() { return _patronymic; }

    public void SetGroup(string group) { _group = group; }
    public string GetGroup() { return _group; }

    public void SetAge(int age) { _age = age; }
    public int GetAge() { return _age; }

    public void SetGrades(int[][] grades) { _grades = grades; }
    public int[][] GetGrades() { return _grades; }

    public void Input()
    {
        Console.Write("Фамилия: ");
        _lastName = Console.ReadLine() ?? "";
        Console.Write("Имя: ");
        _firstName = Console.ReadLine() ?? "";
        Console.Write("Отчество: ");
        _patronymic = Console.ReadLine() ?? "";
        Console.Write("Группа: ");
        _group = Console.ReadLine() ?? "";
        Console.Write("Возраст: ");
        int.TryParse(Console.ReadLine(), out _age);

        Console.WriteLine("Оценки по программированию (через пробел):");
        string[] prog = (Console.ReadLine() ?? "").Split();
        _grades[0] = new int[prog.Length];
        for (int i = 0; i < prog.Length; i++)
            int.TryParse(prog[i], out _grades[0][i]);

        Console.WriteLine("Оценки по администрированию (через пробел):");
        string[] adm = (Console.ReadLine() ?? "").Split();
        _grades[1] = new int[adm.Length];
        for (int i = 0; i < adm.Length; i++)
            int.TryParse(adm[i], out _grades[1][i]);

        Console.WriteLine("Оценки по дизайну (через пробел):");
        string[] design = (Console.ReadLine() ?? "").Split();
        _grades[2] = new int[design.Length];
        for (int i = 0; i < design.Length; i++)
            int.TryParse(design[i], out _grades[2][i]);
    }

    public void Print()
    {
        Console.WriteLine($"Студент: {_lastName} {_firstName} {_patronymic}");
        Console.WriteLine($"Группа: {_group}, Возраст: {_age}");
        Console.Write("Программирование: ");
        if (_grades[0] != null)
            foreach (int g in _grades[0]) Console.Write(g + " ");
        Console.WriteLine();
        Console.Write("Администрирование: ");
        if (_grades[1] != null)
            foreach (int g in _grades[1]) Console.Write(g + " ");
        Console.WriteLine();
        Console.Write("Дизайн: ");
        if (_grades[2] != null)
            foreach (int g in _grades[2]) Console.Write(g + " ");
        Console.WriteLine();
    }
}

    // задание 9: 7 чудес света
    namespace Wonders
    {
        class GreatPyramid
        {
            public string Name => "Пирамида Хеопса";
            public string Location => "Гиза, Египет";
            public void Print() => Console.WriteLine($"{Name} - {Location}");
        }

        class HangingGardens
        {
            public string Name => "Висячие сады Семирамиды";
            public string Location => "Вавилон, Ирак";
            public void Print() => Console.WriteLine($"{Name} - {Location}");
        }

        class StatueOfZeus
        {
            public string Name => "Статуя Зевса в Олимпии";
            public string Location => "Олимпия, Греция";
            public void Print() => Console.WriteLine($"{Name} - {Location}");
        }

        class TempleOfArtemis
        {
            public string Name => "Храм Артемиды в Эфесе";
            public string Location => "Эфес, Турция";
            public void Print() => Console.WriteLine($"{Name} - {Location}");
        }

        class Mausoleum
        {
            public string Name => "Мавзолей в Галикарнасе";
            public string Location => "Бодрум, Турция";
            public void Print() => Console.WriteLine($"{Name} - {Location}");
        }

        class Colossus
        {
            public string Name => "Колосс Родосский";
            public string Location => "Родос, Греция";
            public void Print() => Console.WriteLine($"{Name} - {Location}");
        }

        class Lighthouse
        {
            public string Name => "Александрийский маяк";
            public string Location => "Александрия, Египет";
            public void Print() => Console.WriteLine($"{Name} - {Location}");
        }
    }

    // задание 10: столицы
    namespace Ukraine
    {
        class Kyiv
        {
            public string Name => "Киев";
            public int Population => 2962180;
            public void Print() => Console.WriteLine($"{Name} - население {Population}");
        }
    }

    namespace France
    {
        class Paris
        {
            public string Name => "Париж";
            public int Population => 2148327;
            public void Print() => Console.WriteLine($"{Name} - население {Population}");
        }
    }

    namespace Japan
    {
        class Tokyo
        {
            public string Name => "Токио";
            public int Population => 13960000;
            public void Print() => Console.WriteLine($"{Name} - население {Population}");
        }
    }

    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n5-7 - Структуры с перечислениями");
                Console.WriteLine("8 - Класс Студент");
                Console.WriteLine("9 - 7 чудес света");
                Console.WriteLine("10 - Сравнение столиц");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "5": Task5_7(); break;
                    case "8": Task8(); break;
                    case "9": Task9(); break;
                    case "10": Task10(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод"); break;
                }
            }
        }

        static void Task5_7()
        {
            Article article = new Article
            {
                Code = 1,
                Name = "Ноутбук",
                Price = 25000,
                Type = ArticleType.Electronics
            };

            Client client = new Client
            {
                Code = 1,
                FullName = "Иванов Иван",
                Address = "ул. Пушкина, 1",
                Phone = "123456789",
                OrdersCount = 5,
                TotalSum = 50000,
                Type = ClientType.Gold
            };

            RequestItem item = new RequestItem
            {
                Product = article,
                Quantity = 2
            };

            Request request = new Request
            {
                Code = 1,
                Client = client,
                Date = DateTime.Now,
                Items = new RequestItem[] { item },
                Payment = PayType.Card
            };

            Console.WriteLine($"Товар: {article.Name}, тип: {article.Type}");
            Console.WriteLine($"Клиент: {client.FullName}, тип: {client.Type}");
            Console.WriteLine($"Заказ №{request.Code}, оплата: {request.Payment}, сумма: {request.TotalSum}");
        }

        static void Task8()
        {
            Student student = new Student();
            student.Input();
            Console.WriteLine("\nИнформация о студенте:");
            student.Print();
        }

        static void Task9()
        {
            Wonders.GreatPyramid p1 = new Wonders.GreatPyramid();
            Wonders.HangingGardens p2 = new Wonders.HangingGardens();
            Wonders.StatueOfZeus p3 = new Wonders.StatueOfZeus();
            Wonders.TempleOfArtemis p4 = new Wonders.TempleOfArtemis();
            Wonders.Mausoleum p5 = new Wonders.Mausoleum();
            Wonders.Colossus p6 = new Wonders.Colossus();
            Wonders.Lighthouse p7 = new Wonders.Lighthouse();

            p1.Print();
            p2.Print();
            p3.Print();
            p4.Print();
            p5.Print();
            p6.Print();
            p7.Print();
        }

        static void Task10()
        {
            Ukraine.Kyiv kyiv = new Ukraine.Kyiv();
            France.Paris paris = new France.Paris();
            Japan.Tokyo tokyo = new Japan.Tokyo();

            kyiv.Print();
            paris.Print();
            tokyo.Print();

            Console.WriteLine("\nСравнение населения:");
            if (kyiv.Population > paris.Population && kyiv.Population > tokyo.Population)
                Console.WriteLine($"Киев ({kyiv.Population}) - самый населенный");
            else if (paris.Population > kyiv.Population && paris.Population > tokyo.Population)
                Console.WriteLine($"Париж ({paris.Population}) - самый населенный");
            else
                Console.WriteLine($"Токио ({tokyo.Population}) - самый населенный");
        }
    }
}