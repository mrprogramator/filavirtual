using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual
{
    public class FilaVirtualAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FilaVirtual";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FilaVirtual_default",
                "FilaVirtual/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
