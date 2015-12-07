using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Repositories
{
    public class AccesoGrupoRepository
    {
        DBContext context;

        public AccesoGrupoRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.AccesoGrupo GetById(Object programaId, Object GrupoId)
        {
            return context.AccesoGrupos.Find(programaId, GrupoId);
        }

        public DbSet<Entities.AccesoGrupo> AccesoGrupos()
        {
            return this.context.AccesoGrupos;
        }

        public void Insert(Entities.AccesoGrupo entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.AccesoGrupos.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Entities.AccesoGrupo entity)
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

        public void Delete(Entities.AccesoGrupo entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.AccesoGrupos.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}