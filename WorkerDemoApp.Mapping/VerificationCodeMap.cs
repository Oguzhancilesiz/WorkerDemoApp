using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDemoApp.Entity;

namespace WorkerDemoApp.Mapping
{
    public class VerificationCodeMap : BaseMap<VerificationCode>, IEntityTypeConfiguration<VerificationCode>
    {
        public override void Configure(EntityTypeBuilder<VerificationCode> b)
        {
            base.Configure(b);

            b.Property(x => x.Code).IsRequired().HasMaxLength(12);
            b.Property(x => x.ExpiresAtUtc).IsRequired();
            b.Property(x => x.ReminderCount).HasDefaultValue(0);
            b.Property(x => x.FirstReminderUtc);

            b.HasOne(x => x.User)
             .WithMany()
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => new { x.UserId, x.Type, x.Status });
            b.HasIndex(x => x.ExpiresAtUtc);

        }
    }
}
