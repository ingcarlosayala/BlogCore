using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewsModels
{
    public class HomeVM
    {
        public IEnumerable<Articulo> ListaArticulos { get; set; }
        public IEnumerable<Slider> ListaSlider { get; set; }
    }
}
