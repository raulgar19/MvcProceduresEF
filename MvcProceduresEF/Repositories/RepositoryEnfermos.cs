using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcProceduresEF.Data;
using MvcProceduresEF.Models;
using System;
using System.Data;
using System.Data.Common;

namespace MvcProceduresEF.Repositories
{
    #region PROCEDURES
    //  create procedure SP_ALL_ENFERMOS
    //  as
    //    select* from ENFERMO
    //  go

    //  create procedure SP_FIND_ENFERMO
    //  (@inscripcion nvarchar(50))
    //  as
    //    select* from ENFERMO where INSCRIPCION = @inscripcion
    //  go

    //  create procedure SP_DELETE_ENFERMO
    //  (@inscripcion nvarchar(50))
    //  as
    //    delete from ENFERMO where INSCRIPCION = @inscripcion
    //  go

    //  create procedure SP_INSERT_ENFERMO
    //  (@inscripcion nvarchar(50), @apellido nvarchar(50), @direccion nvarchar(50), @fechaNacimiento datetime, @genero nvarchar(50), @nss nvarchar(50))
    //  as
	//    insert into ENFERMO values(@inscripcion, @apellido, @direccion, @fechaNacimiento, @genero, @nss)
    //  go
    #endregion

    public class RepositoryEnfermos
    {
        private HospitalContext context;

        public RepositoryEnfermos(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Enfermo>> GetEnfermosAsync()
        {
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_ALL_ENFERMOS";
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;

                await com.Connection.OpenAsync();

                DbDataReader reader = await com.ExecuteReaderAsync();

                List<Enfermo> enfermos = new List<Enfermo>();

                while (reader.Read())
                {
                    Enfermo enfermo = new Enfermo
                    {
                        Inscripcion = reader["INSCRIPCION"].ToString(),
                        Apellido = reader["APELLIDO"].ToString(),
                        Direccion = reader["DIRECCION"].ToString(),
                        FechaNacimiento = DateTime.Parse(reader["FECHA_NAC"].ToString()),
                        Genero = reader["S"].ToString(),
                        Nss = reader["NSS"].ToString()
                    };
                    enfermos.Add(enfermo);
                }

                await reader.CloseAsync();
                await com.Connection.CloseAsync();

                return enfermos;
            }
        }

        public async Task<Enfermo> FindEnfermoAsync(string inscripcion)
        {
            string sql = "SP_FIND_ENFERMO @inscripcion";

            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);

            var consulta = await this.context.Enfermos.FromSqlRaw(sql, pamIns).ToListAsync();

            Enfermo enfermo = consulta.FirstOrDefault();

            return enfermo;
        }

        public async Task DeleteEnfermoAsync(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO";

            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);

            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Parameters.Add(pamIns);

                await com.Connection.OpenAsync();

                await com.ExecuteNonQueryAsync();

                await com.Connection.CloseAsync();
                com.Parameters.Clear();
            }
        }

        public async Task DeleteEnfermoRawAsync(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @inscripcion";

            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);

            await this.context.Database.ExecuteSqlRawAsync(sql, pamIns);
        }

        public async Task InsertEnfermoAsync(string inscripcion, string apellido, string direccion, DateTime fechaNacimiento, string genero, string nss)
        {
            string sql = "SP_INSERT_ENFERMO @inscripcion, @apellido, @direccion, @fechaNacimiento, @genero, @nss";

            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);
            SqlParameter pamApellido = new SqlParameter("@apellido", apellido);
            SqlParameter pamDireccion = new SqlParameter("@direccion", direccion);
            SqlParameter pamFechaNacimiento = new SqlParameter("@fechaNacimiento", fechaNacimiento);
            SqlParameter pamGenero = new SqlParameter("@genero", genero);
            SqlParameter pamNss = new SqlParameter("@nss", nss);

            await this.context.Database.ExecuteSqlRawAsync(sql, [pamIns, pamApellido, pamDireccion, pamFechaNacimiento, pamGenero, pamNss]);
        }
    }
}