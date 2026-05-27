namespace HomeworkTasks;

//задание 1

// абстрактный класс Геометрическая фигура
abstract class Figure
{
    public abstract double GetArea();
    public abstract double GetPerimeter();
}

// Треугольник (по трём сторонам)
class Triangle : Figure
{
    private double _a, _b, _c;

    public Triangle(double a, double b, double c)
    {
        _a = a;
        _b = b;
        _c = c;
    }

    public override double GetArea()
    {
        double p = (_a + _b + _c) / 2;
        return Math.Sqrt(p * (p - _a) * (p - _b) * (p - _c));
    }

    public override double GetPerimeter()
    {
        return _a + _b + _c;
    }
}

// Квадрат
class Square : Figure
{
    private double _side;

    public Square(double side)
    {
        _side = side;
    }

    public override double GetArea()
    {
        return _side * _side;
    }

    public override double GetPerimeter()
    {
        return 4 * _side;
    }
}

// Ромб (по диагоналям)
class Rhombus : Figure
{
    private double _d1, _d2;

    public Rhombus(double d1, double d2)
    {
        _d1 = d1;
        _d2 = d2;
    }

    public override double GetArea()
    {
        return (_d1 * _d2) / 2;
    }

    public override double GetPerimeter()
    {
        double side = Math.Sqrt((_d1 / 2) * (_d1 / 2) + (_d2 / 2) * (_d2 / 2));
        return 4 * side;
    }
}

// Прямоугольник
class Rectangle : Figure
{
    private double _width, _height;

    public Rectangle(double width, double height)
    {
        _width = width;
        _height = height;
    }

    public override double GetArea()
    {
        return _width * _height;
    }

    public override double GetPerimeter()
    {
        return 2 * (_width + _height);
    }
}

// Параллелограмм (сторона, высота)
class Parallelogram : Figure
{
    private double _side, _height;

    public Parallelogram(double side, double height)
    {
        _side = side;
        _height = height;
    }

    public override double GetArea()
    {
        return _side * _height;
    }

    public override double GetPerimeter()
    {
        return 4 * _side; // упрощённо (для ромба)
    }
}

// Трапеция (основания, высота)
class Trapezoid : Figure
{
    private double _a, _b, _height;

    public Trapezoid(double a, double b, double height)
    {
        _a = a;
        _b = b;
        _height = height;
    }

    public override double GetArea()
    {
        return 0.5 * (_a + _b) * _height;
    }

    public override double GetPerimeter()
    {
        double side = Math.Sqrt(_height * _height + ((_b - _a) / 2) * ((_b - _a) / 2));
        return _a + _b + 2 * side;
    }
}

// Круг
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

    public override double GetPerimeter()
    {
        return 2 * Math.PI * _radius;
    }
}

// Эллипс (полуоси)
class Ellipse : Figure
{
    private double _a, _b;

    public Ellipse(double a, double b)
    {
        _a = a;
        _b = b;
    }

    public override double GetArea()
    {
        return Math.PI * _a * _b;
    }

    public override double GetPerimeter()
    {
        return 2 * Math.PI * Math.Sqrt((_a * _a + _b * _b) / 2);
    }
}

// Составная фигура
class CompositeFigure : Figure
{
    private Figure[] _figures;

    public CompositeFigure(params Figure[] figures)
    {
        _figures = figures;
    }

    public override double GetArea()
    {
        double sum = 0;
        for (int i = 0; i < _figures.Length; i++)
            sum += _figures[i].GetArea();
        return sum;
    }

    public override double GetPerimeter()
    {
        double sum = 0;
        for (int i = 0; i < _figures.Length; i++)
            sum += _figures[i].GetPerimeter();
        return sum;
    }
}

//задание 2

// абстрактный класс Товар
abstract class Product
{
    protected string _name = "";
    protected double _price;
    protected int _quantity;

    public Product(string name, double price, int quantity)
    {
        _name = name;
        _price = price;
        _quantity = quantity;
    }

