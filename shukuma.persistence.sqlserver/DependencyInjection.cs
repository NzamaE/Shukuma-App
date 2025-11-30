using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace shukuma.persistence.sqlserver
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<UserDbContext>(options
                => options.UseSqlServer(connectionString));
        }
    }
}
