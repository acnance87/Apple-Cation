using Apple_Cation;
using System.Reflection;
using Newtonsoft.Json;

internal class Program {
    private static async Task Main() {
        Console.WriteLine("Thank you for your time, and enjoy! (Press any key to continue)" + Environment.NewLine);
        Console.ReadKey(true);

        var questions = new string[] {
            "Which department has the largest number of employees who like Pineapple on their pizzas?",
            "Which department prefers Peperoni and Onions?",
            "How many people prefer Anchovies?",
            "How many pizzas would you need to order to feed the Engineering department, assuming a pizza feeds 4 people? Ignore personal preferences.",
            "Which pizza topping combination is the most popular in each department and how many employees prefer it?"
        };

        var pizzaData = await GetPizzaData();

        var pineappleFans = pizzaData
            .Where(pizza => pizza.Toppings != null && pizza.Toppings.Contains("Pineapple"))
            .GroupBy(data => data.Department)
            .OrderByDescending(group => group.Count())
            .FirstOrDefault()?
            .Select(final => final.Department)
            .First();

        WriteConsoleMessage(questions[0], pineappleFans);

        var pepperoniAndOnionFans = pizzaData
            .Where(pizza => pizza.Toppings != null && pizza.Toppings
            .Contains("Pepperoni") && pizza.Toppings.Contains("Onions"))
            .GroupBy(data => data.Department)
            .OrderByDescending(group => group.Count())
            .FirstOrDefault()?
            .Select(final => final.Department)
            .First();

        WriteConsoleMessage(questions[1], pepperoniAndOnionFans);

        var anchovieFans = pizzaData
            .Where(pizza => pizza.Toppings != null && pizza.Toppings
            .Contains("Anchovies"))
            .Count();

        WriteConsoleMessage(questions[2], anchovieFans);

        float peopleCount = pizzaData
            .Where(e => e.Department.Equals("Engineering", StringComparison.OrdinalIgnoreCase))
            .Count();

        var pizzaCount = Math.Round(peopleCount / 4f, 0);

        WriteConsoleMessage(questions[3], pizzaCount);


        var toppingCombosByDepartment = pizzaData
            .GroupBy(pizza => pizza.Department)
                    .Select(group => new {
                        Department = group.Key,
                        MostPopularToppings = group
                            .Select(pizza => string.Join(",", pizza.Toppings))
                            .GroupBy(toppings => toppings)
                            .OrderByDescending(toppingGroup => toppingGroup.Count())
                            .Select(toppingGroup => new {
                                Toppings = toppingGroup.Key,
                                EmployeeCount = toppingGroup.Count()
                            })
                            .FirstOrDefault()
                    })
                    .ToList();

        Console.WriteLine($"Starting with the most-favored topping combination by department...{Environment.NewLine}");
        foreach (var toppingCombo in toppingCombosByDepartment.OrderByDescending(e => e.MostPopularToppings.EmployeeCount)) {
            Console.WriteLine($"The {toppingCombo.Department} department prefers:");
            Console.WriteLine(
                $"{toppingCombo.MostPopularToppings.Toppings.Split(",")[0]} and " +
                $"{toppingCombo.MostPopularToppings.Toppings.Split(",")[1]} " +
                $"which is favored by {toppingCombo.MostPopularToppings.EmployeeCount} employees.{Environment.NewLine}");
        }

        Console.ReadKey(true);
    }

    /// <summary>
    /// Reads the embedded resource into a <see cref="IEnumerable{PizzaDatum}"/> collection
    /// </summary>
    /// <returns><see cref="Task{IEnumerable{PizzaDatum}}"/></returns>
    private static async Task<IEnumerable<PizzaDatum>> GetPizzaData() {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("Apple_Cation.Data.json");
        using var reader = new StreamReader(stream);

        return JsonConvert.DeserializeObject<List<PizzaDatum>>(await reader.ReadToEndAsync());
    }

    /// <summary>
    /// Function to save repeating code lines in writing console messages
    /// </summary>
    /// <param name="question"></param>
    /// <param name="answer"></param>
    private static void WriteConsoleMessage(string question, object answer) {
        Console.WriteLine(question);
        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine($"{answer}{Environment.NewLine}");
        Console.ResetColor();
    }
}