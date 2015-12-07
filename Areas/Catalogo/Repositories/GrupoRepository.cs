using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Repositories
{
    public class GrupoRepository
    {
        DBContext context;

        public GrupoRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.Grupo GetById(Object id)
        {
            return context.Grupos.Find(id);
        }

        public DbSet<Entities.Grupo> Grupos()
        {
            return this.context.Grupos;
        }

        public void Insert(Entities.Grupo entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Grupos.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Entities.Grupo entity)
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

        public void Delete(Entities.Grupo entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Grupos.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}