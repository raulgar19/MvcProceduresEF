using Microsoft.AspNetCore.Mvc;
using MvcProceduresEF.Models;
using MvcProceduresEF.Repositories;
using System.Threading.Tasks;

namespace MvcProceduresEF.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<VistaEmpleado> empleados = await this.repo.GetVistaEmpleadosAsync();

            return View(empleados);
        }
    }
}