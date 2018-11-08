using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess;
using System.Reflection;
using Jaylosy.Kernel.Attributes;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
namespace Jaylosy.Kernel.Database
{
    public class OracleDbHelper:IDbHelper
    {
        public string ConnectionString { get; set; }
        public OracleDbHelper(string conStr)
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
            throw new NotImplementedException();
        }

        public int BatchInsert<T>(List<T> modelList, System.Data.IDbTransaction trans)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var table = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
            DataTable dt = new DataTable(table.Name);
            string[] keys = (table.PrimaryKey==null?"":table.PrimaryKey).Split(',');
            List<object[]> objList = new List<object[]>();
            StringBuilder sb_insert = new StringBuilder();
            StringBuilder sb_insert_values = new StringBuilder();
            sb_insert.Append("insert into "+table.Name+" ");
            sb_insert.Append("(");
            for (int i=0;i<properties.Length;i++)
            {
                sb_insert.Append(properties[i].Name);
                sb_insert_values.Append(":"+(i+1).ToString());
                if(i+1==properties.Length)
                {

                }
                else
                {
                    sb_insert.Append(",");
                    sb_insert_values.Append(",");
                }

                var column = new DataColumn(properties[i].Name, properties[i].PropertyType);
                dt.Columns.Add(column);
            }
            sb_insert.Append(") ");
            sb_insert.Append("values(");
            sb_insert.Append(sb_insert_values.ToString());
            sb_insert.Append(")");

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

            OracleParameter[] parameters = new OracleParameter[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var value = dt.AsEnumerable().Select(a => a[i]).ToArray();

                parameters[i] = new OracleParameter();
                if (dt.Columns[i].DataType == typeof(System.Int32))
                {
                    parameters[i].OracleDbType = OracleDbType.Int32;
                }
                if (dt.Columns[i].DataType == typeof(System.String))
                {
                    parameters[i].OracleDbType = OracleDbType.NVarchar2;
                }
                if (dt.Columns[i].DataType == typeof(System.Int64))
                {
                    parameters[i].OracleDbType = OracleDbType.Int64;
                }
                if (dt.Columns[i].DataType == typeof(System.DateTime))
                {
                    parameters[i].OracleDbType = OracleDbType.Date;
                }
                if (dt.Columns[i].DataType == typeof(System.Boolean))
                {
                    parameters[i].OracleDbType = OracleDbType.Boolean;
                }
                parameters[i].Value = value;
            }

            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.Transaction = (OracleTransaction)trans;
                    cmd.CommandText = sb_insert.ToString();
                    cmd.ArrayBindCount = dt.Rows.Count;
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
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

        public int BatchUpdate<T>(List<T> modelList, System.Data.IDbTransaction trans)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var table = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
            DataTable dt = new DataTable(table.Name);
            string[] keys = (table.PrimaryKey == null ? "" : table.PrimaryKey).Split(',');
            List<object[]> objList = new List<object[]>();
            StringBuilder sb_update = new StringBuilder();
            StringBuilder sb_update_where = new StringBuilder();
            sb_update.Append("update " + table.Name + " ");
            sb_update.Append("Set ");
            for (int i = 0; i < properties.Length; i++)
            {
                if (keys.Contains(properties[i].Name))
                {
                    sb_update_where.AppendFormat("{0}=:{1}", properties[i].Name, (i + 1).ToString());
                }
                else
                {
                    sb_update.AppendFormat("{0}=:{1}", properties[i].Name, (i + 1).ToString());
                }
                if (i + 1 == properties.Length)
                {

                }
                else
                {
                    if (keys.Contains(properties[i].Name))
                    { }
                    else
                    {
                        sb_update.Append(",");
                    }
                    if (sb_update_where.Length > 0)
                    {
                        sb_update_where.Append(" And ");
                    }
                }

                var column = new DataColumn(properties[i].Name, properties[i].PropertyType);
                dt.Columns.Add(column);
            }

            if (sb_update.ToString().LastIndexOf(',') >= 0)
            {
                sb_update = new StringBuilder(sb_update.ToString().Substring(0, sb_update.ToString().LastIndexOf(',')));
            }

