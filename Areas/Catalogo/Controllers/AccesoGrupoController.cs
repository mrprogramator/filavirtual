using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Controllers
{
    [RouteArea("Catalogo", AreaPrefix="catalogo")]
    public class AccesoGrupoController : Controller
    {
        //
        // GET: /AccesoGrupo/
        Data.UnitOfWork unitOfWork;
        Repositories.AccesoGrupoRepository accesoRepository;

        public AccesoGrupoController()
        {
            unitOfWork = new Data.UnitOfWork();
            accesoRepository = unitOfWork.AccesoGrupoRepository();
        }

        [HttpPost]
        public ActionResult Create(String programaId, String grupoId)
        {
            try
            {
                var entity = new Entities.AccesoGrupo();
                entity.ProgramaId = programaId;
                entity.GrupoId = grupoId;
                entity.FechaInicio = DateTime.Now;
                entity.UnidadEmpId = "ue001";
                entity.SucursalId = "suc001";

                accesoRepository.Insert(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true });
        }

        [HttpPost]
        public ActionResult Remove(String programaId, String grupoId)
        {
            try
            {
                var entity = accesoRepository.GetById(programaId, grupoId);
                accesoRepository.Delete(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true });
        }

        [HttpPost]
        public ActionResult Update(Entities.AccesoGrupo model)
        {
            try
            {
                var entity = accesoRepository.GetById(model.ProgramaId, model.GrupoId);
                entity.CheckSel = model.CheckSel;
                entity.CheckIns = model.CheckIns;
                entity.CheckMod = model.CheckMod;
                entity.CheckEli = model.CheckEli;
                entity.CheckImp = model.CheckImp;
                accesoRepository.Update(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true });
        }

        public ActionResult Detail(String programaId, String grupoId)
        {
            var model = accesoRepository.GetById(programaId, grupoId);
            return View(model);
        }

        public JsonResult GetAccessByProgram(String id)
        {
            var data = accesoRepository.AccesoGrupos().Where(a => a.ProgramaId.Equals(id)).ToArray();
            return Json(data);
        }

        public JsonResult GetAccessByGroup(String id)
        {
            var data = accesoRepository.AccesoGrupos().Where(a => a.GrupoId.Equals(id)).ToArray();
            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
