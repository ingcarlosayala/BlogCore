using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo:IDisposable
    {
        ICategoriaRepositorio Categoria { get;}
        IArticuloRepositorio Articulo { get;}
        ISliderRepositorio Slider { get;}
        IAppUsuarioRepositorio AppUsuario { get;}
        Task Guardar();
    }
}
