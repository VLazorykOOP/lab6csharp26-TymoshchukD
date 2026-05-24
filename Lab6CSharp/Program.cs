using System;
using System.Collections;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("===== ЗАВДАННЯ 1: ІНТЕРФЕЙСИ ДЛЯ ТОВАРІВ =====");

        IGoods[] goods =
        {
            new Toy("LEGO", 1500, "Конструктор", 7),
            new Toy("М'яч", 350, "Спортивна іграшка", 3),
            new Product("Хліб", 25, "КиївХліб", 5),
            new Product("Шоколад", 60, "Roshen", 180),
            new DairyProduct("Молоко", 42, "Галичина", 7, 2.5),
            new DairyProduct("Йогурт", 35, "Danone", 14, 3.2)
        };

        foreach (IGoods item in goods)
        {
            item.Show();
            Console.WriteLine();
        }

        Console.WriteLine("--- Особливі методи через type pattern ---");
        ShowSpecialMethods(goods);

        Console.WriteLine("\n===== ЗАВДАННЯ 2: ІНТЕРФЕЙСИ ДЛЯ КЛІЄНТІВ БАНКУ =====");

        IBankClient[] clients =
        {
            new Depositor("Іваненко", new DateTime(2024, 5, 10), 20000, 12),
            new Creditor("Петренко", new DateTime(2024, 5, 10), 50000, 18, 15000),
            new Organization("ТОВ Альфа", new DateTime(2023, 3, 15), "UA123456", 120000),
            new Depositor("Сидоренко", new DateTime(2025, 1, 20), 35000, 10),
            new Organization("ПП Весна", new DateTime(2024, 5, 10), "UA987654", 80000)
        };

        Console.WriteLine("--- Повна база клієнтів ---");
        foreach (IBankClient client in clients)
        {
            client.Show();
            Console.WriteLine();
        }

        DateTime searchDate = new DateTime(2024, 5, 10);

        Console.WriteLine($"--- Пошук клієнтів за датою {searchDate:dd.MM.yyyy} ---");
        foreach (IBankClient client in clients)
        {
            if (client.IsMatch(searchDate))
            {
                client.Show();
                Console.WriteLine();
            }
        }

        Console.WriteLine("--- Сортування клієнтів за датою початку співпраці ---");
        Array.Sort(clients);

        foreach (IBankClient client in clients)
        {
            client.Show();
            Console.WriteLine();
        }

        Console.WriteLine("\n===== ЗАВДАННЯ 3: ОБРОБКА ПОМИЛОК =====");

        try
        {
            Product testProduct = new Product("Тестовий товар", -50, "Test", 10);
        }
        catch (InvalidPriceException ex)
        {
            Console.WriteLine("Власний виняток:");
            Console.WriteLine(ex.Message);
        }

        try
        {
            checked
            {
                int a = int.MaxValue;
                int result = a + 1;
                Console.WriteLine(result);
            }
        }
        catch (OverflowException ex)
        {
            Console.WriteLine("Стандартний виняток OverflowException:");
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\n===== ЗАВДАННЯ 4: FOREACH ДЛЯ ВЛАСНОГО КЛАСУ =====");

        GoodsCollection collection = new GoodsCollection();

        collection.Add(new Toy("Лялька", 500, "Дитяча іграшка", 4));
        collection.Add(new Product("Печиво", 45, "Світоч", 120));
        collection.Add(new DairyProduct("Кефір", 38, "Молокія", 5, 1.0));

        foreach (IGoods item in collection)
        {
            item.Show();
            Console.WriteLine();
        }

        Console.WriteLine("Кінець програми.");
    }

    static void ShowSpecialMethods(IGoods[] goods)
    {
        foreach (IGoods item in goods)
        {
            if (item is Toy toy)
            {
                Console.WriteLine($"{toy.Name}: {toy.GetAgeRecommendation()}");
            }
            else if (item is DairyProduct dairy)
            {
                Console.WriteLine($"{dairy.Name}: {dairy.GetFatInfo()}");
            }
            else if (item is Product product)
            {
                Console.WriteLine($"{product.Name}: {product.GetExpirationInfo()}");
            }
        }
    }
}

// ======================================================
// ЗАВДАННЯ 1
// Інтерфейси для товарів
// ======================================================

interface IShowable
{
    void Show();
}

interface IGoods : IShowable, IComparable<IGoods>
{
    string Name { get; set; }
    double Price { get; set; }
}

interface IExpirable
{
    int ExpirationDays { get; set; }
    string GetExpirationInfo();
}

interface IToyInfo
{
    string GetAgeRecommendation();
}

// Базовий абстрактний клас
abstract class Goods : IGoods
{
    public string Name { get; set; }

    private double price;

    public double Price
    {
        get
        {
            return price;
        }
        set
        {
            if (value < 0)
            {
                throw new InvalidPriceException("Ціна товару не може бути від'ємною.");
            }

            price = value;
        }
    }

    public Goods(string name, double price)
    {
        Name = name;
        Price = price;
    }

    public abstract void Show();

    public int CompareTo(IGoods? other)
    {
        if (other == null)
        {
            return 1;
        }

        return Price.CompareTo(other.Price);
    }
}

class Toy : Goods, IToyInfo
{
    public string Type { get; set; }
    public int MinAge { get; set; }

    public Toy(string name, double price, string type, int minAge)
        : base(name, price)
    {
        Type = type;
        MinAge = minAge;
    }

    public override void Show()
    {
        Console.WriteLine("Тип: Іграшка");
        Console.WriteLine($"Назва: {Name}");
        Console.WriteLine($"Ціна: {Price} грн");
        Console.WriteLine($"Вид іграшки: {Type}");
        Console.WriteLine($"Мінімальний вік: {MinAge}+");
    }

