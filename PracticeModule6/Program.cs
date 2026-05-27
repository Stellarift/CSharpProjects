namespace PracticeModule6;

//задание 1
class Human
{
    protected string _name = "";
    protected int _age;
    protected string _gender = "";

    public void SetName(string name) { _name = name; }
    public string GetName() { return _name; }

    public void SetAge(int age) { _age = age; }
    public int GetAge() { return _age; }

    public void SetGender(string gender) { _gender = gender; }
    public string GetGender() { return _gender; }

    public virtual void Input()
    {
        Console.Write("Имя: ");
        _name = Console.ReadLine() ?? "";
        Console.Write("Возраст: ");
        int.TryParse(Console.ReadLine(), out _age);
        Console.Write("Пол: ");
        _gender = Console.ReadLine() ?? "";
    }

    public virtual void Print()
    {
        Console.WriteLine($"Имя: {_name}, Возраст: {_age}, Пол: {_gender}");
    }
}

class Builder : Human
{
    private string _specialization = "";
    private int _experience;

    public void SetSpecialization(string spec) { _specialization = spec; }
    public string GetSpecialization() { return _specialization; }

    public void SetExperience(int exp) { _experience = exp; }
    public int GetExperience() { return _experience; }

    public override void Input()
    {
        base.Input();
        Console.Write("Специализация (каменщик/электрик/сантехник): ");
        _specialization = Console.ReadLine() ?? "";
        Console.Write("Стаж (лет): ");
        int.TryParse(Console.ReadLine(), out _experience);
    }

    public override void Print()
    {
        base.Print();
        Console.WriteLine($"Специализация: {_specialization}, Стаж: {_experience} лет");
    }
}

class Sailor : Human
{
    private string _rank = "";
    private int _shipYears;

    public void SetRank(string rank) { _rank = rank; }
    public string GetRank() { return _rank; }

    public void SetShipYears(int years) { _shipYears = years; }
    public int GetShipYears() { return _shipYears; }

    public override void Input()
    {
        base.Input();
        Console.Write("Звание: ");
        _rank = Console.ReadLine() ?? "";
        Console.Write("Лет на флоте: ");
        int.TryParse(Console.ReadLine(), out _shipYears);
    }

    public override void Print()
    {
        base.Print();
        Console.WriteLine($"Звание: {_rank}, Лет на флоте: {_shipYears}");
    }
}

class Pilot : Human
{
    private string _aircraftType = "";
    private int _flightHours;

    public void SetAircraftType(string type) { _aircraftType = type; }
    public string GetAircraftType() { return _aircraftType; }

    public void SetFlightHours(int hours) { _flightHours = hours; }
    public int GetFlightHours() { return _flightHours; }

    public override void Input()
    {
        base.Input();
        Console.Write("Тип самолёта: ");
        _aircraftType = Console.ReadLine() ?? "";
        Console.Write("Налёт часов: ");
        int.TryParse(Console.ReadLine(), out _flightHours);
    }

    public override void Print()
    {
        base.Print();
        Console.WriteLine($"Тип самолёта: {_aircraftType}, Налёт часов: {_flightHours}");
    }
}

// задание 2
class Passport
{
    protected string _passportNumber = "";
    protected string _fullName = "";
    protected DateTime _birthDate;
    protected string _birthPlace = "";
    protected DateTime _issueDate;
    protected string _issuedBy = "";

    public void SetPassportNumber(string number) { _passportNumber = number; }
    public string GetPassportNumber() { return _passportNumber; }

    public virtual void Input()
    {
        Console.Write("Номер паспорта: ");
        _passportNumber = Console.ReadLine() ?? "";
        Console.Write("ФИО: ");
        _fullName = Console.ReadLine() ?? "";
        Console.Write("Дата рождения (гггг-мм-дд): ");
        DateTime.TryParse(Console.ReadLine(), out _birthDate);
        Console.Write("Место рождения: ");
        _birthPlace = Console.ReadLine() ?? "";
        Console.Write("Дата выдачи (гггг-мм-дд): ");
        DateTime.TryParse(Console.ReadLine(), out _issueDate);
        Console.Write("Кем выдан: ");
        _issuedBy = Console.ReadLine() ?? "";
    }

    public virtual void Print()
    {
        Console.WriteLine($"Паспорт №: {_passportNumber}");
        Console.WriteLine($"ФИО: {_fullName}");
        Console.WriteLine($"Дата рождения: {_birthDate:dd.MM.yyyy}");
        Console.WriteLine($"Место рождения: {_birthPlace}");
        Console.WriteLine($"Выдан: {_issueDate:dd.MM.yyyy}, {_issuedBy}");
    }
}

class ForeignPassport : Passport
{
    private string _foreignPassportNumber = "";
    private string[] _visas = new string[10];
    private int _visaCount;

    public void SetForeignPassportNumber(string number) { _foreignPassportNumber = number; }
    public string GetForeignPassportNumber() { return _foreignPassportNumber; }

    public void AddVisa(string visa)
    {
        if (_visaCount < _visas.Length)
            _visas[_visaCount++] = visa;
    }

    public string[] GetVisas() { return _visas; }
    public int GetVisaCount() { return _visaCount; }

