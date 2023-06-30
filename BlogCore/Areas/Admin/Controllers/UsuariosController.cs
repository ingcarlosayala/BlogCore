using BlogCore.AccesoDatos.Repositorio.IRepositorio;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Admin)]
    public class UsuariosController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;

        public UsuariosController(IUnidadTrabajo unidadTrabajo)
        {
            this.unidadTrabajo = unidadTrabajo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listaUsuario = await unidadTrabajo.AppUsuario.ObtenerTodos();
            return View(listaUsuario);
        }

        [HttpGet]
        public async Task<IActionResult> BloquearUsuario(string id )
        {
            await unidadTrabajo.AppUsuario.BloquearUsuario(id);
            await unidadTrabajo.Guardar();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DesbloquearUsuario(string id)
        {
            await unidadTrabajo.AppUsuario.DesbloquearUsuario(id);
            await unidadTrabajo.Guardar();
            return RedirectToAction(nameof(Index));
        }
    }
}
