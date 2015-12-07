using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("dtatencion", Schema = "crm")]
    public class DetalleAtencion
    {
        [Key]
        [Column("id")]
        public Int32 Id { get; set; }

        [ForeignKey("Atencion")]
        [Column("atencion_id")]
        public Int32 AtencionId { get; set; }

        [ForeignKey("Servicio")]
        [Column("codser")]
        public String ServicioId { get; set; }

        [Column("obs")]
        public String Observaciones { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("fecfin")]
        public DateTime FechaFin { get; set; }

        public virtual Atencion Atencion { get; set; }

        public virtual SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro Servicio { get; set; }
    }
}