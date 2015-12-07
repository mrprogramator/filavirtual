using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("atencion", Schema = "crm")]
    public class Atencion
    {
        [Key]
        [Column("id")]
        public Int32 Id { get; set; }

        [Column("nrotic")]
        public String NroTicket { get; set; }

        [ForeignKey("Punto")]
        [Column("codpto", Order = 0)]
        public String PuntoId { get; set; }

        [ForeignKey("Agente")]
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

        [ForeignKey("Estado")]
        [Column("codest",Order=2)]
        public String EstadoId { get; set; }

        [Column("urlrec")]
        public String UrlRec { get; set; }

        public virtual Agente Agente { get; set; }

        public virtual Punto Punto { get; set; }

        public virtual SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro Estado { get; set; }
    }
}