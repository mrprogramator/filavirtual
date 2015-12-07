using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Models
{
    public class Grupo
    {
        public String Id { get; set; }

        public String Nombre { get; set; }

        public String EstadoId { get; set; }

        public String EstadoNombre { get; set; }
    }
}