using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.Reporte.Repositories
{
    public class AtencionesHora
    {
        FilaVirtual.Data.UnitOfWork unitOfWork;
        FilaVirtual.Repositories.AtencionRepository atencionRepository;

        public IEnumerable<FilaVirtual.Entities.Atencion> CalcularAtencionesPorHora()
        {
            unitOfWork = new FilaVirtual.Data.UnitOfWork();
            atencionRepository = unitOfWork.AtencionRepository();

            return null;
            /*
             select cat.tabusu.codusu as USUARIO, crm.mesa.nroage as MESA, count(*) as TOTAL
from cat.tabusu, crm.mesa
join crm.atencion on crm.mesa.nroage = crm.atencion.nroage
where cat.tabusu.codusu = crm.mesa.logusr 
group by cat.tabusu.codusu, crm.mesa.nroage
             */

        }
    }
}