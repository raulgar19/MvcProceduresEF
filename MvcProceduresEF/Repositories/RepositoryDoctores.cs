using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcProceduresEF.Data;
using MvcProceduresEF.Models;
using System.Data;
using System.Data.Common;
using System.Numerics;

namespace MvcProceduresEF.Repositories
{
    #region PROCEDURES
    //create procedure SP_ALL_ESPECIALIDADES
    //as
    // select distinct ESPECIALIDAD from DOCTOR
    //go

    //create procedure SP_GET_DOCTORES_ESPECIALIDAD
    //(@especialidad nvarchar(50))
    //as
    // select * from DOCTOR where ESPECIALIDAD = @especialidad
    //go

    //create procedure SP_ALL_DOCTORES
    //as
    // select* from DOCTOR
    //go

    //create procedure SP_UPDATE_SALARIO_ESPECIALIDAD
    //(@especialidad nvarchar(50), @incremento int)
    //as
    // update DOCTOR set SALARIO = SALARIO + @incremento
    // where ESPECIALIDAD = @especialidad
    //go
    #endregion

    public class RepositoryDoctores
    {
        private EnfermosContext context;

        public RepositoryDoctores(EnfermosContext context)
        {
            this.context = context;
        }

        public async Task<List<string>> GetEspecialidadesAsync()
        {
            string sql = "SP_ALL_ESPECIALIDADES";

            var consulta = await this.context.Database.SqlQueryRaw<string>(sql).ToListAsync();

            return consulta;
        }

        public async Task<List<Doctor>> GetDoctoresAsync()
        {
            string sql = "SP_ALL_DOCTORES";

            var consulta = await this.context.Doctores.FromSqlRaw(sql).ToListAsync();

            return consulta;
        }

        public async Task<List<Doctor>> GetDoctoresEspecialidadAsync(string especialidad)
        {
            string sql = "SP_GET_DOCTORES_ESPECIALIDAD @especialidad";

            SqlParameter paramEspecialidad = new SqlParameter("@especialidad", especialidad);

            var consulta = await this.context.Doctores.FromSqlRaw(sql, paramEspecialidad).ToListAsync();

            return consulta;
        }

        public async Task<int> ActualizarSalarioEspecialidadSPAsync(string especialidad, int incremento)
        {
            string sql = "SP_UPDATE_SALARIO_ESPECIALIDAD @especialidad, @incremento";

            SqlParameter paramEspecialidad = new SqlParameter("@especialidad", especialidad);
            SqlParameter paramIncremento = new SqlParameter("@incremento", incremento);

            int result = await this.context.Database.ExecuteSqlRawAsync(sql, paramEspecialidad, paramIncremento);

            return result;
        }

        public async Task ActualizarSalarioEspecialidadAsync(string especialidad, int incremento)
        {
            var doctores = await this.GetDoctoresEspecialidadAsync(especialidad);

            foreach (var doctor in doctores)
            {
                doctor.Salario = doctor.Salario + incremento;
            }

            await this.context.SaveChangesAsync();
        }
    }
}