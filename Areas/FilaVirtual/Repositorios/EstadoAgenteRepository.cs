using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SistemaDeGestionDeFilas.Data;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Repositories
{
    public class EstadoAgenteRepository
    {
        DBContext context;

        String ACTIVO = "estage1";
        String INACTIVO = "estage2";
        private static readonly object locker = new object();

        public EstadoAgenteRepository(DBContext context)
        {
            this.context = context;
        }

        public Entities.EstadoAgente GetById(Object id)
        {
            return context.EstadoAgentes.Find(id);
        }

        public DbSet<Entities.EstadoAgente> EstadoAgentes()
        {
            return this.context.EstadoAgentes;
        }

        public Entities.EstadoAgente Insert(Entities.EstadoAgente entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entity = this.context.EstadoAgentes.Add(entity);
                this.context.SaveChanges();
                
                return entity;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Entities.EstadoAgente entity)
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

        public void Delete(Entities.EstadoAgente entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                this.context.EstadoAgentes.Remove(entity);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Entities.EstadoAgente GetEstadoActualAgente(String id)
        {
            lock (locker)
            {
                var estado = EstadoAgentes()
                    .Where(e => e.AgenteId.Equals(id))
                    .OrderByDescending(e => e.FechaInicio)
                    .FirstOrDefault();

                if (estado == null)
                {
                    var mesa = new Data.UnitOfWork().AgenteRepository().GetById(id);
                    var estadoInactivo = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(INACTIVO);

                    estado = new Entities.EstadoAgente()
                    {
                        EstadoId = INACTIVO,
                        Estado = estadoInactivo,
                        Agente = mesa
                    };
                }
                else if (estado.FechaFin < new DateTime(3000,12,31))
                {
                    var estadoInactivo = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetById(INACTIVO);
                    estado.EstadoId = INACTIVO;
                    estado.Estado = estadoInactivo;
                }

                return estado;
            }
        }

        #region Reportes
        public IEnumerable<Areas.Reporte.Models.TiempoOperador> GetTiempoPorOperador(DateTime inicio, DateTime fin)
        {
            var tiempos = EstadoAgentes()
                .Where(e => e.FechaInicio > inicio && e.FechaInicio < fin && e.FechaFin > inicio && e.FechaFin < fin && e.EstadoId.Equals(ACTIVO))
                .ToArray();


            var lista = new List<Areas.Reporte.Models.TiempoOperador>();
            foreach(var tiempo in tiempos)
            {
                var item = lista.Where(i => i.UsuarioId.Equals(tiempo.Agente.LogUsr)).FirstOrDefault();

                var diff = DateDiff(tiempo.FechaFin, tiempo.FechaInicio).TotalHours;

                diff = Math.Round(diff);

                if (item == null)
                {
                    item = new Areas.Reporte.Models.TiempoOperador()
                    {
                        UsuarioId = tiempo.Agente.LogUsr,
                        UsuarioNombre = tiempo.Agente.Usuario.Nombre,
                        MesaId = tiempo.AgenteId,
                        Tiempo = diff
                    };

                    lista.Add(item);
                    
                    continue;
                }

                item.Tiempo += diff;
            }

            lista.OrderByDescending(i => i.Tiempo);
            
            return lista;
        }

        public IEnumerable<Areas.Reporte.Models.TiempoOperador> GetTiempoPorOperador(DateTime inicio, DateTime fin, String puntoId)
        {
            var tiempos = EstadoAgentes()
                .Where(e => e.FechaInicio > inicio && e.FechaInicio < fin && e.FechaFin > inicio && e.FechaFin < fin && e.EstadoId.Equals(ACTIVO) && e.Agente.PuntoId.Equals(puntoId))
                .ToArray();

            var lista = new List<Areas.Reporte.Models.TiempoOperador>();
            foreach (var tiempo in tiempos)
            {
                var item = lista.Where(i => i.UsuarioId.Equals(tiempo.Agente.LogUsr)).FirstOrDefault();

                var diff = DateDiff(tiempo.FechaFin, tiempo.FechaInicio).TotalHours;

                diff = Math.Round(diff);

                if (item == null)
                {
                    item = new Areas.Reporte.Models.TiempoOperador()
                    {
                        UsuarioId = tiempo.Agente.LogUsr,
                        UsuarioNombre = tiempo.Agente.Usuario.Nombre,
                        MesaId = tiempo.AgenteId,
                        Tiempo = diff
                    };

                    lista.Add(item);

                    continue;
                }

                item.Tiempo += diff;
            }

            return lista.OrderByDescending(i => i.Tiempo);
        }

        public IEnumerable<Areas.Reporte.Models.TiempoOperador> GetTiemposPorOperadorPorEstado(DateTime inicio, DateTime fin, String estadoId)
        {
            var tiempos = EstadoAgentes()
                .Where(e => e.FechaInicio > inicio && e.FechaInicio < fin && e.FechaFin > inicio && e.FechaFin < fin && e.EstadoId.Equals(estadoId))
                .ToArray();


            var lista = new List<Areas.Reporte.Models.TiempoOperador>();
            foreach (var tiempo in tiempos)
            {
                var item = lista.Where(i => i.UsuarioId.Equals(tiempo.Agente.LogUsr)).FirstOrDefault();

                var diff = DateDiff(tiempo.FechaFin, tiempo.FechaInicio).TotalHours;

                diff = Math.Round(diff);

                if (item == null)
                {
                    item = new Areas.Reporte.Models.TiempoOperador()
                    {
                        UsuarioId = tiempo.Agente.LogUsr,
                        MesaId = tiempo.AgenteId,
                        Tiempo = diff
                    };

                    lista.Add(item);

                    continue;
                }

                item.Tiempo += diff;
            }

            lista.OrderByDescending(i => i.Tiempo);

            return lista;
        }

        public Areas.Reporte.Models.TiempoOperador GetTiempoPorOperadorPorMotivo(DateTime inicio, DateTime fin, String operadorId, String motivoId)
        {
            var tiempos = EstadoAgentes()
                .Where(e => e.FechaInicio > inicio && e.FechaInicio < fin && e.FechaFin > inicio && e.FechaFin < fin && e.MotivoId.Equals(motivoId) && e.Agente.LogUsr.Equals(operadorId))
                .ToArray();

            Areas.Reporte.Models.TiempoOperador tiempoOperador = null;
            
            foreach (var tiempo in tiempos)
            {
                var diff = DateDiff(tiempo.FechaFin, tiempo.FechaInicio).TotalMinutes;

                diff = Math.Round(diff);

                if (tiempoOperador == null)
                {
                    tiempoOperador = new Areas.Reporte.Models.TiempoOperador()
                    {
                        UsuarioId = tiempo.Agente.LogUsr,
                        MesaId = tiempo.AgenteId,
                        Tiempo = diff
                    };
                    continue;
                }

                tiempoOperador.Tiempo += diff;
            }

            return tiempoOperador;
        }

        public IEnumerable<Areas.Reporte.Models.AusenciaOperador> GetTiempoAusentePorOperador(DateTime inicio, DateTime fin, String operadorId)
        {
            var ausencias = new Areas.Catalogo.Data.UnitOfWork().ParametroRepository().GetAusenciasOperador();

            var lista = new List<Areas.Reporte.Models.AusenciaOperador>();

            var i = inicio;

            while(i < fin)
            {

                var operador = new Areas
                    .Catalogo
                    .Data
                    .UnitOfWork()
                    .UsuarioRepository()
                    .GetById(operadorId);

                var ausenciaOperador = new Areas.Reporte.Models.AusenciaOperador()
                {
                    OperadorId = operadorId,
                    Fecha = i,
                    Tiempo = 0
                };

                if (operador != null)
                {
                    ausenciaOperador.OperadorNombre = operador.Nombre;
                }
                
                var k = 0;
                foreach (var ausencia in ausencias)
                {
                    var tiempoOperador = GetTiempoPorOperadorPorMotivo(inicio: i, fin: i.AddDays(1), operadorId: operadorId, motivoId: ausencia.Id);


                    switch (k)
                    {
                        case 0:
                            ausenciaOperador.Ocupado = new Areas.Reporte.Models.Ausencia()
                            {
                                Id = ausencia.Id,
                                Descripcion = ausencia.Nombre,
                                Fecha = i
                            };

                            if (tiempoOperador != null)
                            {
                                ausenciaOperador.Ocupado.Tiempo = tiempoOperador.Tiempo;
                            }
                            ausenciaOperador.Tiempo += ausenciaOperador.Ocupado.Tiempo;

                            break;

                        case 1:
                            ausenciaOperador.Baño = new Areas.Reporte.Models.Ausencia()
                            {
                                Id = ausencia.Id,
                                Descripcion = ausencia.Nombre,
                                Fecha = i
                            };

                            if (tiempoOperador != null)
                            {
                                ausenciaOperador.Baño.Tiempo = tiempoOperador.Tiempo;
                            }
                            ausenciaOperador.Tiempo += ausenciaOperador.Baño.Tiempo;

                            break;

                        case 2:
                            ausenciaOperador.Fotocopia = new Areas.Reporte.Models.Ausencia()
                            {
                                Id = ausencia.Id,
                                Descripcion = ausencia.Nombre,
                                Fecha = i
                            };

                            if (tiempoOperador != null)
                            {
                                ausenciaOperador.Fotocopia.Tiempo = tiempoOperador.Tiempo;
                            }
                            ausenciaOperador.Tiempo += ausenciaOperador.Fotocopia.Tiempo;

                            break;

                        case 3:
                            ausenciaOperador.ConsultaSupervisor = new Areas.Reporte.Models.Ausencia()
                            {
                                Id = ausencia.Id,
                                Descripcion = ausencia.Nombre,
                                Fecha = i
                            };

                            if (tiempoOperador != null)
                            {
                                ausenciaOperador.ConsultaSupervisor.Tiempo = tiempoOperador.Tiempo;
                            }
                            ausenciaOperador.Tiempo += ausenciaOperador.ConsultaSupervisor.Tiempo;

                            break;

                        case 4:
                            ausenciaOperador.ConsultaMedica = new Areas.Reporte.Models.Ausencia()
                            {
                                Id = ausencia.Id,
                                Descripcion = ausencia.Nombre,
                                Fecha = i
                            };

                            if (tiempoOperador != null)
                            {
                                ausenciaOperador.ConsultaMedica.Tiempo = tiempoOperador.Tiempo;
                            }
                            ausenciaOperador.Tiempo += ausenciaOperador.ConsultaMedica.Tiempo;

                            break;

                        case 5:
                            ausenciaOperador.Personal = new Areas.Reporte.Models.Ausencia()
                            {
                                Id = ausencia.Id,
                                Descripcion = ausencia.Nombre,
                                Fecha = i
                            };

                            if (tiempoOperador != null)
                            {
                                ausenciaOperador.Personal.Tiempo = tiempoOperador.Tiempo;
                            }
                            ausenciaOperador.Tiempo += ausenciaOperador.Personal.Tiempo;
                            break;
                    }

                    ++k;
                }

                lista.Add(ausenciaOperador);

                i = i.AddDays(1);
            }

            return lista;
        }
        #endregion

        private TimeSpan DateDiff(DateTime max, DateTime min)
        {
            var diff = max - min;

            return diff;
        }
    }
}