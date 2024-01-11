using Newtonsoft.Json;

namespace Apple_Cation {
    /// <summary>
    /// Represents a single record of pizza data.
    /// Information includes:
    /// <list type="bullet">
    /// <item><see cref="Name"/> (a person's name)</item>
    /// <item><see cref="Department"/> (the department the person belongs to)</item>
    /// <item><see cref="Toppings"/> (the preferred topping(s) of the person)</item>
    /// </list>
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Department"></param>
    /// <param name="Toppings"></param>
    public record PizzaDatum(
        [property: JsonProperty("name")] string Name, 
        [property: JsonProperty("department")] string Department, 
        [property: JsonProperty("toppings")] string[] Toppings);
}