using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Entities
{
    [Table("tabacc", Schema = "cat")]
    public class Acceso
    {
        [Key]
        [ForeignKey("Programa")]
        [Column("codpro", Order = 0)]
        public String ProgramaId { get; set; }

        [Key]
        [ForeignKey("Usuario")]
        [Column("codusu", Order=1)]
        public String UsuarioId { get; set; }

        [Column("fecini")]
        public DateTime FechaInicio { get; set; }

        [Column("accsel")]
        public Boolean CheckSel { get; set; }

        [Column("accins")]
        public Boolean CheckIns { get; set; }

        [Column("accupd")]
        public Boolean CheckMod { get; set; }

        [Column("accdel")]
        public Boolean CheckEli { get; set; }

        [Column("accpri")]
        public Boolean CheckImp { get; set; }

        [Column("coduen")]
        public String UnidadEmpId { get; set; }

        [Column("codsuc")]
        public String SucursalId { get; set; }

        public virtual Programa Programa { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}