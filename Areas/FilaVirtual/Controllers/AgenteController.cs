using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    [RouteArea("FilaVirtual", AreaPrefix="filavirtual")]
    public class AgenteController : Controller
    {
        //
        // GET: /Agente/

        private Data.UnitOfWork unitOfWork;
        private Repositories.AgenteRepository agenteRepository;

        public AgenteController()
        {
            unitOfWork = new Data.UnitOfWork();
            agenteRepository = unitOfWork.AgenteRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("puntos/{id}/agentes")]
        public ActionResult Index(String id)
        {
            return View((object)id);
        }

        [HttpPost]
        [Route("puntos/{id}/agentes")]
        public JsonResult AgentesByPunto(String id)
        {
            var data = agenteRepository.Agentes()
                .Where(a => a.PuntoId.Equals(id))
                .OrderBy(a => a.Id)
                .ToArray();

            return Json(data);
        }


        [HttpGet]
        [Route("mesas/monitoreo")]
        public ActionResult Monitoreo()
        {
            return View();
        }

        [HttpGet]
        [Route("puntos/{id}/mesas/monitorear-mesas")]
        public ActionResult MonitoreoMesas()
        {
            return View();
        }

        [HttpGet]
        [Route("mesas/{id}/edit")]
        public ActionResult Edit(String id)
        {
            Entities.Agente entity;
            Models.Agente model;
            try
            {
                entity = agenteRepository.GetById(id);
                
                model = new Models.Agente();
                model.Id = entity.Id;
                model.PuntoId = entity.PuntoId;
                model.PuntoDescripcion = entity.Punto.Descripcion;
                model.LogUsr = entity.LogUsr;
            }
            catch (Exception e)
            {
                throw e;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Models.Agente model)
        {
            try
            {
                var entity = agenteRepository.GetById(model.Id);
                entity.PuntoId = model.PuntoId;
                entity.LogUsr = model.LogUsr;
                agenteRepository.Update(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index") });
        }

        [HttpPost]
        public ActionResult Create(Models.Agente model)
        {
            try
            {
                var entity = new Entities.Agente();
                entity.Id = model.Id;
                entity.PuntoId = model.PuntoId;
                entity.LogUsr = model.LogUsr;
                agenteRepository.Insert(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index") });
        }

        [HttpGet]
        [Route("mesas/{id}/delete")]
        public ActionResult Delete(String id)
        {
            var entity = agenteRepository.GetById(id);
            
            var model = new Models.Agente();
            model.Id = entity.Id;
            model.PuntoId = entity.PuntoId;
            model.PuntoDescripcion = entity.Punto.Descripcion;
            model.LogUsr = entity.LogUsr;

            return View(model);
        }
        [HttpPost]
        [Route("mesas/{id}/ticket")]
        public JsonResult GetCurrentTicket(String id)
        {
            var item = unitOfWork.FilaRepository().GetCurrentTicket(id);

            return Json(item);
        }

        [HttpPost]
        [Route("mesas/{id}/delete")]
        public ActionResult Remove(String id)
        {
            try
            {
                var entity = agenteRepository.GetById(id);
                agenteRepository.Delete(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index") });
        }


        [HttpGet]
        [Route("mesas/{id}/detail")]
        public ActionResult Detail(String id)
        {
            var entity = agenteRepository.GetById(id);
            
            var model = new Models.Agente();
            model.Id = entity.Id;
            model.PuntoId = entity.PuntoId;
            model.PuntoDescripcion = entity.Punto.Descripcion;
            model.LogUsr = entity.LogUsr;
            
            return View(model);
        }

        [HttpGet]
        [Route("mesas/{id}/tipo-atenciones")]
        public ActionResult Tipos(String id)
        {
            return View();
        }

        [HttpPost]
        [Route("mesas/{id}/tipo-atenciones")]
        public JsonResult GetTiposByMesa(String id)
        {
            var data = unitOfWork.TipoMesaRepository().TipoMesas().Where(a => a.MesaId.Equals(id)).ToArray();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetAgentes()
        {
            var data = agenteRepository.Agentes().ToArray();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetAgenteByLogUsr(String log)
        {
            var data = agenteRepository.Agentes().Where(a => a.LogUsr.Equals(log)).FirstOrDefault();
            return Json(data);
        }

        [HttpPost]
        [Route("mesas/{id}")]
        public JsonResult GetAgente(String id)
        {
            var data = agenteRepository.GetById(id);
            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
