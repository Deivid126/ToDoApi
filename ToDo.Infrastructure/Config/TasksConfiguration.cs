using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Entities;

namespace ToDo.Infrastructure.Config
{
    public class TasksConfiguration : IEntityTypeConfiguration<Tasks>
    {
        public void Configure(EntityTypeBuilder<Tasks> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(x => x.CreateDate)
                .IsRequired();

            builder.Property(x => x.UpdateDate)
                .IsRequired();

            builder.Property(x => x.DeleteDate)
                .IsRequired(false);

            builder.Property(x => x.IdUser)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(d => d.tasks)
                .HasForeignKey(x => x.IdUser);
        }
    }
}