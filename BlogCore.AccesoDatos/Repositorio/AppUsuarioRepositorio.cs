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
    public class AppUsuarioRepositorio : Repositorio<AppUsuario>, IAppUsuarioRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public AppUsuarioRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task BloquearUsuario(string id)
        {
            var usuarioDb = await dbContext.AppUsuario.FirstOrDefaultAsync(u => u.Id == id);
            if (usuarioDb != null)
            {
                usuarioDb.LockoutEnd = DateTime.Now.AddYears(1);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DesbloquearUsuario(string id)
        {
            var usuarioDb = await dbContext.AppUsuario.FirstOrDefaultAsync(u => u.Id == id);
            if (usuarioDb != null)
            {
                usuarioDb.LockoutEnd = DateTime.Now;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
