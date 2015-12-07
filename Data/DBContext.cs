using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Reflection;
using System.Data.Entity.ModelConfiguration;

namespace SistemaDeGestionDeFilas.Data
{
    public class DBContext : DbContext
    {
        public DBContext()
            : base("name=SQLDBConnection")
        {
        }

        public DbSet<Areas.Catalogo.Entities.Acceso> Accesos { get; set; }

        public DbSet<Areas.Catalogo.Entities.AccesoGrupo> AccesoGrupos { get; set; }

        public DbSet<Areas.Catalogo.Entities.Grupo> Grupos { get; set; }

        public DbSet<Areas.Catalogo.Entities.Parametro> Parametros { get; set; }

        public DbSet<Areas.Catalogo.Entities.Programa> Programas { get; set; }

        public DbSet<Areas.Catalogo.Entities.Usuario> Usuarios { get; set; }

        public DbSet<Areas.Catalogo.Entities.UsuarioGrupo> UsuarioGrupos { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.Punto> Puntos { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.Agente> Agentes { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.EstadoAgente> EstadoAgentes { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.Ticketera> Ticketeras { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.ConfTicketera> ConfTicketeras { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.Fila> Filas { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.Atencion> Atenciones { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.AudioAtencion> AudioAtenciones { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.DetalleAtencion> DetalleAtenciones { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.TipoMesa> TipoMesas { get; set; }

    }
}