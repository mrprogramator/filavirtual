using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class Atencion
    {
        public Int32 Id { get; set; }

        public String NroTicket { get; set; }

        public String PuntoId { get; set; }

        public String PuntoDescripcion { get; set; }

        public String AgenteId { get; set; }

        public String LogUsr { get; set; }

        public Nullable<DateTime> FechaLlamado { get; set; }
        
        public DateTime FechaEmision { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public String ServicioId { get; set; }

        public Int32 NroServicio { get; set; }

        public String EstadoId { get; set; }

        public String EstadoValor { get; set; }

        public String UrlRec { get; set; }
    }
}