using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;
using SistemaDeGestionDeFilas.Areas.Reporte.Models;
using SistemaDeGestionDeFilas.Areas.Catalogo.Models;

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

        public Int32 GetCantidadTransaccionesPorParamétrica(DateTime inicio, DateTime fin, String puntoId, String paramId)
        {
            var cantidad = DetalleAtenciones()
                .Where(d => d.Fecha > inicio && d.FechaFin < fin && d.Atencion.PuntoId.Equals(puntoId) && d.ServicioId.Equals(paramId))
                .Count();

            return cantidad;
        }

        public Areas.Reporte.Models.ArbolTransaccion GetArbolTransaccion(DateTime inicio, DateTime fin, String puntoId)
        {
            var raizId = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().TIPAT;

            var raiz = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(raizId);

            var arbol = new Areas.Reporte.Models.ArbolTransaccion(raiz.Id, raiz.Nombre, 0, new List<ArbolTransaccion>());

            arbol = FillTree(arbol, inicio, fin, puntoId);

            return arbol;
        }

        private ArbolTransaccion FillTree(ArbolTransaccion arbol, DateTime inicio, DateTime fin, String puntoId)
        {
            var hijos = new Areas
                .Catalogo
                .Data
                .UnitOfWork()
                .ParametroRepository()
                .GetByGroup(arbol.Id)
                .OrderByDescending(h => h.Id);

            foreach (var hijo in hijos)
            {
                var cantidad = GetCantidadTransaccionesPorParamétrica(inicio, fin, puntoId, hijo.Id);

                var rama = new ArbolTransaccion(hijo.Id, hijo.Nombre, cantidad, new List<ArbolTransaccion>());
                rama = FillTree(rama, inicio, fin, puntoId);
                arbol.Cantidad += rama.Cantidad;
                arbol.Hijos.Add(rama);
            }

            return arbol;
        }
    }
}