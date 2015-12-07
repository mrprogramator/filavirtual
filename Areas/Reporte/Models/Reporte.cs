using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.Reporte.Models
{
    public class Reporte
    {
        public Reporte()
        {
            Parameters = new List<Parameter>();
        }

        public String TypeName { get; set; }

        public List<Parameter> Parameters { get; set; }
    }
}