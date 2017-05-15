using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PortalCliente.Models
{
    public partial class PortalClienteDataContext
    {

        public static PortalClienteDataContext CurrentContext
        {
            get
            {
                PortalClienteDataContext context = (PortalClienteDataContext)HttpContext.Current.Items["PortalClienteDataContext"];
                if (context == null)
                {
                    context = new PortalClienteDataContext(ConfigurationManager.ConnectionStrings["PortalClienteConnectionString"].ConnectionString);
                    HttpContext.Current.Items.Add("PortalClienteDataContext", context);
                }

                return context;
            }
        }

    }
}