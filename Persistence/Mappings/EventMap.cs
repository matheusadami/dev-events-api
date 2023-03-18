using Microsoft.EntityFrameworkCore;
using DevEventsApi.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevEventsApi.Persistence.Mappings;

public class EventMap : IEntityTypeConfiguration<Event>
{
  public void Configure(EntityTypeBuilder<Event> builder)
  {
    builder.HasKey(e => e.Uid);
    builder.Property(e => e.Description).HasMaxLength(255);
    builder.HasMany(e => e.Speakers).WithOne(s => s.Event).HasForeignKey(s => s.EventUid);
  }
}