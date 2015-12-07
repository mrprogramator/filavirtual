using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Models
{
    public class Usuario
    {
        public String Id { get; set; }

        public String Nombre { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }
    }
}