using BlogCore.AccesoDatos.Data;
using BlogCore.AccesoDatos.Repositorio.IRepositorio;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public CategoriaRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Actualizar(Categoria categoria)
        {
            var obj = await dbContext.Categoria.FirstOrDefaultAsync(c => c.Id == categoria.Id);
            if (obj != null)
            {
                obj.Nombre = categoria.Nombre;
                obj.Estado = categoria.Estado;
            }
        }

        public IEnumerable<SelectListItem> GetListaCategorias()
        {
            return dbContext.Categoria.Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.Id.ToString()
            });
        }
    }
}
