using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFALibrary.Domain;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Linq.Expressions;

namespace SFALibrary.Common
{
    public class DatabaseAccess
    {
        private JavaScriptSerializer javaScriptSerializer;
        public DatabaseAccess()
        {
            javaScriptSerializer = new JavaScriptSerializer();
        }
        public Object deserializeToDomain(string fileName, string dataObject)
        {
            Object obj = new Object();
            switch (fileName)
            {
                case "Accounts":
                    obj = (Object)javaScriptSerializer.Deserialize<Account>(dataObject);
                    break;
                default:
                    break;
            }

            return obj;
        }
    }
}