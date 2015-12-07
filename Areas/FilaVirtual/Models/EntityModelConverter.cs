using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.Models
{
    public class EntityModelConverter
    {
        public static Models.Punto LoadModel(Entities.Punto entity)
        {
            var model = new Models.Punto();
            model.Id = entity.Id;
            model.Descripcion = entity.Descripcion;

            return model;
        }

        public static Entities.Punto LoadEntity(Models.Punto model)
        {
            var entity = new Entities.Punto();
            entity.Id = model.Id;
            entity.Descripcion = model.Descripcion;

            return entity;
        }

        public static Models.Ticketera LoadModel(Entities.Ticketera entity)
        {
            var model = new Models.Ticketera();
            model.Id = entity.Id;
            model.Descripcion = entity.Descripcion;

            model.PuntoId = entity.Punto.Id;
            model.PuntoDescripcion = entity.Punto.Descripcion;
            return model;
        }

        public static Entities.Ticketera LoadEntity(Models.Ticketera model)
        {
            var entity = new Entities.Ticketera();

            entity.Id = model.Id;
            entity.Descripcion = model.Descripcion;

            entity.PuntoId = model.PuntoId;

            return entity;
        }

        public static Models.ConfTicketera LoadModel(Entities.ConfTicketera entity)
        {
            var model = new Models.ConfTicketera();
            model.Id = entity.Id;
            model.Descripcion = entity.Descripcion;
            model.TicketeraDescripcion = entity.Ticketera.Descripcion;
            model.TipoId = entity.Tipo.Id;
            model.TipoDescripcion = entity.Tipo.Valor;
            model.Imagen = entity.Imagen;

            if (entity.Padre != null)
            {
                model.PadreId = entity.Padre.Id;
                model.PadreDescripcion = entity.Padre.Descripcion;
            }
            
            return model;
        }
    }
}