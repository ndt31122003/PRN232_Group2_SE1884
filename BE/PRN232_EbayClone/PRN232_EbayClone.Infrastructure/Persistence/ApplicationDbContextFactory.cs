using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PRN232_EbayClone.Infrastructure.Persistence;

namespace PRN232_EbayClone.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = "Host=aws-1-ap-northeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.cdvfbycudyhkqowylnhk;Password=EmThinhShizuka";
        optionsBuilder.UseNpgsql(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
