using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class Agente
    {
        public String Id { get; set; }

        public String PuntoId { get; set; }

        public String PuntoDescripcion { get; set; }

        public String LogUsr { get; set; }

        public String EstadoId { get; set; }

        public String EstadoValor { get; set; }
    }
}