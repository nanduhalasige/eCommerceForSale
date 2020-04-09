using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Data.Repositories.IRepositories
{
    public interface ISPRepository : IDisposable
    {
        T Single<T>(string SpName, DynamicParameters param = null);

        void Execute(string SpName, DynamicParameters param = null);

        T OneRecord<T>(string SpName, DynamicParameters param = null);

        IEnumerable<T> List<T>(string SpName, DynamicParameters param = null);

        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string SpName, DynamicParameters param = null);
    }
}