using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    [RouteArea("FilaVirtual", AreaPrefix="filavirtual")]
    public class PuntoController : Controller
    {
        //
        // GET: /Punto/

        private Data.UnitOfWork unitOfWork;
        private Repositories.PuntoRepository puntoRepository;

        public PuntoController()
        {
            unitOfWork = new Data.UnitOfWork();
            puntoRepository = unitOfWork.PuntoRepository();
        }

        public ActionResult Index()
        {
            var puntos = puntoRepository.Puntos();
            return View(puntos);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("puntos/{id}/edit")]
        public ActionResult Edit(String id)
        {
            Entities.Punto entity;
            Models.Punto model;
            try
            {
                entity = puntoRepository.GetById(id);
                model = new Models.Punto()
                {
                    Id = entity.Id,
                    Descripcion = entity.Descripcion,
                    Mensaje1 = entity.Mensaje1,
                    Mensaje2 = entity.Mensaje2,
                    Mensaje3 = entity.Mensaje3,
                    OrdenAtencion = entity.OrdenAtencion
                };
            }
            catch (Exception e)
            {
                throw e;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Models.Punto model)
        {
            try
            {
                var entity = puntoRepository.GetById(model.Id);
                entity.Descripcion = model.Descripcion;
                entity.Mensaje1 = model.Mensaje1;
                entity.Mensaje2 = model.Mensaje2;
                entity.Mensaje3 = model.Mensaje3;
                entity.OrdenAtencion = model.OrdenAtencion;
                puntoRepository.Update(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index", puntoRepository.Puntos()) });
        }

        [HttpPost]
        public ActionResult Create(Models.Punto model)
        {
            try 
            {
                var entity = new Entities.Punto()
                {
                    Id = model.Id,
                    Descripcion = model.Descripcion,
                    Mensaje1 = model.Mensaje1,
                    Mensaje2 = model.Mensaje2,
                    Mensaje3 = model.Mensaje3,
                    OrdenAtencion = model.OrdenAtencion
                };
                puntoRepository.Insert(entity);
            }
            catch(Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index",puntoRepository.Puntos()) });
        }

        [HttpGet]
        [Route("puntos/{id}/delete")]
        public ActionResult Delete(String id)
        {
            var entity = puntoRepository.GetById(id);
            var model = new Models.Punto()
            {
                Id = entity.Id,
                Descripcion = entity.Descripcion,
                Mensaje1 = entity.Mensaje1,
                Mensaje2 = entity.Mensaje2,
                Mensaje3 = entity.Mensaje3,
                OrdenAtencion = entity.OrdenAtencion
            };

            return View(model);
        }

        [HttpPost]
        [Route("puntos/{id}/fila")]
        public JsonResult GetFilaLenght(String id)
        {
            var data = unitOfWork.FilaRepository().GetFilaLenghtByPunto(id);
            return Json(data);
        }

        [HttpPost]
        [Route("puntos/{puntoId}/servicios/{servicioId}/fila")]
        public JsonResult GetFilaLenght(String puntoId, String servicioId)
        {
            var data = unitOfWork.FilaRepository().GetFilaLenghtByServicio(puntoId, servicioId);
            return Json(data);
        }

        [HttpPost]
        [Route("puntos/{id}/delete")]
        public ActionResult Remove(String id)
        {
            try
            {
                var entity = puntoRepository.GetById(id);
                puntoRepository.Delete(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index", puntoRepository.Puntos()) });
        }


        [HttpGet]
        [Route("puntos/{id}/detail")]
        public ActionResult Detail(String id)
        {
            var entity = puntoRepository.GetById(id);
            var model = new Models.Punto()
            {
                Id = entity.Id,
                Descripcion = entity.Descripcion,
                Mensaje1 = entity.Mensaje1,
                Mensaje2 = entity.Mensaje2,
                Mensaje3 = entity.Mensaje3,
                OrdenAtencion = entity.OrdenAtencion
            };
            return View(model);
        }

        [HttpPost]
        [Route("puntos")]
        public JsonResult GetPuntos()
        {
            var data = puntoRepository.Puntos();
            return Json(data);
        }

        [HttpPost]
        [Route("puntos/{id}")]
        public JsonResult GetPuntoById(String id)
        {
            var data = puntoRepository.GetById(id);
            return Json(data);
        }
        
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
