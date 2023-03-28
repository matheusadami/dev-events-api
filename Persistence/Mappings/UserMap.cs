using DevEventsApi.Entities;
using DevEventsApi.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevEventsApi.Persistence.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.HasKey(e => e.Uid);
    builder.Property(e => e.Name).HasMaxLength(255);
    builder.Property(e => e.Username).HasMaxLength(25);
    builder.Property(e => e.Role).HasDefaultValue(EUserRoles.Employee);
    builder.HasMany(e => e.Events).WithOne(e => e.User).HasForeignKey(e => e.UserUid);
  }
}