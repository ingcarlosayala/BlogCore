using BlogCore.AccesoDatos.Repositorio.IRepositorio;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var usuarios = await unidadTrabajo.AppUsuario.ObtenerTodos(u => u.Id != usuarioActual.Value);
            
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> BloquearUsuario(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await unidadTrabajo.AppUsuario.BloquearUsuario(id);
            await unidadTrabajo.Guardar();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DesbloquearUsuario(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await unidadTrabajo.AppUsuario.DesbloquearUsuario(id);
            await unidadTrabajo.Guardar();
            return RedirectToAction(nameof(Index));
        }
    }
}
