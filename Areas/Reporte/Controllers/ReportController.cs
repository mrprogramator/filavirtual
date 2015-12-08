using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Reporting;

namespace SistemaDeGestionDeFilas.Areas.Reporte.Controllers
{
    [RouteArea("Reporte", AreaPrefix="reporte")]
    public class ReportController : Controller
    {
        public ActionResult TransaccionesParametrica()
        {
            return View();
        }

        public ActionResult AtencionesOperador()
        {
            return View();
        }

        public ActionResult TiempoOperador()
        {
            return View();
        }

        public ActionResult AusenciaOperador()
        {
            return View();
        }

        public ActionResult ArbolTransaccion()
        {
            return View();
        }

        public ActionResult ReportView(Models.Reporte model)
        {
            var report = new TypeReportSource() { TypeName = model.TypeName };
            if (report != null)
            {
                foreach (var parameter in model.Parameters)
                {
                    report.Parameters.Add(new Parameter(parameter.Name, parameter.Value));
                }
            }
            return View(report);
        }
    }
}
