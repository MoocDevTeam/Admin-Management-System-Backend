using Microsoft.EntityFrameworkCore.Design;
using Mooc.Model.DBContext;

public class MoocDbContextFactory : IDesignTimeDbContextFactory<MoocDBContext>
{
    public MoocDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MoocDBContext>();
        var connectionString = "Data Source=../MoocWebApi/AppData/moocdb.db"; 
        optionsBuilder.UseSqlite(connectionString);
        return new MoocDBContext(optionsBuilder.Options);
    }
}
