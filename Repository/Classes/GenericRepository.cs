using Dapper;
using MVCDemo.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MVCDemo.Repository.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _tableName;

        public GenericRepository(string tableName)
        {
            _tableName = tableName;
        }

        private SqlConnection DBConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["mvcDemo"].ConnectionString);
        }
        private IDbConnection CreateConnection()
        {
            SqlConnection conn = DBConnection();
            conn.Open();
            return conn;
        }

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private string GenerateInsertQuery()
        {
            StringBuilder insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

            insertQuery.Append("(");

            List<string> properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        private string GenerateUpdateQuery()
        {
            StringBuilder updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            List<string> properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("ID"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE ID=@ID");

            return updateQuery.ToString();
        }

        public List<T> GetAll()
        {
            try
            {
                using (IDbConnection connection = CreateConnection())
                {
                    return connection.Query<T>($"SELECT * FROM {_tableName}").ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public T GetById(Guid id)
        {
            try
            {
                using (IDbConnection connection = CreateConnection())
                {
                    return connection.QueryFirstOrDefault<T>($"SELECT * FROM {_tableName} WHERE ID=@ID", new { ID = id });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                using (IDbConnection connection = CreateConnection())
                {
                    int count = connection.Execute($"DELETE FROM {_tableName} WHERE ID=@ID", new { ID = id });
                    return count > 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Insert(T t)
        {
            string insertQuery = GenerateInsertQuery();

            try
            {
                using (IDbConnection connection = CreateConnection())
                {
                    connection.Execute(insertQuery, t);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Update(T t)
        {
            string updateQuery = GenerateUpdateQuery();

            try
            {
                using (IDbConnection connection = CreateConnection())
                {
                    connection.Execute(updateQuery, t);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}