using Dapper;
using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace eCommerceForSale.Data.Repositories
{
    public class SPRepository : ISPRepository
    {
        private readonly ApplicationDbContext context;
        private static string ConnectionString = "";

        public SPRepository(ApplicationDbContext _context)
        {
            context = _context;
            ConnectionString = _context.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void Execute(string SpName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnect = new SqlConnection(ConnectionString))
            {
                sqlConnect.Open();
                sqlConnect.Execute(SpName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> List<T>(string SpName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnect = new SqlConnection(ConnectionString))
            {
                sqlConnect.Open();
                return sqlConnect.Query<T>(SpName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string SpName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnect = new SqlConnection(ConnectionString))
            {
                sqlConnect.Open();
                var result = sqlConnect.QueryMultiple(SpName, param, commandType: CommandType.StoredProcedure);
                var item1 = result.Read<T1>().AsList();
                var item2 = result.Read<T2>().AsList();
                if (item1 != null && item2 != null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
                }
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }

        public T OneRecord<T>(string SpName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnect = new SqlConnection(ConnectionString))
            {
                sqlConnect.Open();
                var value = sqlConnect.Query<T>(SpName, param, commandType: CommandType.StoredProcedure);
                return (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));
            }
        }

        public T Single<T>(string SpName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnect = new SqlConnection(ConnectionString))
            {
                sqlConnect.Open();
                return (T)Convert.ChangeType(sqlConnect.Query<T>(SpName, param, commandType: CommandType.StoredProcedure), typeof(T));
            }
        }
    }
}