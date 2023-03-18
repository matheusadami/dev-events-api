using Microsoft.EntityFrameworkCore;
using DevEventsApi.Entities;
using DevEventsApi.Persistence.Mappings;

namespace DevEventsApi.Persistence;

public class DatabaseContext : DbContext
{
  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
  {
  }

  public DbSet<Event> Events => Set<Event>();
  public DbSet<EventSpeaker> EventSpeakers => Set<EventSpeaker>();

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.ApplyConfiguration<Event>(new EventMap());
    builder.ApplyConfiguration<EventSpeaker>(new EventSpeakerMap());
  }
}