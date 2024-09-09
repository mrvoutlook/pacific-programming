using Microsoft.EntityFrameworkCore;
using tech_test.Models;

namespace tech_test.Data;

public class DataDbContext: DbContext
{
    public virtual DbSet<Image> Images { get; set; }

    public DataDbContext(DbContextOptions<DataDbContext> options)
        : base(options)
    {
    }
}
