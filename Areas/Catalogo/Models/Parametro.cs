using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Models
{
    public class Parametro
    {
        public String Id { get; set; }

        public String Nombre { get; set; }

        public String Valor { get; set; }

        public String Tipo { get; set; }

        public String EstadoId { get; set; }

        public String EstadoNombre { get; set; }

        public String GrupoId { get; set; }

        public String GrupoNombre { get; set; }
    }
}