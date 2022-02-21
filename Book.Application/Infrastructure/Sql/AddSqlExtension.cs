using Book.Application.Domain;
using Book.Application.Domain.ChangeHistory;
using Book.Application.Domain.Helper;
using Book.Application.Domain.Repository;
using Book.Application.Infrastructure.Sql.ChangeHistory;
using Microsoft.EntityFrameworkCore;

namespace Book.Application.Infrastructure.Sql
{
    public static class AddSqlExtension
    {
        public static IServiceCollection AddSqlDependencies(this IServiceCollection services, string connectionString)
        {


            services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IContextChangeHandler>(options =>
            {
                var context = options.GetRequiredService<Context>();
                var sqlContextChangeHandler = new SqlContextChangeHandler(context);
                context.ChangeTracker.StateChanged += sqlContextChangeHandler.OnSaveFinished;
                context.SavingChanges += sqlContextChangeHandler.OnSaving;
                return sqlContextChangeHandler;
            });

            services.AddScoped<IHistoryHandler>(options =>
            {
                IContextChangeHandler contextChangeHandler = options.GetRequiredService<IContextChangeHandler>();
                var historyHandler = new BookHistoryHandler(connectionString);
                contextChangeHandler.HistoryChanged += historyHandler.OnChangeExtracted;
                return historyHandler;

            });
            services.AddScoped<IQueryHelper<Domain.Book>, QueryHelper<Domain.Book>>();
            services.AddScoped<IQueryHelper<Author>, QueryHelper<Author>>();
            services.AddScoped<IQueryHelper<BookHistory>, QueryHelper<BookHistory>>();
            services.AddScoped<IBookRepository, Repository.SqlBookRepository>();


            services.AddScoped<ISaver, Repository.SqlSaver>();
            services.AddScoped<IBookHistoryRepository, Repository.SqlBookHistoryRepository>();
            services.AddScoped<IAuthorRepository, Repository.SqlAuthorRepository>();

            return services;
        }

    }
}
