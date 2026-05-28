using System;

// Делегат для старта гонки
public delegate void StartRaceDelegate();

// Абстрактный класс Автомобиль
abstract class Car
{
    public string Name { get; protected set; }
    public double Position { get; protected set; }
    protected double _speed;
    protected Random _rnd = new Random();

    public event Action<Car>? FinishEvent;

    public Car(string name)
    {
        Name = name;
        Position = 0;
        _speed = 0;
    }

    public void Go()
    {
        if (Position >= 100) return;
        
        _speed = _rnd.Next(5, 20);
        Position += _speed;

        if (Position >= 100 && FinishEvent != null)
            FinishEvent(this);
    }

    public abstract void ShowStatus();
}

// Спортивный автомобиль
class SportCar : Car
{
    public SportCar(string name) : base(name) { }

    public override void ShowStatus()
    {
        Console.WriteLine($"Спортивный {Name}: {Math.Min(Position, 100):F1} км");
    }
}

// Легковой автомобиль
class PassengerCar : Car
{
    public PassengerCar(string name) : base(name) { }

    public override void ShowStatus()
    {
        Console.WriteLine($"Легковой {Name}: {Math.Min(Position, 100):F1} км");
    }
}

// Грузовой автомобиль
class Truck : Car
{
    public Truck(string name) : base(name) { }

    public override void ShowStatus()
    {
        Console.WriteLine($"Грузовой {Name}: {Math.Min(Position, 100):F1} км");
    }
}

// Автобус
class Bus : Car
{
    public Bus(string name) : base(name) { }

    public override void ShowStatus()
    {
        Console.WriteLine($"Автобус {Name}: {Math.Min(Position, 100):F1} км");
    }
}

// Класс игры
class RaceGame
{
    private Car[] _cars;
    private bool _isFinished;

    public RaceGame(params Car[] cars)
    {
        _cars = cars;
        _isFinished = false;

        foreach (Car c in _cars)
            c.FinishEvent += OnFinish;
    }

    private void OnFinish(Car winner)
    {
        if (!_isFinished)
        {
            _isFinished = true;
            Console.WriteLine($"\n*** ПОБЕДИТЕЛЬ: {winner.Name} ***\n");
        }
    }

    public void Start()
    {
        Console.WriteLine("ГОНКА НАЧАЛАСЬ!\n");

        while (!_isFinished)
        {
            for (int i = 0; i < _cars.Length; i++)
            {
                _cars[i].Go();
                _cars[i].ShowStatus();
            }
            Console.WriteLine("------------------------");
            System.Threading.Thread.Sleep(500);
        }
    }
}

class Program
{
    static void Main()
    {
        Car[] cars = new Car[]
        {
            new SportCar("Ferrari"),
            new PassengerCar("Toyota"),
            new Truck("Volvo"),
            new Bus("Mercedes")
        };

        RaceGame game = new RaceGame(cars);
        game.Start();
    }
}
