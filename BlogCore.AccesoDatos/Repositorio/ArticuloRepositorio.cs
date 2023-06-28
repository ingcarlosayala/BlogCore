using BlogCore.AccesoDatos.Data;
using BlogCore.AccesoDatos.Repositorio.IRepositorio;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Repositorio
{
    public class ArticuloRepositorio : Repositorio<Articulo>, IArticuloRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public ArticuloRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Actualizar(Articulo articulo)
        {
            var articuloDB = await dbContext.Articulo.FirstOrDefaultAsync(a => a.Id == articulo.Id);

            if (articuloDB != null)
            {
                articuloDB.Nombre = articulo.Nombre;
                articuloDB.Descripcion = articulo.Descripcion;
                articuloDB.ImagenUrl = articulo.ImagenUrl;
                articuloDB.CategoriaId = articulo.CategoriaId;
            }
        }
    }
}
