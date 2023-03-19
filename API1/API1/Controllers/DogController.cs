using API1.Entities;
using API1.Data;
using API1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API1.Controllers
{
    [ApiController]
    [Route("api/v1/dogs")]
    public class DogController : ControllerBase
    {
        private readonly DogsContext _context;
        private readonly string _imageBaseUrl;
        public DogController(DogsContext context, IConfiguration config)
        {
            _context = context;
            _imageBaseUrl = config.GetSection("apiImageUrl").Value;
        }

        [HttpGet()]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Dogs
                .Where(c => c.IsSold == false)
                .Select(v => new
                {
                    Id = v.Id,
                    Name = v.Name,
                    Breed = v.Breed,
                    Age = v.Age,
                    ImageUrl = _imageBaseUrl + v.ImageUrl ?? "no-dog.png",
                    IsSold = v.IsSold,
                    Price = v.Price
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Dogs
                .Select(v => new
                {
                    Id = v.Id,
                    Name = v.Name,
                    Breed = v.Breed,
                    Age = v.Age,
                    ImageUrl = _imageBaseUrl + v.ImageUrl ?? "no-dog.png",
                    IsSold = v.IsSold,
                    Price = v.Price
                })
                .SingleOrDefaultAsync(c => c.Id == id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DogBaseViewmodel dog)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            if (await _context.Dogs.SingleOrDefaultAsync(c => c.Name == dog.Name) is not null)
            {
                return BadRequest($"Hunden med namnet {dog.Name} finns redan i systemet");
            }

            var dogToAdd = new Dog
            {
                Name = dog.Name,
                Breed = dog.Breed,
                Age = dog.Age,
                ImageUrl = dog.ImageUrl ?? "no-dog.png",
                IsSold = dog.IsSold,
                Price = dog.Price
            };

            try
            {
                await _context.Dogs.AddAsync(dogToAdd);

                if (await _context.SaveChangesAsync() > 0)
                {
                    // return StatusCode(201);
                    return CreatedAtAction(nameof(GetById), new { id = dogToAdd.Id },
                    new
                    {
                        Id = dogToAdd.Id,
                        Name = dogToAdd.Name,
                        Breed = dogToAdd.Breed,
                        Age = dogToAdd.Age,
                        ImageUrl = dog.ImageUrl ?? "no-dog.png",
                        IsSold = dog.IsSold,
                        Price = dog.Price
                    });
                }

                return StatusCode(500, "Internal Server Error");
            }
            catch (Exception ex)
            {
                // loggning till en databas som hanterar debug information...
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDog (int id, DogUpdateViewmodel model)
        {
            if(!ModelState.IsValid) return BadRequest("Felaktig information");

            var dog = await _context.Dogs.FindAsync(id);
            {
                dog.Id = model.Id;
                dog.Name = model.Name;
                dog.Breed = model.Breed;
                dog.Age = model.Age;
                dog.IsSold = model.IsSold;
                dog.Price = model.Price;
                dog.ImageUrl = string.IsNullOrEmpty(model.ImageUrl) ? "no-dog.png" : model.ImageUrl;
            };

            if (dog is null) return BadRequest("Tyvärr vi kan inte hitta hunden som ska ändras");
            
            _context.Dogs.Update(dog);

            if(await _context.SaveChangesAsync() > 0)
                return NoContent();

            return StatusCode(500, "Internal server error");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> MarkAsSold(int id)
        {
            var dog = await _context.Dogs.FindAsync(id);

            if(dog is null) return NotFound("Hittade inte hunden");

            dog.IsSold = true;

            _context.Dogs.Update(dog);

            if(await _context.SaveChangesAsync() > 0)
                return NoContent();

            return StatusCode(500, "internal server error");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            var dog = await _context.Dogs.FindAsync(id);

            if(dog is null) return NotFound("Hittade inte hunden");

            _context.Dogs.Remove(dog);

            if(await _context.SaveChangesAsync() > 0)
                return NoContent();

            return StatusCode(500, "internal server error");
        }
    }
}