using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("estadoagente",Schema="crm")]
    public class EstadoAgente
    {
        [Key]
        [Column("id")]
        public Int32 Id { get; set; }

        [ForeignKey("Agente")]
        [Column("nroage",Order=0)]
        public String AgenteId { get; set;}

        [ForeignKey("Estado")]
        [Column("codest", Order=1)]
        public String EstadoId { get; set; }

        [Column("fecini")]
        public DateTime FechaInicio { get; set; }

        [Column("fecfin")]
        public DateTime FechaFin { get; set; }

        [ForeignKey("Motivo")]
        [Column("motivo", Order=2)]
        public String MotivoId { get; set; }

        public virtual Entities.Agente Agente { get; set; }

        public virtual SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro Estado { get; set; }

        public virtual SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro Motivo { get; set; }
    }
}