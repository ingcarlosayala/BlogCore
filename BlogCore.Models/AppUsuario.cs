using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class AppUsuario : IdentityUser
    {
        [Required(ErrorMessage = "Nombre es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Direccion es requerida")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Ciudad es requerida")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "Pais es requerido")]
        public string Pais { get; set; }
    }
}
