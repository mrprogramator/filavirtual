using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Controllers
{
    [RouteArea("Catalogo", AreaPrefix="catalogo")]
    public class UsuarioGrupoController : Controller
    {
        //
        // GET: /UsuarioGrupo/
        Data.UnitOfWork unitOfWork;
        Repositories.UsuarioGrupoRepository usuarioGrupoRepository;

        public UsuarioGrupoController()
        {
            unitOfWork = new Data.UnitOfWork();
            usuarioGrupoRepository = unitOfWork.UsuarioGrupoRepository();
        }

        [HttpPost]
        public ActionResult Create(String usuarioId, String grupoId)
        {
            try
            {
                var entity = new Entities.UsuarioGrupo();
                entity.UsuarioId = usuarioId;
                entity.GrupoId = grupoId;
                entity.FechaInicio = DateTime.Now;
                entity.FechaFin = DateTime.MaxValue;
                usuarioGrupoRepository.Insert(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true });
        }

        [HttpPost]
        public ActionResult Remove(String usuarioId, String grupoId)
        {
            try
            {
                var entity = usuarioGrupoRepository.GetById(usuarioId, grupoId);
                usuarioGrupoRepository.Delete(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true });
        }

        public JsonResult GetGroupsByUser(String id)
        {
            var data = usuarioGrupoRepository.UsuarioGrupos().Where(a => a.UsuarioId.Equals(id)).ToArray();
            return Json(data);
        }

        public JsonResult GetUsersByGroup(String id)
        {
            var data = usuarioGrupoRepository.UsuarioGrupos().Where(a => a.GrupoId.Equals(id)).ToArray();
            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
