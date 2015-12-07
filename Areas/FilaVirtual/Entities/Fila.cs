using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities
{
    [Table("fila", Schema = "crm")]
    public class Fila
    {
        [Key]
        [Column("id")]
        public Int32 Id { get; set; }

        [Column("nrotic")]
        public String NroTicket { get; set; }

        [ForeignKey("Punto")]
        [Column("codpto",Order=1)]
        public String PuntoId { get; set; }

        [Column("fecemi")]
        public DateTime FechaEmision { get; set; }

        [Column("codser")]
        public String ServicioId { get; set; }

        [Column("nroser")]
        public Int32 NroServicio { get; set; }

        [Column("nomcli")]
        public String NombreCliente { get; set; }

        [Column("nroage")]
        public String AgenteId { get; set; }

        [Column("tipat")]
        public String Preferencia { get; set; }

        public virtual Punto Punto { get; set; }
    }
}