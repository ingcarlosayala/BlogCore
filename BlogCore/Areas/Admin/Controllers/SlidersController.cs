using BlogCore.AccesoDatos.Repositorio.IRepositorio;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;
        private readonly IWebHostEnvironment webHostEnvironment;

        public SlidersController(IUnidadTrabajo unidadTrabajo,IWebHostEnvironment webHostEnvironment)
        {
            this.unidadTrabajo = unidadTrabajo;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> listaSlider = await unidadTrabajo.Slider.ObtenerTodos();

            return View(listaSlider);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = webHostEnvironment.WebRootPath;
                var archivo = HttpContext.Request.Form.Files;

                if (slider.Id == 0)
                {
                    //Subir una imagen
                    string nombreArchivo = Guid.NewGuid().ToString();
                    string subida = Path.Combine(rutaPrincipal, @"imagenes\slider");
                    var extension = Path.GetExtension(archivo[0].FileName);

                    using (var fileStrems = new FileStream(Path.Combine(subida, nombreArchivo + extension),FileMode.Create))
                    {
                        archivo[0].CopyTo(fileStrems);
                    }

                    slider.ImagenUrl = @"\imagenes\slider\" + nombreArchivo + extension;

                    await unidadTrabajo.Slider.Agregar(slider);
                    await unidadTrabajo.Guardar();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(slider);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sliderDB = await unidadTrabajo.Slider.Obtener(id.GetValueOrDefault());

            if (sliderDB == null)
            {
                return NotFound();
            }

            return View(sliderDB);
        }

        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarSlider(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = webHostEnvironment.WebRootPath;
                var archivo = HttpContext.Request.Form.Files;

                var ImagenDB = await unidadTrabajo.Slider.Obtener(slider.Id);

                if (archivo.Count() > 0)
                {
                    //Actualizar Imagen

                    string nombreArchivo = Guid.NewGuid().ToString();
                    string subida = Path.Combine(rutaPrincipal, @"imagenes\slider");
                    var extension = Path.GetExtension(archivo[0].FileName);


                    //Eliminar la imagen
                    var RutaImagenDB = Path.Combine(rutaPrincipal, ImagenDB.ImagenUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(RutaImagenDB))
                    {
                        System.IO.File.Delete(RutaImagenDB);
                    }

                    //Subir otra imagen
                    using (var fileStrems = new FileStream(Path.Combine(subida,nombreArchivo + extension),FileMode.Create))
                    {
                        archivo[0].CopyTo(fileStrems);
                    }

                    slider.ImagenUrl = @"\imagenes\slider\" + nombreArchivo + extension;

                    await unidadTrabajo.Slider.Actualizar(slider);
                    await unidadTrabajo.Guardar();
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    slider.ImagenUrl = ImagenDB.ImagenUrl;
                }
                await unidadTrabajo.Slider.Actualizar(slider);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            return View(slider);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var listaSlider = await unidadTrabajo.Slider.ObtenerTodos();
            return Json(new { data = listaSlider});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            string rutaPrincipal = webHostEnvironment.WebRootPath;

            var imagenDB = await unidadTrabajo.Slider.Obtener(id.GetValueOrDefault());

            if (imagenDB == null)
            {
                return Json(new {success = false, message = "Error al eliminar el slider"});
            }

            var rutaImagenDB = Path.Combine(rutaPrincipal, imagenDB.ImagenUrl.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagenDB))
            {
                System.IO.File.Delete(rutaImagenDB);
            }

            unidadTrabajo.Slider.Eliminar(imagenDB);
            await unidadTrabajo.Guardar();
            return Json(new {success = true, message = "Slider eliminado exitosamente"});
        }
        #endregion
    }
}
