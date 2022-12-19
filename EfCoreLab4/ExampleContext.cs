using Microsoft.EntityFrameworkCore;

namespace EfCoreLab4;

public sealed class ExampleContext : DbContext
{
    public DbSet<ExampleEntity> Examples { get; set; } = null!;

	public ExampleContext()
	{
	}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=ShindQQ;Database=EfCoreLab4;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
