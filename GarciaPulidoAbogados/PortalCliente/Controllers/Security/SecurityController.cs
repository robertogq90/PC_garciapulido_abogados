using PortalCliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PortalCliente.Controllers.Security
{
    public class SecurityController : ApiController
    {

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Authenticate(SEG_User entity)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { username = "admin" });
        }

    }
}
