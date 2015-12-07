using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class AtencionRepositoryPSQL
    {
        PostgreSQLDBContext context;

        public readonly String NOATENDIDO = "par008";
        public readonly String ATENDIDO = "par009";
        public readonly String FINALIZADO = "par010";

        public AtencionRepositoryPSQL(PostgreSQLDBContext context)
        {
            this.context = context;
        }

        public Entities.AtencionPSQL GetById(Object id)
        {
            return context.Atenciones.Find(id);
        }

        public DbSet<Entities.AtencionPSQL> Atenciones()
        {
            return this.context.Atenciones;
        }

        public Entities.AtencionPSQL Insert(Entities.AtencionPSQL entity)
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

        public void Update(Entities.AtencionPSQL entity)
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

        public void Delete(Entities.AtencionPSQL entity)
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
    }
}