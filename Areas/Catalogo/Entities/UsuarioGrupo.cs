using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Entities
{
    [Table("usugru", Schema = "cat")]
    public class UsuarioGrupo
    {
        [Key]
        [ForeignKey("Usuario")]
        [Column("codusu", Order = 0)]
        public String UsuarioId { get; set; }

        [Key]
        [ForeignKey("Grupo")]
        [Column("codgru", Order = 1)]
        public String GrupoId { get; set; }

        [Column("fecini")]
        public DateTime FechaInicio { get; set; }

        [Column("fecfin")]
        public DateTime FechaFin { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual Grupo Grupo { get; set; }
    }
}