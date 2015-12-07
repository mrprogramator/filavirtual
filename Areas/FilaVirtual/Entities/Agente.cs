using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SistemaDeGestionDeFilas.Areas.Catalogo.Entities;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("mesa",Schema="crm")]
    public class Agente
    {
        [Key]
        [Column("nroage")]
        public String Id { get; set;}

        [ForeignKey("Punto")]
        [Column("codpto")]
        public String PuntoId { get; set; }

        [ForeignKey("Usuario")]
        [Column("logusr")]
        public String LogUsr { get; set; }

        [ForeignKey("Estado")]
        [Column("codest")]
        public String EstadoId { get; set; }

        public virtual Punto Punto { get; set; }

        public virtual Catalogo.Entities.Usuario Usuario {get; set;}

        public virtual Parametro Estado { get; set; }
    }
}