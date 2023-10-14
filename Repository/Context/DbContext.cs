using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Context;

public class DbContext : IDisposable
{
    string connectionString = "Data Source=JRDNOTE\\SQLEXPRESS;Initial Catalog=poc-api;Integrated Security=True";
    public IDbConnection Connection { get; }

    public DbContext()
    {
        Connection = new SqlConnection(connectionString);
        Connection.Open();
    }
    public void Dispose() => Connection?.Dispose();
}