    public string GetName() { return _name; }
    public double GetPrice() { return _price; }
    public int GetQuantity() { return _quantity; }

    public abstract string GetCategory();
}

// Бытовая химия
class HouseholdChemicals : Product
{
    public HouseholdChemicals(string name, double price, int quantity)
        : base(name, price, quantity) { }

    public override string GetCategory()
    {
        return "Бытовая химия";
    }
}

// Продукты питания
class Food : Product
{
    public Food(string name, double price, int quantity)
        : base(name, price, quantity) { }

    public override string GetCategory()
    {
        return "Продукты питания";
    }
}

// Управление потоком товаров
class ProductFlow
{
    private Product[] _products = new Product[100];
    private int _count;

    public void Add(Product p)
    {
        if (_count < _products.Length)
            _products[_count++] = p;
    }

    public void Arrived(string name, int quantity)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_products[i].GetName() == name)
            {
                _products[i] = new HouseholdChemicals(
                    _products[i].GetName(),
                    _products[i].GetPrice(),
                    _products[i].GetQuantity() + quantity
                );
                break;
            }
        }
    }

    public void Sold(string name, int quantity)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_products[i].GetName() == name)
            {
                int newQty = _products[i].GetQuantity() - quantity;
                if (newQty < 0) newQty = 0;
                _products[i] = new Food(
                    _products[i].GetName(),
                    _products[i].GetPrice(),
                    newQty
                );
                break;
            }
        }
    }

    public void WrittenOff(string name, int quantity)
    {
        Sold(name, quantity); // списание = продажа без денег
    }

    public void Transferred(string name, int quantity, ProductFlow target)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_products[i].GetName() == name)
            {
                int currentQty = _products[i].GetQuantity();
                if (currentQty >= quantity)
                {
                    target.Add(new Food(name, _products[i].GetPrice(), quantity));
                    int newQty = currentQty - quantity;
                    _products[i] = new Food(name, _products[i].GetPrice(), newQty);
                }
                break;
            }
        }
    }

    public void Print()
    {
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine($"Товар: {_products[i].GetName()}, " +
                              $"Категория: {_products[i].GetCategory()}, " +
                              $"Цена: {_products[i].GetPrice()}, " +
                              $"Количество: {_products[i].GetQuantity()}");
        }
    }
}


class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1 - Геометрические фигуры");
            Console.WriteLine("2 - Управление товарами");
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
                case "0":
                 return;
                default: Console.WriteLine("Неверный ввод");
                 break;
            }
        }
    }

    static void Task1()
    {
        Figure[] figures =
        {
            new Triangle(3, 4, 5),
            new Square(5),
            new Rhombus(6, 8),
            new Rectangle(4, 7),
            new Parallelogram(5, 4),
            new Trapezoid(6, 10, 4),
            new Circle(5),
            new Ellipse(6, 4)
        };

        string[] names = { "Треугольник", "Квадрат", "Ромб", "Прямоугольник",
                           "Параллелограмм", "Трапеция", "Круг", "Эллипс" };

        for (int i = 0; i < figures.Length; i++)
        {
            Console.WriteLine($"{names[i]}: Площадь = {figures[i].GetArea():F2}, " +
                              $"Периметр = {figures[i].GetPerimeter():F2}");
        }

        Console.WriteLine("\nСоставная фигура (круг + квадрат):");
        CompositeFigure composite = new CompositeFigure(new Circle(3), new Square(4));
        Console.WriteLine($"Площадь = {composite.GetArea():F2}, Периметр = {composite.GetPerimeter():F2}");
    }

    static void Task2()
    {
        ProductFlow flow = new ProductFlow();

        flow.Add(new HouseholdChemicals("Мыло", 50, 100));
        flow.Add(new Food("Хлеб", 30, 200));

        Console.WriteLine("Начальное состояние:");
        flow.Print();

        flow.Arrived("Мыло", 50);
        flow.Sold("Хлеб", 30);
        flow.WrittenOff("Мыло", 20);

        Console.WriteLine("\nПосле операций:");
        flow.Print();
    }
}