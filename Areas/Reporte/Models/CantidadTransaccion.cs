using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaDeGestionDeFilas.Areas.Catalogo.Entities;

namespace SistemaDeGestionDeFilas.Areas.Reporte.Models
{
    public class CantidadTransaccion
    {
        public CantidadTransaccion()
        {
            Hola = new List<Prueba>();
            for (var i = 0; i < 5; i++)
            {
                Hola.Add(new Prueba(i.ToString()));
            }
        }

        public String NombreTransaccion { get; set; }
        public Parametro Parametro { get; set; }
        public List<Prueba> Hola { get; set; }
        public Int32 Cantidad { get; set; }

        public Prueba GetItemValue(List<Prueba> lista, int index)
        {
            return lista[index];
        }
    }

    public class Prueba
    {
        public Prueba(String nombre)
        {
            Nombre = Nombre;
        }

        public String Nombre { get; set; }
    }
}