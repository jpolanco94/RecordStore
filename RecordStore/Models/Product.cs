using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RecordStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please enter an album name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the artist's name")]
        public string Artist { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage ="Please enter a positive price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage ="Please enter a genre")]
        public string Genre { get; set; }
    }
}
