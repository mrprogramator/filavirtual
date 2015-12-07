using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using System.IO;
using SistemaDeGestionDeFilas.Services;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Controllers
{
    public class OperadorController : Controller
    {
        //
        // GET: /Operador/

        [HttpGet]
        [Route("operador")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("operador/login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [Route("operador/{filename}/download")]
        public ActionResult DownloadOperadorRelase(String filename, Int32? width, Int32? height, Boolean? openDevTools)
        {
            try
            {
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority
                            + Request.ApplicationPath.TrimEnd('/') + "/";

                var settings = new ClientAppSettings
                {
                    Filename = filename,
                    ServerURL = baseUrl,
                    ScreenWidth = width.GetValueOrDefault(600),
                    ScreenHeight = height.GetValueOrDefault(500),
                    OpenDevTools = openDevTools.GetValueOrDefault(false)
                };

                var zipPath = ClientAppPackager.Instance.CreatePackage(settings);

                return new FilePathResult(zipPath, "application/zip")
                {
                    FileDownloadName = filename + ".zip"
                };
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("operadores")]
        public JsonResult GetOperadores()
        {
            var data = new Data.UnitOfWork().AgenteRepository().GetOperadores();

            return Json(data);
        }
    }
}
