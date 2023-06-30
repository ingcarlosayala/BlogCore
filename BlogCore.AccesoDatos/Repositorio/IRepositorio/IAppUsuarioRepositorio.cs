using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Repositorio.IRepositorio
{
    public interface IAppUsuarioRepositorio : IRepositorio<AppUsuario>
    {
        Task DesbloquearUsuario(string id);
        Task BloquearUsuario(string id);
    }
}
