using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.ApiControllers
{
    [RouteArea("FilaVirtual", AreaPrefix="filavirtual")]
    public class EstadoAgenteController : Controller
    {
        Data.UnitOfWork unitOfWork;
        Repositories.EstadoAgenteRepository estadoAgenteRepository;

        public EstadoAgenteController()
        {
            unitOfWork = new Data.UnitOfWork();
            estadoAgenteRepository = unitOfWork.EstadoAgenteRepository();
        }

        [HttpPost]
        [Route("mesas/{id}/estado-actual")]
        public JsonResult EstadoActualAgente(String id)
        {
            var data = estadoAgenteRepository.GetEstadoActualAgente(id);

            return Json(data);
        }

        [HttpPost]
        [Route("agentes/{id}/estados/{estadoId}/registrar")]
        public JsonResult Registrar(String id, String estadoId)
        {
            try
            {
                var entity = new Entities.EstadoAgente()
                {
                    AgenteId = id,
                    EstadoId = estadoId,
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.MaxValue
                };

                entity = estadoAgenteRepository.Insert(entity);

                var agenteEntity = unitOfWork.AgenteRepository().GetById(id);
                agenteEntity.EstadoId = estadoId;
                unitOfWork.AgenteRepository().Update(agenteEntity);

                return Json(new { result = true, value = entity });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("agentes/{id}/estados/{estadoId}/motivos/{motivoId}/registrar")]
        public JsonResult RegistrarMotivo(String id, String estadoId, String motivoId)
        {
            try
            {
                var entity = new Entities.EstadoAgente()
                {
                    AgenteId = id,
                    EstadoId = estadoId,
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.MaxValue,
                    MotivoId = motivoId
                };

                entity = estadoAgenteRepository.Insert(entity);

                var agenteEntity = unitOfWork.AgenteRepository().GetById(id);
                agenteEntity.EstadoId = estadoId;
                unitOfWork.AgenteRepository().Update(agenteEntity);

                return Json(new { result = true, value = entity });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }

        [HttpPost]
        [Route("agentes/estados/{id}/finalizar")]
        public JsonResult FinalizarEstado(Int32 id)
        {
            try
            {
                var entity = estadoAgenteRepository.GetById(id);
                entity.FechaFin = DateTime.Now;
                estadoAgenteRepository.Update(entity);
                return Json(new { result = true });
            }
            catch (Exception e)
            {
                return Json(new { result = false, value = e.Message });
            }
        }
    }
}