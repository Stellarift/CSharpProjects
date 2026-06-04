using System;
using System.Xml;
using static System.Console;

namespace XmlOrdersApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlFile = "Orders.xml";
            
            // ЗАДАНИЕ 1: Создание XML-файла с заказами
            CreateOrdersXml(xmlFile);
            
            // ЗАДАНИЕ 2: Чтение XML-файла с помощью XmlDocument
            WriteLine("\n=== Чтение с помощью XmlDocument ===");
            ReadWithXmlDocument(xmlFile);
            
            // ЗАДАНИЕ 3: Чтение XML-файла с помощью XmlTextReader
            WriteLine("\n=== Чтение с помощью XmlTextReader ===");
            ReadWithXmlTextReader(xmlFile);
        }
        
        // Создание XML-файла с заказами
        static void CreateOrdersXml(string fileName)
        {
            XmlTextWriter writer = null;
            try
            {
                writer = new XmlTextWriter(fileName, System.Text.Encoding.Unicode);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("Orders");
                
                // Заказ №1
                writer.WriteStartElement("Order");
                writer.WriteAttributeString("Id", "1001");
                writer.WriteAttributeString("Date", "2024-01-15");
                writer.WriteElementString("Customer", "Иван Иванов");
                
                writer.WriteStartElement("Products");
                
                // Товар 1
                writer.WriteStartElement("Product");
                writer.WriteAttributeString("Code", "A001");
                writer.WriteElementString("Name", "Ноутбук Lenovo");
                writer.WriteElementString("Quantity", "1");
                writer.WriteElementString("Price", "45000");
                writer.WriteEndElement();
                
                // Товар 2
                writer.WriteStartElement("Product");
                writer.WriteAttributeString("Code", "A002");
                writer.WriteElementString("Name", "Мышь беспроводная");
                writer.WriteElementString("Quantity", "2");
                writer.WriteElementString("Price", "1200");
                writer.WriteEndElement();
                
                writer.WriteEndElement(); // Products
                writer.WriteElementString("Total", "47400");
                writer.WriteEndElement(); // Order
                
                // Заказ №2
                writer.WriteStartElement("Order");
                writer.WriteAttributeString("Id", "1002");
                writer.WriteAttributeString("Date", "2024-01-16");
                writer.WriteElementString("Customer", "Петр Петров");
                
                writer.WriteStartElement("Products");
                
                // Товар 3
                writer.WriteStartElement("Product");
                writer.WriteAttributeString("Code", "B001");
                writer.WriteElementString("Name", "Смартфон Samsung");
                writer.WriteElementString("Quantity", "1");
                writer.WriteElementString("Price", "32000");
                writer.WriteEndElement();
                
                // Товар 4
                writer.WriteStartElement("Product");
                writer.WriteAttributeString("Code", "B002");
                writer.WriteElementString("Name", "Чехол для телефона");
                writer.WriteElementString("Quantity", "1");
                writer.WriteElementString("Price", "800");
                writer.WriteEndElement();
                
                writer.WriteEndElement(); // Products
                writer.WriteElementString("Total", "32800");
                writer.WriteEndElement(); // Order
                
                writer.WriteEndElement(); // Orders
                writer.WriteEndDocument();
                
                WriteLine($"Файл {fileName} успешно создан!");
            }
            catch (Exception ex)
            {
                WriteLine($"Ошибка при создании XML: {ex.Message}");
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        
        // Чтение с помощью XmlDocument
        static void ReadWithXmlDocument(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                
                XmlNodeList orders = doc.GetElementsByTagName("Order");
                
                foreach (XmlNode order in orders)
                {
                    string orderId = order.Attributes["Id"]?.Value ?? "N/A";
                    string orderDate = order.Attributes["Date"]?.Value ?? "N/A";
                    string customer = order["Customer"]?.InnerText ?? "N/A";
                    string total = order["Total"]?.InnerText ?? "N/A";
                    
                    WriteLine($"\nЗаказ №{orderId} от {orderDate}");
                    WriteLine($"Клиент: {customer}");
                    WriteLine($"Общая сумма: {total} руб.");
                    WriteLine("Товары:");
                    
                    XmlNode productsNode = order["Products"];
                    if (productsNode != null)
                    {
                        XmlNodeList products = productsNode.SelectNodes("Product");
                        foreach (XmlNode product in products)
                        {
                            string code = product.Attributes["Code"]?.Value ?? "N/A";
                            string name = product["Name"]?.InnerText ?? "N/A";
                            string quantity = product["Quantity"]?.InnerText ?? "N/A";
                            string price = product["Price"]?.InnerText ?? "N/A";
                            
                            WriteLine($"  - {name} (Код: {code})");
                            WriteLine($"    Количество: {quantity}, Цена: {price} руб.");
                        }
                    }
                    WriteLine(new string('-', 40));
                }
            }
            catch (Exception ex)
            {
                WriteLine($"Ошибка при чтении XmlDocument: {ex.Message}");
            }
        }
        
        // Чтение с помощью XmlTextReader
        static void ReadWithXmlTextReader(string fileName)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(fileName);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                
                string currentElement = "";
                string currentOrderId = "";
                string currentOrderDate = "";
                string currentCustomer = "";
                string currentTotal = "";
                
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        currentElement = reader.Name;
                        
                        if (reader.Name == "Order")
                        {
                            currentOrderId = reader.GetAttribute("Id") ?? "N/A";
                            currentOrderDate = reader.GetAttribute("Date") ?? "N/A";
                            WriteLine($"\nЗаказ №{currentOrderId} от {currentOrderDate}");
                        }
                        else if (reader.Name == "Customer")
                        {
                            currentCustomer = reader.ReadElementString();
                            WriteLine($"Клиент: {currentCustomer}");
                        }
                        else if (reader.Name == "Product")
                        {
                            string productCode = reader.GetAttribute("Code") ?? "N/A";
                            WriteLine($"Товар (Код: {productCode}):");
                        }
                        else if (reader.Name == "Name")
                        {
                            string productName = reader.ReadElementString();
                            WriteLine($"  Название: {productName}");
                        }
                        else if (reader.Name == "Quantity")
                        {
                            string quantity = reader.ReadElementString();
                            WriteLine($"  Количество: {quantity}");
                        }
                        else if (reader.Name == "Price")
                        {
                            string price = reader.ReadElementString();
                            WriteLine($"  Цена: {price} руб.");
                        }
                        else if (reader.Name == "Total")
                        {
                            currentTotal = reader.ReadElementString();
                            WriteLine($"Итого: {currentTotal} руб.");
                            WriteLine(new string('-', 40));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine($"Ошибка при чтении XmlTextReader: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}