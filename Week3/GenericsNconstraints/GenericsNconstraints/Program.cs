//Console.WriteLine("----Demo - Generic Repository----");

//var userRepo = new Repository<User>();
//var productRepo = new Repository<Product>();

//userRepo.Add(new User { Name = "Sushant", Email = "svkohli1@gmail.com" });
//userRepo.Add(new User { Name = "Alice", Email = "alicia@gmail.com" });
//userRepo.CreateAndAdd();

//productRepo.Add(new Product { Name = "Car", Price = 200000.0m });
//productRepo.Add(new Product { Name = "Bike", Price = 50000.0m });

//Console.WriteLine($"All Users: {string.Join(",", userRepo.GetAll().Select(u => u.Name))}");
//Console.WriteLine($"User Name with Id 1: {userRepo.GetById(1).Name}");
//Console.WriteLine($"User Name with Id 3: {userRepo.GetById(3).Name}");
//userRepo.Remove(2);
//Console.WriteLine($"All Users: {string.Join(",", userRepo.GetAll().Select(u => u.Name))}");
//Console.WriteLine($"All Products: {string.Join(",", productRepo.GetAll().Select(u => u.Name))}");

//Console.WriteLine("---Generic method with IComparable---");

//static T Max<T>(T a, T b) where T : IComparable<T>
//    => a.CompareTo(b) >= 0 ? a : b;


//static T Min<T>(T a, T b) where T : IComparable<T>
//    => a.CompareTo(b) <= 0 ? a : b;

//Console.WriteLine($"\nMax(3,5): {Max(3, 5)}");
//Console.WriteLine($"\nMax(\"Apple\",\"Banana\"): {Max("Apple", "Banana")}");
//Console.WriteLine($"\nMin(2,6): {Min(2.6, 6.4)}");

//Console.WriteLine("---Covariance--");

//IEnumerable<string> strings = new List<string> { "Sushant", "Vyas", "Ji" };
//IEnumerable<object> objects = strings;
//// Safe because you can only READ from IEnumerable, not add to it

//foreach (var obj in objects)
//{
//    Console.WriteLine($"Type: {obj.GetType().Name}, Value: {obj}");
//}

//Console.WriteLine("---Contravariance---");

//Action<object> printObject = obj
//    => Console.WriteLine($"Printing object: {obj}");
//Action<string> printString = printObject;

//printString("Contravariance says Hiiii");

//// Show typeof(T) — useful for debugging/logging:
//void PrintTypeName<T>()
//    => Console.WriteLine($"T is: {typeof(T).Name}");


//PrintTypeName<int>();
//PrintTypeName<string>();
//PrintTypeName<User>();

//Console.WriteLine("---Action, Func & Predicate---");

//Func<string, string> trim = s => s.Trim();
//Func<string, string> upper = s => s.ToUpper();
//Func<string, int> countWord = s => s.Split(' ').Length;

//string sample = "  My Name is Sushant Vyas ";
//string processed = upper(trim(sample));

//Console.WriteLine($"Processed sample:{processed}");
//Console.WriteLine($"Word Count: {countWord(processed)}");

//Action<string, ConsoleColor> colorPrint = (msg, color) =>
//{
//    Console.ForegroundColor = color;
//    Console.WriteLine(msg);
//    Console.ResetColor();
//};

//colorPrint("Success Message", ConsoleColor.Green);
//colorPrint("Error Message", ConsoleColor.Red);

//Predicate -- Filtering

//var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

//Predicate<int> evenNums = n => n % 2 == 0;
//Predicate<int> oddNums = n => n % 2 == 1;

//var evenNumbers = numbers.FindAll(evenNums);
//var oddNumbers = numbers.FindAll(oddNums);

//Console.WriteLine($"Even Numbers: {string.Join(",", evenNumbers)}");
//Console.WriteLine($"Even Numbers: {string.Join(",", oddNumbers)}");

//Console.WriteLine("---Demo - Event Mechanism System---");
//Console.OutputEncoding = Encoding.UTF8;

//var priceMonitor = new PriceMonitor();

//priceMonitor.PriceChanged += (sender, e) =>
//{
//    Console.WriteLine($"[Email Alert] {e.ProductName}: {e.OldPrice} -> {e.newPrice}:C");
//};

//priceMonitor.PriceChanged += (sender, e) =>
//{
//    Console.WriteLine($"[Discount Alert] {e.ProductName} dropped by {e.OldPrice - e.newPrice}:C");
//};

