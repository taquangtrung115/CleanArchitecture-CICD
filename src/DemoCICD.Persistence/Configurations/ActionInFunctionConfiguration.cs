

using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Configurations;

internal sealed class ActionInFunctionConfiguration : IEntityTypeConfiguration<ActionInFunction>
{
    public void Configure(EntityTypeBuilder<ActionInFunction> builder)
    {
        builder.ToTable(TableNames.ActionInFunctions);

        builder.HasKey(x => new { x.ActionId, x.FunctionId });

        builder.HasOne(af => af.Action)
            .WithMany(a => a.ActionInFunctions)
            .HasForeignKey(af => af.ActionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(af => af.Function)
            .WithMany(f => f.ActionInFunctions)
            .HasForeignKey(af => af.FunctionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
