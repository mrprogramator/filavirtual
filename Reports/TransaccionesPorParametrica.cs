namespace SistemaDeGestionDeFilas.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using Areas.FilaVirtual;
    using System.Linq;

    public partial class TransaccionesPorParametrica : Telerik.Reporting.Report
    {
        public TransaccionesPorParametrica()
        {
            InitializeComponent();

            this.DataSource = null;
            this.NeedDataSource += Report_NeedDataSource;
        }

        void Report_NeedDataSource(object sender, EventArgs e)
        {
            var report = sender as Telerik.Reporting.Processing.Report;
            
            DateTime fechaInicio = (DateTime)report.Parameters["fechaInicio"].Value;
            DateTime fechaFin = (DateTime)report.Parameters["fechaFin"].Value;
            String puntoId = report.Parameters["puntoId"].Value.ToString();

            fechaFin = fechaFin.AddDays(1);

            var detalleRepository = new Areas
                .FilaVirtual
                .Data
                .UnitOfWork()
                .DetalleAtencionRepository();

            var transacciones = detalleRepository
                .GetCantidadTransacciones(fechaInicio, fechaFin, puntoId);

            this.table1.DataSource = transacciones.ToList();
            this.table1.StyleName = "Corporate.TableNormal";

            //this.DataSource = transacciones.ToList();
        }
    }
}