using API1.Entities;
using System.Text.Json;

namespace API1.Data
{
    public class SeedData
    {

        public static async Task LoadDogData(DogsContext context)
        {
            // Steg 1.
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (context.Dogs.Any()) return;

            var json = System.IO.File.ReadAllText("Data/json/dogs.json");

            var dogs = JsonSerializer.Deserialize<List<Dog>>(json, options);

            if (dogs is not null && dogs.Count > 0)
            {
                await context.Dogs.AddRangeAsync(dogs);
                await context.SaveChangesAsync();
            }
        }
    }
}