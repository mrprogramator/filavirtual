using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    public class TelevisorController : Controller
    {
        //
        // GET: /FilaVirtual/Televisor/

        [HttpGet]
        [Route("puntos/{puntoId}/televisor/vertical")]
        public ActionResult Vertical()
        {
            return View();
        }

        [HttpGet]
        [Route("puntos/{puntoId}/televisor/horizontal")]
        public ActionResult Horizontal()
        {
            return View();
        }

        [HttpGet]
        [Route("puntos/{puntoId}/televisor/horizontal/prueba")]
        public ActionResult HorizontalTest()
        {
            return View();
        }

        [HttpGet]
        [Route("puntos/{puntoId}/televisor/vertical/prueba")]
        public ActionResult VerticalPrueba()
        {
            return View();
        }
    }
}
