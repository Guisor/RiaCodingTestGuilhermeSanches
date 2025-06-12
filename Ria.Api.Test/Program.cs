using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int Age { get; set; }
}

public class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string[] firstNames = new[]
    {
        "Leia", "Sadie", "Jose", "Sara", "Frank",
        "Dewey", "Tomas", "Joel", "Lukas", "Carlos"
    };

    private static readonly string[] lastNames = new[]
    {
        "Liberty", "Ray", "Harrison", "Ronan", "Drew",
        "Powell", "Larsen", "Chan", "Anderson", "Lane"
    };

    private static int nextId = 1;
    private static readonly object idLock = new();
    private static readonly string API_URL = "http://localhost:5174/api/costumer";

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Waiting  API start...");
        await Task.Delay(5000);

        var tasks = new List<Task>();

        for (int i = 0; i < 10; i++)
        {
            tasks.Add(SendPostRequest());
            tasks.Add(SendGetRequest());
        }

        await Task.WhenAll(tasks);
        Console.WriteLine("Test finished, press any key");
        Console.ReadLine();
    }

    private static async Task SendPostRequest()
    {
        var customers = new List<Customer>();

        int customerCount = new Random().Next(2, 5);

        for (int i = 0; i < customerCount; i++)
        {
            customers.Add(GenerateCustomer());
        }

        try
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(API_URL, customers);
            Console.WriteLine($"POST - Status: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"POST - Erro: {ex.Message}");
        }
    }

    private static async Task SendGetRequest()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync(API_URL);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"GET - {result.Length} caracteres recebidos.");
            }
            else
            {
                Console.WriteLine($"GET - Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GET - Erro: {ex.Message}");
        }
    }

    private static Customer GenerateCustomer()
    {
        var rnd = new Random();

        string first = firstNames[rnd.Next(firstNames.Length)];
        string last = lastNames[rnd.Next(lastNames.Length)];
        int age = rnd.Next(10, 91);

        int id;
        lock (idLock)
        {
            id = nextId++;
        }

        return new Customer
        {
            Id = id,
            FirstName = first,
            LastName = last,
            Age = age
        };
    }
}
