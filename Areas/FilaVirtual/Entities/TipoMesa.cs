using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SistemaDeGestionDeFilas.Areas.Catalogo.Entities;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("tipomesa",Schema="crm")]
    public class TipoMesa
    {
        [Key]
        [ForeignKey("Mesa")]
        [Column("codmesa", Order=0)]
        public String MesaId { get; set;}

        [Key]
        [ForeignKey("Tipo")]
        [Column("codtipo", Order=1)]
        public String TipoId { get; set; }

        public virtual Agente Mesa { get; set; }

        public virtual Parametro Tipo { get; set; }
    }
}