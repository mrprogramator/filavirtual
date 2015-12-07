using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Entities
{
    [Table("tabgru", Schema = "cat")]
    public class Grupo
    {
        [Key]
        [Column("codgru")]
        public String Id { get; set; }

        [Column("nomgru")]
        public String Nombre { get; set; }

        [ForeignKey("Estado")]
        [Column("estgru")]
        public String EstadoId { get; set; }

        public virtual Parametro Estado { get; set; }
    }
}