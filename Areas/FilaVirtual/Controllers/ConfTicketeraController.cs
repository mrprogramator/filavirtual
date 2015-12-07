using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    [RouteArea("FilaVirtual", AreaPrefix="filavirtual")]
    public class ConfTicketeraController : Controller
    {
        //
        // GET: /ConfTicketera/
        

        private Data.UnitOfWork unitOfWork;
        private Repositories.ConfTicketeraRepository conftickRepository;

        public ConfTicketeraController()
        {
            unitOfWork = new Data.UnitOfWork();
            conftickRepository = unitOfWork.ConfTicketeraRepository();
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{ticketeraId}/configs/create")]
        public ActionResult Create(String puntoId, String ticketeraId)
        {
            return View();
        }

        [HttpPost]
        [Route("ticketeras/configs/create")]
        public ActionResult Create(Models.ConfTicketera model)
        {
            try
            {
                var entity = new Entities.ConfTicketera()
                {
                    Descripcion = model.Descripcion,
                    PadreId = model.PadreId,
                    TipoAtencionId = model.TipoAtencionId,
                    PuntoId = model.PuntoId,
                    TicketeraId = model.TicketeraId,
                    TipoId = model.TipoId,
                    Imagen = model.Imagen,
                    Prefijo = model.Prefijo,
                    NroTicket = model.NroTicket
                };

                conftickRepository.Insert(entity);
                return Json(new {result = true });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{ticketeraId}/configs/{id}/edit")]
        public ActionResult Edit(Int32 id)
        {
            var entity = conftickRepository.GetById(id);
            var model = new Models.ConfTicketera()
            {
                Id = entity.Id,
                Descripcion = entity.Descripcion,
                PadreId = entity.PadreId,
                PuntoId = entity.PuntoId,
                TicketeraId = entity.TicketeraId,
                TicketeraDescripcion = entity.Ticketera.Descripcion,
                TipoId = entity.TipoId,
                TipoAtencionId = entity.TipoAtencionId,
                Imagen = entity.Imagen,
                Prefijo = entity.Prefijo,
                NroTicket = entity.NroTicket
            };

            if (entity.Padre != null)
            {
                model.PadreDescripcion = entity.Padre.Descripcion;
            }

            if (!String.IsNullOrEmpty(entity.TipoId))
            {
                model.TipoDescripcion = entity.Tipo.Valor;
            }

            if (!String.IsNullOrEmpty(entity.TipoAtencionId))
            {
                model.TipoAtencionDescripcion = entity.TipoAtencion.Valor;
            }

            return View(model);
        }

        [HttpPost]
        [Route("ticketeras/configs/edit")]
        public ActionResult Edit(Models.ConfTicketera model)
        {
            try
            {
                var entity = conftickRepository.GetById(model.Id);
                entity.Descripcion = model.Descripcion;
                entity.PadreId = model.PadreId;
                entity.TipoId = model.TipoId;
                entity.Imagen = model.Imagen;
                entity.Prefijo = model.Prefijo;
                entity.TipoAtencionId = model.TipoAtencionId;
                entity.NroTicket = model.NroTicket;
                conftickRepository.Update(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("ViewConf", "Ticketera", new { id = model.TicketeraId, puntoId = model.PuntoId }) });
        }
        
        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{ticketeraId}/configs/{id}/delete")]
        public ActionResult Delete(Int32 id)
        {
            var entity = conftickRepository.GetById(id);
            var model = new Models.ConfTicketera()
            {
                Id = entity.Id,
                Descripcion = entity.Descripcion,
                PadreId = entity.PadreId,
                PuntoId = entity.PuntoId,
                TicketeraId = entity.TicketeraId,
                TicketeraDescripcion = entity.Ticketera.Descripcion,
                TipoId = entity.TipoId,
                TipoAtencionId = entity.TipoAtencionId,
                Imagen = entity.Imagen,
                Prefijo = entity.Prefijo,
                NroTicket = entity.NroTicket
            };

            if (entity.Padre != null)
            {
                model.PadreDescripcion = entity.Padre.Descripcion;
            }

            if (!String.IsNullOrEmpty(entity.TipoId))
            {
                model.TipoDescripcion = entity.Tipo.Valor;
            }

            if (!String.IsNullOrEmpty(entity.TipoAtencionId))
            {
                model.TipoAtencionDescripcion = entity.TipoAtencion.Nombre;
            }
            
            return View(model);
        }

        [HttpPost]
        [Route("ticketeras/configs/{id}/delete")]
        public ActionResult Remove(Int32 id)
        {
            try
            {
                var entity = conftickRepository.GetById(id);
                conftickRepository.Delete(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("ViewConf", "Ticketera") });
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{ticketeraId}/configs/{id}/detail")]
        public ActionResult Detail(Int32 id)
        {
            var entity = conftickRepository.GetById(id);
            var model = new Models.ConfTicketera()
            {
                Id = entity.Id,
                Descripcion = entity.Descripcion,
                PadreId = entity.PadreId,
                PuntoId = entity.PuntoId,
                TicketeraId = entity.TicketeraId,
                TipoAtencionId = entity.TipoAtencionId,
                TicketeraDescripcion = entity.Ticketera.Descripcion,
                TipoId = entity.TipoId,
                Imagen = entity.Imagen,
                Prefijo = entity.Prefijo,
                NroTicket = entity.NroTicket
            };

            if (entity.Padre != null)
            {
                model.PadreDescripcion = entity.Padre.Descripcion;
            }

            if (!String.IsNullOrEmpty(entity.TipoId))
            {
                model.TipoDescripcion = entity.Tipo.Valor;
            }

            if (!String.IsNullOrEmpty(entity.TipoAtencionId))
            {
                model.TipoAtencionDescripcion = entity.TipoAtencion.Nombre;
            }

            return View(model);
        }

        [HttpPost]
        [Route("ticketeras/{ticketeraId}/configs/")]
        public JsonResult GetConfTicketera(String ticketeraId)
        {
            var data = conftickRepository
                .ConfTicketeras()
                .Select(config => new Models.ConfTicketera()
                {
                    Id = config.Id,
                    Descripcion = config.Descripcion,
                    PuntoId = config.PuntoId,
                    TicketeraId = config.TicketeraId,
                    TicketeraDescripcion = config.Ticketera.Descripcion,
                    PadreId = config.PadreId,
                    PadreDescripcion = config.Padre.Descripcion,
                    TipoId = config.TipoId,
                    TipoDescripcion = config.Tipo.Valor,
                    Imagen = config.Imagen,
                    Prefijo = config.Prefijo,
                    NroTicket = config.NroTicket
                })
                .Where(c => c.TicketeraId.Equals(ticketeraId))
                .OrderBy(c => c.Id)
                .ToArray();

            return Json(data);
        }

        [HttpPost]
        [Route("ticketeras/{ticketeraId}/configs/grupos")]
        public JsonResult GetGrupoConf(String ticketeraId)
        {
            var data = conftickRepository
                .ConfTicketeras()
                .Select(config => new Models.ConfTicketera()
                {
                    Id = config.Id,
                    Descripcion = config.Descripcion,
                    PuntoId = config.PuntoId,
                    TicketeraId = config.TicketeraId,
                    TicketeraDescripcion = config.Ticketera.Descripcion,
                    PadreId = config.PadreId,
                    PadreDescripcion = config.Padre.Descripcion,
                    TipoId = config.TipoId,
                    TipoDescripcion = config.Tipo.Valor,
                    Imagen = config.Imagen,
                    Prefijo = config.Prefijo,
                    NroTicket = config.NroTicket
                })
                .Where(c => c.TicketeraId.Equals(ticketeraId) && c.TipoId.Equals(conftickRepository.CODGRU))
                .OrderBy(c => c.Id)
                .ToArray();

            return Json(data);
        }

        [HttpPost]
        [Route("ticketeras/{ticketeraId}/configs/grupos/{id}/hijos")]
        public JsonResult GetChildGrupo(String ticketeraId, Nullable<Int32> id)
        {
            var data = conftickRepository
                .ConfTicketeras()
                .Where(c => c.TicketeraId.Equals(ticketeraId) && c.TipoId.Equals(conftickRepository.CODPAR) && c.PadreId == (int?)id )
                .OrderBy(c => c.Id)
                .ToArray();

            return Json(data);
        }

        [HttpPost]
        [Route("ticketeras/configs/grupos")]
        public JsonResult GetPadres()
        {
            var data = conftickRepository
                .ConfTicketeras()
                .Select(config => new Models.ConfTicketera()
                {
                    Id = config.Id,
                    Descripcion = config.Descripcion,
                    PuntoId = config.PuntoId,
                    TicketeraId = config.TicketeraId,
                    TicketeraDescripcion = config.Ticketera.Descripcion,
                    TipoId = config.TipoId,
                    TipoDescripcion = config.Tipo.Valor,
                    Imagen = config.Imagen,
                    Prefijo = config.Prefijo,
                    NroTicket = config.NroTicket
                })
                .Where(c => c.TipoId.Equals(conftickRepository.CODGRU))
                .OrderBy(c => c.Id)
                .ToArray();
            return Json(data);
        }

        [HttpPost]
        [Route("configs/{id}")]
        public JsonResult GetConfig(Int32 id)
        {
            var data = conftickRepository.GetById(id);
            return Json(data);
        }
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
