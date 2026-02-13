using Microsoft.AspNetCore.Mvc;
using MvcProceduresEF.Models;
using MvcProceduresEF.Repositories;
using System.Threading.Tasks;

namespace MvcProceduresEF.Controllers
{
    public class TrabajadoresController : Controller
    {
        private RepositoryEmpleados repo;

        public TrabajadoresController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            TrabajadoresModel model = await this.repo.GetTrabajadoresModelAsync();
            List<string> oficios = await this.repo.GetOficiosAsync();
            ViewData["OFICIOS"] = oficios;
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string oficio)
        {
            TrabajadoresModel model = await this.repo.GetTrabajadoresModelOficioAsync(oficio);
            List<string> oficios = await this.repo.GetOficiosAsync();
            ViewData["OFICIOS"] = oficios;

            return View(model);
        }
    }
}