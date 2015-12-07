using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class ConfTicketeraRepository
    {
        DBContext context;
        public readonly String CODPAR = "TICOPC";
        public readonly String CODGRU = "TICGRU";

        public ConfTicketeraRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.ConfTicketera GetById(Object id)
        {
            return context.ConfTicketeras.Find(id);
        }

        public IEnumerable<Entities.ConfTicketera> GetByTicketeraId(Object ticketeraId, Object puntoId)
        {
            return context.ConfTicketeras.ToArray().Where(c => c.TicketeraId.Equals(ticketeraId) && c.PuntoId.Equals(puntoId));
        }

        public DbSet<Entities.ConfTicketera> ConfTicketeras()
        {
            return this.context.ConfTicketeras;
        }

        public void Insert(Entities.ConfTicketera entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.ConfTicketeras.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Entities.ConfTicketera entity)
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

        public void Delete(Entities.ConfTicketera entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.ConfTicketeras.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}