using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Contexts;

//public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
//{
//    public DataContext CreateDbContext(string[] args)
//    {
//        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
//        optionsBuilder.UseSqlServer("DefaultConnection");

//        return new DataContext(optionsBuilder.Options);
//    }
//}

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var grandParent = Directory.GetParent(Directory.GetCurrentDirectory());
        if (grandParent == null)
        {
            Console.WriteLine("Could not find the grandparent directory.");
        }
        else
        {
            var parent = Directory.GetParent(grandParent.FullName);
            var config = new ConfigurationBuilder()
                .SetBasePath(parent + @"\Alpha_Mvc")
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new DataContext(optionsBuilder.Options);
        }
        return null!;
    }
}
