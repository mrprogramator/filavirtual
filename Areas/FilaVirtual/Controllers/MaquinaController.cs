using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    public class MaquinaController : Controller
    {
        //
        // GET: /Ticketera/

        Data.UnitOfWork unitOfWork;
        Repositories.TicketeraRepository ticketeraRepository;
        
        public MaquinaController()
        {
            unitOfWork = new Data.UnitOfWork();
            ticketeraRepository = unitOfWork.TicketeraRepository();
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{ticketeraId}")]
        public ActionResult Maquina(String ticketeraId, String puntoId) 
        {
            return View();
        }
    }
}
