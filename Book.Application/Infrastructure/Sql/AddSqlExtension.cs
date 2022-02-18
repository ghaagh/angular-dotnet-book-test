using Book.Application.Domain.Helper;
using Book.Application.Domain.Repository;
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
            services.AddScoped<IQueryHelper<Domain.Book>, QueryHelper<Domain.Book>>();
            services.AddScoped<IBookRepository, Repository.SqlBookRepository>();
            services.AddScoped<ISaver, Repository.SqlSaver>();
            services.AddScoped<IAuthorRepository, Repository.SqlAuthorRepository>();

            return services;
        }

    }
}
