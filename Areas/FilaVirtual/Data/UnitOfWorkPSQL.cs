using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Data
{
    public class UnitOfWorkPSQL : IDisposable
    {
        private readonly PostgreSQLDBContext context;
        private Boolean disposed;

        public UnitOfWorkPSQL(PostgreSQLDBContext context)
        {
            this.context = context; 
        }

        public UnitOfWorkPSQL()
        {
            context = new PostgreSQLDBContext();
        }
        
        public void Dispose()
        {
            context.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        public void Save()
        {
            context.SaveChanges();
        }

        public virtual void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (!disposing)
                {
                    //context.Dispose();
                }
            }
            disposed = true;
        }

        public Repositories.AtencionRepositoryPSQL AtencionRepository() 
        {
            return new Repositories.AtencionRepositoryPSQL(context);
        }

        public Repositories.AudioAtencionRepositoryPSQL AudioAtencionRepository()
        {
            return new Repositories.AudioAtencionRepositoryPSQL(context);
        }

        public Repositories.DetalleAtencionRepositoryPSQL DetalleAtencionRepository()
        {
            return new Repositories.DetalleAtencionRepositoryPSQL(context);
        }
    }
}