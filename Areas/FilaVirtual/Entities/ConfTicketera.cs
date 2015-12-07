using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("confticketera", Schema = "crm")]
    public class ConfTicketera
    {
        [Key]
        [Column("id")]
        public Int32 Id { get; set; }

        [Column("codtic", Order=0)]
        [ForeignKey("Ticketera")]
        public String TicketeraId { get; set; }

        [Column("codpto", Order=1)]
        [ForeignKey("Ticketera")]
        public String PuntoId { get; set; }

        [Column("despar")]
        public String Descripcion { get; set; }

        [Column("img")]
        public String Imagen { get; set; }

        [Column("prefijo")]
        public String Prefijo { get; set; }

        [Column("nrotic")]
        public Int32 NroTicket { get; set; }

        [ForeignKey("Tipo")]
        [Column("tippar")]
        public String TipoId { get; set; }

        [ForeignKey("TipoAtencion")]
        [Column("tipat")]
        public String TipoAtencionId { get; set; }

        [ForeignKey("Padre")]
        [Column("parpad")]
        public Nullable<Int32> PadreId { get; set; }

        public virtual Ticketera Ticketera { get; set; }

        public virtual SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro Tipo { get; set; }

        public virtual ConfTicketera Padre { get; set; }

        public virtual SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro TipoAtencion { get; set; }
    }
}