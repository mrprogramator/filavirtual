using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaDeGestionDeFilas.Areas.FilaVirtual.Entities;
using SistemaDeGestionDeFilas.Areas.FilaVirtual.Data;
using Microsoft.AspNet.SignalR;
using System.Threading;

namespace SistemaDeGestionDeFilas.Helpers
{
    public class ExporterHelpers
    {
        UnitOfWork unitOfWork;
        UnitOfWorkPSQL unitOfWorkPSQL;

        public Int32 ExportAtenttions(DateTime From, DateTime To, Int32 cantidad)
        {
            unitOfWork = new UnitOfWork();
            unitOfWorkPSQL = new UnitOfWorkPSQL();

            var context = GlobalHost.ConnectionManager.GetHubContext<Hubs.MainHub>();

            var count = 0;

            Array atenciones;
            if (cantidad <= 0)
            {
                atenciones = unitOfWork
                .AtencionRepository()
                .Atenciones()
                .Where(a => a.FechaEmision >= From && a.FechaEmision <= To)
                .ToArray();
            }

            else
            {
                atenciones = unitOfWork
                .AtencionRepository()
                .Atenciones()
                .Where(a => a.FechaEmision >= From && a.FechaEmision <= To)
                .Take(cantidad)
                .ToArray();
            }
            
            context.Clients.All.Length(atenciones.Length);

            foreach (Atencion atencion in atenciones) 
            {
                var atencionPSQL = unitOfWorkPSQL
                    .AtencionRepository()
                    .GetById(atencion.Id);

                if (atencionPSQL == null)
                {
                    atencionPSQL = new AtencionPSQL()
                    {
                        Id = atencion.Id,
                        AgenteId = atencion.AgenteId,
                        EstadoId = atencion.EstadoId,
                        FechaEmision = atencion.FechaEmision,
                        FechaFin = atencion.FechaFin,
                        FechaInicio = atencion.FechaInicio,
                        FechaLlamado = atencion.FechaLlamado,
                        NroTicket = atencion.NroTicket,
                        PuntoId = atencion.PuntoId,
                        ServicioId = atencion.ServicioId
                    };
                    atencionPSQL = unitOfWorkPSQL
                        .AtencionRepository()
                        .Insert(atencionPSQL);
                }
                
                var audio = unitOfWork
                    .AudioAtencionRepository()
                    .GetById(atencion.Id);

                if (audio != null) 
                {
                    var audioPSQL = unitOfWorkPSQL
                        .AudioAtencionRepository()
                        .GetById(audio.Id);

                    if (audioPSQL == null)
                    {
                        audioPSQL = new AudioAtencionPSQL()
                        {
                            Id = atencionPSQL.Id,
                            Audio = audio.Audio
                        };
                        unitOfWorkPSQL
                            .AudioAtencionRepository()
                            .Insert(audioPSQL);
                    }
                    unitOfWork
                        .AudioAtencionRepository()
                        .Delete(audio);
                }

                var detalle = unitOfWork
                    .DetalleAtencionRepository()
                    .DetalleAtenciones()
                    .Where(d => d.AtencionId.Equals(atencion.Id))
                    .FirstOrDefault();

                if (detalle != null)
                {
                    var detallePSQL = unitOfWorkPSQL
                        .DetalleAtencionRepository()
                        .GetById(detalle.Id);

                    if (detallePSQL == null)
                    {
                        detallePSQL = new DetalleAtencionPSQL()
                        {
                            Id = detalle.Id,
                            AtencionId = detalle.AtencionId,
                            Fecha = detalle.Fecha,
                            FechaFin = detalle.FechaFin,
                            Observaciones = detalle.Observaciones,
                            ServicioId = detalle.ServicioId
                        };
                        unitOfWorkPSQL
                            .DetalleAtencionRepository()
                            .Insert(detallePSQL);
                    }
                }

                var model = new Areas.FilaVirtual.Models.Atencion()
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
                };

                context.Clients.All.exportada(model);
                ++count;
            }

            unitOfWork.Dispose();
            unitOfWorkPSQL.Dispose();

            return count;
        }
    }
}