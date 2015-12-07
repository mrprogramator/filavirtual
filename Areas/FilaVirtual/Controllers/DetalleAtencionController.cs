using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    [RouteArea("FilaVirtual", AreaPrefix="filavirtual")]
    public class DetalleAtencionController : Controller
    {
        //
        // GET: /DetalleAtencion/

        public Data.UnitOfWork unitOfWork;
        public Repositories.DetalleAtencionRepository dtAtencionRepository;

        public DetalleAtencionController()
        {
            unitOfWork = new Data.UnitOfWork();
            dtAtencionRepository = unitOfWork.DetalleAtencionRepository();
        }

        [HttpGet]
        [Route("hola/{id}/mundo")]
        public JsonResult hola(String id)
        {
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("detalle-atenciones/create")]
        public JsonResult Create(Models.DetalleAtencion model)
        {
            var entity = new Entities.DetalleAtencion();
            try
            {
                entity.AtencionId = model.AtencionId;
                entity.ServicioId = model.ServicioId;
                entity.Observaciones = model.Observaciones;
                entity.Fecha = DateTime.Now;
                entity.FechaFin = DateTime.MaxValue;
                entity = dtAtencionRepository.Insert(entity);
                model.Id = entity.Id;
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = model });
        }

        [HttpPost]
        [Route("detalle-atenciones/update")]
        public JsonResult Update(Models.DetalleAtencion model)
        {
            var entity = dtAtencionRepository.GetById(model.Id);

            try
            {
                entity.Observaciones = model.Observaciones;
                entity.FechaFin = DateTime.Now;

                dtAtencionRepository.Update(entity);
            }
            catch(Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true });

        }

        public JsonResult GetTransacciones(Int32 atencionId)
        {
            var data = dtAtencionRepository
              .DetalleAtenciones().Select( dt => new Models.DetalleAtencion() { 
                  Id = dt.Id,
                  AtencionId = dt.AtencionId,
                  ServicioId = dt.ServicioId,
                  ServicioValor = dt.Servicio.Nombre,
                  Observaciones = dt.Observaciones,
                  Fecha = dt.Fecha,
                  FechaFin = dt.FechaFin
              })
              .Where(dt => dt.AtencionId.Equals(atencionId))
              .ToArray();
            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