    public override void Input()
    {
        base.Input();
        Console.Write("Номер загранпаспорта: ");
        _foreignPassportNumber = Console.ReadLine() ?? "";
        Console.Write("Сколько виз добавить? ");
        int count;
        int.TryParse(Console.ReadLine(), out count);
        for (int i = 0; i < count; i++)
        {
            Console.Write($"Визы {i + 1}: ");
            AddVisa(Console.ReadLine() ?? "");
        }
    }

    public override void Print()
    {
        base.Print();
        Console.WriteLine($"Загранпаспорт №: {_foreignPassportNumber}");
        Console.WriteLine("Визы:");
        for (int i = 0; i < _visaCount; i++)
            Console.WriteLine($"  - {_visas[i]}");
    }
}

// задание 3
class Animal
{
    protected string _name = "";
    protected string _habitat = "";

    public Animal(string name, string habitat)
    {
        _name = name;
        _habitat = habitat;
    }

    public virtual void Print()
    {
        Console.WriteLine($"Животное: {_name}, Среда обитания: {_habitat}");
    }
}

class Tiger : Animal
{
    private string _stripesPattern = "";

    public Tiger(string name, string habitat, string stripes) : base(name, habitat)
    {
        _stripesPattern = stripes;
    }

    public override void Print()
    {
        base.Print();
        Console.WriteLine($"Тигр, окрас полос: {_stripesPattern}");
    }
}

class Crocodile : Animal
{
    private int _length;

    public Crocodile(string name, string habitat, int length) : base(name, habitat)
    {
        _length = length;
    }

    public override void Print()
    {
        base.Print();
        Console.WriteLine($"Крокодил, длина: {_length} м");
    }
}

class Kangaroo : Animal
{
    private double _jumpHeight;

    public Kangaroo(string name, string habitat, double jumpHeight) : base(name, habitat)
    {
        _jumpHeight = jumpHeight;
    }

    public override void Print()
    {
        base.Print();
        Console.WriteLine($"Кенгуру, высота прыжка: {_jumpHeight} м");
    }
}

//задание 4
abstract class Figure
{
    public abstract double GetArea();
}

class Rectangle : Figure
{
    private double _width;
    private double _height;

    public Rectangle(double width, double height)
    {
        _width = width;
        _height = height;
    }

    public override double GetArea()
    {
        return _width * _height;
    }
}

class Circle : Figure
{
    private double _radius;

    public Circle(double radius)
    {
        _radius = radius;
    }

    public override double GetArea()
    {
        return Math.PI * _radius * _radius;
    }
}

class RightTriangle : Figure
{
    private double _legA;
    private double _legB;

    public RightTriangle(double legA, double legB)
    {
        _legA = legA;
        _legB = legB;
    }

    public override double GetArea()
    {
        return 0.5 * _legA * _legB;
    }
}

class Trapezoid : Figure
{
    private double _baseA;
    private double _baseB;
    private double _height;

    public Trapezoid(double baseA, double baseB, double height)
    {
        _baseA = baseA;
        _baseB = baseB;
        _height = height;
    }

    public override double GetArea()
    {
        return 0.5 * (_baseA + _baseB) * _height;
    }
}

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1 - Люди (Human/Builder/Sailor/Pilot)");
            Console.WriteLine("2 - Паспорта (Passport/ForeignPassport)");
            Console.WriteLine("3 - Животные (Tiger/Crocodile/Kangaroo)");
            Console.WriteLine("4 - Фигуры (Rectangle/Circle/Triangle/Trapezoid)");
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
        Builder builder = new Builder();
        Sailor sailor = new Sailor();
        Pilot pilot = new Pilot();

        Console.WriteLine("Строитель");
        builder.Input();
        Console.WriteLine("\nМоряк");
        sailor.Input();
        Console.WriteLine("\nПилот");
        pilot.Input();

        Console.WriteLine("\nРезультат");
        builder.Print();
        Console.WriteLine();
        sailor.Print();
        Console.WriteLine();
        pilot.Print();
    }

    static void Task2()
    {
        Console.WriteLine("Внутренний паспорт");
        Passport passport = new Passport();
        passport.Input();

        Console.WriteLine("\nЗагранпаспорт");
        ForeignPassport foreign = new ForeignPassport();
        foreign.Input();

        Console.WriteLine("\nРезультат");
        passport.Print();
        Console.WriteLine();
        foreign.Print();
    }

    static void Task3()
    {
        Tiger tiger = new Tiger("Шерхан", "Джунгли", "чёрные полосы");
        Crocodile croc = new Crocodile("Гена", "Река Нил", 5);
        Kangaroo kangaroo = new Kangaroo("Скип", "Австралия", 3.5);

        Console.WriteLine("Животные");
        tiger.Print();
        croc.Print();
        kangaroo.Print();
    }

    static void Task4()
    {
        Figure[] figures = new Figure[]
        {
            new Rectangle(5, 10),
            new Circle(7),
            new RightTriangle(3, 4),
            new Trapezoid(6, 10, 4)
        };

        string[] names = { "Прямоугольник", "Круг", "Прямоугольный треугольник", "Трапеция" };

        for (int i = 0; i < figures.Length; i++)
        {
            Console.WriteLine($"{names[i]}: площадь = {figures[i].GetArea():F2}");
        }
    }
}