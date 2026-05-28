using System;

// Делегат
public delegate void PropertyEventHandler(object sender, PropertyEventArgs e);

// Аргументы события
public class PropertyEventArgs : EventArgs
{
    public string PropertyName { get; }
    
    public PropertyEventArgs(string propertyName)
    {
        PropertyName = propertyName;
    }
}

// Интерфейс
interface IPropertyChanged
{
    event PropertyEventHandler? PropertyChanged;
}

// Класс, реализующий интерфейс
class User : IPropertyChanged
{
    private string _name = string.Empty;
    private int _age;

    public event PropertyEventHandler? PropertyChanged;

    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public int Age
    {
        get { return _age; }
        set
        {
            _age = value;
            OnPropertyChanged(nameof(Age));
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyEventArgs(propertyName));
    }
}

class Program
{
    static void Main()
    {
        User user = new User();

        // Подписываемся на событие
        user.PropertyChanged += (sender, e) =>
        {
            Console.WriteLine($"Свойство '{e.PropertyName}' было изменено");
        };

        // Меняем свойства
        user.Name = "Иван";
        user.Age = 30;
        user.Name = "Петр";
    }
}