using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("audioatencion", Schema = "crm")]
    public class AudioAtencion
    {
        [Key]
        [ForeignKey("Atencion")]
        [Column("id")]
        public Int32 Id { get; set; }

        [Column("audio")]
        public Byte[] Audio { get; set; }

        public virtual Atencion Atencion { get; set; }

    }
}