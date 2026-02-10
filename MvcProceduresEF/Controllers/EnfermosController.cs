using Microsoft.AspNetCore.Mvc;
using MvcProceduresEF.Models;
using MvcProceduresEF.Repositories;

namespace MvcProceduresEF.Controllers
{
    public class EnfermosController : Controller
    {
        private RepositoryEnfermos repo;

        public EnfermosController(RepositoryEnfermos repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> IndexAsync()
        {
            List<Enfermo> enfermos = await this.repo.GetEnfermosAsync();

            return View(enfermos);
        }
    }
}