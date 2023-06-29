using BlogCore.AccesoDatos.Data;
using BlogCore.AccesoDatos.Repositorio.IRepositorio;
using BlogCore.Models;
using BlogCore.Models.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;
        private readonly ApplicationDbContext dbContext;

        public HomeController(IUnidadTrabajo unidadTrabajo,ApplicationDbContext dbContext)
        {
            this.unidadTrabajo = unidadTrabajo;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                ListaArticulos = await unidadTrabajo.Articulo.ObtenerTodos(),
                ListaSlider = await unidadTrabajo.Slider.ObtenerTodos()
            };

            return View(homeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var articulo = await dbContext.Articulo.Include(c => c.Categoria).FirstOrDefaultAsync(a => a.Id == id);

            return View(articulo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}