using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Controllers
{
    [RouteArea("Catalogo", AreaPrefix="catalogo")]
    public class ProgramaController : Controller
    {
        //
        // GET: /Programa/

        

        Data.UnitOfWork unitOfWork;
        Repositories.ProgramaRepository programaRepository;

        public ProgramaController()
        {
            unitOfWork = new Data.UnitOfWork();
            programaRepository = unitOfWork.ProgramaRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Programa model)
        {
            try
            {
                var entity = new Entities.Programa()
                {
                    Id = model.Id,
                    Nombre = model.Nombre,
                    TipoId = model.TipoId,
                    PadreId = model.PadreId,
                    Orden = model.Orden,
                    EstadoId = model.EstadoId,
                    Url = model.Url,
                    CheckSel = model.CheckSel,
                    CheckIns = model.CheckIns,
                    CheckMod = model.CheckMod,
                    CheckEli = model.CheckEli,
                    CheckImp = model.CheckImp
                };
                programaRepository.Insert(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true, value = Url.Action("Index") });
        }


        [HttpGet]
        [Route("programas/{id}/edit")]
        public ActionResult Edit(String id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Models.Programa model)
        {
            try
            {
                var entity = programaRepository.GetById(model.Id);
                entity.Nombre = model.Nombre;
                entity.TipoId = model.TipoId;
                entity.PadreId = model.PadreId;
                entity.Orden = model.Orden;
                entity.EstadoId = model.EstadoId;
                entity.Url = model.Url;
                entity.CheckSel = model.CheckSel;
                entity.CheckIns = model.CheckIns;
                entity.CheckMod = model.CheckMod;
                entity.CheckEli = model.CheckEli;
                entity.CheckImp = model.CheckImp;

                programaRepository.Update(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true, value = Url.Action("Index") });
        }

        [HttpGet]
        [Route("programas/{id}/delete")]
        public ActionResult Delete(String id)
        {
            var entity = programaRepository.GetById(id);

            var model = new Models.Programa()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                TipoId = entity.TipoId,
                TipoNombre = entity.Tipo.Nombre,
                PadreId = entity.PadreId,
                Orden = entity.Orden,
                EstadoId = entity.EstadoId,
                EstadoNombre = entity.Estado.Nombre,
                Url = entity.Url,
                CheckSel = entity.CheckSel,
                CheckIns = entity.CheckIns,
                CheckMod = entity.CheckMod,
                CheckEli = entity.CheckEli,
                CheckImp = entity.CheckImp
            };

            if (!String.IsNullOrEmpty(model.PadreId))
            {
                model.PadreNombre = entity.Padre.Nombre;
            }

            return View(model);
        }

        [HttpPost]
        [Route("programas/{id}/delete")]
        public ActionResult Remove(String id)
        {
            try
            {
                var entity = programaRepository.GetById(id);

                programaRepository.Delete(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

            return Json(new { result = true, value = Url.Action("Index") });
        }

        [HttpGet]
        [Route("programas/{id}/detail")]
        public ActionResult Detail(String id)
        {
            return View();
        }

        [HttpPost]
        [Route("programas/{id}")]
        public JsonResult GetProgram(String id)
        {
            var entity = programaRepository.GetById(id);

            var model = new Models.Programa()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                TipoId = entity.TipoId,
                PadreId = entity.PadreId,
                Orden = entity.Orden,
                EstadoId = entity.EstadoId,
                Url = entity.Url,
                CheckSel = entity.CheckSel,
                CheckIns = entity.CheckIns,
                CheckMod = entity.CheckMod,
                CheckEli = entity.CheckEli,
                CheckImp = entity.CheckImp
            };

            if (!String.IsNullOrEmpty(model.PadreId))
            {
                model.PadreNombre = entity.Padre.Nombre;
            }
            if (entity.Tipo != null)
            {
                model.TipoNombre = entity.Tipo.Nombre;
            }
            if (entity.Estado != null)
            {
                model.EstadoNombre = entity.Estado.Nombre;
            }

            return Json(model);
        }

        [HttpGet]
        [Route("programas/{id}/accesos")]
        public ActionResult Access(String id)
        {
            var entity = programaRepository.GetById(id);

            var model = new Models.Programa()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                TipoId = entity.TipoId,
                PadreId = entity.PadreId,
                Orden = entity.Orden,
                EstadoId = entity.EstadoId,
                Url = entity.Url,
                CheckSel = entity.CheckSel,
                CheckIns = entity.CheckIns,
                CheckMod = entity.CheckMod,
                CheckEli = entity.CheckEli,
                CheckImp = entity.CheckImp
            };

            if (!String.IsNullOrEmpty(model.PadreId))
            {
                model.PadreNombre = entity.Padre.Nombre;
            }
            if (entity.Tipo != null)
            {
                model.TipoNombre = entity.Tipo.Nombre;
            }
            if (entity.Estado != null)
            {
                model.EstadoNombre = entity.Estado.Nombre;
            }

            return View(model);
        }

        [HttpGet]
        [Route("programas/{id}/accesos-grupos")]
        public ActionResult AccessGroup(String id)
        {
            var entity = programaRepository.GetById(id);

            var model = new Models.Programa()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                TipoId = entity.TipoId,
                PadreId = entity.PadreId,
                Orden = entity.Orden,
                EstadoId = entity.EstadoId,
                Url = entity.Url,
                CheckSel = entity.CheckSel,
                CheckIns = entity.CheckIns,
                CheckMod = entity.CheckMod,
                CheckEli = entity.CheckEli,
                CheckImp = entity.CheckImp
            };

            if (!String.IsNullOrEmpty(model.PadreId))
            {
                model.PadreNombre = entity.Padre.Nombre;
            }
            if (entity.Tipo != null)
            {
                model.TipoNombre = entity.Tipo.Nombre;
            }
            if (entity.Estado != null)
            {
                model.EstadoNombre = entity.Estado.Nombre;
            }
            return View(model);
        }

        [HttpPost]
        [Route("programas")]
        public JsonResult GetItems()
        {
            var data = programaRepository.Programas().OrderBy(p => p.Orden).ToArray();

            return Json(data);
        }

        public JsonResult GetSoluciones()
        {
            var data = programaRepository.Programas().Where(p => p.TipoId.Equals(programaRepository.CODSOL)).ToArray();
            return Json(data);
        }

        public JsonResult GetCarpetas()
        {
            var data = programaRepository.Programas().Where(p => p.TipoId.Equals(programaRepository.CODCAR)).ToArray();
            return Json(data);
        }

        public JsonResult GetProgramas()
        {
            var data = programaRepository.Programas().Where(p => p.TipoId.Equals(programaRepository.CODPRO)).ToArray();
            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
