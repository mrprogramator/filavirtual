using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SistemaDeGestionDeFilas.Areas.FilaVirtual.ApiControllers
{
    public class AtencionPSQLApiController : ApiController
    {
        private readonly Data.UnitOfWorkPSQL unitOfWork;
        private readonly Repositories.AudioAtencionRepositoryPSQL audioAtencionRepository;

        public AtencionPSQLApiController()
        {
            unitOfWork = new Data.UnitOfWorkPSQL();
            audioAtencionRepository = unitOfWork.AudioAtencionRepository();
        }

        public HttpResponseMessage GetRecording(Int32 id)
        {
            var entity = audioAtencionRepository.GetById(id);

            if (entity == null || entity.Audio == null)
            {
                return null;
            }

            var queryParams = Request.RequestUri.ParseQueryString();

            if (Request.Headers.Range == null || queryParams["stream"] == "false")
            {
                return SendContent(entity.Audio, "audio/ogg");
            }
            else
            {
                return StreamContent(Request.Headers.Range, entity.Audio, "audio/ogg");
            }

        }

        private static HttpResponseMessage StreamContent(RangeHeaderValue rangeHeader, Byte[] bytes, String contentType)
        {
            var range = rangeHeader.Ranges.First();

            var length = bytes.LongLength;

            var from = range.From.GetValueOrDefault(0L);
            var to = range.To.GetValueOrDefault(length - 1);

            if (to >= length)
            {
                to = length - 1;
            }

            var index = from;
            var count = to - from + 1;
            var memory = new MemoryStream(bytes, (int)index, (int)count);

            var result = new HttpResponseMessage(HttpStatusCode.PartialContent)
            {
                Content = new StreamContent(memory)
            };

            result.Headers.Add("Accept-Ranges", "bytes");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            result.Content.Headers.ContentLength = count;
            result.Content.Headers.ContentRange = new ContentRangeHeaderValue(from, to, length);

            return result;
        }

        private static HttpResponseMessage SendContent(Byte[] bytes, String contentType)
        {
            var memory = new MemoryStream(bytes);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(memory)
            };

            result.Content.Headers.ContentLength = bytes.LongLength;
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return result;
        }
    }
}