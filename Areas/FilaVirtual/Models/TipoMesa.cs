using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class TipoMesa
    {
        public String MesaId { get; set;}

        public String MesaDescripcion { get; set; }

        public String TipoId { get; set; }

        public String TipoDescripcion { get; set; }
    }
}