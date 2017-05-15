using PortalCliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PortalCliente.Controllers.Operation
{
    public class QuestionController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Save(OP_CustomerMessage entity)
        {
            try
            {
                if (entity.isNew)
                    entity.createDate = DateTime.Now;


                var result = entity.Save(true);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}
