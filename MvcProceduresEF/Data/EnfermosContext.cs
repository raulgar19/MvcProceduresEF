using Microsoft.EntityFrameworkCore;
using MvcProceduresEF.Models;

namespace MvcProceduresEF.Data
{
    public class EnfermosContext: DbContext
    {
        public EnfermosContext(DbContextOptions<EnfermosContext> options) : base(options) { }
        public DbSet<Enfermo> Enfermos { get; set; }
        public DbSet<Doctor> Doctores { get; set; }
    }
}