            if (sb_update_where.Length > 0)
            {
                int pos = sb_update_where.ToString().LastIndexOf("And");
                if (pos < 0)
                {
                    sb_update.AppendFormat(" Where {0}", sb_update_where.ToString());
                }
                else
                {
                    sb_update.AppendFormat(" Where {0}", sb_update_where.ToString().Substring(0, pos));
                }
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

            OracleParameter[] parameters = new OracleParameter[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var value = dt.AsEnumerable().Select(a => a[i]).ToArray();

                parameters[i] = new OracleParameter();
                if (dt.Columns[i].DataType == typeof(System.Int32))
                {
                    parameters[i].OracleDbType = OracleDbType.Int32;
                }
                if (dt.Columns[i].DataType == typeof(System.String))
                {
                    parameters[i].OracleDbType = OracleDbType.NVarchar2;
                }
                if (dt.Columns[i].DataType == typeof(System.Int64))
                {
                    parameters[i].OracleDbType = OracleDbType.Int64;
                }
                if (dt.Columns[i].DataType == typeof(System.DateTime))
                {
                    parameters[i].OracleDbType = OracleDbType.Date;
                }
                if (dt.Columns[i].DataType == typeof(System.Boolean))
                {
                    parameters[i].OracleDbType = OracleDbType.Boolean;
                }
                parameters[i].Value = value;
            }

            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.Transaction = (OracleTransaction)trans;
                    cmd.CommandText = sb_update.ToString();
                    cmd.ArrayBindCount = dt.Rows.Count;
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
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

        public int BatchDelete<T>(List<T> modelList, System.Data.IDbTransaction trans)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var table = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
            DataTable dt = new DataTable(table.Name);
            string[] keys = (table.PrimaryKey == null ? "" : table.PrimaryKey).Split(',');
            List<object[]> objList = new List<object[]>();
            StringBuilder sb_delete = new StringBuilder();
            StringBuilder sb_delete_where = new StringBuilder();
            sb_delete.Append("delete from " + table.Name + " ");
            int j = 0;
            for (int i = 0; i < properties.Length; i++)
            {
                if (!keys.Contains(properties[i].Name))
                {
                    continue;
                }
                sb_delete_where.AppendFormat("{0}=:{1}", properties[i].Name, (j + 1).ToString());
                if (i + 1 == properties.Length)
                {

                }
                else
                {
                    if (sb_delete_where.Length > 0)
                    {
                        sb_delete_where.Append(" And ");
                    }
                }
                j++;
                var column = new DataColumn(properties[i].Name, properties[i].PropertyType);
                dt.Columns.Add(column);
            }

            if (sb_delete.ToString().LastIndexOf(',') >= 0)
            {
                sb_delete = new StringBuilder(sb_delete.ToString().Substring(0, sb_delete.ToString().LastIndexOf(',')));
            }
            if (sb_delete_where.Length > 0)
            {
                int pos = sb_delete_where.ToString().LastIndexOf("And");
                if (pos < 0)
                {
                    sb_delete.AppendFormat(" Where {0}", sb_delete_where.ToString());
                }
                else
                {
                    sb_delete.AppendFormat(" Where {0}", sb_delete_where.ToString().Substring(0, pos));
                }
            }

            foreach (var model in modelList)
            {
                DataRow row = dt.NewRow();
                foreach (var property in properties)
                {
                    if (!keys.Contains(property.Name))
                    {
                        continue;
                    }
                    var value = property.GetValue(model, null);
                    row[property.Name] = value;
                }
                dt.Rows.Add(row);
            }

            OracleParameter[] parameters = new OracleParameter[dt.Columns.Count];
            j = 0;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var value = dt.AsEnumerable().Select(a => a[i]).ToArray();

                parameters[i] = new OracleParameter();
                if (dt.Columns[i].DataType == typeof(System.Int32))
                {
                    parameters[i].OracleDbType = OracleDbType.Int32;
                }
                if (dt.Columns[i].DataType == typeof(System.String))
                {
                    parameters[i].OracleDbType = OracleDbType.NVarchar2;
                }
                if (dt.Columns[i].DataType == typeof(System.Int64))
                {
                    parameters[i].OracleDbType = OracleDbType.Int64;
                }
                if (dt.Columns[i].DataType == typeof(System.DateTime))
                {
                    parameters[i].OracleDbType = OracleDbType.Date;
                }
                if (dt.Columns[i].DataType == typeof(System.Boolean))
                {
                    parameters[i].OracleDbType = OracleDbType.Boolean;
                }
                parameters[i].Value = value;
            }

            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.Transaction = (OracleTransaction)trans;
                    cmd.CommandText = sb_delete.ToString();
                    cmd.ArrayBindCount = dt.Rows.Count;
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
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
    }
}
