namespace HomeworkTasks;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1 - Фильтрация массива");
            Console.WriteLine("2 - Веб-сайт");
            Console.WriteLine("3 - Журнал");
            Console.WriteLine("4 - Магазин");
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

    // задача 1: фильтрация массива
    static void Task1()
    {
        int[] original = { 1, 2, 6, -1, 88, 7, 6 };
        int[] filter = { 6, 88, 7 };
        
        Console.WriteLine("Оригинальный массив:");
        PrintArr(original);
        Console.WriteLine("Массив для фильтрации:");
        PrintArr(filter);

        int[] result = FilterArray(original, filter);
        
        Console.WriteLine("Результат:");
        PrintArr(result);
    }

    static int[] FilterArray(int[] original, int[] filter)
    {
        int[] temp = new int[original.Length];
        int idx = 0;
        
        for (int i = 0; i < original.Length; i++)
        {
            bool found = false;
            for (int j = 0; j < filter.Length; j++)
            {
                if (original[i] == filter[j])
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                temp[idx++] = original[i];
        }
        
        int[] result = new int[idx];
        for (int i = 0; i < idx; i++)
            result[i] = temp[i];
        
        return result;
    }

    // задача 2: класс Веб-сайт
    class Website
    {
        private string _name = string.Empty;
        private string _path = string.Empty;
        private string _description = string.Empty;
        private string _ip = string.Empty;

        public void SetName(string name) { _name = name; }
        public string GetName() { return _name; }
        
        public void SetPath(string path) { _path = path; }
        public string GetPath() { return _path; }
        
        public void SetDescription(string desc) { _description = desc; }
        public string GetDescription() { return _description; }
        
        public void SetIP(string ip) { _ip = ip; }
        public string GetIP() { return _ip; }
        
        public void Input()
        {
            Console.Write("Название сайта: ");
            _name = Console.ReadLine() ?? "";
            Console.Write("Путь к сайту: ");
            _path = Console.ReadLine() ?? "";
            Console.Write("Описание: ");
            _description = Console.ReadLine() ?? "";
            Console.Write("IP адрес: ");
            _ip = Console.ReadLine() ?? "";
        }
        
        public void Output()
        {
            Console.WriteLine($"Название: {_name}");
            Console.WriteLine($"Путь: {_path}");
            Console.WriteLine($"Описание: {_description}");
            Console.WriteLine($"IP: {_ip}");
        }
    }

    static void Task2()
    {
        Website site = new Website();
        site.Input();
        Console.WriteLine("\nИнформация о сайте:");
        site.Output();
    }

    // задача 3: класс Журнал
    class Magazine
    {
        private string _name = string.Empty;
        private int _year;
        private string _description = string.Empty;
        private string _phone = string.Empty;
        private string _email = string.Empty;

        public void SetName(string name) { _name = name; }
        public string GetName() { return _name; }
        
        public void SetYear(int year) { _year = year; }
        public int GetYear() { return _year; }
        
        public void SetDescription(string desc) { _description = desc; }
        public string GetDescription() { return _description; }
        
        public void SetPhone(string phone) { _phone = phone; }
        public string GetPhone() { return _phone; }
        
        public void SetEmail(string email) { _email = email; }
        public string GetEmail() { return _email; }
        
        public void Input()
        {
            Console.Write("Название журнала: ");
            _name = Console.ReadLine() ?? "";
            Console.Write("Год основания: ");
            int.TryParse(Console.ReadLine(), out _year);
            Console.Write("Описание: ");
            _description = Console.ReadLine() ?? "";
            Console.Write("Контактный телефон: ");
            _phone = Console.ReadLine() ?? "";
            Console.Write("Контактный e-mail: ");
            _email = Console.ReadLine() ?? "";
        }
        
        public void Output()
        {
            Console.WriteLine($"Название: {_name}");
            Console.WriteLine($"Год основания: {_year}");
            Console.WriteLine($"Описание: {_description}");
            Console.WriteLine($"Телефон: {_phone}");
            Console.WriteLine($"E-mail: {_email}");
        }
    }

    static void Task3()
    {
        Magazine mag = new Magazine();
        mag.Input();
        Console.WriteLine("\nИнформация о журнале:");
        mag.Output();
    }

    // задача 4: класс Магазин
    class Shop
    {
        private string _name = string.Empty;
        private string _address = string.Empty;
        private string _profile = string.Empty;
        private string _phone = string.Empty;
        private string _email = string.Empty;

        public void SetName(string name) { _name = name; }
        public string GetName() { return _name; }
        
        public void SetAddress(string address) { _address = address; }
        public string GetAddress() { return _address; }
        
        public void SetProfile(string profile) { _profile = profile; }
        public string GetProfile() { return _profile; }
        
        public void SetPhone(string phone) { _phone = phone; }
        public string GetPhone() { return _phone; }
        
        public void SetEmail(string email) { _email = email; }
        public string GetEmail() { return _email; }
        
        public void Input()
        {
            Console.Write("Название магазина: ");
            _name = Console.ReadLine() ?? "";
            Console.Write("Адрес: ");
            _address = Console.ReadLine() ?? "";
            Console.Write("Описание профиля: ");
            _profile = Console.ReadLine() ?? "";
            Console.Write("Контактный телефон: ");
            _phone = Console.ReadLine() ?? "";
            Console.Write("Контактный e-mail: ");
            _email = Console.ReadLine() ?? "";
        }
        
        public void Output()
        {
            Console.WriteLine($"Название: {_name}");
            Console.WriteLine($"Адрес: {_address}");
            Console.WriteLine($"Профиль: {_profile}");
            Console.WriteLine($"Телефон: {_phone}");
            Console.WriteLine($"E-mail: {_email}");
        }
    }

    static void Task4()
    {
        Shop shop = new Shop();
        shop.Input();
        Console.WriteLine("\nИнформация о магазине:");
        shop.Output();
    }

    static void PrintArr(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
            Console.Write(arr[i] + " ");
        Console.WriteLine();
    }
}