//priceMonitor.SetPrice("Laptop", 1200m);
//priceMonitor.SetPrice("Laptop", 1100m);
//priceMonitor.SetPrice("Laptop", 890m);
//priceMonitor.SetPrice("Laptop", 1300m);
//priceMonitor.SetPrice("Mouse", 29m);
//priceMonitor.SetPrice("Mouse", 25m);

//Console.WriteLine("---Demo - Closure Trap---");

//var wrongActions = new List<Action>();

//for (int i = 0; i < 3; i++)
//{
//    wrongActions.Add(() => Console.WriteLine(i));
//}

//wrongActions.ForEach(a => a());

//Console.WriteLine();

//var rightActions = new List<Action>();

//for (int i = 0; i < 3; i++)
//{
//    int captured = i;
//    rightActions.Add(() => Console.WriteLine(captured));
//}

//rightActions.ForEach(a => a());

//Console.WriteLine();

//Console.WriteLine("\n==Closure Practical User==\n");

//Func<int, Func<int>> makeCounter = start =>
//{
//    int count = start;
//    return () => count++;
//};

//var counter1 = makeCounter(0);
//var counter2 = makeCounter(100);

//Console.WriteLine(counter1());
//Console.WriteLine(counter2());
//Console.WriteLine(counter1());
//Console.WriteLine(counter2());

//var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
//var query = numbers.Where(n =>
//{
//    Console.WriteLine($"Iterating {n}");
//    return n > 2;
//});

//Console.WriteLine("1. Query defined — nothing evaluated yet");
//Console.WriteLine("2. Starting iteration...");

//foreach (int n in query)
//{
//    Console.WriteLine(n);
//}

//numbers.Add(9);
//Console.WriteLine("Iterating again");

//foreach (int n in query)
//{
//    Console.WriteLine(n);
//}

//var materialised = query.ToList();

//numbers.Add(10);
//numbers.Add(11);

//Console.WriteLine($"After Materialisation: {string.Join(", ", materialised)}");

//Console.WriteLine(materialised.Contains(10));

//Console.WriteLine("---LINQ Queries---");

//var users = new List<User>
//{
//    new("Alice", 30, "Dublin", true, 75000),
//    new("Bob",   25, "Cork",   false, 55000),
//    new("Carol", 35, "Dublin", true,  90000),
//    new("Dave",  28, "Dublin", true,  65000),
//    new("Eve",   32, "Cork",   true,  80000),
//    new("Frank", 27, "Dublin", false, 48000),
//};

//Console.WriteLine("Active Dublin Users by Salary");

//users.Where(u => u.IsActive && u.City == "Dublin")
//    .OrderByDescending(u => u.Salary)
//    .Select(u => $" {u.Name}: {u.Salary}")
//    .ToList()
//    .ForEach(Console.WriteLine);

//Console.WriteLine("\nSalary stats (active users only):");
//var active = users.Where(u => u.IsActive);

//Console.WriteLine($" Count: {active.Count()}");
//Console.WriteLine($" Average: {active.Average(u => u.Salary):C}");
//Console.WriteLine($" Max: {active.Max(u => u.Salary):C}");
//Console.WriteLine($" Min: {active.Min(u => u.Salary):C}");

//Console.WriteLine("\nExistence Checks\n");
//Console.WriteLine($"Any under 26: {users.Any(u => u.Age < 26)}");
//Console.WriteLine($"All Active: {users.All(u => u.IsActive)}");
//Console.WriteLine($"All have salary >40K: {users.All(u => u.Salary > 40000)}");

//Console.WriteLine("---Pagination - Page 1, 2 items per page---");

//users.OrderBy(u => u.Name)
//    .Skip(0).Take(2)
//    .Select(u => $"  {u.Name}")
//    .ToList()
//    .ForEach(Console.WriteLine);

//Console.WriteLine("---Pagination - Page 2, 2 items per page---");

//users.OrderBy(u => u.Name)
//    .Skip(2).Take(2)
//    .Select(u => $"  {u.Name}")
//    .ToList()
//    .ForEach(Console.WriteLine);

//Console.WriteLine("\n Multiple Enumeration Trap \n");

//int evalCount = 0;
//var expensiveQuery = users.Where(u =>
//{
//    evalCount++;
//    return u.IsActive;
//});

//int count = expensiveQuery.Count();
//var firstUser = expensiveQuery.FirstOrDefault();

//Console.WriteLine($"Multiple enumeration: evaluated {evalCount} for 2 operations");

