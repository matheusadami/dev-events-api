using Microsoft.EntityFrameworkCore;
using DevEventsApi.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevEventsApi.Persistence.Mappings;

public class EventSpeakerMap : IEntityTypeConfiguration<EventSpeaker>
{
  public void Configure(EntityTypeBuilder<EventSpeaker> builder)
  {
    builder.HasKey(e => e.Uid);
  }
}