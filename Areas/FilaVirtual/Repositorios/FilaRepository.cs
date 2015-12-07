using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class FilaRepository
    {
        DBContext context;
        private static readonly String PREFERENCIAL = "PREF";


        public FilaRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.Fila GetById(Object id)
        {
            return context.Filas.Find(id);
        }

        public DbSet<Entities.Fila> Filas()
        {
            return this.context.Filas;
        }

        public void Insert(Entities.Fila entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Filas.Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Entities.Fila entity)
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

        public void Delete(Entities.Fila entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.Filas.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Int32 Truncate()
        {
            var deletedRows = this.context.Filas.RemoveRange(this.context.Filas);
            return deletedRows.Count();
        }

        public Entities.Fila GetFirstInLine()
        {
            return context.Filas
                .OrderBy(i => i.FechaEmision)
                .Where(i => String.IsNullOrEmpty(i.AgenteId))
                .FirstOrDefault();
        }

        public Entities.Fila GetFirstInLine(IEnumerable<String> servicios, String agenteId)
        {
            var data =  context
            .Filas
            .OrderBy(i => i.FechaEmision)
            .Where(i => servicios.Contains(i.ServicioId) && String.IsNullOrEmpty(i.AgenteId))
            .FirstOrDefault();

            if (data != null)
            {
                data.AgenteId = agenteId;
                Update(data);
            }

            return data;
        }

        public Entities.Fila GetFirstInLinePreferencial(IEnumerable<String> servicios, String agenteId)
        {
            var data = context
            .Filas
            .OrderBy(i => i.FechaEmision)
            .Where(i => i.Preferencia.Equals(PREFERENCIAL)
                && servicios.Contains(i.ServicioId) && String.IsNullOrEmpty(i.AgenteId))
            .FirstOrDefault();

            if (data != null)
            {
                data.AgenteId = agenteId;
                Update(data);
            }
                
            return data;
        }

        public Entities.Fila GetFirstInLinePreferencial()
        {
            return context
                .Filas
                .OrderBy(i => i.FechaEmision)
                .Where(i => i.Preferencia.Equals(PREFERENCIAL) && String.IsNullOrEmpty(i.AgenteId))
                .FirstOrDefault();
        }

        public Entities.Fila GetCurrentTicket(String agenteId)
        {
            var item = Filas()
                .Where(i => i.AgenteId.Equals(agenteId) && !String.IsNullOrEmpty(i.ServicioId))
                .OrderByDescending(i => i.FechaEmision)
                .FirstOrDefault();

            return item;
        }

        public Int32 GetFilaLenghtByPunto(String puntoId)
        {
            var length = Filas()
                .Where(i => i.PuntoId.Equals(puntoId) && String.IsNullOrEmpty(i.AgenteId))
                .ToArray()
                .Count();

            return length;
        }

        public Int32 GetFilaLenghtByServicio(String puntoId, String servicioId)
        {
            var tipoMesa = new Data
                .UnitOfWork()
                .TipoMesaRepository()
                .TipoMesas()
                .Where(a => a.TipoId.Equals(servicioId) && a.Mesa.PuntoId.Equals(puntoId))
                .FirstOrDefault();
            
            if (tipoMesa == null) 
            {
                return -1;
            }
            
            var length = Filas()
                .Where(i => i.PuntoId.Equals(puntoId) && i.ServicioId.Equals(servicioId) && String.IsNullOrEmpty(i.AgenteId))
                .ToArray()
                .Count();

            return length;
        }
    }
}