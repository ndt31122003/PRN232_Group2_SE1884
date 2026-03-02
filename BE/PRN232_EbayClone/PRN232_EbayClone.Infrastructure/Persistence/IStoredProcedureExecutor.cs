using Dapper;
using System.Data;

namespace PRN232_EbayClone.Infrastructure.Persistence;

public interface IStoredProcedureExecutor
{
    Task<IEnumerable<T>> QueryAsync<T>(string procedureName, object? parameters = default);
    Task<T?> QuerySingleAsync<T>(string procedureName, object? parameters = default);
    Task<int> ExecuteAsync(string procedureName, object? parameters = default);
}

public sealed class DapperStoredProcedureExecutor : IStoredProcedureExecutor
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperStoredProcedureExecutor(IDbConnectionFactory connectionFactory)
        => _connectionFactory = connectionFactory;

    public async Task<IEnumerable<T>> QueryAsync<T>(string procedureName, object? parameters = null)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync();
        return await conn.QueryAsync<T>(
            procedureName,
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<T?> QuerySingleAsync<T>(string procedureName, object? parameters = null)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync();
        return await conn.QuerySingleOrDefaultAsync<T>(
            procedureName,
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> ExecuteAsync(string procedureName, object? parameters = null)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync();
        return await conn.ExecuteAsync(
            procedureName,
            parameters,
            commandType: CommandType.StoredProcedure);
    }
}

