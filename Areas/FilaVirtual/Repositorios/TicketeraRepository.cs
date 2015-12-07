using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class TicketeraRepository
    {
        DBContext context;

        public readonly String PREFERENCIAL = "PREF";


        public TicketeraRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.Ticketera GetById(Object id, Object puntoId)
        {
            return context.Ticketeras.Find(id,puntoId);
        }

        public IEnumerable<Entities.Ticketera> Ticketeras()
        {
            return this.context.Ticketeras.OrderBy(t => t.Id).ToArray();
        }

        public Entities.Ticketera Insert(Entities.Ticketera entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            
            var nuevo = this.context.Ticketeras.Add(entity);
            this.context.SaveChanges();
            return nuevo;
        }

        public void Update(Entities.Ticketera entity)
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

        public void Delete(Entities.Ticketera entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Ticketeras.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
    }
}