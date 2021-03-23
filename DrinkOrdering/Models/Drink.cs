using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DrinkOrdering.Models
{
    public class Drink
    {
        [Key]
        public int DrinkId { get; set; }
        [Required]
        [Display(Name="Drink's Name")]
        public string Name { get; set; }
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }
        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
        [Display(Name = "Drink's Image")]
        public byte[] ImageThumbnailUrl { get; set; }
        [Display(Name = "Is this drink preferred one?")]
        public bool IsPreferredDrink { get; set; }
        [Display(Name = "Is it available on stock?")]
        public bool InStock { get; set; }
        [Display( Name= "Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
