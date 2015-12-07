using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Models
{
    public class Programa
    {
        public String Id { get; set; }

        public String Nombre { get; set; }

        public String TipoId { get; set; }

        public String TipoNombre { get; set; }

        public String PadreId { get; set; }

        public String PadreNombre { get; set; }

        public Int16 Orden { get; set; }

        public String Url { get; set; }

        public String EstadoId { get; set; }

        public String EstadoNombre { get; set; }

        public Boolean CheckSel { get; set; }

        public Boolean CheckIns { get; set; }

        public Boolean CheckMod { get; set; }

        public Boolean CheckEli { get; set; }

        public Boolean CheckImp { get; set; }
    }
}