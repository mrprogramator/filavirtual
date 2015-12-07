using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class EstadoAgente
    {
        public String Id { get; set;}

        public String AgenteId { get; set; }

        public String EstadoId { get; set; }

        public String EstadoValor { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public String MotivoId { get; set; }

        public String MotivoDescripcion { get; set; }
    }
}