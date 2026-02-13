using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcProceduresEF.Data;
using MvcProceduresEF.Models;
using System;
using System.Data;

namespace MvcProceduresEF.Repositories
{
    #region PROCEDURES AND VIEWS
    //create view V_EMPLEADOS_DEPARTAMENTOS
    //as
    // select cast(
    //    isnull(
    //    ROW_NUMBER() over (order by EMP.APELLIDO), 0) as int)
    // as ID, 
    // EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO, 
    // DEPT.DNOMBRE as DEPARTAMENTO, 
    // DEPT.LOC as LOCALIDAD
    //    from EMP
    //    inner join DEPT

    //    on EMP.DEPT_NO = DEPT.DEPT_NO
    //go

    //create view V_TRABAJADORES
    //as
    // select EMP_NO as IDTRABAJADOR, 
    // APELLIDO, OFICIO, SALARIO from EMP
    //    union

    //    select DOCTOR_NO, APELLIDO, ESPECIALIDAD, SALARIO from DOCTOR

    //    union
    //    select EMPLEADO_NO, APELLIDO, FUNCION, SALARIO from PLANTILLA
    //go

    //create procedure SP_TRABAJADORES_OFICIO
    //(@oficio nvarchar(50), @personas int out, @media int out, @suma int out)
    //as
	   // select* from V_TRABAJADORES where OFICIO = @oficio
    //    select @personas = count(IDTRABAJADOR), @media = avg(SALARIO), @suma = sum(SALARIO)

    //    from V_TRABAJADORES where OFICIO = @oficio
    //go
    #endregion

    public class RepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<TrabajadoresModel> GetTrabajadoresModelAsync()
        {
            var consulta = from datos in this.context.Trabajadores
                           select datos;

            TrabajadoresModel model = new TrabajadoresModel();
            
            model.Trabajadores = await consulta.ToListAsync();
            model.Personas = await consulta.CountAsync();
            model.SumaSalarial = await consulta.SumAsync(z => z.Salario);
            model.MediaSalarial = (int) await consulta.AverageAsync(z => z.Salario);
            
            return model;
        }

        public async Task<List<string>> GetOficiosAsync()
        {
            var consulta = (from datos in this.context.Trabajadores
                           select datos.Oficio).Distinct();

            return await consulta.ToListAsync();
        }

        public async Task<List<VistaEmpleado>> GetVistaEmpleadosAsync()
        {
            var consulta = from datos in this.context.VistaEmpleados
                           select datos;

            return await consulta.ToListAsync();
        }

        public async Task<TrabajadoresModel> GetTrabajadoresModelOficioAsync(string oficio)
        {
            string sql = "SP_TRABAJADORES_OFICIO @oficio, @personas out, @media out, @suma out";

            SqlParameter pamOficio = new SqlParameter("@oficio", oficio);
            SqlParameter pamPersonas = new SqlParameter("@personas", -1);
            pamPersonas.Direction = ParameterDirection.Output;
            SqlParameter pamMedia = new SqlParameter("@media", -1);
            pamMedia.Direction = ParameterDirection.Output;
            SqlParameter pamSuma = new SqlParameter("@suma", -1);
            pamSuma.Direction = ParameterDirection.Output;

            var consulta = this.context.Trabajadores.FromSqlRaw(sql, pamOficio, pamPersonas, pamMedia, pamSuma);

            TrabajadoresModel model = new TrabajadoresModel();

            model.Trabajadores = await consulta.ToListAsync();
            model.Personas = int.Parse(pamPersonas.Value.ToString());
            model.MediaSalarial = int.Parse(pamMedia.Value.ToString());
            model.SumaSalarial = int.Parse(pamSuma.Value.ToString());

            return model;
        }
    }
}