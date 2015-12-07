using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    [RouteArea("FilaVirtual", AreaPrefix="filavirtual")]
    public class AtencionPSQLController : Controller
    {
        
        //
        // GET: /Atencion/

        Data.UnitOfWorkPSQL unitOfWork;
        Repositories.AtencionRepositoryPSQL atencionRepository;
        Repositories.AudioAtencionRepositoryPSQL audioAtencionRepository;

        public AtencionPSQLController()
        {
            unitOfWork = new Data.UnitOfWorkPSQL();
            atencionRepository = unitOfWork.AtencionRepository();
            audioAtencionRepository = unitOfWork.AudioAtencionRepository();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("atenciones-pasadas/{id}/audio")]
        public JsonResult GetAudio(Int32 id)
        {
            var entity = audioAtencionRepository.GetById(id);
            var data = File(entity.Audio, "audio/ogg");
            var json = Json(data, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        [HttpPost]
        [Route("atenciones-pasadas/{id}/audio/download")]
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
        [Route("atenciones-pasadas/{id}/detail")]
        public ActionResult Detail(Int32 id)
        {
            return View();
        }

        [HttpGet]
        [Route("atenciones-pasadas/{id}/transacciones")]
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
        [Route("atenciones-pasadas/create")]
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
        [Route("atenciones-pasadas/{id}")]
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
                    AgenteId = atencion.AgenteId,
                    EstadoId = atencion.EstadoId,
                    LogUsr = atencion.AgenteId,
                    FechaLlamado = atencion.FechaLlamado,
                    FechaEmision = atencion.FechaEmision,
                    FechaInicio = atencion.FechaInicio,
                    FechaFin = atencion.FechaFin

                })
                .FirstOrDefault(atencion => atencion.Id == id);
            
            var mesa = new Data.UnitOfWork().AgenteRepository().GetById(data.AgenteId);
            data.LogUsr = mesa.LogUsr;

            var estado = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(data.EstadoId);
            data.EstadoValor = estado.Nombre;

            var punto = new Data.UnitOfWork().PuntoRepository().GetById(data.PuntoId);
            data.PuntoDescripcion = punto.Descripcion;


            return Json(data);
        }

        [HttpPost]
        [Route("atenciones-pasadas")]
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
                    EstadoId = atencion.EstadoId,
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
                    AgenteId = atencion.AgenteId,
                    PuntoId = atencion.PuntoId,
                    EstadoId = atencion.EstadoId,
                    NroTicket = atencion.NroTicket,
                    FechaEmision = atencion.FechaEmision,
                    FechaLlamado = atencion.FechaLlamado
                })
                .OrderByDescending(a => a.Id)
                .Take(10)
                .ToArray();
            }

            foreach(var atencion in data)
            {
                var mesa = new Data.UnitOfWork().AgenteRepository().GetById(atencion.AgenteId);
                atencion.LogUsr = mesa.LogUsr;

                var estado = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(atencion.EstadoId);
                atencion.EstadoValor = estado.Nombre;

                var punto = new Data.UnitOfWork().PuntoRepository().GetById(atencion.PuntoId);
                atencion.PuntoDescripcion = punto.Descripcion;
            }

            return Json(data);
        }

        [HttpPost]
        [Route("atenciones-pasadas/tickets/{nroTicket}/{inicio}/{fin}")]
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
                        EstadoId = atencion.EstadoId,
                        AgenteId = atencion.AgenteId,
                        NroTicket = atencion.NroTicket,
                        FechaLlamado = atencion.FechaLlamado,
                        FechaEmision = atencion.FechaEmision
                    })
                    .OrderByDescending(a => a.FechaEmision)
                    .Where(a => a.FechaEmision <= fin && a.FechaEmision >= inicio && a.NroTicket.Equals(nroTicket))
                    .ToArray();
                
                foreach (var atencion in data)
                {
                    var mesa = new Data.UnitOfWork().AgenteRepository().GetById(atencion.AgenteId);
                    atencion.LogUsr = mesa.LogUsr;

                    var estado = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(atencion.EstadoId);
                    atencion.EstadoValor = estado.Nombre;

                    var punto = new Data.UnitOfWork().PuntoRepository().GetById(atencion.PuntoId);
                    atencion.PuntoDescripcion = punto.Descripcion;
                }
                return Json(data);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("atenciones-pasadas/tickets/{nroTicket}/{inicio}/{fin}/{nroAgente}")]
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
                        EstadoId = atencion.EstadoId,
                        AgenteId = atencion.AgenteId,
                        NroTicket = atencion.NroTicket,
                        FechaEmision = atencion.FechaEmision,
                        FechaLlamado = atencion.FechaLlamado
                    })
                    .OrderByDescending(a => a.FechaEmision)
                    .Where(a => a.FechaEmision <= fin && a.FechaEmision >= inicio && a.NroTicket.Equals(nroTicket) && a.AgenteId.Equals(nroAgente))
                    .ToArray();

                foreach (var atencion in data)
                {
                    var mesa = new Data.UnitOfWork().AgenteRepository().GetById(atencion.AgenteId);
                    atencion.LogUsr = mesa.LogUsr;

                    var estado = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(atencion.EstadoId);
                    atencion.EstadoValor = estado.Nombre;

                    var punto = new Data.UnitOfWork().PuntoRepository().GetById(atencion.PuntoId);
                    atencion.PuntoDescripcion = punto.Descripcion;
                }

                return Json(data);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("atenciones-pasadas/tickets/{nroTicket}")]
        public JsonResult GetAtencionesByTicket(String nroTicket)
        {
            var data = atencionRepository
                    .Atenciones()
                    .Select(atencion => new Models.Atencion
                    {
                        Id = atencion.Id,
                        PuntoId = atencion.PuntoId,
                        PuntoDescripcion = atencion.PuntoId,
                        LogUsr = atencion.AgenteId,
                        EstadoId = atencion.EstadoId,
                        AgenteId = atencion.AgenteId,
                        NroTicket = atencion.NroTicket,
                        EstadoValor = atencion.EstadoId,
                        FechaEmision = atencion.FechaEmision,
                        FechaLlamado = atencion.FechaLlamado
                    })
                    .Where(a => a.NroTicket.Equals(nroTicket))
                    .ToArray();
           
            foreach (var atencion in data)
            {
                var mesa = new Data.UnitOfWork().AgenteRepository().GetById(atencion.AgenteId);
                atencion.LogUsr = mesa.LogUsr;

                var estado = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(atencion.EstadoId);
                atencion.EstadoValor = estado.Nombre;

                var punto = new Data.UnitOfWork().PuntoRepository().GetById(atencion.PuntoId);
                atencion.PuntoDescripcion = punto.Descripcion;
            }

            return Json(data);
        }

        [HttpPost]
        [Route("atenciones-pasadas/{inicio}/{fin}")]
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
                        EstadoId = atencion.EstadoId,
                        AgenteId = atencion.AgenteId,
                        NroTicket = atencion.NroTicket,
                        FechaEmision = atencion.FechaEmision,
                        FechaLlamado = atencion.FechaLlamado
                    })
                    .OrderByDescending(a => a.FechaEmision)
                    .Where(a => a.FechaEmision <= fin && a.FechaEmision >= inicio)
                    .ToArray();

                    foreach (var atencion in data)
                    {
                        var mesa = new Data.UnitOfWork().AgenteRepository().GetById(atencion.AgenteId);
                        atencion.LogUsr = mesa.LogUsr;

                        var estado = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(atencion.EstadoId);
                        atencion.EstadoValor = estado.Nombre;

                        var punto = new Data.UnitOfWork().PuntoRepository().GetById(atencion.PuntoId);
                        atencion.PuntoDescripcion = punto.Descripcion;
                    }
                    return Json(data);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("atenciones-pasadas/{inicio}/{fin}/{nroAgente}")]
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
                        EstadoId = atencion.EstadoId,
                        AgenteId = atencion.AgenteId,
                        NroTicket = atencion.NroTicket,
                        FechaEmision = atencion.FechaEmision,
                        FechaLlamado = atencion.FechaLlamado
                    })
                    .OrderByDescending(a => a.FechaEmision)
                    .Where(a => a.FechaEmision <= fin && a.FechaEmision >= inicio && a.AgenteId.Equals(nroAgente))
                    .ToArray();
                
                foreach (var atencion in data)
                {
                    var mesa = new Data.UnitOfWork().AgenteRepository().GetById(atencion.AgenteId);
                    atencion.LogUsr = mesa.LogUsr;

                    var estado = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(atencion.EstadoId);
                    atencion.EstadoValor = estado.Nombre;

                    var punto = new Data.UnitOfWork().PuntoRepository().GetById(atencion.PuntoId);
                    atencion.PuntoDescripcion = punto.Descripcion;
                }
                return Json(data);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("puntos/{puntoId}/atenciones-pasadas/asignadas/{cantidad}")]
        public JsonResult GetAsignidados(String puntoId, Int32 cantidad = 3)
        {
            var data = atencionRepository
                .Atenciones()
                .Select(atencion => new Models.Atencion
                {
                    Id = atencion.Id,
                    PuntoId = atencion.PuntoId,
                    EstadoId = atencion.EstadoId,
                    AgenteId = atencion.AgenteId,
                    NroTicket = atencion.NroTicket,
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
            
            foreach (var atencion in data)
            {
                var mesa = new Data.UnitOfWork().AgenteRepository().GetById(atencion.AgenteId);
                atencion.LogUsr = mesa.LogUsr;

                var estado = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(atencion.EstadoId);
                atencion.EstadoValor = estado.Nombre;

                var punto = new Data.UnitOfWork().PuntoRepository().GetById(atencion.PuntoId);
                atencion.PuntoDescripcion = punto.Descripcion;
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
