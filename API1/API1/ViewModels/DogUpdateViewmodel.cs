using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace API1.ViewModels
{
    public class DogUpdateViewmodel : DogBaseViewmodel
    {
        public int Id {get; set;}
    }
}