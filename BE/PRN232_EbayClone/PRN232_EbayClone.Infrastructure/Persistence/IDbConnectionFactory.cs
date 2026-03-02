using System.Data;
using Npgsql;

namespace PRN232_EbayClone.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public async Task<IDbConnection> CreateConnectionAsync()
    {
        NpgsqlConnection connection = new(connectionString);

        await connection.OpenAsync();

        return connection;
    }
}
