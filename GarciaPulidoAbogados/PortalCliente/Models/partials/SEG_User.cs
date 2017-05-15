using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalCliente.Models
{

    public interface IUser
    {
        [JsonIgnore]
        System.Data.Linq.Binary password { get; set; }
    }

    public partial class SEG_User : IDataEntity, IUser
    {

        #region Properties

        public string Id { get { return this.user_id.ToString(); } }

        public bool isNew { get { return this.user_id == 0; } }

        public string passwordString
        {
            get;
            set;
        }


        #endregion

        #region Methods

        public SEG_User Save(bool submiit = false)
        {
            var helper = new LinqHelper(PortalClienteDataContext.CurrentContext);
            return helper.InsertOrUpdate<SEG_User>(this, submiit);
        }

        public List<SEG_User> Get()
        {
            return PortalClienteDataContext.CurrentContext.SEG_Users.ToList();
        }

        public SEG_User Get(int id)
        {
            return PortalClienteDataContext.CurrentContext.SEG_Users.FirstOrDefault(x => x.user_id == id);
        }

        #endregion

    }
}