using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProceduresEF.Models
{
    [Table("V_TRABAJADORES")]
    public class Trabajador
    {
        [Key]
        [Column("IDTRABAJADOR")]
        public int TrabajadorId { get; set; }

        [Column("APELLIDO")]
        public string Apellido { get; set; }

        [Column("OFICIO")]
        public string Oficio { get; set; }

        [Column("SALARIO")]
        public int Salario { get; set; }
    }
}
