using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class AtencionesHora
    {
        public Int32 Hora { get; set; }

        public Int32 Atenciones { get; set; }
    }

    public class Dia
    {
        public String Fecha { get; set; }

        public IEnumerable<AtencionesHora> Lista { get; set; }
    }
}