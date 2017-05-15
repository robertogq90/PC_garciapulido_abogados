using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalCliente.Models
{
    public partial class OP_CustomerMessage: IDataEntity
    {

        public string Id { get { return this.customerMessage_id.ToString(); } }

        public bool isNew { get { return this.customerMessage_id == 0; } }

        public OP_CustomerMessage Save(bool submit = false)
        {
            var helper = new LinqHelper(PortalClienteDataContext.CurrentContext);
            return helper.InsertOrUpdate<OP_CustomerMessage>(this, submit);
        }

    }
}