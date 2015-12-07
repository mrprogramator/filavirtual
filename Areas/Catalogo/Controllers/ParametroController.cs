using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Controllers
{
    [RouteArea("Catalogo", AreaPrefix="catalogo")]
    public class ParametroController : Controller
    {
        //
        // GET: /Parametro/

        

        Repositories.ParametroRepository parametroRepository;
        Data.UnitOfWork unitOfWork;

        public ParametroController()
        {
            unitOfWork = new Data.UnitOfWork();
            parametroRepository = unitOfWork.ParametroRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("parametros/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("parametros/{id}/add-child")]
        public ActionResult AddChild()
        {
            return View("Create");
        }

        [HttpPost]
        [Route("parametros/create")]
        public ActionResult Create(Models.Parametro model)
        {
            try
            {
                var entity = new Entities.Parametro()
                {
                    Id = model.Id,
                    Nombre = model.Nombre,
                    Valor = model.Valor,
                    EstadoId = model.EstadoId,
                    GrupoId = model.GrupoId,
                    Tipo = model.Tipo
                };

                entity = parametroRepository.Insert(entity);

                return Json(new { result = true, value = entity });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("parametros/password-ticketera")]
        public JsonResult GetPasswordTicketera()
        {
            var data = parametroRepository.GetById(parametroRepository.PWDTIC);
            return Json(data);
        }

        [HttpPost]
        [Route("parametros/tiempo-llamaut")]
        public JsonResult GetTiempoLlamadoAutomatico()
        {
            var data = parametroRepository.GetById(parametroRepository.LLAUT);
            return Json(data);
        }

        [HttpGet]
        [Route("parametros/{id}/edit")]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("parametros/edit")]
        public ActionResult Edit(Models.Parametro model)
        {
            try
            {
                var entity = parametroRepository.GetById(model.Id);
                entity.Nombre = model.Nombre;
                entity.Valor = model.Valor;
                entity.EstadoId = model.EstadoId;
                entity.GrupoId = model.GrupoId;
                parametroRepository.Update(entity);

                return Json(new { result = true });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpGet]
        [Route("parametros/{id}/detail")]
        public ActionResult Detail()
        {
            return View();
        }

        [HttpGet]
        [Route("parametros/{id}/delete")]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [Route("parametros/{id}/delete")]
        public JsonResult Delete(String id)
        {
            try
            {
                var entity = parametroRepository.GetById(id);

                entity = parametroRepository.Delete(entity);

                return Json(new { result = true, value = entity });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }
        
        [HttpPost]
        [Route("parametros")]
        public JsonResult GetParametros()
        {
            var data = parametroRepository.Parametros.ToArray();
            return Json(data);
        }

        [HttpPost]
        [Route("parametros/vigentes")]
        public JsonResult GetParametrosVigentes()
        {
            var data = parametroRepository
                .Parametros
                .Where(p => p.EstadoId.Equals(parametroRepository.VIGENTE))
                .ToArray();
            return Json(data);
        }

        [HttpPost]
        [Route("parametros/tipos")]
        public JsonResult GetTiposParametro()
        {
            var data = parametroRepository.GetByGroup(parametroRepository.CODTIP).OrderBy(p => p.Id);
            return Json(data);
        }

        [HttpPost]
        [Route("parametros/estados-agente")]
        public JsonResult GetEstadosAgente()
        {
            var data = parametroRepository
                .GetByGroup(parametroRepository.ESTAG)
                .Where(p => p.EstadoId.Equals(parametroRepository.VIGENTE))
                .OrderBy(p => p.Id);
            return Json(data);
        }


        [HttpPost]
        [Route("parametros/tipo-atenciones")]
        public JsonResult GetTiposAtenciones()
        {
            var data = parametroRepository.GetByGroup(parametroRepository.TIPAT).OrderBy(p => p.Id);
            return Json(data);
        }

        [HttpPost]
        [Route("parametros/ausencias-agente")]
        public JsonResult GetAusenciasAgente()
        {
            var data = parametroRepository
                .GetByGroup(parametroRepository.AUSAG)
                .Where(p => p.EstadoId.Equals(parametroRepository.VIGENTE))
                .OrderBy(p => p.Id);
            return Json(data);
        }

        [HttpPost]
        [Route("parametros/tipos-ticket")]
        public JsonResult GetTiposTicket()
        {
            var data = parametroRepository
                .GetByGroup(parametroRepository.TIPTIC)
                .Where(p => p.EstadoId.Equals(parametroRepository.VIGENTE))
                .OrderBy(p => p.Id);
            return Json(data);

        }

        [HttpPost]
        [Route("parametros/grupos")]
        public JsonResult GetGroups()
        {
            var data = parametroRepository.GetByGroup(parametroRepository.GRUPO).OrderBy(p => p.Id);
            return Json(data);
        }

        [HttpPost]
        [Route("parametros/{id}/hijos/todos")]
        public JsonResult GetAllChildren(String id)
        {
            var data = parametroRepository.GetByGroup(id).OrderBy(p => p.Id);
            return Json(data);
        }

        [HttpPost]
        [Route("parametros/{id}/hijos")]
        public JsonResult GetChildren(String id)
        {
            var data = parametroRepository
                .GetByGroup(id)
                .Where(p => p.EstadoId.Equals(parametroRepository.VIGENTE))
                .OrderBy(p => p.Id);
            return Json(data);
        }

        [HttpPost]
        [Route("parametros/{id}")]
        public JsonResult GetParametro(String id)
        {
            var entity = parametroRepository.GetById(id);

            var model = new Models.Parametro()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Valor = entity.Valor,
                Tipo = entity.Tipo,
                GrupoId = entity.GrupoId,
                GrupoNombre = entity.Grupo.Nombre,
                EstadoId = entity.EstadoId,
            };

            var estado = parametroRepository.GetById(model.EstadoId);

            if (estado != null)
            {
                model.EstadoNombre = estado.Nombre;
            }
            return Json(model);
        }

        public JsonResult GetTipoProgramas()
        {
            var data = parametroRepository.GetByGroup(parametroRepository.TIPPRO);
            return Json(data);
        }

        [HttpPost]
        [Route("parametros/estados")]
        public JsonResult GetEstados()
        {
            var data = parametroRepository
                .GetByGroup(parametroRepository.CODEST)
                .Where(p => p.EstadoId.Equals(parametroRepository.VIGENTE));
            return Json(data);
        }

        

        public JsonResult GetEstadosAtencion()
        {
            var data = parametroRepository.GetByGroup(parametroRepository.CODAT);
            return Json(data);
        }

        public JsonResult GetTiposAtencion()
        {
            var data = parametroRepository.GetByGroup(parametroRepository.TIPAT).OrderBy(p => p.Id);
            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
