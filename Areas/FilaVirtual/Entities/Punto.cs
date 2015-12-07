using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("punto", Schema = "crm")]
    public class Punto
    {
        [Key]
        [Column("codpto")]
        public String Id { get; set; }

        [Column("despto")]
        public String Descripcion { get; set; }

        [Column("msg1")]
        public String Mensaje1 { get; set; }

        [Column("msg2")]
        public String Mensaje2 { get; set; }
        
        [Column("msg3")]
        public String Mensaje3 { get; set; }

        [Column("ordat")]
        public Nullable<Int32> OrdenAtencion { get; set; }

        [Column("atact")]
        public Nullable<Int32> AtencionActual { get; set; }
    }
}