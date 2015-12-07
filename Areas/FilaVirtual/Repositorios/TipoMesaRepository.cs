using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class TipoMesaRepository
    {
        DBContext context;

        public TipoMesaRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.TipoMesa GetById(Object MesaId, Object TipoId)
        {
            return context.TipoMesas.Find(MesaId, TipoId);
        }

        public IEnumerable<Entities.TipoMesa> TipoMesas()
        {
            return this.context.TipoMesas.ToArray();
        }

        public Entities.TipoMesa Insert(Entities.TipoMesa entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entity = this.context.TipoMesas.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            return entity;
        }

        public void Update(Entities.TipoMesa entity)
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

        public void Delete(Entities.TipoMesa entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.TipoMesas.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Boolean CheckServicesInLine(IEnumerable<Entities.TipoMesa> servicioMesa, Entities.Fila item)
        {
            foreach (var sm in servicioMesa)
            {
                if (sm.TipoId.Equals(item.ServicioId))
                    return true;
            }

            return false;
        }
    }
}