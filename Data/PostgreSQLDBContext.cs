using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Reflection;
using System.Data.Entity.ModelConfiguration;

namespace SistemaDeGestionDeFilas.Data
{
    public class PostgreSQLDBContext : DbContext
    {
        public PostgreSQLDBContext()
            : base("name=PostgreSQLDBConnection")
        {
        }

        public DbSet<Areas.FilaVirtual.Entities.AtencionPSQL> Atenciones { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.AudioAtencionPSQL> AudioAtenciones { get; set; }

        public DbSet<Areas.FilaVirtual.Entities.DetalleAtencionPSQL> DetalleAtenciones { get; set; }
    }
}