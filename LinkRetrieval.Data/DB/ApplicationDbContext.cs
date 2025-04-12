using LinkRetrieval.Data.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace LinkRetrieval.Data.DB
{
  public partial class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext()
    {

    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
      try
      {
        Database.EnsureCreated();
      }
      catch (Exception ex)
      {
        File.WriteAllText(@"C:\home\site\wwwroot\Logs\ApplicationDbContext.txt", ex.ToString());
      }
    }

    public virtual DbSet<Search> Searches { get; set; }
    public virtual DbSet<MatchResult> MatchResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=LinkRetrieval;Trusted_Connection=True;";
        if (string.IsNullOrEmpty(connectionString))
        {
          throw new InvalidOperationException("The connection string is not configured.");
        }
        optionsBuilder.UseSqlServer(connectionString);
      }
      optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      OnModelCreatingPartial(modelBuilder);

      modelBuilder.Entity<MatchResult>()
        .HasOne(m => m.Search)
        .WithMany(s => s.MatchResults)
        .HasForeignKey(m => m.SearchId)
        .OnDelete(DeleteBehavior.Cascade);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }
}
