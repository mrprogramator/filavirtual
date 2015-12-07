using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class DetalleAtencionRepositoryPSQL
    {
        PostgreSQLDBContext context;

        public DetalleAtencionRepositoryPSQL(PostgreSQLDBContext context)
        {
            this.context = context;
        }

        public Entities.DetalleAtencionPSQL GetById(Object id)
        {
            return context.DetalleAtenciones.Find(id);
        }

        public DbSet<Entities.DetalleAtencionPSQL> DetalleAtenciones()
        {
            return this.context.DetalleAtenciones;
        }

        public Entities.DetalleAtencionPSQL Insert(Entities.DetalleAtencionPSQL entity)
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

        public void Update(Entities.DetalleAtencionPSQL entity)
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

        public void Delete(Entities.DetalleAtencionPSQL entity)
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
    }
}