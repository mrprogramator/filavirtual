using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Entities
{
    [Table("tabpro",Schema="cat")]
    public class Programa
    {
        [Key]
        [Column("codpro")]
        public String Id { get; set; }

        [Column("nompro")]
        public String Nombre { get; set; }

        [ForeignKey("Tipo")]
        [Column("tippro")]
        public String TipoId { get; set; }

        [ForeignKey("Padre")]
        [Column("propad")]
        public String PadreId { get; set; }

        [Column("ordpro")]
        public Int16 Orden { get; set; }

        [Column("urlpro")]
        public String Url { get; set; }
        
        [ForeignKey("Estado")]
        [Column("estpro")]
        public String EstadoId { get; set; }

        [Column("prosel")]
        public Boolean CheckSel { get; set; }

        [Column("proins")]
        public Boolean CheckIns { get; set; }

        [Column("proupd")]
        public Boolean CheckMod { get; set; }

        [Column("prodel")]
        public Boolean CheckEli { get; set; }

        [Column("propri")]
        public Boolean CheckImp { get; set; }

        public virtual Parametro Tipo { get; set; }

        public virtual Programa Padre { get; set; }

        public virtual Parametro Estado { get; set; }
    }
}