//evalCount = 0;
//var materialised = expensiveQuery.ToList();
//int count2 = materialised.Count();
//var firstUser2 = materialised.FirstOrDefault();

//Console.WriteLine($"After materialise: evaluated {evalCount} for 2 operations");
//public record User(string Name, int Age, string City, bool IsActive, decimal Salary);

Console.WriteLine("---Records---");

//var add1 = new Address("Westmoreland", "Dublin", "Ireland");
//var add2 = new Address("Westmoreland", "Dublin", "Ireland");
//var add3 = new Address("Marlborough", "Dublin", "Ireland");

//var emp1 = new Employee("Sushant", 30, 'M');
//var emp2 = new Employee("Anil", 65, 'M');

//Console.WriteLine($"add1==add2: {add1 == add2}");
//Console.WriteLine($"add1==add3: {add1 == add3}");

//var emp3 = emp1 with { Age = 35 };

//Console.WriteLine(emp1);
//Console.WriteLine(emp3);
//Console.WriteLine(emp2);
//Console.WriteLine(ReferenceEquals(emp1, emp3));

//Records in collections - value equality so distinct works correctly
//Console.WriteLine("---Records in Collections---");

//var employees = new List<Employee>
//{
//    new Employee("Sushant", 30, 'M'),
//    new Employee("Sushant", 30, 'M'),
//    new Employee("Anil", 65, 'M')
//};

//Console.WriteLine($"Total Employees: {employees.Count()}");
//Console.WriteLine($"Total Employees: {employees.Distinct().Count()}");

//public record Address(string Street, string City, string Country);
//public record Employee(string Name, int Age, char Sex)
//{
//    public bool isSenior => Age >= 40;
//};

//Console.WriteLine("---Pattern Matching---");

//var Orders = new List<Order>
//{
//    new(1, "Pending",   150.00m, "Dublin"),
//    new(2, "Completed", 2500.00m,"Cork"),
//    new(3, "Cancelled", 50.00m,  "Dublin"),
//    new(4, "Pending",   8000.00m,"Dublin"),
//    new(5, "Completed", 75.00m,  "Galway"),
//};

//string ClassifyOrder(Order o) => o switch
//{
//    { Status: "Cancelled" } => "Cancelled - no action",
//    { Status: "Pending", Amount: > 5000 } => "High-value pending — priority review",
//    { Status: "Pending", CustomerCity: "Dublin" } => "Dublin pending — local team",
//    { Status: "Pending" } => "Standard pending",
//    { Status: "Completed", Amount: > 1000 } => "High-value completed",
//    { Status: "Completed" } => "Standard completed",
//    _ => "Unknown status"
//};

//foreach (var order in Orders)
//{
//    Console.WriteLine($"Order {order.Id}: {ClassifyOrder(order)}");
//}


//public record Order(int Id, string Status, decimal Amount, string CustomerCity);


//Console.WriteLine("---Type Pattern---");

//Console.WriteLine("\n=== Type patterns ===\n");

//object[] values = { 42, "hello", 3.14, true, null, new List<int> { 1, 2, 3 } };

//foreach (var val in values)
//{
//    string description = val switch
//    {
//        int n => $"Integer: {n}",
//        string s => $"String: {s}",
//        double d => $"Double: {d:F2}",
//        bool b => $"Bool:{b}",
//        null => "Null value",
//        IEnumerable<int> l => $"List with {l.Count()} items",
//        _ => $"Unknown type: {val.GetType().Name}"
//    };
//    Console.WriteLine(description);

Console.WriteLine("\n=== Init-only + Nullable reference types ===\n");


var success = new ApiResponse
{
    StatusCode = 200,
    Message = "OK"
};

var error = new ApiResponse
{
    StatusCode = 400,
    Message = "Bad Request",
    ErrorDetail = "Missing required field: email"
};

Console.WriteLine($"Success: {success.StatusCode} - {success.Message}");
Console.WriteLine($"Error: {error.StatusCode} - {error.Message}");
Console.WriteLine($"Error Detail: {error.ErrorDetail ?? "none"}");
Console.WriteLine($"Success error Detail: {success.ErrorDetail ?? "none"}");

string? name = null;

string display = name ?? "no name provided";
int? nameLen = name?.Length;
name ??= "Default Name";

Console.WriteLine($"display: {display}");
Console.WriteLine($"nameLen: {nameLen?.ToString() ?? "null"}");
Console.WriteLine($"name: {name}");
public class ApiResponse
{
    public int StatusCode { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? ErrorDetail { get; init; }
    public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
}
