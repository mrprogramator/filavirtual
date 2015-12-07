using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    [RouteArea("FilaVirtual", AreaPrefix="filavirtual")]
    public class TicketeraController : Controller
    {
        //
        // GET: /Ticketera/

        private Data.UnitOfWork unitOfWork;
        private Repositories.TicketeraRepository ticketeraRepository;
        
        public TicketeraController()
        {
            unitOfWork = new Data.UnitOfWork();
            ticketeraRepository = unitOfWork.TicketeraRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("puntos/{id}/ticketeras")]
        public ActionResult Index(String id)
        {
            return View((object)id);
        }

        [HttpPost]
        [Route("puntos/{id}/ticketeras")]
        public JsonResult TicketerasByPunto(String id)
        {
            var data = ticketeraRepository
                .Ticketeras()
                .Where(t => t.PuntoId.Equals(id))
                .ToArray();

            return Json(data);
        }

        [HttpGet]
        [Route("puntos/ticketeras/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{id}/edit")]
        public ActionResult Edit(String id, String puntoId)
        {
            Entities.Ticketera entity;
            Models.Ticketera model;
            try
            {
                entity = ticketeraRepository.GetById(id, puntoId);
                model = new Models.Ticketera()
                {
                    Id = entity.Id,
                    PuntoId = entity.PuntoId,
                    Descripcion = entity.Descripcion,
                    PuntoDescripcion = entity.Punto.Descripcion,
                };
            }
            catch (Exception e)
            {
                throw e;
            }
            return View(model);
        }

        [HttpPost]
        [Route("ticketeras/edit")]
        public ActionResult Edit(Models.Ticketera model)
        {
            try
            {
                var entity = ticketeraRepository.GetById(model.Id, model.PuntoId);
                entity.PuntoId = model.PuntoId;
                entity.Descripcion = model.Descripcion;
                ticketeraRepository.Update(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index", ticketeraRepository.Ticketeras()) });
        }

        [HttpPost]
        [Route("ticketeras/create")]
        public ActionResult Create(Models.Ticketera model)
        {
            try 
            {
                var entity = Models.EntityModelConverter.LoadEntity(model);
                entity = ticketeraRepository.Insert(entity);
            }
            catch(Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index") });
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{id}/delete")]
        public ActionResult Delete(String id, String puntoId)
        {
            var entity = ticketeraRepository.GetById(id, puntoId);
            var model = new Models.Ticketera()
            {
                Id = entity.Id,
                PuntoId = entity.PuntoId,
                Descripcion = entity.Descripcion,
                PuntoDescripcion = entity.Punto.Descripcion,
            };
            return View(model);
        }

        [HttpPost]
        [Route("puntos/{puntoId}/ticketeras/{id}/delete")]
        public ActionResult Remove(String puntoId, String id)
        {
            try
            {
                var entity = ticketeraRepository.GetById(id, puntoId);
                ticketeraRepository.Delete(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index") });
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{id}/detail")]
        public ActionResult Detail(String puntoId, String id)
        {
            var entity = ticketeraRepository.GetById(id, puntoId);
            var model = new Models.Ticketera()
            {
                Id = entity.Id,
                PuntoId = entity.PuntoId,
                Descripcion = entity.Descripcion,
                PuntoDescripcion = entity.Punto.Descripcion,
            };
            return View(model);
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{id}/preview")]
        public ActionResult Preview(String puntoId, String id)
        {
            var entity = ticketeraRepository.GetById(id, puntoId);

            var model = new Models.Ticketera()
            {
                Id = entity.Id,
                Descripcion = entity.Descripcion,
                PuntoId = entity.PuntoId,
                PuntoDescripcion = entity.Punto.Descripcion,

            };

            return View(model);
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{ticketeraId}")]
        public ActionResult Maquina(String ticketeraId, String puntoId) 
        {
            return View();
        }

        [HttpGet]
        [Route("maquina2/{puntoId}/{ticketeraId}")]
        public ActionResult Maquina2(String ticketeraId, String puntoId)
        {
            var entity = ticketeraRepository.GetById(ticketeraId, puntoId);
            var model = Models.EntityModelConverter.LoadModel(entity);
            return View(model);
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{id}/configs")]
        public ActionResult ViewConf()
        {
            return View();
        }

        [Route("ticketeras")]
        public JsonResult GetTicketeras()
        {
            var data = ticketeraRepository.Ticketeras();
            return Json(data);
        }

        [HttpPost]
        [Route("puntos/{puntoId}/ticketeras/{id}")]
        public JsonResult Ticketera(String puntoId, String id)
        {
            var data = ticketeraRepository.GetById(id, puntoId);
            
            return Json(data);
        }

        public JsonResult GetTicketera(String tickId)
        {
            var tickconfs = ticketeraRepository
                .Ticketeras()
                .Where(c => c.Id.Equals(tickId))
                .ToArray();

            return Json(tickconfs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("puntos/{puntoId}/ticketeras/{ticketeraId}/ticket/{id}")]
        public ActionResult Ticket(Object puntoId, Object ticketeraId, Int32 id)
        {
            var ticketeraEntity = ticketeraRepository.GetById(ticketeraId, puntoId);

            var confTicketeraEntity = unitOfWork.ConfTicketeraRepository().GetById(id);

            var lastAttentionByService = unitOfWork
                .AtencionRepository()
                .Atenciones()
                .Where(a => a.NroTicket.Contains(confTicketeraEntity.Prefijo))
                .OrderByDescending(a => a.FechaEmision)
                .FirstOrDefault();

            var lastAttention = unitOfWork
                .AtencionRepository()
                .Atenciones()
                .OrderByDescending(a => a.FechaEmision)
                .FirstOrDefault();

            var firstInLine = unitOfWork
                .FilaRepository()
                .Filas()
                .Where(a => a.NroTicket.Contains(confTicketeraEntity.Prefijo))
                .OrderByDescending(f => f.FechaEmision)
                .FirstOrDefault();
            
            if ( (lastAttentionByService == null || lastAttentionByService.FechaEmision.Date < DateTime.Today) && (firstInLine == null || firstInLine.FechaEmision.Date < DateTime.Today))
            {
                confTicketeraEntity.NroTicket = 1;
            }
            else
            {
                confTicketeraEntity.NroTicket++;
            }
            unitOfWork.ConfTicketeraRepository().Update(confTicketeraEntity);

            var model = new Models.Ticket()
            {
                Servicio = confTicketeraEntity.Padre.Descripcion,
                Lugar = ticketeraEntity.Punto.Descripcion,
                Turno = confTicketeraEntity.Prefijo + confTicketeraEntity.NroTicket.ToString().PadLeft(3, '0'),
                Mensaje = ticketeraEntity.Punto.Mensaje3
            };

            var filaItem = new Entities.Fila() {
                NroTicket = confTicketeraEntity.Prefijo + confTicketeraEntity.NroTicket.ToString().PadLeft(3, '0'),
                PuntoId = ticketeraEntity.PuntoId,
                ServicioId = confTicketeraEntity.Padre.TipoAtencionId,
                FechaEmision = DateTime.Now,
                Preferencia = confTicketeraEntity.TipoAtencionId
            };

            unitOfWork.FilaRepository().Insert(filaItem);

            Hubs.MainHub.HubContext.Clients.All.Update();
            
            return View(model);
        }

        //CAMBIAR EL TIPO DE ATENCION NO ES PADREID
        [HttpPost]
        [Route("puntos/{puntoId}/ticket/{nroTicket}/derivar/servicio/{servicioId}")]
        public JsonResult DerivarTicket(Object puntoId, String nroTicket, String servicioId)
        {
            var puntoEntity = unitOfWork.PuntoRepository().GetById(puntoId);

            var confTicketeraEntity = unitOfWork
                .ConfTicketeraRepository()
                .ConfTicketeras()
                .Where(c => c.Padre.TipoAtencionId.Equals(servicioId) && c.TipoAtencionId.Equals(ticketeraRepository.PREFERENCIAL))
                .FirstOrDefault();

            if (confTicketeraEntity == null)
            {
                confTicketeraEntity = unitOfWork
                .ConfTicketeraRepository()
                .ConfTicketeras()
                .Where(c => c.Padre.TipoAtencionId.Equals(servicioId))
                .FirstOrDefault();
            }
                    
            var model = new Models.Ticket()
            {
                Servicio = confTicketeraEntity.Padre.Descripcion,
                Lugar = puntoEntity.Descripcion,
                Turno = nroTicket,
                Mensaje = puntoEntity.Mensaje3
            };

            var filaItem = new Entities.Fila()
            {
                NroTicket = nroTicket,
                PuntoId = puntoEntity.Id,
                ServicioId = confTicketeraEntity.Padre.TipoAtencionId,
                FechaEmision = DateTime.Now,
                Preferencia = confTicketeraEntity.TipoAtencionId
            };

            unitOfWork.FilaRepository().Insert(filaItem);

            Hubs.MainHub.HubContext.Clients.All.Update();

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
