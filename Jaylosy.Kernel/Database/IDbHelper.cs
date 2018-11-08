using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Jaylosy.Kernel.Database
{
    public interface IDbHelper
    {
        int ExecuteNonQuery(string sql);
        int ExecuteNonQuery(string sql, IDbTransaction trans);
        int ExecuteNonQuery(string sql, DbParameter[] parameters, IDbTransaction trans);
        int ExecuteNonQuery<T>(T model, IDbTransaction trans);
        int BatchInsert<T>(List<T> modelList, IDbTransaction trans);
        int BatchUpdate<T>(List<T> modelList, System.Data.IDbTransaction trans);
        int BatchDelete<T>(List<T> modelList, System.Data.IDbTransaction trans);
    }
}
