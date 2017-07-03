using System;
using System.Collections.Generic;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;
using SFALibrary.Common;
using SFALibrary.Domain;


namespace SFA_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service : IService
    {
        #region Test
        public Response SelectAll(string fileName)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ServiceAccess serviceAccess = new ServiceAccess();
            DatabaseAccess dbAccess = new DatabaseAccess();
            Response response = new Response();
            response.ID = 200;
            List<Object> list = new List<Object>();

            switch (fileName)
            {
                case "Accounts":
                    response.Data = javaScriptSerializer.Serialize(serviceAccess.SelectAll<Account>(fileName));
                    break;
                default:
                    response.Data = "No records found";
                    response.ID = 300;
                    break;
            }
            return response;
        }

        public Response Insert(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ServiceAccess serviceAccess = new ServiceAccess();
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.deserializeToDomain(fileName, dataObject);
            response.Data = javaScriptSerializer.Serialize(serviceAccess.Insert(fileName, obj));
            return response;

        }

        public Response Update(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ServiceAccess serviceAccess = new ServiceAccess();
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.deserializeToDomain(fileName, dataObject);
            response.Data = javaScriptSerializer.Serialize(serviceAccess.Update(fileName, obj));
            return response;
        }

        public Response UpdateAllFields(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ServiceAccess serviceAccess = new ServiceAccess();
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.deserializeToDomain(fileName, dataObject);
            response.Data = javaScriptSerializer.Serialize(serviceAccess.UpdateAllFields(fileName, obj));
            return response;
        }
        public Response Delete(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ServiceAccess serviceAccess = new ServiceAccess();
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.deserializeToDomain(fileName, dataObject);
            response.Data = javaScriptSerializer.Serialize(serviceAccess.Delete(fileName, obj));
            return response;

        }

        public Response Select(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ServiceAccess serviceAccess = new ServiceAccess();
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.deserializeToDomain(fileName, dataObject);
            response.ID = 200;
            List<Object> list = new List<Object>();

            switch (fileName)
            {
                case "Accounts":
                    response.Data = javaScriptSerializer.Serialize(serviceAccess.Select<Account>(fileName, obj));
                    break;
                default:
                    response.Data = "No records found";
                    response.ID = 300;
                    break;
            }
            return response;


        }

        #endregion
    }
}
