namespace SistemaDeGestionDeFilas.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Linq;

    public partial class AusenciaOperador : Telerik.Reporting.Report
    {
        public AusenciaOperador()
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
            String operadorId = report.Parameters["operadorId"].Value.ToString();

            fechaFin = fechaFin.AddDays(1);

            var repository = new Areas
                    .FilaVirtual
                    .Data
                    .UnitOfWork()
                    .EstadoAgenteRepository();

            var atencionRepository = new Areas
                    .FilaVirtual
                    .Data
                    .UnitOfWork()
                    .AtencionRepository();

            

            var ausenciasOperador = repository.GetTiempoAusentePorOperador(fechaInicio, fechaFin, operadorId);

            this.table1.DataSource = ausenciasOperador.ToList();



            var atencionesOperador = atencionRepository.GetAtencionesPorOperador(fechaInicio, fechaFin, operadorId);

            if (atencionesOperador.Count() > 0)
            {
                this.tableAtencionesOperador.DataSource = atencionesOperador.ToList();
            }

            else
            {
                this.tableAtencionesOperador.Visible = false;
                this.textBox17.Value = "El operador no tiene atenciones realizadas";
            }
            
        }
    }
}