using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
