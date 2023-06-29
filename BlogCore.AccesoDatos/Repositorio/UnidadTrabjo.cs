using BlogCore.AccesoDatos.Data;
using BlogCore.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Repositorio
{
    public class UnidadTrabjo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext dbContext;

        public ICategoriaRepositorio Categoria { get; private set; }
        public IArticuloRepositorio Articulo { get; private set; }
        public ISliderRepositorio Slider { get; private set; }

        public UnidadTrabjo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            Categoria = new CategoriaRepositorio(dbContext);
            Articulo = new ArticuloRepositorio(dbContext);
            Slider = new SliderRepositorio(dbContext);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public async Task Guardar()
        {
           await dbContext.SaveChangesAsync();
        }
    }
}
