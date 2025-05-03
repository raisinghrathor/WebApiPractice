using System.ComponentModel.DataAnnotations;
using WebApp.Models.Validations;

namespace WebApp.Models
{
    public class Shirt
    {
        public int ShirtId {  get; set; }
        [Required]
        public string? Brand { get; set; }
        [Required]
        public string? Color { get; set; }
        [Shirt_EnsureCorrectSizingAttribute]
        public int? Size { get; set; }
        [Required]
        public string? Gender { get; set; }
        public Double? Price { get; set; }
    }
}
