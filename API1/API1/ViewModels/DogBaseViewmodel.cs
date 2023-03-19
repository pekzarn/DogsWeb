using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace API1.ViewModels
{
    public class DogBaseViewmodel
    {
        [Required(ErrorMessage = "Namn måste anges")]
        public string Name {get; set;}
        public string Breed {get; set;}
        public int Age {get; set;}
        public string ImageUrl { get; set; }
        public bool IsSold { get; set; }
        public int Price { get; set; }
    }
}