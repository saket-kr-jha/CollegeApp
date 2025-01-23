using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_New.Data.Config
{
    public class UserTypeConfig : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserTypes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(n => n.Description).HasMaxLength(1500).IsRequired(false);
            builder.Property(n => n.Name).IsRequired().HasMaxLength(250);

            builder.HasData(new List<UserType>()
            {
                new UserType
                {
                    Id = 1,
                    Name = "Student",
                    Description = "For Students"
                },
                new UserType
                {
                    Id = 2,
                    Name = "Faculty",
                    Description = "For Faculty"
                },
                new UserType
                {
                    Id = 3,
                    Name = "Supporting Faculty",
                    Description = "For Supporting Faculty"
                },new UserType
                {
                    Id = 4,
                    Name = "Parents",
                    Description = "For Parents"
                }
            });

        }
    }
}
