using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    [RouteArea("FilaVirtual", AreaPrefix="filavirtual")]
    public class FilaController : Controller
    {
        //
        // GET: /Fila/
        private static readonly String NOATENDIDO = "par008";

        private static readonly object locker = new object();


        private Data.UnitOfWork unitOfWork;
        private Repositories.FilaRepository filaRepository;

        public FilaController()
        {
            unitOfWork = new Data.UnitOfWork();
            filaRepository = unitOfWork.FilaRepository();
        }

        [HttpPost]
        [Route("fila")]
        public JsonResult GetFila()
        {
            var data = filaRepository.Filas().OrderBy(i => i.FechaEmision);
            return Json(data);
        }

        [HttpPost]
        [Route("mesas/{mesaId}/fila")]
        public JsonResult GetFilaByMesa(String mesaId)
        {
            var servicioMesa = 
                unitOfWork
                .TipoMesaRepository()
                .TipoMesas()
                .Where(t => t.MesaId.Equals(mesaId))
                .Select(t => t.TipoId)
                .ToArray();

            var data = 
                filaRepository.Filas()
                .Where(i => servicioMesa.Contains(i.ServicioId) && String.IsNullOrEmpty(i.AgenteId))
                .OrderBy(i => i.FechaEmision);
            return Json(data);
        }

        

        [HttpPost]
        [Route("fila/limpiar")]
        public JsonResult CleanLine()
        {
            int count = filaRepository.Truncate();
            return Json(new { result = true, value = count });
        }

        [HttpPost]
        [Route("tickets/abandonados")]
        public JsonResult TicketsAbandonados()
        {
            var data = filaRepository
                .Filas()
                .Where(i => String.IsNullOrEmpty(i.ServicioId))
                .OrderByDescending(i => i.FechaEmision)
                .ToArray();

            return Json(data);
        }

        [HttpPost]
        [Route("tickets/abandonados/{inicio}/{fin}")]
        public JsonResult GetTicketsAbandonadasRange(DateTime inicio, DateTime fin)
        {
            try
            {
                var data = filaRepository
                    .Filas()
                    .OrderByDescending(a => a.FechaEmision)
                    .Where(i => String.IsNullOrEmpty(i.ServicioId) && i.FechaEmision <= fin && i.FechaEmision >= inicio)
                    .ToArray();
                return Json(data);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("tickets/abandonados/{inicio}/{fin}/{nroTicket}")]
        public JsonResult GetTicketsAbandonadasRangeNroTicket(DateTime inicio, DateTime fin, String nroTicket)
        {
            try
            {
                var data = filaRepository
                    .Filas()
                    .OrderByDescending(a => a.FechaEmision)
                    .Where(i => String.IsNullOrEmpty(i.ServicioId) && i.FechaEmision <= fin && i.FechaEmision >= inicio && i.NroTicket.Equals(nroTicket))
                    .ToArray();
                return Json(data);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        Entities.Fila AsignarTicket(string nroAgente)
        {
            var agenteEntity = unitOfWork
            .AgenteRepository()
            .GetById(nroAgente);

            if (agenteEntity == null)
            {
                throw new Exception("No se encuentra la mesa " + nroAgente);
            }

            var puntoEntity = unitOfWork
                .PuntoRepository()
                .GetById(agenteEntity.PuntoId);
            if (puntoEntity == null)
            {
                throw new Exception("No se encuentra el punto " + agenteEntity.PuntoId);
            }

            var servicioMesa = unitOfWork
                .TipoMesaRepository()
                .TipoMesas()
                .Where(t => t.MesaId.Equals(nroAgente))
                .Select(t => t.TipoId)
                .ToArray();

            if (servicioMesa == null)
            {
                throw new Exception("La mesa " + nroAgente + " no tiene asignado un servicio");
            }

            var first = filaRepository.GetFirstInLinePreferencial(servicioMesa, agenteEntity.Id);

            if (first != null) //Si existe un ticket preferencial
            {
                if (puntoEntity.OrdenAtencion == puntoEntity.AtencionActual)
                {
                    puntoEntity.AtencionActual = 0;
                }
                else
                {
                    first.AgenteId = "";
                    filaRepository.Update(first);

                    first = filaRepository.GetFirstInLine(servicioMesa, agenteEntity.Id);
                    puntoEntity.AtencionActual++;
                }

                unitOfWork.PuntoRepository().Update(puntoEntity);
            }
            else
            {
                first = filaRepository.GetFirstInLine(servicioMesa, agenteEntity.Id);
            }


            if (first == null)
            {
                throw new Exception("La fila está vacía");
            }

            first.AgenteId = nroAgente;
            Hubs.MainHub.HubContext.Clients.All.Update();
            return first;
        }

        [HttpPost]
        [Route("fila/{id}/abandonar")]
        public JsonResult AbandonarTicket(Int32 id)
        {
            try
            {
                var entity = filaRepository.GetById(id);

                entity.ServicioId = "";

                filaRepository.Update(entity);

                return Json(new { result = true });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("mesas/{nroAgente}/fila/{id}/recuperar")]
        public JsonResult RecuperarTicket(String nroAgente, Int32 id)
        {
            try
            {
                var agenteEntity = unitOfWork
                    .AgenteRepository()
                    .GetById(nroAgente);

                if (agenteEntity == null)
                {
                    throw new Exception("No se encuentra la mesa " + nroAgente);
                }

                var servicio = unitOfWork
                    .TipoMesaRepository()
                    .TipoMesas()
                    .Where(t => t.MesaId.Equals(nroAgente))
                    .Select(t => t.TipoId)
                    .FirstOrDefault();
                
                var entity = filaRepository.GetById(id);

                if (servicio == null)
                {
                    throw new Exception("La mesa " + nroAgente + " no tiene asignado un servicio");
                }
                
                entity.ServicioId = servicio;
                entity.AgenteId = nroAgente;

                filaRepository.Update(entity);

                return Json(new { result = true, value = entity });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("mesas/{nroAgente}/fila/pull")]
        public JsonResult PullFirstInLine(String nroAgente)
        {
            try
            {
                lock (locker)
                {
                    var asignado = filaRepository
                        .Filas()
                        .Where(i => i.AgenteId.Equals(nroAgente) && !String.IsNullOrEmpty(i.ServicioId))
                        .FirstOrDefault();

                    if (asignado == null)
                    {
                        var first = AsignarTicket(nroAgente);

                        var atencionEntity = new Entities.Atencion()
                        {
                            NroTicket = first.NroTicket,
                            AgenteId = first.AgenteId,
                            PuntoId = first.PuntoId,
                            ServicioId = first.ServicioId,
                            FechaEmision = first.FechaEmision,
                            FechaLlamado = DateTime.Now,
                            FechaInicio = DateTime.MaxValue,
                            FechaFin = DateTime.MaxValue,
                            EstadoId = NOATENDIDO
                        };

                        atencionEntity = unitOfWork
                            .AtencionRepository()
                            .Insert(atencionEntity);

                        return Json(new { result = true, value = first });
                    }

                    var atencionAsignada = unitOfWork
                        .AtencionRepository()
                        .Atenciones()
                        .Where(a => a.NroTicket.Equals(asignado.NroTicket) && a.AgenteId.Equals(nroAgente))
                        .OrderByDescending(a => a.FechaEmision)
                        .FirstOrDefault();

                    if (atencionAsignada == null)
                    {
                        var atencionEntity = new Entities.Atencion()
                        {
                            NroTicket = asignado.NroTicket,
                            AgenteId = asignado.AgenteId,
                            PuntoId = asignado.PuntoId,
                            ServicioId = asignado.ServicioId,
                            FechaEmision = asignado.FechaEmision,
                            FechaLlamado = DateTime.Now,
                            FechaInicio = DateTime.MaxValue,
                            FechaFin = DateTime.MaxValue,
                            EstadoId = NOATENDIDO
                        };

                        atencionEntity = unitOfWork
                            .AtencionRepository()
                            .Insert(atencionEntity);

                        return Json(new { result = true, value = asignado });
                    }

                    atencionAsignada.EstadoId = NOATENDIDO;
                    atencionAsignada.FechaLlamado = DateTime.Now;
                    atencionAsignada.FechaInicio = DateTime.MaxValue;
                    atencionAsignada.FechaFin = DateTime.MaxValue;
                    unitOfWork.AtencionRepository().Update(atencionAsignada);

                    return Json(new { result = true, value = asignado });
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
