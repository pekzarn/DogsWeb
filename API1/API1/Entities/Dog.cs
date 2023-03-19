using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API1.Entities
{
    public class Dog
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Breed {get; set;}
        public int Age {get; set;}
        public string ImageUrl { get; set; }
        public bool IsSold { get; set; }
        public int Price { get; set; }
    }
}