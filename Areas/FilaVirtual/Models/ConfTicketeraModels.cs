using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class ConfTicketera
    {
        public Int32 Id { get; set; }

        public String PuntoId { get; set; }

        public String TicketeraId { get; set; }

        public String TicketeraDescripcion { get; set; }

        public String Descripcion { get; set; }

        public String Imagen { get; set; }

        public String Prefijo { get; set; }

        public Int32 NroTicket { get; set; }

        public Nullable<Int32> PadreId { get; set; }

        public String PadreDescripcion { get; set; }

        public String TipoId { get; set; }

        public String TipoDescripcion { get; set; }

        public String TipoAtencionId { get; set; }

        public String TipoAtencionDescripcion { get; set; }
    }
}