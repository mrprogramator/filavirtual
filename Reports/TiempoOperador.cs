namespace SistemaDeGestionDeFilas.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Linq;

    public partial class TiempoOperador : Telerik.Reporting.Report
    {
        public TiempoOperador()
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

            var atencionRepository = new Areas
                .FilaVirtual
                .Data
                .UnitOfWork()
                .EstadoAgenteRepository();

            var tiempos = atencionRepository
                .GetTiempoPorOperador(fechaInicio, fechaFin, puntoId);

            this.table1.DataSource = tiempos.ToList();
            //this.DataSource = transacciones.ToList();
        }
    }
}