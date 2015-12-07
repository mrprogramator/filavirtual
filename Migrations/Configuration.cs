namespace SistemaDeGestionDeFilas.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SistemaDeGestionDeFilas.Areas;
    internal sealed class Configuration : DbMigrationsConfiguration<SistemaDeGestionDeFilas.Data.DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "SistemaDeGestionDeFilas.Data.DBContext";
        }

        protected override void Seed(SistemaDeGestionDeFilas.Data.DBContext context)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SistemaDeGestionDeFilas.Data.DBContext, Configuration>());
            context.Parametros.AddOrUpdate(
                p => p.Id,
                new Areas.Catalogo.Entities.Parametro()
                {
                    Id = "GRU",
                    Nombre = "Grupos"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "TIPEST",
                    Nombre = "Tipos de Estados",
                    GrupoId = "GRU"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "VIG",
                    Nombre = "Vigente",
                    GrupoId = "TIPEST"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "NVG",
                    Nombre = "No Vigente",
                    GrupoId = "TIPEST"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "TIPPRO",
                    Nombre = "Tipos de Programas",
                    GrupoId = "GRU"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "SOL",
                    Nombre = "Solución",
                    GrupoId = "TIPPRO"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "CAR",
                    Nombre = "Carpeta",
                    GrupoId = "TIPPRO"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "PRO",
                    Nombre = "Programa",
                    GrupoId = "TIPPRO"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "ESTAT",
                    Nombre = "Estados de Atención",
                    GrupoId = "GRU"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "par008",
                    Nombre = "No Atendida",
                    GrupoId = "ESTAT"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "par009",
                    Nombre = "Atendida",
                    GrupoId = "ESTAT"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "par010",
                    Nombre = "Finalizada",
                    GrupoId = "ESTAT"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "TIPPAR",
                    Nombre = "Tipos de Configuración de Ticketera",
                    GrupoId = "GRU"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "TICGRU",
                    Nombre = "Grupo",
                    GrupoId = "TIPPAR"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "TICOPC",
                    Nombre = "Opción",
                    GrupoId = "TIPPAR"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "TIPTIC",
                    Nombre = "Tipos de Ticket",
                    GrupoId = "GRU"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "NORM",
                    Nombre = "Normal",
                    GrupoId = "TIPTIC"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "PREF",
                    Nombre = "Preferencial",
                    GrupoId = "TIPTIC"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "AUSAG",
                    Nombre = "Ausencias de Operador",
                    GrupoId = "GRU"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "AUS1",
                    Nombre = "Baño",
                    Valor = "5",
                    GrupoId = "AUSAG"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "AUS2",
                    Nombre = "Fotocopia",
                    Valor = "6",
                    GrupoId = "AUSAG"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "AUS3",
                    Nombre = "Consulta al Supervisor",
                    Valor = "5",
                    GrupoId = "AUSAG"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "AUS4",
                    Nombre = "Consulta Médica",
                    Valor = "20",
                    GrupoId = "AUSAG"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "AUS5",
                    Nombre = "Personal",
                    Valor = "10",
                    GrupoId = "AUSAG"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "ESTAG",
                    Nombre = "Estados de Operador",
                    GrupoId = "GRU"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "estage1",
                    Nombre = "Activo",
                    GrupoId = "ESTAG"
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Parametro()
                {
                    Id = "estage2",
                    Nombre = "Inactivo",
                    GrupoId = "ESTAG"
                }
            );
            context.Usuarios.AddOrUpdate(
                u => u.Id,
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Usuario()
                {
                    Id = "admin",
                    Email = "admin@gmail.com",
                    Password = SistemaDeGestionDeFilas.Areas.Catalogo.Security.PasswordHash.CreateHash("0000")
                }
            );
            context.Grupos.AddOrUpdate(
                g => g.Id,
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Grupo()
                {
                    Id = "GRADM",
                    Nombre = "Administrador",
                    EstadoId = "VIG"
                }
            );
            context.UsuarioGrupos.AddOrUpdate(
                g => new { g.UsuarioId, g.GrupoId },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.UsuarioGrupo()
                {
                    UsuarioId = "admin",
                    GrupoId = "GRADM",
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.MaxValue
                }
            );
            context.Programas.AddOrUpdate(
                p => p.Id,
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "COT",
                    Nombre = "COTAS",
                    TipoId = "SOL",
                    Orden = 0,
                    EstadoId = "VIG",
                    CheckSel = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "CAT",
                    Nombre = "Catálogo",
                    TipoId = "CAR",
                    Orden = 1,
                    EstadoId = "VIG",
                    PadreId = "COT",
                    CheckSel = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "PRO",
                    Nombre = "Programas",
                    TipoId = "PRO",
                    Orden = 2,
                    EstadoId = "VIG",
                    PadreId = "CAT",
                    Url = "Programa",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "USU",
                    Nombre = "Usuarios",
                    TipoId = "PRO",
                    Orden = 3,
                    EstadoId = "VIG",
                    PadreId = "CAT",
                    Url = "Usuario",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "GRU",
                    Nombre = "Grupos",
                    TipoId = "PRO",
                    Orden = 4,
                    EstadoId = "VIG",
                    PadreId = "CAT",
                    Url = "Grupo",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "FIL",
                    Nombre = "Fila Virtual",
                    TipoId = "CAR",
                    Orden = 4,
                    EstadoId = "VIG",
                    PadreId = "COT",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "PNT",
                    Nombre = "Puntos",
                    TipoId = "PRO",
                    Orden = 5,
                    EstadoId = "VIG",
                    PadreId = "FIL",
                    Url = "Punto",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "TIC",
                    Nombre = "Ticketeras",
                    TipoId = "PRO",
                    Orden = 6,
                    EstadoId = "VIG",
                    PadreId = "FIL",
                    Url = "Ticketera",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "MES",
                    Nombre = "Mesas",
                    TipoId = "PRO",
                    Orden = 7,
                    EstadoId = "VIG",
                    PadreId = "FIL",
                    Url = "Mesa",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "ATE",
                    Nombre = "Atenciones",
                    TipoId = "PRO",
                    Orden = 8,
                    EstadoId = "VIG",
                    PadreId = "FIL",
                    Url = "Atencion",
                    CheckSel = true,
                    CheckImp = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.Programa()
                {
                    Id = "PAR",
                    Nombre = "Parámetros",
                    TipoId = "PRO",
                    Orden = 9,
                    EstadoId = "VIG",
                    PadreId = "FIL",
                    Url = "Parametro",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true
                }

            );
            context.AccesoGrupos.AddOrUpdate(
                a => new {ProgramaId = a.ProgramaId, GrupoId = a.GrupoId },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.AccesoGrupo()
                {
                    ProgramaId = "COT",
                    GrupoId = "GRADM",
                    CheckSel = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.AccesoGrupo()
                {
                    ProgramaId = "CAT",
                    GrupoId = "GRADM",
                    CheckSel = true
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.AccesoGrupo()
                {
                    ProgramaId = "PRO",
                    GrupoId = "GRADM",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true,
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.AccesoGrupo()
                {
                    ProgramaId = "USU",
                    GrupoId = "GRADM",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true,
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.AccesoGrupo()
                {
                    ProgramaId = "GRU",
                    GrupoId = "GRADM",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true,
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.AccesoGrupo()
                {
                    ProgramaId = "FIL",
                    GrupoId = "GRADM",
                    CheckSel = true,
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.AccesoGrupo()
                {
                    ProgramaId = "PNT",
                    GrupoId = "GRADM",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true,
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.AccesoGrupo()
                {
                    ProgramaId = "TIC",
                    GrupoId = "GRADM",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true,
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.AccesoGrupo()
                {
                    ProgramaId = "MES",
                    GrupoId = "GRADM",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true,
                },
                new SistemaDeGestionDeFilas.Areas.Catalogo.Entities.AccesoGrupo()
                {
                    ProgramaId = "PAR",
                    GrupoId = "GRADM",
                    CheckSel = true,
                    CheckIns = true,
                    CheckMod = true,
                    CheckEli = true,
                    CheckImp = true,
                }
            );


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
