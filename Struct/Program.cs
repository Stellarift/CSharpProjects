namespace HomeworkTasks;

struct Article
{
    public int Code;
    public string Name;
    public double Price;
}

struct Client
{
    public int Code;
    public string FullName;
    public string Address;
    public string Phone;
    public int OrdersCount;
    public double TotalSum;
}

struct RequestItem
{
    public Article Product;
    public int Quantity;
}

struct Request
{
    public int Code;
    public Client Client;
    public DateTime Date;
    public RequestItem[] Items;
    
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

class Program
{
    static void Main()
    {
        // пример использования
        Article article = new Article();
        article.Code = 1;
        article.Name = "Ноутбук";
        article.Price = 25000;

        Client client = new Client();
        client.Code = 1;
        client.FullName = "Иванов Иван Иванович";
        client.Address = "ул. Пушкина, 1";
        client.Phone = "+380123456789";
        client.OrdersCount = 0;
        client.TotalSum = 0;

        RequestItem item = new RequestItem();
        item.Product = article;
        item.Quantity = 2;

        Request request = new Request();
        request.Code = 1;
        request.Client = client;
        request.Date = DateTime.Now;
        request.Items = new RequestItem[] { item };

        Console.WriteLine($"Сумма заказа: {request.TotalSum} руб.");
    }
}