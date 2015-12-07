using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class Ticket
    {
        public String Logo 
        {
            get 
            {
                return "/Resources/img/Cotas-Ltda-.jpg";
            }
        }

        public String DatosEmpresa 
        {
            get
            {
                return "Cooperativa de Telecomunicaciones Santa Cruz\n"
                    + "\nTeléfonos: (591) 336000, Fax: (591) 3361636\n"
                    + "\nWeb: http://www.cotas.com";
            }
        }

        public String Servicio { get; set; }

        public String Turno { get; set; }

        public DateTime FechaHora 
        {
            get 
            {
                return DateTime.Now;
            }
        }

        public String Lugar { get; set; }

        public String Mensaje { get; set; }
    }
}