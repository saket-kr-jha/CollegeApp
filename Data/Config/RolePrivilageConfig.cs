using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_New.Data.Config
{
    public class RolePrivilageConfig : IEntityTypeConfiguration<RolePrivilage>
    {
        public void Configure(EntityTypeBuilder<RolePrivilage> builder)
        {
            builder.ToTable("RolePrivilages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(n => n.RolePrivilageName).HasMaxLength(250).IsRequired();
            builder.Property(n => n.Description).IsRequired(false);
            builder.Property(n => n.IsActive).IsRequired();
            builder.Property(n => n.IsDeleted).IsRequired();
            builder.Property(n => n.CreatedDate).IsRequired();

            builder.HasOne(n => n.Role).
                    WithMany(n => n.RolePrivilages).
                    HasForeignKey(n => n.RoleId).
                    HasConstraintName("FK_RolePrivilages_Roles");
        }
    }
}
