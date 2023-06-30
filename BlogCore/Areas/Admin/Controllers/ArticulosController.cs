using BlogCore.AccesoDatos.Repositorio.IRepositorio;
using BlogCore.Models;
using BlogCore.Models.ViewsModels;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Admin)]
    public class ArticulosController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;
        private readonly IWebHostEnvironment webHost;

        public ArticulosController(IUnidadTrabajo unidadTrabajo,IWebHostEnvironment webHost)
        {
            this.unidadTrabajo = unidadTrabajo;
            this.webHost = webHost;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new Articulo(),
                ListaCategorias = unidadTrabajo.Categoria.GetListaCategorias()
            };

            return View(articuloVM);
        }

        [HttpPost,ActionName("Crear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearArticulo(ArticuloVM articuloVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = webHost.WebRootPath;
                var archivo = HttpContext.Request.Form.Files;

                if (articuloVM.Articulo.Id == 0)
                {
                    //Nuevo Articulo

                    string nombreArchivo = Guid.NewGuid().ToString();
                    string subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivo[0].FileName);

                    using (var fileStrems = new FileStream(Path.Combine(subidas, nombreArchivo + extension),FileMode.Create))
                    {
                        archivo[0].CopyTo(fileStrems);
                    }

                    articuloVM.Articulo.FechaCreacion = DateTime.Now;
                    articuloVM.Articulo.ImagenUrl = @"\imagenes\articulos\" + nombreArchivo + extension;

                    await unidadTrabajo.Articulo.Agregar(articuloVM.Articulo);
                    await unidadTrabajo.Guardar();
                    return RedirectToAction(nameof(Index));
                }
            }

            articuloVM.ListaCategorias = unidadTrabajo.Categoria.GetListaCategorias();
            return View(articuloVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new Articulo(),
                ListaCategorias = unidadTrabajo.Categoria.GetListaCategorias()
            };

            if (id  == null)
            {
                return View(articuloVM);
            }

            articuloVM.Articulo = await unidadTrabajo.Articulo.Obtener(id.GetValueOrDefault());
            return View(articuloVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticuloVM articuloVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = webHost.WebRootPath;
                var archivo = HttpContext.Request.Form.Files;

                var ArticuloDB = await unidadTrabajo.Articulo.Obtener(articuloVM.Articulo.Id);

                if (archivo.Count() > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subida = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var Extension = Path.GetExtension(archivo[0].FileName);
                    var NuevaExtension = Path.GetExtension(archivo[0].FileName);

                    var ImagenRuta = Path.Combine(rutaPrincipal, ArticuloDB.ImagenUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(ImagenRuta))
                    {
                        System.IO.File.Delete(ImagenRuta);
                    }

                    using (var FileStrems = new FileStream(Path.Combine(subida, nombreArchivo + NuevaExtension), FileMode.Create))
                    {
                        archivo[0].CopyTo(FileStrems);
                    }

                    articuloVM.Articulo.ImagenUrl = @"\imagenes\articulos\" + nombreArchivo + NuevaExtension;

                    await unidadTrabajo.Articulo.Actualizar(articuloVM.Articulo);
                    await unidadTrabajo.Guardar();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    articuloVM.Articulo.ImagenUrl = ArticuloDB.ImagenUrl;

                }

                await unidadTrabajo.Articulo.Actualizar(articuloVM.Articulo);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            articuloVM.ListaCategorias = unidadTrabajo.Categoria.GetListaCategorias();

            return View(articuloVM);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var articuloDb = await unidadTrabajo.Articulo.ObtenerTodos(incluirPropiedades:"Categoria");
            return Json(new {data = articuloDb});

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            string rutaPrincipal = webHost.WebRootPath;
            var articuloDb = await unidadTrabajo.Articulo.Obtener(id.GetValueOrDefault());

            if (id == null)
            {
                return Json(new { success = false, message = "Error al eliminar el articulo" });
            }

            var RutaImagen = Path.Combine(rutaPrincipal, articuloDb.ImagenUrl.TrimStart('\\'));

            if (System.IO.File.Exists(RutaImagen)) //Buscar la imagen en el directorio
            {
                System.IO.File.Delete(RutaImagen);
            }

            unidadTrabajo.Articulo.Eliminar(articuloDb);
            await unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Articulo eliminado exitosamente"});
        }
        #endregion
    }
}
