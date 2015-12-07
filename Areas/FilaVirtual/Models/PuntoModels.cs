using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class Punto
    {
        public String Id { get; set; }

        public String Descripcion { get; set; }

        public String Mensaje1 { get; set; }

        public String Mensaje2 { get; set; }

        public String Mensaje3 { get; set; }

        public Nullable<Int32> OrdenAtencion { get; set; }

    }
}