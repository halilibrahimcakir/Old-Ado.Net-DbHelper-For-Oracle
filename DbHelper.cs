using Insurance.Data.Entity;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Data
{
    internal static class DbHelper
    {
        static string conn = "";
        static OracleTransaction transaction = null;
        public static bool ExecuteAdd(string procedureName, params object[] parameters)
        {
            try
            {
                List<OracleParameter> filters = new List<OracleParameter>();
                using (OracleConnection con = new OracleConnection(conn))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = procedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        transaction = con.BeginTransaction();
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            IList<PropertyInfo> props = new List<PropertyInfo>(parameters[i].GetType().GetProperties());

                            foreach (PropertyInfo prop in props)
                            {
                                string propName = prop.Name;
                                var propValue = prop.GetValue(parameters[i], null);
                                if (propName != "ID")
                                {
                                    filters.Add(new OracleParameter(propName, propValue));
                                }
                            }
                        }
                        foreach (var item in filters)
                        {
                            cmd.Parameters.Add(item.ParameterName, item.Value);
                        }
                        cmd.ExecuteNonQuery();

                        transaction.Commit();

                        con.Close();

                        transaction.Dispose();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error on save changes", ex);
            }
        }
        public static void ExecuteDelete(string procedureName, int id)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conn))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = procedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("ID", id);
                        con.Open();
                        transaction = con.BeginTransaction();
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error on save changes", ex);
            }
        }
        public static DataTable ExecuteGetAll(string procedureName)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conn))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        DataTable dt = new DataTable();

                        OracleParameter oracleParameter = new OracleParameter();
                        oracleParameter.ParameterName = "RESULTS";
                        oracleParameter.Direction = ParameterDirection.Output;
                        oracleParameter.OracleDbType = OracleDbType.RefCursor;

                        cmd.Connection = con;
                        cmd.CommandText = procedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(oracleParameter);
                        con.Open();
                        transaction = con.BeginTransaction();
                        OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                        adapter.Fill(dt);
                        transaction.Commit();
                        con.Close();

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error on save changes", ex);
            }
        }
        public static DataTable ExecuteGetById(string procedureName, int id)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(conn))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        DataTable dt = new DataTable();

                        OracleParameter oracleParameter = new OracleParameter();
                        oracleParameter.ParameterName = "RESULTS";
                        oracleParameter.Direction = ParameterDirection.Output;
                        oracleParameter.OracleDbType = OracleDbType.RefCursor;

                        cmd.Connection = con;
                        cmd.CommandText = procedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("ID", id);
                        cmd.Parameters.Add(oracleParameter);

                        con.Open();
                        transaction = con.BeginTransaction();
                        OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                        adapter.Fill(dt);
                        transaction.Commit();
                        con.Close();

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error on save changes", ex);
            }
        }
        public static bool ExecudeUpdate(string procedureName, params object[] parameters)
        {
            try
            {
                List<OracleParameter> filters = new List<OracleParameter>();
                using (OracleConnection con = new OracleConnection(conn))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = procedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        transaction = con.BeginTransaction();
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            IList<PropertyInfo> props = new List<PropertyInfo>(parameters[i].GetType().GetProperties());

                            foreach (PropertyInfo prop in props)
                            {
                                string propName = prop.Name;
                                var propValue = prop.GetValue(parameters[i], null);

                                filters.Add(new OracleParameter(propName, propValue));
                            }
                        }
                        foreach (var item in filters)
                        {
                            cmd.Parameters.Add(item.ParameterName, item.Value);
                        }
                        cmd.ExecuteNonQuery();

                        transaction.Commit();

                        con.Close();

                        transaction.Dispose();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error on save changes", ex);
            }
        }
    }
}


