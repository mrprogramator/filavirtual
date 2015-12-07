using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class Ticketera
    {
        public String Id { get; set; }

        public String PuntoId { get; set; }

        public String PuntoDescripcion { get; set; }

        public String Descripcion { get; set; }

        public Int32 NroTicket { get; set; }

        public Int32 NroTicketP { get; set; }

        public Int32 NroTicketR { get; set; }

        public Int32 NumeroMin { get; set; }

        public Int32 NumeroMax { get; set; }
    }

    
}