using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogsMVC.Viewmodels.Dogs
{
    public class DogViewModel
    {
        public int Id {get; set;}
        [Required(ErrorMessage = "Namn saknas!")]
        [DisplayName("Namn")]
        public string Name {get; set;}
        public string Breed {get; set;}
        public int Age {get; set;}
        public bool IsSold { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
        public string FullImageUrl => $"http://localhost:5000/{ImageUrl}";
    }
}