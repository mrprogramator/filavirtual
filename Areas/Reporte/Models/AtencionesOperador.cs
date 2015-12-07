using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.Reporte.Models
{
    public class AtencionesOperador
    {
        public String UsuarioId { get; set; }

        public String UsuarioNombre { get; set; }
        
        public String MesaId { get; set; }

        public Int32 Cantidad { get; set; }
    }
}