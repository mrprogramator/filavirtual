using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Repositories
{
    public class UsuarioRepository
    {
        DBContext context;

        public UsuarioRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.Usuario GetById(Object id)
        {
            return context.Usuarios.Find(id);
        }

        public DbSet<Entities.Usuario> Usuarios
        {
            get
            {
                return this.context.Usuarios;
            }
        }

        public void Insert(Entities.Usuario entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Usuarios.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Entities.Usuario entity)
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

        public void Delete(Entities.Usuario entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Usuarios.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}