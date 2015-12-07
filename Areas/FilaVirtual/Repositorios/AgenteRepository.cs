using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class AgenteRepository
    {
        DBContext context;

        public AgenteRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.Agente GetById(Object id)
        {
            return context.Agentes.Find(id);
        }

        public DbSet<Entities.Agente> Agentes()
        {
            return this.context.Agentes;
        }

        public void Insert(Entities.Agente entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Agentes.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Entities.Agente entity)
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

        public void Delete(Entities.Agente entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Agentes.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Areas.Catalogo.Entities.Usuario> GetOperadores()
        {
            var mesas = Agentes().ToArray();

            var operadores = new List<Areas.Catalogo.Entities.Usuario>();

            foreach (var mesa in mesas)
            {
                if (mesa.Usuario != null)
                {
                    operadores.Add(mesa.Usuario);
                }
            }

            return operadores;
        }

    }
}