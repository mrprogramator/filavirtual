using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly DBContext context;
        private Boolean disposed;

        public UnitOfWork(DBContext context)
        {
            this.context = context; 
        }

        public UnitOfWork()
        {
            context = new DBContext();
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

        public Repositories.AgenteRepository AgenteRepository() 
        {
            return new Repositories.AgenteRepository(context);
        }

        public Repositories.EstadoAgenteRepository EstadoAgenteRepository()
        {
            return new Repositories.EstadoAgenteRepository(context);
        }

        public Repositories.PuntoRepository PuntoRepository()
        {
            return new Repositories.PuntoRepository(context);
        }

        public Repositories.TicketeraRepository TicketeraRepository()
        {
            return new Repositories.TicketeraRepository(context);
        }

        public Repositories.ConfTicketeraRepository ConfTicketeraRepository()
        {
            return new Repositories.ConfTicketeraRepository(context);
        }

        public Repositories.FilaRepository FilaRepository()
        {
            return new Repositories.FilaRepository(context);
        }

        public Repositories.AtencionRepository AtencionRepository() 
        {
            return new Repositories.AtencionRepository(context);
        }

        public Repositories.AudioAtencionRepository AudioAtencionRepository()
        {
            return new Repositories.AudioAtencionRepository(context);
        }

        public Repositories.DetalleAtencionRepository DetalleAtencionRepository()
        {
            return new Repositories.DetalleAtencionRepository(context);
        }

        public Repositories.TipoAtencionRepository TipoAtencionRepository()
        {
            return new Repositories.TipoAtencionRepository(context);
        }

        public Repositories.TipoMesaRepository TipoMesaRepository()
        {
            return new Repositories.TipoMesaRepository(context);
        }
    }
}