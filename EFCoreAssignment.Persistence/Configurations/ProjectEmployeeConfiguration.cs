using EFCoreAssignment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreAssignment.Persistence.Configurations;

public class ProjectEmployeeConfiguration : BaseEntityConfiguration<ProjectEmployee>
{
    public override void Configure(EntityTypeBuilder<ProjectEmployee> builder)
    {
        builder.ToTable("ProjectEmployees");
        
        // Composite key
        builder.HasKey(pe => new { pe.ProjectId, pe.EmployeeId });
        
        // Many-to-Many: Project to Employee
        builder.HasOne(pe => pe.Project)
            .WithMany(p => p.ProjectEmployees)
            .HasForeignKey(pe => pe.ProjectId);
            
        builder.HasOne(pe => pe.Employee)
            .WithMany(e => e.ProjectEmployees)
            .HasForeignKey(pe => pe.EmployeeId);
            
        builder.Property(pe => pe.Enable)
            .IsRequired()
            .HasDefaultValue(true);
    }
}