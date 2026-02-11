using Microsoft.AspNetCore.Mvc;
using MvcProceduresEF.Models;
using MvcProceduresEF.Repositories;
using System.Threading.Tasks;

namespace MvcProceduresEF.Controllers
{
    public class DoctoresController : Controller
    {
        private RepositoryDoctores repo;

        public DoctoresController (RepositoryDoctores repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            List<Doctor> doctores = await this.repo.GetDoctoresAsync();

            DoctorViewModel model = new DoctorViewModel
            {
                Especialidades = especialidades,
                Doctores = doctores
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string especialidad)
        {
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            List<Doctor> doctores = await this.repo.GetDoctoresEspecialidadAsync(especialidad);

            DoctorViewModel model = new DoctorViewModel
            {
                Especialidades = especialidades,
                Doctores = doctores
            };

            ViewBag.EspecialidadSeleccionada = especialidad;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarSP(string especialidad, int incremento)
        {
            await this.repo.ActualizarSalarioEspecialidadSPAsync(especialidad, incremento);

            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            List<Doctor> doctores = await this.repo.GetDoctoresEspecialidadAsync(especialidad);

            DoctorViewModel model = new DoctorViewModel
            {
                Especialidades = especialidades,
                Doctores = doctores
            };

            ViewBag.EspecialidadSeleccionada = especialidad;

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(string especialidad, int incremento)
        {
            await this.repo.ActualizarSalarioEspecialidadAsync(especialidad, incremento);

            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            List<Doctor> doctores = await this.repo.GetDoctoresEspecialidadAsync(especialidad);

            DoctorViewModel model = new DoctorViewModel
            {
                Especialidades = especialidades,
                Doctores = doctores
            };

            ViewBag.EspecialidadSeleccionada = especialidad;

            return View("Index", model);
        }
    }
}