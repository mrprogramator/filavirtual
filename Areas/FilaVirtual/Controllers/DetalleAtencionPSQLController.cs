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
    public class DetalleAtencionPSQLController : Controller
    {
        //
        // GET: /DetalleAtencion/

        public Data.UnitOfWorkPSQL unitOfWork;
        public Repositories.DetalleAtencionRepositoryPSQL dtAtencionRepository;

        public DetalleAtencionPSQLController()
        {
            unitOfWork = new Data.UnitOfWorkPSQL();
            dtAtencionRepository = unitOfWork.DetalleAtencionRepository();
        }

        [HttpGet]
        [Route("hola/{id}/mundo")]
        public JsonResult hola(String id)
        {
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransacciones(Int32 atencionId)
        {
            var data = dtAtencionRepository
              .DetalleAtenciones().Select( dt => new Models.DetalleAtencion() { 
                  Id = dt.Id,
                  AtencionId = dt.AtencionId,
                  ServicioId = dt.ServicioId,
                  Observaciones = dt.Observaciones,
                  Fecha = dt.Fecha,
                  FechaFin = dt.FechaFin
              })
              .Where(dt => dt.AtencionId.Equals(atencionId))
              .ToArray();

            foreach(var detalle in data)
            {
                var servicio = new Areas
                    .Catalogo
                    .Data
                    .UnitOfWork()
                    .ParametroRepository()
                    .GetById(detalle.ServicioId);

                if (servicio != null)
                {
                    detalle.ServicioValor = servicio.Nombre;
                }
            }

            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
