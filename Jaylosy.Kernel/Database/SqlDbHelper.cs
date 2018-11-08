using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jaylosy.Kernel.Attributes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace Jaylosy.Kernel.Database
{
    public class SqlDbHelper : IDbHelper
    {
        public string ConnectionString { get; set; }
        public SqlDbHelper(string conStr)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[conStr].ConnectionString;
        }
        public int ExecuteNonQuery(string sql)
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery(string sql, System.Data.IDbTransaction trans)
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery(string sql, System.Data.Common.DbParameter[] parameters, System.Data.IDbTransaction trans)
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery<T>(T model, System.Data.IDbTransaction trans)
        {
            List<T> modelList = new List<T>();
            modelList.Add(model);
            return BatchInsert(modelList, trans);
        }

        public int BatchInsert<T>(List<T> modelList, System.Data.IDbTransaction trans)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var table = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
            DataTable dt = new DataTable(table.Name);
           
            foreach (var property in properties)
            {
                var column = new DataColumn(property.Name, property.PropertyType);
                dt.Columns.Add(column);
            }

            foreach (var model in modelList)
            {
                DataRow row = dt.NewRow();
                foreach (var property in properties)
                {
                    var value = property.GetValue(model, null);
                    row[property.Name] = value;
                }
                dt.Rows.Add(row);
            }
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, (SqlTransaction)trans);
                    bulkCopy.DestinationTableName = table.Name;
                    bulkCopy.BatchSize = dt.Rows.Count;
                    bulkCopy.WriteToServer(dt);
                    bulkCopy.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return 1;
        }


        public int BatchUpdate<T>(List<T> modelList, IDbTransaction trans)
        {
            throw new NotImplementedException();
        }

        public int BatchDelete<T>(List<T> modelList, IDbTransaction trans)
        {
            throw new NotImplementedException();
        }
    }
}
