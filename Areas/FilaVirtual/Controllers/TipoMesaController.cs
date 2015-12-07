using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    [RouteArea("FilaVirtual", AreaPrefix = "filavirtual")]
    public class TipoMesaController : Controller
    {
        //
        // GET: /TipoMesa/
        Data.UnitOfWork unitOfWork;
        Repositories.TipoMesaRepository tipoMesaRepository;

        public TipoMesaController()
        {
            unitOfWork = new Data.UnitOfWork();
            tipoMesaRepository = unitOfWork.TipoMesaRepository();
        }

        [HttpPost]
        [Route("mesas/{mesaId}/tipo-atenciones/{tipoId}/create")]
        public ActionResult Create(String mesaId, String tipoId)
        {
            try
            {
                var entity = new Entities.TipoMesa();
                entity.MesaId = mesaId;
                entity.TipoId = tipoId;

                tipoMesaRepository.Insert(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true });
        }

        [HttpPost]
        [Route("mesas/{mesaId}/tipo-atenciones/{tipoId}/delete")]
        public ActionResult Remove(String mesaId, String tipoId)
        {
            try
            {
                var entity = tipoMesaRepository.GetById(mesaId, tipoId);
                tipoMesaRepository.Delete(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true });
        }

        [HttpPost]
        [Route("tipo-atenciones/{id}/mesas")]
        public JsonResult GetMesasByTipo(String id)
        {
            var data = tipoMesaRepository.TipoMesas().Where(a => a.TipoId.Equals(id)).ToArray();
            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
