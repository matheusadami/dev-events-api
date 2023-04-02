using System.Collections.Generic;
using System.Text.Json;
using DevEventsApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevEventsApi.Persistence.Mappings;

public class AuditingMap : IEntityTypeConfiguration<Auditing>
{
  public void Configure(EntityTypeBuilder<Auditing> builder)
  {
    builder.HasKey(e => e.Uid);
    builder.Property(e => e.TimeStamp).HasDefaultValue(DateTime.UtcNow);
    builder.Property(e => e.Changes).HasConversion(
      value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
      serializedValue => JsonSerializer.Deserialize<Dictionary<string, object?>>(serializedValue, JsonSerializerOptions.Default) ?? new Dictionary<string, object?>()
    );
  }
}