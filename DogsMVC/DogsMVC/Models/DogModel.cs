using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DogsMVC.Models
{
    public class DogModel
    {
        [Key]
        public int Id {get; set;}
        public string Name {get; set;}
        public string Breed {get; set;}
        public int Age {get; set;}
        public string ImageUrl { get; set; }
        public bool IsSold { get; set; }
        public int Price { get; set; }
    }
}