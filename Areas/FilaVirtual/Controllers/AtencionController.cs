using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SistemaDeGestionDeFilas.Helpers;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    [RouteArea("FilaVirtual", AreaPrefix="filavirtual")]
    public class AtencionController : Controller
    {
        
        //
        // GET: /Atencion/

        public Data.UnitOfWork unitOfWork;
        public Repositories.AtencionRepository atencionRepository;
        public Repositories.AudioAtencionRepository audioAtencionRepository;

        public AtencionController()
        {
            unitOfWork = new Data.UnitOfWork();
            atencionRepository = unitOfWork.AtencionRepository();
            audioAtencionRepository = unitOfWork.AudioAtencionRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("atenciones/exportar")]
        public ActionResult Export()
        {
            return View();
        }

        [HttpPost]
        [Route("atenciones/exportar/{inicio}/{fin}")]
        public JsonResult ExportarAtenciones(DateTime inicio, DateTime fin)
        {
            try
            {
                var count = new ExporterHelpers().ExportAtenttions(inicio, fin, -1);
                
                if (count == 1)
                {
                    return Json(new { result = true, value = count + " atención exportada con éxito" });
                }
                
                return Json(new { result = true, value = count + " atenciones exportadas con éxito" });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("atenciones/exportar/{inicio}/{fin}/{cantidad}")]
        public JsonResult ExportarAtencionesPorCantidad(DateTime inicio, DateTime fin, Int32 cantidad)
        {
            if (fin >= DateTime.Today.AddDays(1) || inicio >= DateTime.Today)
            {
                return Json(new { result = false, value = "No se pueden exportar las atenciones de hoy." });
            }
            try
            {
                var count = new ExporterHelpers().ExportAtenttions(inicio, fin, cantidad);

                if (count == 1)
                {
                    return Json(new { result = true, value = count + " atención exportada con éxito" });
                }

                return Json(new { result = true, value = count + " atenciones exportadas con éxito" });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpGet]
        [Route("atenciones/{id}/audio")]
        public JsonResult GetAudio(Int32 id)
        {
            var entity = audioAtencionRepository.GetById(id);
            var data = File(entity.Audio, "audio/ogg");
            var json = Json(data, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        [HttpPost]
        [Route("atenciones/{id}/audio/download")]
        public JsonResult DownloadAudio(Int32 id)
        {
            try
            {
                var entity = audioAtencionRepository.GetById(id);
                if (entity.Audio == null)
                {
                    return Json(new { result = false,
                        value = "No existe una grabación de la atención especificada" });
                }

                var audioPath = HttpContext.Server.MapPath("~/Resources/recordings/" + entity.Id.ToString() + ".ogg");
                System.IO.File.WriteAllBytes(audioPath, entity.Audio);

                return Json(new { result = true });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }

        }

        [HttpGet]
        [Route("atenciones/{id}/detail")]
        public ActionResult Detail(Int32 id)
        {
            return View();
        }

        [HttpGet]
        [Route("atenciones/{id}/transacciones")]
        public ActionResult Transacciones(Int32 id)
        {
            var entity = atencionRepository.GetById(id);
            var model = new Models.Atencion()
            {
                Id = entity.Id
            };
            return View(model);
        }

        [HttpPost]
        [Route("atenciones/create")]
        public JsonResult Create(Entities.Fila model)
        {
            var entity = atencionRepository
                .Atenciones()
                .Where(a => a.NroTicket.Equals(model.NroTicket) && a.PuntoId.Equals(model.PuntoId))
                .OrderByDescending(a => a.FechaEmision)
                .FirstOrDefault();

            if (entity == null)
            {
                return Json(new { result = false, value = "No se encontró la atención" });
            }

            try
            {
                entity.AgenteId = model.AgenteId;
                entity.ServicioId = model.ServicioId;
                entity.FechaInicio = DateTime.Now;
                entity.EstadoId = atencionRepository.ATENDIDO;
                atencionRepository.Update(entity);
                Hubs.MainHub.HubContext.Clients.All.Update();
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = entity});
        }

        [HttpPost]
        [Route("atenciones/{id}/finalizar")]
        public JsonResult Finish(Int32 id)
        {
            try
            {
                var entity = atencionRepository.GetById(id);
                entity.FechaFin = DateTime.Now;
                entity.EstadoId = atencionRepository.FINALIZADO;
                atencionRepository.Update(entity);

                var itemInLine = unitOfWork
                    .FilaRepository()
                    .Filas()
                    .Where(i => i.NroTicket.Equals(entity.NroTicket) && i.AgenteId.Equals(entity.AgenteId) && i.FechaEmision.Equals(entity.FechaEmision))
                    .FirstOrDefault();

                if (itemInLine != null)
                {
                    unitOfWork.FilaRepository().Delete(itemInLine);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true });
        }

        [HttpPost]
        [Route("mesas/{nroAgente}/atenciones/atendidas")]
        public JsonResult GetAtendidos(String nroAgente)
        {
            List<dynamic> at = new List<dynamic>();
            var data =
                atencionRepository
                .Atenciones()
                .Where(a => a.AgenteId.Equals(nroAgente) 
                    && a.EstadoId.Equals(atencionRepository.FINALIZADO) 
                    && a.FechaFin.Day.Equals(DateTime.Now.Day)
                    && a.FechaFin.Month.Equals(DateTime.Now.Month)
                    && a.FechaFin.Year.Equals(DateTime.Now.Year))
                .OrderByDescending(a => a.FechaEmision)
                .ToArray();
            foreach (var a in data)
            {
                at.Add(new { NroTicket = a.NroTicket });
            }
            return Json(at);
        }

        [HttpPost]
        [Route("atenciones/{id}/audio/send")]
        public void ReceiveRecord(Int32 id)
        {
            var data = new Byte[Request.ContentLength];
            Request.InputStream.Read(data, 0, Request.ContentLength);
            var atencionEntity = atencionRepository.GetById(id);

            if (atencionEntity == null)
            {
                return;
            }

            var entity = new Entities.AudioAtencion();
            
            entity.Audio = data;
            entity.Id = id;
            audioAtencionRepository.Insert(entity);
        }

        [HttpPost]
        [Route("atenciones/group-by/hora")]
        public JsonResult GetAtencionesGroupByHora()
        {
            var atenciones = atencionRepository
                .Atenciones()
                .Select(a => new Models.Atencion()
                {
                    FechaEmision = a.FechaEmision
                })
                .OrderBy(a => a.FechaEmision)
                .ToArray();
            
            if (atenciones.Length == 0) 
            {
                return Json(new { result = false, value = "No existen atenciones registradas" });
            }

            var data = new List<Models.Dia>();
            var initialDate = atenciones[0].FechaEmision.Date;
            while(initialDate <= atenciones.Last().FechaEmision)
            {
                var yesterday = initialDate.AddDays(-1);
                var tomorrow = initialDate.AddDays(1);
                var x = new Data.UnitOfWork()
                    .AtencionRepository()
                    .Atenciones()
                    .Where(a => a.FechaInicio < tomorrow && a.FechaInicio >= initialDate)
                    .OrderBy(a => a.FechaInicio)
                    .GroupBy(a => a.FechaEmision.Hour)
                    .Select(g => new Models.AtencionesHora() 
                    { 
                        Hora = g.Key,
                        Atenciones = g.Count() 
                    })
                    .ToArray();
                data.Add(new Models.Dia { Fecha = initialDate.Day + "/" + initialDate.Month + "/" + initialDate.Year, Lista = x });

                initialDate = tomorrow;
            }

            var avg = new List<Models.AtencionesHora>();
            for (int i = 0; i < 24; i++)
            {
                var model = new Models.AtencionesHora()
                {
                    Hora = i
                };

                var n = 0;
                foreach (var d in data)
                {
                    var z = d.Lista.Where(a => a.Hora.Equals(i)).FirstOrDefault();
                    
                    if (z == null)
                        continue;

                    model.Atenciones += z.Atenciones;
                    n++;
                }
                if (n != 0)
                    model.Atenciones /= n;
                avg.Add(model);
            }


            return Json(avg);
        }

        [HttpPost]
        [Route("atenciones/{id}")]
        public ActionResult GetAtencion(Int32 id)
        {
            var data =
                atencionRepository
                .Atenciones()
                .Select(atencion => new Models.Atencion
                {
                    Id = atencion.Id,
                    PuntoId = atencion.PuntoId,
                    NroTicket = atencion.NroTicket,
                    PuntoDescripcion = atencion.Punto.Descripcion,
                    AgenteId = atencion.AgenteId,
                    EstadoId = atencion.EstadoId,
                    LogUsr = atencion.Agente.LogUsr,
                    EstadoValor = atencion.Estado.Nombre,
                    FechaLlamado = atencion.FechaLlamado,
                    FechaEmision = atencion.FechaEmision,
                    FechaInicio = atencion.FechaInicio,
                    FechaFin = atencion.FechaFin

                })
                .FirstOrDefault(atencion => atencion.Id == id);

            return Json(data);
        }

        [HttpPost]
        [Route("atenciones")]
        public JsonResult GetAtenciones()
        {
            var data =
                atencionRepository
                .Atenciones()
                .Select(atencion => new Models.Atencion
                {
                    Id = atencion.Id,
                    PuntoId = atencion.PuntoId,
                    AgenteId = atencion.AgenteId,
                    NroTicket = atencion.NroTicket,
                    LogUsr = atencion.Agente.LogUsr,
                    PuntoDescripcion = atencion.Punto.Descripcion,
                    EstadoId = atencion.EstadoId,
                    EstadoValor = atencion.Estado.Nombre,
                    FechaEmision = atencion.FechaEmision,
                    FechaLlamado = atencion.FechaLlamado
                })
                .Where(a => a.FechaEmision.Day.Equals(DateTime.Now.Day) && 
                    a.FechaEmision.Month.Equals(DateTime.Now.Month) && 
                    a.FechaEmision.Year.Equals(DateTime.Now.Year))
                .OrderByDescending(a => a.Id)
                .ToArray();

            if (data.Length == 0)
            {
                data =
                atencionRepository
                .Atenciones()
                .Select(atencion => new Models.Atencion
                {
                    Id = atencion.Id,
                    PuntoId = atencion.PuntoId,
                    AgenteId = atencion.AgenteId,
                    PuntoDescripcion = atencion.Punto.Descripcion,
                    EstadoId = atencion.EstadoId,
                    EstadoValor = atencion.Estado.Nombre,
                    LogUsr = atencion.Agente.LogUsr,
                    NroTicket = atencion.NroTicket,
                    FechaEmision = atencion.FechaEmision,
                    FechaLlamado = atencion.FechaLlamado
                })
                .OrderByDescending(a => a.Id)
                .Take(10)
                .ToArray();
            }

            return Json(data);
        }

        [HttpPost]
        [Route("atenciones/tickets/{nroTicket}/{inicio}/{fin}")]
        public JsonResult GetAtencionesRangeByTicket(DateTime inicio, DateTime fin, String nroTicket)
        {
            try
            {
                var data = atencionRepository
                    .Atenciones()
                    .Select(atencion => new Models.Atencion
                    {
                        Id = atencion.Id,
                        PuntoId = atencion.PuntoId,
                        PuntoDescripcion = atencion.Punto.Descripcion,
                        LogUsr = atencion.Agente.LogUsr,
                        EstadoId = atencion.EstadoId,
                        AgenteId = atencion.AgenteId,
                        NroTicket = atencion.NroTicket,
                        EstadoValor = atencion.Estado.Nombre,
                        FechaLlamado = atencion.FechaLlamado,
                        FechaEmision = atencion.FechaEmision
                    })
                    .OrderByDescending(a => a.FechaEmision)
                    .Where(a => a.FechaEmision <= fin && a.FechaEmision >= inicio && a.NroTicket.Equals(nroTicket))
                    .ToArray();
                return Json(data);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("atenciones/tickets/{nroTicket}/{inicio}/{fin}/{nroAgente}")]
        public JsonResult GetAtencionesRangeByTicketByAgente(DateTime inicio, DateTime fin, String nroTicket, String nroAgente)
        {
            try
            {
                var data = atencionRepository
                    .Atenciones()
                    .Select(atencion => new Models.Atencion
                    {
                        Id = atencion.Id,
                        PuntoId = atencion.PuntoId,
                        PuntoDescripcion = atencion.Punto.Descripcion,
                        LogUsr = atencion.Agente.LogUsr,
                        EstadoId = atencion.EstadoId,
                        AgenteId = atencion.AgenteId,
                        NroTicket = atencion.NroTicket,
                        EstadoValor = atencion.Estado.Nombre,
                        FechaEmision = atencion.FechaEmision,
                        FechaLlamado = atencion.FechaLlamado
                    })
                    .OrderByDescending(a => a.FechaEmision)
                    .Where(a => a.FechaEmision <= fin && a.FechaEmision >= inicio && a.NroTicket.Equals(nroTicket) && a.AgenteId.Equals(nroAgente))
                    .ToArray();
                return Json(data);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("atenciones/tickets/{nroTicket}")]
        public JsonResult GetAtencionesByTicket(String nroTicket)
        {
            var data = atencionRepository
                        .Atenciones()
                        .Select(atencion => new Models.Atencion
                        {
                            Id = atencion.Id,
                            PuntoId = atencion.PuntoId,
                            PuntoDescripcion = atencion.Punto.Descripcion,
                            LogUsr = atencion.Agente.LogUsr,
                            EstadoId = atencion.EstadoId,
                            AgenteId = atencion.AgenteId,
                            NroTicket = atencion.NroTicket,
                            EstadoValor = atencion.Estado.Nombre,
                            FechaEmision = atencion.FechaEmision,
                            FechaLlamado = atencion.FechaLlamado
                        })
                        .Where(a => a.NroTicket.Equals(nroTicket))
                        .ToArray();

            return Json(data);
        }

        [HttpPost]
        [Route("atenciones/{inicio}/{fin}")]
        public JsonResult GetAtencionesRange(DateTime inicio, DateTime fin)
        {
            try
            {
                var data = atencionRepository
                    .Atenciones()
                    .Select(atencion => new Models.Atencion
                    {
                        Id = atencion.Id,
                        PuntoId = atencion.PuntoId,
                        PuntoDescripcion = atencion.Punto.Descripcion,
                        LogUsr = atencion.Agente.LogUsr,
                        EstadoId = atencion.EstadoId,
                        AgenteId = atencion.AgenteId,
                        NroTicket = atencion.NroTicket,
                        EstadoValor = atencion.Estado.Nombre,
                        FechaEmision = atencion.FechaEmision,
                        FechaLlamado = atencion.FechaLlamado
                    })
                    .OrderByDescending(a => a.FechaEmision)
                    .Where(a => a.FechaEmision <= fin && a.FechaEmision >= inicio)
                    .ToArray();
                    return Json(data);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("atenciones/{inicio}/{fin}/{nroAgente}")]
        public JsonResult GetAtencionesRangeByAgente(DateTime inicio, DateTime fin, String nroAgente)
        {
            try
            {
                var data = atencionRepository
                    .Atenciones()
                    .Select(atencion => new Models.Atencion
                    {
                        Id = atencion.Id,
                        PuntoId = atencion.PuntoId,
                        PuntoDescripcion = atencion.Punto.Descripcion,
                        EstadoId = atencion.EstadoId,
                        LogUsr = atencion.Agente.LogUsr,
                        AgenteId = atencion.AgenteId,
                        NroTicket = atencion.NroTicket,
                        EstadoValor = atencion.Estado.Nombre,
                        FechaEmision = atencion.FechaEmision,
                        FechaLlamado = atencion.FechaLlamado
                    })
                    .OrderByDescending(a => a.FechaEmision)
                    .Where(a => a.FechaEmision <= fin && a.FechaEmision >= inicio && a.AgenteId.Equals(nroAgente))
                    .ToArray();

                return Json(data);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("puntos/{puntoId}/atenciones/asignadas/{cantidad}")]
        public JsonResult GetAsignidados(String puntoId, Int32 cantidad = 3)
        {
            var data = atencionRepository
                .Atenciones()
                .Select(atencion => new Models.Atencion
                {
                    Id = atencion.Id,
                    PuntoId = atencion.PuntoId,
                    PuntoDescripcion = atencion.Punto.Descripcion,
                    EstadoId = atencion.EstadoId,
                    LogUsr = atencion.Agente.LogUsr,
                    AgenteId = atencion.AgenteId,
                    NroTicket = atencion.NroTicket,
                    EstadoValor = atencion.Estado.Nombre,
                    FechaEmision = atencion.FechaEmision,
                    FechaLlamado = atencion.FechaLlamado

                })
                .OrderByDescending(a => a.FechaEmision)
                .Where(a => a.PuntoId.Equals(puntoId)
                    && a.EstadoId.Equals(atencionRepository.NOATENDIDO)
                    && a.FechaEmision.Day.Equals(DateTime.Now.Day)
                    && a.FechaEmision.Month.Equals(DateTime.Now.Month)
                    && a.FechaEmision.Year.Equals(DateTime.Now.Year))
                .ToArray()
                .Take(cantidad);

            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
