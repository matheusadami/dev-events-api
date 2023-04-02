using Microsoft.EntityFrameworkCore;
using DevEventsApi.Entities;
using DevEventsApi.Persistence.Mappings;
using DevEventsApi.Persistence.Interceptors;

namespace DevEventsApi.Persistence;

public class DatabaseContext : DbContext
{
  private IServiceProvider ServiceProvider;
  private IConfiguration Configuration { get; set; }

  public DatabaseContext(DbContextOptions options, IConfiguration configuration, IServiceProvider serviceProvider) : base(options)
    => (ServiceProvider, Configuration) = (serviceProvider, configuration);

  public DbSet<Event> Events => Set<Event>();
  public DbSet<EventSpeaker> EventSpeakers => Set<EventSpeaker>();

  public DbSet<User> Users => Set<User>();

  public DbSet<Auditing> Auditings => Set<Auditing>();

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite(Configuration.GetConnectionString("Database"));
    optionsBuilder.AddInterceptors(ServiceProvider.GetRequiredService<AuditableEntitiesInterceptor>());

    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.ApplyConfiguration<Event>(new EventMap());
    builder.ApplyConfiguration<EventSpeaker>(new EventSpeakerMap());
    builder.ApplyConfiguration<User>(new UserMap());
    builder.ApplyConfiguration<Auditing>(new AuditingMap());
  }
}