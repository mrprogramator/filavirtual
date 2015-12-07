using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class TipoAtencionRepository
    {
        private static readonly String TIPOAT = "TIPAT";
        DBContext context;

        public TipoAtencionRepository(DBContext context)
        {
            this.context = context;
        }

        public Catalogo.Entities.Parametro GetById(Object id)
        {
            return context.Parametros.Find(id);
        }

        public IEnumerable<Catalogo.Entities.Parametro> TipoAtenciones()
        {
            return this.context.Parametros
                .Where( p => p.GrupoId.Equals(TIPOAT))
                .OrderBy(p => p.Id)
                .ToArray();
        }

        public IEnumerable<Catalogo.Entities.Parametro> Parametros()
        {
            return this.context.Parametros
                .OrderBy(p => p.Id)
                .ToArray();
        }

        public Catalogo.Entities.Parametro Insert(Catalogo.Entities.Parametro entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entity = this.context.Parametros.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            return entity;
        }

        public void Update(Catalogo.Entities.Parametro entity)
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

        public void Delete(Catalogo.Entities.Parametro entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Parametros.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}