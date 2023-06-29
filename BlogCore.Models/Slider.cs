using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [Display(Name = "Slider")]
        public string Nombre { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string ImagenUrl { get; set; }

        [Required(ErrorMessage = "Estado es requerido")]
        public bool Estado { get; set; }
    }
}
