using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("ticketera", Schema = "crm")]
    public class Ticketera
    {
        [Key]
        [Column("codtic",Order=0)]
        public String Id { get; set; }

        [Key]
        [ForeignKey("Punto")]
        [Column("codpto",Order=1)]
        public String PuntoId { get; set; }

        [Column("destic")]
        public String Descripcion { get; set; }

        public virtual Punto Punto { get; set; }
    }
}