using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class DetalleAtencion
    {
        public Int32 Id { get; set; }

        public Int32 AtencionId { get; set; }

        public String AtencionDescripcion { get ;set; }

        public String ServicioId { get; set; }

        public String ServicioValor { get; set; }

        public String Observaciones { get; set; }

        public DateTime Fecha { get; set; }

        public DateTime FechaFin { get; set; }
    }
}