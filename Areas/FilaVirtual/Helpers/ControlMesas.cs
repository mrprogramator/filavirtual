using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Helpers
{
    public class ControlMesas
    {
        static ControlMesas instance;

        private ControlMesas()
        {
            Asignados = new List<String>();
            Disponibles = new List<String>();
        }

        public static ControlMesas Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ControlMesas();
                }

                return instance;
            }
        }
        
        public List<String> Asignados { get; set; }

        public List<String> Disponibles { get; set; }
    }
}