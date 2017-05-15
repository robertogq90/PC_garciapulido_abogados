using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace PortalCliente.Models
{
    public interface IDataEntity
    {
        [JsonIgnore]
        string Id { get; }
    }

    public class LinqHelper
    {

        public System.Data.Linq.DataContext context { get; set; }

        public static List<string> ValidTypesTocopy =
            new List<string> { "Int32", "Int64", "Decimal", "DateTime", "Float", "Double", "String", "Boolean", "short", "Int16", "Guid",
                "Nullable`1" };

        public LinqHelper(System.Data.Linq.DataContext context)
        {
            this.context = context;
        }

        public Type GetPrimaryKeyType<T>() where T : class
        {
            try
            {


                // get the table by the type passed in
                var table = this.context.GetTable<T>();

                // get the metamodel mappings (database to
                // domain objects)
                MetaModel modelMapping = table.Context.Mapping;

                // get the data members for this type                   
                ReadOnlyCollection<MetaDataMember> dataMembers
                = modelMapping.GetMetaType(typeof(T))
                .DataMembers;

                // find the primary key and return its type
                return (dataMembers.Single<MetaDataMember>(m =>
                m.IsPrimaryKey)).Type;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetPrimaryKeyName<T>() where T : class
        {
            try
            {

                // get the table by the type passed in
                var table = context.GetTable<T>();

                // get the metamodel mappings (database to
                // domain objects)
                MetaModel modelMapping = table.Context.Mapping;

                // get the data members for this type
                ReadOnlyCollection<MetaDataMember> dataMembers
                = modelMapping.GetMetaType(typeof(T))
                .DataMembers;

                // find the primary key field and return its
                // name
                return (dataMembers.Single<MetaDataMember>(m
                => m.IsPrimaryKey)).Name;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public T SelectByPK<T>(string id) where T : class
        {
            try
            {
                var table = context.GetTable<T>();
                MetaModel modelMap = context.Mapping;

                //Obtieme proriedades de la entidad
                ReadOnlyCollection<MetaDataMember> dataMembers = modelMap.GetMetaType(typeof(T)).DataMembers;

                string PrimaryKeyName = (dataMembers.FirstOrDefault<MetaDataMember>(m => m.IsPrimaryKey)).Name;

                return table.FirstOrDefault<T>(delegate (T t)
                {
                    String memberId = t.GetType().GetProperty(PrimaryKeyName).GetValue(t, null).ToString();
                    return memberId.ToString() == id.ToString();
                });


            }
            catch (Exception)
            {
                throw;
            }
        }

        public T InsertOrUpdate<T>(T item, bool submit = false) where T : class, IDataEntity
        {
            try
            {
                bool isNew = (item.Id == "0" || string.IsNullOrEmpty(item.Id));

                if (isNew)
                {
                    Type type = typeof(T);
                    var table = context.GetTable(type);
                    table.InsertOnSubmit(item);
                }
                else
                {
                    Type type = typeof(T);
                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
                    var table = context.Mapping.GetTable(type);
                    var members = table.RowType.DataMembers;
                    var key = members.Where(x => x.IsPrimaryKey == true).FirstOrDefault();

                    string predicate = "";
                    if (key != null)
                    {
                        predicate = (key.Name != null ? string.Format("{0}=@0", key.Name) : "");
                    }

                    var updated = SelectByPK<T>(item.Id);

                    foreach (PropertyDescriptor currentProp in properties)
                    {
                        if (currentProp.Attributes[typeof(System.Data.Linq.Mapping.ColumnAttribute)] != null)
                        {
                            object val = currentProp.GetValue(item);
                            currentProp.SetValue(updated, val);
                        }
                    }
                }

                if (submit) context.SubmitChanges();
                return item;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Remove<T>(T item, bool submit = false) where T : class
        {
            try
            {
                Type tType = item.GetType();
                Object newObj = Activator.CreateInstance(tType, new object[0]);

                PropertyDescriptorCollection originalProps = TypeDescriptor.GetProperties(item);

                foreach (PropertyDescriptor currentProp in originalProps)
                {
                    if (currentProp.Attributes[typeof(System.Data.Linq.Mapping.ColumnAttribute)] != null)
                    {
                        object val = currentProp.GetValue(item);
                        currentProp.SetValue(newObj, val);
                    }
                }

                var table = context.GetTable<T>();
                table.Attach((T)newObj, true);
                table.DeleteOnSubmit((T)newObj);

                if (submit) context.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}