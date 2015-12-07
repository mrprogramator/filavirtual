using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("tabdtat", Schema = "crm")]
    public class DetalleAtencionPSQL
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public Int32 Id { get; set; }

        [ForeignKey("Atencion")]
        [Column("atencion_id")]
        public Int32 AtencionId { get; set; }

        [Column("codser")]
        public String ServicioId { get; set; }

        [Column("obs")]
        public String Observaciones { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("fecfin")]
        public DateTime FechaFin { get; set; }

        public virtual AtencionPSQL Atencion { get; set; }
    }
}