    public string GetAgeRecommendation()
    {
        return $"Рекомендований вік: від {MinAge} років";
    }
}

class Product : Goods, IExpirable
{
    public string Manufacturer { get; set; }
    public int ExpirationDays { get; set; }

    public Product(string name, double price, string manufacturer, int expirationDays)
        : base(name, price)
    {
        Manufacturer = manufacturer;
        ExpirationDays = expirationDays;
    }

    public override void Show()
    {
        Console.WriteLine("Тип: Продукт");
        Console.WriteLine($"Назва: {Name}");
        Console.WriteLine($"Ціна: {Price} грн");
        Console.WriteLine($"Виробник: {Manufacturer}");
        Console.WriteLine($"Термін придатності: {ExpirationDays} днів");
    }

    public string GetExpirationInfo()
    {
        return $"Термін придатності продукту: {ExpirationDays} днів";
    }
}

class DairyProduct : Product
{
    public double FatPercent { get; set; }

    public DairyProduct(string name, double price, string manufacturer, int expirationDays, double fatPercent)
        : base(name, price, manufacturer, expirationDays)
    {
        FatPercent = fatPercent;
    }

    public override void Show()
    {
        Console.WriteLine("Тип: Молочний продукт");
        Console.WriteLine($"Назва: {Name}");
        Console.WriteLine($"Ціна: {Price} грн");
        Console.WriteLine($"Виробник: {Manufacturer}");
        Console.WriteLine($"Термін придатності: {ExpirationDays} днів");
        Console.WriteLine($"Жирність: {FatPercent}%");
    }

    public string GetFatInfo()
    {
        return $"Жирність молочного продукту: {FatPercent}%";
    }
}

// ======================================================
// ЗАВДАННЯ 2
// Клієнти банку + інтерфейс, який успадковує .NET інтерфейс
// ======================================================

interface IBankClient : IShowable, IComparable<IBankClient>
{
    string Name { get; set; }
    DateTime StartDate { get; set; }

    bool IsMatch(DateTime date);
}

abstract class Client : IBankClient
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }

    public Client(string name, DateTime startDate)
    {
        Name = name;
        StartDate = startDate;
    }

    public abstract void Show();

    public virtual bool IsMatch(DateTime date)
    {
        return StartDate.Date == date.Date;
    }

    public int CompareTo(IBankClient? other)
    {
        if (other == null)
        {
            return 1;
        }

        return StartDate.CompareTo(other.StartDate);
    }
}

class Depositor : Client
{
    public double DepositAmount { get; set; }
    public double DepositPercent { get; set; }

    public Depositor(string surname, DateTime openDate, double depositAmount, double depositPercent)
        : base(surname, openDate)
    {
        DepositAmount = depositAmount;
        DepositPercent = depositPercent;
    }

    public override void Show()
    {
        Console.WriteLine("Тип клієнта: Вкладник");
        Console.WriteLine($"Прізвище: {Name}");
        Console.WriteLine($"Дата відкриття внеску: {StartDate:dd.MM.yyyy}");
        Console.WriteLine($"Розмір внеску: {DepositAmount} грн");
        Console.WriteLine($"Відсоток по внеску: {DepositPercent}%");
    }
}

class Creditor : Client
{
    public double CreditAmount { get; set; }
    public double CreditPercent { get; set; }
    public double DebtBalance { get; set; }

    public Creditor(string surname, DateTime creditDate, double creditAmount, double creditPercent, double debtBalance)
        : base(surname, creditDate)
    {
        CreditAmount = creditAmount;
        CreditPercent = creditPercent;
        DebtBalance = debtBalance;
    }

    public override void Show()
    {
        Console.WriteLine("Тип клієнта: Кредитор");
        Console.WriteLine($"Прізвище: {Name}");
        Console.WriteLine($"Дата видачі кредиту: {StartDate:dd.MM.yyyy}");
        Console.WriteLine($"Розмір кредиту: {CreditAmount} грн");
        Console.WriteLine($"Відсоток по кредиту: {CreditPercent}%");
        Console.WriteLine($"Остача боргу: {DebtBalance} грн");
    }
}

class Organization : Client
{
    public string AccountNumber { get; set; }
    public double AccountBalance { get; set; }

    public Organization(string organizationName, DateTime accountOpenDate, string accountNumber, double accountBalance)
        : base(organizationName, accountOpenDate)
    {
        AccountNumber = accountNumber;
        AccountBalance = accountBalance;
    }

    public override void Show()
    {
        Console.WriteLine("Тип клієнта: Організація");
        Console.WriteLine($"Назва: {Name}");
        Console.WriteLine($"Дата відкриття рахунку: {StartDate:dd.MM.yyyy}");
        Console.WriteLine($"Номер рахунку: {AccountNumber}");
        Console.WriteLine($"Сума на рахунку: {AccountBalance} грн");
    }
}

// ======================================================
// ЗАВДАННЯ 3
// Власний виняток + OverflowException
// ======================================================

class InvalidPriceException : Exception
{
    public InvalidPriceException()
        : base("Некоректна ціна товару.")
    {
    }

    public InvalidPriceException(string message)
        : base(message)
    {
    }
}

// ======================================================
// ЗАВДАННЯ 4
// IEnumerable для використання foreach
// ======================================================

class GoodsCollection : IEnumerable<IGoods>
{
    private List<IGoods> goods = new List<IGoods>();

    public void Add(IGoods item)
    {
        goods.Add(item);
    }

    public IEnumerator<IGoods> GetEnumerator()
    {
        return goods.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}