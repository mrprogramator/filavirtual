using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.Reporte.Models
{
    public class ArbolTransaccion
    {
        public ArbolTransaccion()
        {
            Hijos = new List<ArbolTransaccion>();
        }

        public ArbolTransaccion(String id, String nombre, Int32 cantidad,List<ArbolTransaccion> hijos)
        {
            Id = id;
            Nombre = nombre;
            Hijos = hijos;
            Cantidad = cantidad;
        }

        public String Id { get; set; }

        public String Nombre { get; set; }

        public Int32 Cantidad { get; set; }

        public List<ArbolTransaccion> Hijos { get; set; }
    }
}