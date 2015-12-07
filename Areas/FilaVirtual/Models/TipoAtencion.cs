using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class TipoAtencion
    {
        public String Id { get; set; }

        public String Nombre { get; set; }

        public String PadreId { get; set; }

        public String PadreDescripcion { get; set; }

        public String Valor { get; set; }

        public String Estado { get; set; }
    }
}