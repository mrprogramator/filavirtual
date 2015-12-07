using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class AtencionRepository
    {
        DBContext context;
        
        public readonly String NOATENDIDO = "par008";
        public readonly String ATENDIDO = "par009";
        public readonly String FINALIZADO = "par010";

        public AtencionRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.Atencion GetById(Object id)
        {
            return context.Atenciones.Find(id);
        }

        public DbSet<Entities.Atencion> Atenciones()
        {
            return this.context.Atenciones;
        }

        public Entities.Atencion Insert(Entities.Atencion entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entity = this.context.Atenciones.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            return entity;
        }

        public void Update(Entities.Atencion entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(Entities.Atencion entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Atenciones.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public IEnumerable<Entities.Atencion> GetAtencionesAtendidas()
        {
            var data = this.context
                .Atenciones
                .Where(a => a.EstadoId.Equals(this.ATENDIDO))
                .ToArray();

            return data;
        }

        public IEnumerable<Entities.Atencion> GetAtencionesFinalizadas()
        {
            var data = this.context
                .Atenciones
                .Where(a => a.EstadoId.Equals(this.FINALIZADO))
                .ToArray();

            return data;
        }

        public IEnumerable<Entities.Atencion> GetAtencionesNoAtendidas()
        {
            var data = this.context
                .Atenciones
                .Where(a => a.EstadoId.Equals(this.NOATENDIDO))
                .ToArray();

            return data;
        }

        public IEnumerable<Entities.Atencion> GetAtenciones(DateTime inicio, DateTime fin)
        {
            var data = this.context
                .Atenciones
                .Where(a => a.FechaEmision >= inicio && a.FechaEmision <= fin)
                .ToArray();

            return data;
        }
        #region Reportes
        public IEnumerable<Areas.Reporte.Models.AtencionesOperador> GetCantidadDeAtencionesPorOperador(DateTime inicio, DateTime fin)
        {
            var atenciones = Atenciones()
                .Where(a => a.FechaInicio > inicio && a.FechaInicio < fin && a.FechaFin > inicio && a.FechaFin < fin)
                .GroupBy(a => a.Agente)
                .Select(a => new Areas.Reporte.Models.AtencionesOperador() { 
                    MesaId = a.Key.Id,
                    UsuarioId = a.Key.LogUsr,
                    UsuarioNombre = a.Key.Usuario.Nombre,
                    Cantidad = a.Count()
                })
                .OrderByDescending(a => a.Cantidad)
                .ToArray();

            return atenciones;
        }

        public IEnumerable<Areas.Reporte.Models.AtencionesOperador> GetCantidadDeAtencionesPorOperador(DateTime inicio, DateTime fin, String puntoId)
        {
            var atenciones = Atenciones()
                .Where(a => a.FechaInicio > inicio && a.FechaInicio < fin && a.FechaFin > inicio && a.FechaFin < fin && a.PuntoId.Equals(puntoId))
                .GroupBy(a => a.Agente)
                .Select(a => new Areas.Reporte.Models.AtencionesOperador()
                {
                    MesaId = a.Key.Id,
                    UsuarioId = a.Key.LogUsr,
                    UsuarioNombre = a.Key.Usuario.Nombre,
                    Cantidad = a.Count()
                })
                .OrderByDescending(a => a.Cantidad)
                .ToArray();

            return atenciones;
        }

        public IEnumerable<Entities.Atencion> GetAtencionesPorOperador(DateTime inicio, DateTime fin, String operadorId)
        {
            var atenciones = Atenciones()
                .Where(a => a.Agente.LogUsr.Equals(operadorId) && a.FechaInicio > inicio && a.FechaInicio < fin && a.FechaFin > inicio && a.FechaFin < fin && a.EstadoId.Equals(FINALIZADO))
                .ToArray();

            return atenciones;
        }
        #endregion
    }
}