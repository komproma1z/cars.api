using Microsoft.EntityFrameworkCore;
using Cars.API.Models;


namespace Cars.API.Data {
    
    public class DataContext : DbContext {

        public DataContext(DbContextOptions<DataContext> options) : base (options) {

        }

        public DbSet<Car> Cars {get; set;}
        public DbSet<LogMetadata> Logs {get; set;}
    }
}