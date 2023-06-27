using BlogCore.AccesoDatos.Repositorio.IRepositorio;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;

        public CategoriasController(IUnidadTrabajo unidadTrabajo)
        {
            this.unidadTrabajo = unidadTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }


        [HttpPost] //Crear
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await unidadTrabajo.Categoria.Agregar(categoria);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        [HttpGet] //Editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaDB = await unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());

            if (categoriaDB == null)
            {
                return NotFound();
            }

            return View(categoriaDB);
        }

        [HttpPost, ActionName("Edit")] //Editar Categoria
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCategoria(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await unidadTrabajo.Categoria.Actualizar(categoria);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var listaCategoria = await unidadTrabajo.Categoria.ObtenerTodos();
            return Json(new {data = listaCategoria});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Error al eliminar la categoria" });
            }

            var categoriaDB = await unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());
            unidadTrabajo.Categoria.Eliminar(categoriaDB);
            await unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoria eliminada exitosamente"});
        }
        #endregion
    }
}
