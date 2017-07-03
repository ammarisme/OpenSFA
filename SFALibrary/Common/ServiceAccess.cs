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
    public class ServiceAccess

    {
        private DBConnection dbConnection;
        private string library;
        public ServiceAccess()
        {
            dbConnection = new DBConnection();
            library = System.Configuration.ConfigurationSettings.AppSettings["LibraryName"].ToString();
        }

        public List<T> SelectAll<T>(string file) where T : class
        {
            DataAccessObject dataAccessObject = new DataAccessObject();
            dbConnection.cmd.CommandText = "SELECT * FROM " + library + "." + file;
            dbConnection.cmd.CommandType = System.Data.CommandType.Text;
            List<T> list = new List<T>();

            try
            {
                using (dbConnection.dr = dbConnection.cmd.ExecuteReader())
                {
                    list = dataAccessObject.ReadCollection<T>(dbConnection.dr);
                }
            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return new List<T>();
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            return list;
        }

        public string Insert(string fileName, Object obj)
        {
            string query = "INSERT INTO " + library + "." + fileName + " ";
            string fields = "( ";
            string values = " VALUES ( ";
            Type firstType = obj.GetType();

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                // populating firld name list
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute primary = null;
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    primary = (attr as PrimaryKeyAttribute) != null ? attr as PrimaryKeyAttribute : primary;
                }

                decimal recCount = 0;
                decimal pkId = 0;
                try
                {
                    dbConnection.cmd.CommandText = "Select COUNT(*) from " + library + "." + fileName;
                    recCount = decimal.Parse(dbConnection.cmd.ExecuteScalar().ToString());
                }
                catch (Exception ex)
                {
                }

                if (primary != null && dbField != null & recCount > 0)
                {
                    try
                    {
                        dbConnection.cmd.CommandText = "select max(" + dbField.FieldName + ")+1 from " + library + "." + fileName;
                        pkId = decimal.Parse(dbConnection.cmd.ExecuteScalar().ToString());
                    }
                    catch (Exception ex)
                    {
                        pkId = 0;
                    }

                }

                string value = (propertyInfo.PropertyType == typeof(decimal)) ? propertyInfo.GetValue(obj, null).ToString() : "'" + propertyInfo.GetValue(obj, null) + "'";
                if (dbField != null & primary == null && value != "0" && value != "" && value != "''")
                {
                    fields = fields == "( " ? (fields + dbField.FieldName) : (fields + "," + dbField.FieldName);
                    values = values == " VALUES ( " ? values + value : values + " , " + value;
                }
                else if (dbField != null & primary != null)
                {
                    fields = fields == "( " ? (fields + dbField.FieldName) : (fields + "," + dbField.FieldName);
                    string primaryKey = (propertyInfo.PropertyType == typeof(decimal)) ? pkId.ToString() : "'" + pkId.ToString() + "'";
                    values = values == " VALUES ( " ? values + primaryKey : values + " , " + primaryKey;
                }
            }

            query = "INSERT INTO " + library + "." + fileName + " " + fields + " ) " + values + " )";

            int success = -1;
            try
            {
                dbConnection.cmd.CommandText = query;
                dbConnection.cmd.CommandType = System.Data.CommandType.Text;
                success = dbConnection.cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return ex.Message + " - Query :" + query;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            return (success == 1) ? "Ok  - Query:" + query : "Error - Query:" + query;
        }

        public string Update(string fileName, Object obj)
        {
            string update = "UPDATE " + library + "." + fileName;
            string setValues = "";
            string whereCondition = "";
            string query = "";
            Type firstType = obj.GetType();

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                // populating firld name list
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute primaryKey = null;
                string value = (propertyInfo.PropertyType == typeof(decimal)) ? propertyInfo.GetValue(obj, null).ToString() : "'" + propertyInfo.GetValue(obj, null) + "'";

                // Go through each attribute of the property
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    primaryKey = (attr as PrimaryKeyAttribute) != null ? attr as PrimaryKeyAttribute : primaryKey;
                }
                if (primaryKey != null && primaryKey.IsPrimary)
                {
                    whereCondition = dbField.FieldName + "=" + value;
                    continue;
                }
                if (dbField != null && value != "" && value != "0" & value != string.Empty && value != "''")
                {
                    string condition = dbField.FieldName + "=" + value;
                    condition = setValues == "" ? condition : " , " + condition;
                    setValues += condition;
                }

            }
            setValues = setValues != "" ? " SET " + setValues : setValues;
            whereCondition = whereCondition != "" ? " WHERE " + whereCondition : whereCondition;
            query = update + setValues + whereCondition;
            dbConnection.cmd.CommandText = query;
            dbConnection.cmd.CommandType = System.Data.CommandType.Text;


            int success = -1;
            try
            {
                success = dbConnection.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return ex.Message + " - Query :" + query;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            return "Ok - Query :" + query;
        }

        public string UpdateAllFields(string fileName, Object obj)
        {
            string update = "UPDATE " + library + "." + fileName;
            string setValues = "";
            string whereCondition = "";
            string query = "";
            Type firstType = obj.GetType();

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                // populating firld name list
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute primaryKey = null;
                string value = (propertyInfo.PropertyType == typeof(decimal)) ? propertyInfo.GetValue(obj, null).ToString() : "'" + propertyInfo.GetValue(obj, null) + "'";

                // Go through each attribute of the property
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    primaryKey = (attr as PrimaryKeyAttribute) != null ? attr as PrimaryKeyAttribute : primaryKey;
                }
                if (primaryKey != null && primaryKey.IsPrimary)
                {
                    whereCondition = dbField.FieldName + "=" + value;
                    continue;
                }
                if (dbField != null)
                {
                    string updateValue = dbField.FieldName + "=" + value;
                    updateValue = setValues == "" ? updateValue : " , " + updateValue;
                    setValues += updateValue;
                }

            }
            setValues = setValues != "" ? " SET " + setValues : setValues;
            whereCondition = whereCondition != "" ? " WHERE " + whereCondition : whereCondition;
            query = update + setValues + whereCondition;
            dbConnection.cmd.CommandText = query;
            dbConnection.cmd.CommandType = System.Data.CommandType.Text;


            int success = -1;
            try
            {
                success = dbConnection.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return ex.Message + " - Query :" + query;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            return "Ok - Query :" + query;
        }

        public List<T> Select<T>(string fileName, Object obj) where T : class
        {
            string select = "SELECT * FROM " + library + "." + fileName;
            string whereCondition = "";
            string query = "";
            Type firstType = obj.GetType();
            List<T> list = new List<T>();
            DataAccessObject dataAccessObject = new DataAccessObject();

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute pkField = null;
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                }
                if (dbField != null && (propertyInfo.GetValue(obj, null).ToString() != "" && propertyInfo.GetValue(obj, null).ToString() != "0"))
                {
                    string value = propertyInfo.PropertyType == typeof(string) ? "'" + propertyInfo.GetValue(obj, null).ToString() + "'" : propertyInfo.GetValue(obj, null).ToString();
                    string condition = dbField.FieldName + "=" + value;
                    condition = whereCondition == "" ? " " + condition : " AND " + condition;
                    whereCondition += condition;
                }
                else if (dbField != null && (propertyInfo.GetValue(obj, null).ToString() != "" && pkField != null))
                {
                    string value = propertyInfo.PropertyType == typeof(string) ? "'" + propertyInfo.GetValue(obj, null).ToString() + "'" : propertyInfo.GetValue(obj, null).ToString();
                    string condition = dbField.FieldName + "=" + value;
                    condition = whereCondition == "" ? " " + condition : " AND " + condition;
                    whereCondition += condition;
                }

            }
            whereCondition = whereCondition != "" ? " WHERE " + whereCondition : whereCondition;
            query = select + whereCondition;

            dbConnection.cmd.CommandText = query;
            dbConnection.cmd.CommandType = System.Data.CommandType.Text;

            try
            {
                using (dbConnection.dr = dbConnection.cmd.ExecuteReader())
                {
                    list = dataAccessObject.ReadCollection<T>(dbConnection.dr);
                }
            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return new List<T>();
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            return list;
        }

        public string Delete(string fileName, Object obj)
        {
            string delete = "DELETE FROM " + library + "." + fileName + " WHERE ";
            string whereCondition = "";
            string query = "";
            Type firstType = obj.GetType();

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute pkField = null;
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    pkField = (attr as PrimaryKeyAttribute) != null ? attr as PrimaryKeyAttribute : pkField;
                }
                if (dbField != null && (propertyInfo.GetValue(obj, null).ToString() != "" && propertyInfo.GetValue(obj, null).ToString() != "0" && propertyInfo.GetValue(obj, null).ToString() != "''") && pkField == null)
                {
                    string value = propertyInfo.PropertyType == typeof(string) ? "'" + propertyInfo.GetValue(obj, null).ToString() + "'" : propertyInfo.GetValue(obj, null).ToString();
                    string condition = dbField.FieldName + "=" + value;
                    condition = whereCondition == "" ? " " + condition : " AND " + condition;
                    whereCondition += condition;
                }
                else if (dbField != null && (propertyInfo.GetValue(obj, null).ToString() != "" && propertyInfo.GetValue(obj, null).ToString() != "0" && propertyInfo.GetValue(obj, null).ToString() != "''") && pkField != null)
                {
                    string value = propertyInfo.PropertyType == typeof(string) ? "'" + propertyInfo.GetValue(obj, null).ToString() + "'" : propertyInfo.GetValue(obj, null).ToString();
                    string condition = dbField.FieldName + "=" + value;
                    condition = whereCondition == "" ? " " + condition : " AND " + condition;
                    whereCondition += condition;
                }

            }
            query = delete + whereCondition;

            int success = -1;
            try
            {
                dbConnection.cmd.CommandText = query;
                dbConnection.cmd.CommandType = System.Data.CommandType.Text;
                success = dbConnection.cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return ex.Message + " - Query :" + query;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            return (success == 1) ? "Ok - Query :" + query : "Error - Query :" + query;
        }

    }   
}