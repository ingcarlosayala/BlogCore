using BlogCore.AccesoDatos.Data;
using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Inicializador
{
    public class InicializadorDB : IInicializadorDB
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public InicializadorDB(ApplicationDbContext dbContext,UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public void Inicializar()
        {
            try
            {
                if (dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    dbContext.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

            if (dbContext.Roles.Any(ro => ro.Name == DS.Admin))
            {
                return;
            }

            roleManager.CreateAsync(new IdentityRole(DS.Admin)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(DS.Cliente)).GetAwaiter().GetResult();

            userManager.CreateAsync(new AppUsuario
            {
                UserName = "administrador@gmail.com",
                Email = "administrador@gmail.com",
                EmailConfirmed = true,
                Nombre = "Carlos Ayala"
            },"Admin123*").GetAwaiter().GetResult();

            AppUsuario usuario = dbContext.AppUsuario.Where(u => u.Email == "administrador@gmail.com")
                                                     .FirstOrDefault();
            userManager.AddToRoleAsync(usuario,DS.Admin).GetAwaiter().GetResult();
        }
    }
}
