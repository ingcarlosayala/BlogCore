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
    public class SliderRepositorio : Repositorio<Slider>, ISliderRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public SliderRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Actualizar(Slider slider)
        {
            var sliderDB = await dbContext.Slider.FirstOrDefaultAsync(i => i.Id == slider.Id);

            if (sliderDB != null)
            {
                sliderDB.Nombre = slider.Nombre;
                sliderDB.ImagenUrl = slider.ImagenUrl;
                sliderDB.Estado = slider.Estado;
            }
        }
    }
}
