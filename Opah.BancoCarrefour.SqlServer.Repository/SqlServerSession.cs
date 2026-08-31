using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opah.BancoCarrefour.SqlServer.Repository;

public sealed class SqlServerSession
{
    #region atributos

    private readonly SqlServerContext _context;

    #endregion

    #region construtores

    public SqlServerSession(SqlServerContext context) => _context = context;

    #endregion

    #region propriedades

    public IDbConnection Connection => _context.DbConnection;

    #endregion
}
