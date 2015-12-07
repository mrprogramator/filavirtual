using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SistemaDeGestionDeFilas.Areas.Catalogo.Controllers
{
    [RouteArea("Catalogo", AreaPrefix="catalogo")]
    public class UsuarioController : Controller
    {
        //
        // GET: /Usuario/

        private Data.UnitOfWork unitOfWork;
        private Repositories.UsuarioRepository usuarioRepository;

        public UsuarioController()
        {
            unitOfWork = new Data.UnitOfWork();
            usuarioRepository = unitOfWork.UsuarioRepository();
            //InsertInitialValues();
        }

        public void InsertInitialValues()
        {
            try
            {
                var entity = new Entities.Usuario();
                entity.Id = "admin";
                entity.Password =  Security.PasswordHash.CreateHash("0000");
                entity.Email = "admin@gmail.com";
                usuarioRepository.Insert(entity);
            }
            catch (Exception)
            {
                return;
            }
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Usuario model)
        {
            try
            {
                var entity = new Entities.Usuario();
                entity.Id = model.Id;
                entity.Email = model.Email;
                entity.Nombre = model.Nombre;
                entity.Password = Security.PasswordHash.CreateHash(model.Password);
                
                usuarioRepository.Insert(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index") });
        }

        [HttpGet]
        [Route("usuarios/{id}/edit")]
        public ActionResult Edit(String id)
        {
            var model = new Models.Usuario();
            try
            {
                var entity = usuarioRepository.GetById(id);
                model.Id = entity.Id;
                model.Nombre = entity.Nombre;
                model.Email = entity.Email;
                model.Password = entity.Password;
            }
            catch (Exception e)
            {
                throw e;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Models.Usuario model)
        {
            try
            {
                var entity = usuarioRepository.GetById(model.Id);
                entity.Email = model.Email;
                entity.Nombre = model.Nombre;
                usuarioRepository.Update(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index") });
        }

        [HttpGet]
        [Route("usuarios/{id}/delete")]
        public ActionResult Delete(String id)
        {
            var entity = usuarioRepository.GetById(id);
            var model = new Models.Usuario()
            {
                Id = entity.Id,
                Email = entity.Email,
                Nombre = entity.Nombre,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Remove(String id)
        {
            try
            {
                var entity = usuarioRepository.GetById(id);
                usuarioRepository.Delete(entity);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            return Json(new { result = true, value = Url.Action("Index") });
        }

        [HttpGet]
        [Route("usuarios/{id}/detail")]
        public ActionResult Detail(String id)
        {
            var entity = usuarioRepository.GetById(id);
            var model = new Models.Usuario()
            {
                Id = entity.Id,
                Email = entity.Email,
                Nombre = entity.Nombre
            };
            return View(model);
        }

        [HttpGet]
        [Route("usuarios/{id}/grupos")]
        public ActionResult Groups(String id)
        {
            var entity = usuarioRepository.GetById(id);
            var model = new Models.Usuario()
            {
                Id = entity.Id,
                Email = entity.Email,
                Nombre = entity.Nombre
            };
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Usuario model, String ReturnUrl)
        {
            Entities.Usuario entity;
            try
            {
                entity = usuarioRepository.GetById(model.Id);

                if (entity == null)
                {
                    entity = usuarioRepository.Usuarios.Where(u => u.Email.Equals(model.Id)).FirstOrDefault();

                    if (entity == null)
                    {
                        throw new Exception("Usuario no registrado");
                    }
                }

                var validate = Security.PasswordHash.ValidatePassword(model.Password, entity.Password);

                if (!validate)
                {
                    throw new Exception("Contraseña incorrecta");
                }
                FormsAuthentication.SetAuthCookie(entity.Id,false);
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
    Request.ApplicationPath.TrimEnd('/') + "/";
            return Json(new { result = true, value = baseUrl });
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        [HttpPost]
        [Route("usuarios")]
        public JsonResult GetUsuarios()
        {
            var data = usuarioRepository.Usuarios.ToArray();

            return Json(data);
        }
        [HttpPost]
        [Route("usuarios/{id}")]
        public JsonResult GetUsuario(String id)
        {
            var data = usuarioRepository.GetById(id);

            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
