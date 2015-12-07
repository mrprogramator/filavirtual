using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("tabat", Schema = "crm")]
    public class AtencionPSQL
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public Int32 Id { get; set; }

        [Column("nrotic")]
        public String NroTicket { get; set; }

        [Column("codpto", Order = 0)]
        public String PuntoId { get; set; }

        [Column("nroage", Order = 1)]
        public String AgenteId { get; set; }

        [Column("fecemi")]
        public DateTime FechaEmision { get; set; }

        [Column("fecllam")]
        public Nullable<DateTime> FechaLlamado { get; set; }

        [Column("feciat")]
        public DateTime FechaInicio { get; set; }

        [Column("fecfat")]
        public DateTime FechaFin { get; set; }

        [Column("codser")]
        public String ServicioId { get; set; }

        [Column("codest",Order=2)]
        public String EstadoId { get; set; }
    }
}