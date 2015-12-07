using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class DetalleAtencionRepository
    {
        DBContext context;

        public DetalleAtencionRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.DetalleAtencion GetById(Object id)
        {
            return context.DetalleAtenciones.Find(id);
        }

        public DbSet<Entities.DetalleAtencion> DetalleAtenciones()
        {
            return this.context.DetalleAtenciones;
        }

        public Entities.DetalleAtencion Insert(Entities.DetalleAtencion entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entity = this.context.DetalleAtenciones.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            return entity;
        }

        public void Update(Entities.DetalleAtencion entity)
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

        public void Delete(Entities.DetalleAtencion entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.DetalleAtenciones.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Areas.Reporte.Models.CantidadTransaccion> GetCantidadTransacciones(DateTime inicio, DateTime fin)
        {
            var transacciones = DetalleAtenciones()
                                .Where(d => d.Fecha > inicio && d.FechaFin < fin)
                                .GroupBy(d => d.Servicio)
                                .Select(c => new Areas.Reporte.Models.CantidadTransaccion() 
                                {
                                    NombreTransaccion = c.Key.Nombre,
                                    Cantidad = c.Count()
                                })
                                .OrderByDescending(d => d.Cantidad)
                                .ToArray();

            return transacciones;
        }

        public IEnumerable<Areas.Reporte.Models.CantidadTransaccion> GetCantidadTransacciones(DateTime inicio, DateTime fin, String puntoId)
        {
            var transacciones = DetalleAtenciones()
                                .Where(d => d.Fecha > inicio && d.FechaFin < fin && d.Atencion.PuntoId.Equals(puntoId))
                                .GroupBy(d => d.Servicio)
                                .Select(c => new Areas.Reporte.Models.CantidadTransaccion()
                                {
                                    NombreTransaccion = c.Key.Nombre,
                                    Cantidad = c.Count()
                                })
                                .OrderByDescending(d => d.Cantidad)
                                .ToArray();

            return transacciones;
        }
    }
}