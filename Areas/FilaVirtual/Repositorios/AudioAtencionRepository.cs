using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class AudioAtencionRepository
    {
        DBContext context;

        public AudioAtencionRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.AudioAtencion GetById(Object id)
        {
            return context.AudioAtenciones.Find(id);
        }

        public DbSet<Entities.AudioAtencion> Atenciones()
        {
            return this.context.AudioAtenciones;
        }

        public Entities.AudioAtencion Insert(Entities.AudioAtencion entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entity = this.context.AudioAtenciones.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            return entity;
        }

        public void Update(Entities.AudioAtencion entity)
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

        public void Delete(Entities.AudioAtencion entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.AudioAtenciones.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}