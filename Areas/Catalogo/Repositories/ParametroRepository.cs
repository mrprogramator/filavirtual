using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Repositories
{
    public class ParametroRepository
    {
        DBContext context;

        public readonly String TIPPRO = "TIPPRO";
        public readonly String CODEST = "TIPEST";
        public readonly String CODAT = "ESTAT";
        public readonly String TIPAT = "TIPAT";
        public readonly String CODTIP = "TIPPAR";
        public readonly String ESTAG = "ESTAG";
        public readonly String AUSAG = "AUSAG";
        public readonly String TIPTIC = "TIPTIC";
        public readonly String GRUPO = "GRU";
        public readonly String PWDTIC = "PWD";
        public readonly String LLAUT = "LLAUT";
        public readonly String VIGENTE = "VIG";

        public ParametroRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.Parametro GetById(Object id)
        {
            return context.Parametros.Find(id);
        }

        public IEnumerable<Entities.Parametro> GetByGroup(String grupo)
        {
            return context.Parametros.Where(p => p.GrupoId.Equals(grupo)).ToArray();
        }

        public DbSet<Entities.Parametro> Parametros
        {
            get
            {
                return this.context.Parametros;
            }
        }

        public Entities.Parametro Insert(Entities.Parametro entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entity = this.context.Parametros.Add(entity);
                this.context.SaveChanges();
                return entity;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Entities.Parametro entity)
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

        public Entities.Parametro Delete(Entities.Parametro entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entity = this.context.Parametros.Remove(entity);
                this.context.SaveChanges();
                return entity;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Entities.Parametro> GetAusenciasOperador()
        {
            var data = GetByGroup(AUSAG).ToArray();

            return data;
        }

        public IEnumerable<Entities.Parametro> GetTiposDeAtencion()
        {
            var data = GetByGroup(TIPAT).ToArray();

            return data;
        }
    }
}