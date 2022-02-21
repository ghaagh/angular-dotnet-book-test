using Book.Application.Domain;
using Book.Application.Domain.ChangeHistory;
using Microsoft.Data.SqlClient;

namespace Book.Application.Infrastructure.Sql.ChangeHistory;
public class BookHistoryHandler : IHistoryHandler
{
    private readonly string _connectionString;

    public BookHistoryHandler(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async void OnChangeExtracted(object sender, OnChangedEventArgument e)
    {
        if (e.Changes.Count == 0)
            return;
        using var connection = new SqlConnection(_connectionString);
        using var command = SqlCommand(e.Changes);
        command.Connection = connection;

        try
        {

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            if (connection.State != System.Data.ConnectionState.Closed)
                await connection.CloseAsync();
        }
        catch (Exception ex)
        {

        }
    }

    private static SqlCommand SqlCommand(List<Change> changes)
    {
        var command = new SqlCommand();
        for (int index = 0; index < changes.Count; index++)
        {
            var eventArgument = changes.ElementAt(index);
            var bookHistory = new BookHistory(eventArgument.Id, eventArgument.Field, eventArgument.OldValue, eventArgument.NewValue);
            command.CommandText += $"INSERT INTO BookHistories Values (@BookId{index},@LogDate{index},@OldValue{index},@CurrentValue{index},@Field{index},@Description{index})\n";
            command.Parameters.AddWithValue($"@BookId{index}", eventArgument.Id);
            command.Parameters.AddWithValue($"@LogDate{index}", bookHistory.LogDate);
            command.Parameters.AddWithValue($"@OldValue{index}", string.IsNullOrEmpty(bookHistory.OldValue) ? DBNull.Value : bookHistory.OldValue);
            command.Parameters.AddWithValue($"@CurrentValue{index}", string.IsNullOrEmpty(bookHistory.CurrentValue) ? DBNull.Value : bookHistory.CurrentValue);
            command.Parameters.AddWithValue($"@Field{index}", bookHistory.Field);
            command.Parameters.AddWithValue($"@Description{index}", bookHistory.Description);
        }
        return command;

    }
}

