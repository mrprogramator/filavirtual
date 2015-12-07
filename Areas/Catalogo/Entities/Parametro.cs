using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Entities
{
    [Table("tabpar", Schema = "cat")]
    public class Parametro
    {
        [Key]
        [Column("codpar")]
        public String Id { get; set; }

        [Column("nompar")]
        public String Nombre { get; set; }

        [Column("valpar")]
        public String Valor { get; set; }

        [Column("tippar")]
        public String Tipo { get; set; }

        [Column("estpar")]
        public String EstadoId { get; set; }

        [Column("grupar")]
        [ForeignKey("Grupo")]
        public String GrupoId { get; set; }

        public virtual Parametro Grupo { get; set; }
    }
}