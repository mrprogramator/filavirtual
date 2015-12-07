using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.Reporte.Models
{
    public class AusenciaOperador
    {
        public String OperadorId { get; set; }

        public String OperadorNombre { get; set; }

        public DateTime Fecha { get; set; }

        public Double Tiempo { get; set; }

        public Ausencia Ocupado { get; set; }

        public Ausencia Baño { get; set; }
        
        public Ausencia Fotocopia { get; set; }
        
        public Ausencia ConsultaSupervisor { get; set; }
        
        public Ausencia ConsultaMedica { get; set; }
        
        public Ausencia Personal { get; set; }
    }

    public class Ausencia
    {
        public String Id { get; set; }

        public String Descripcion { get; set; }

        public Double Tiempo { get; set; }

        public DateTime Fecha { get; set; }
    }
}