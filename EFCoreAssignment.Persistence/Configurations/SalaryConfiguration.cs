using EFCoreAssignment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreAssignment.Persistence.Configurations;

public class SalaryConfiguration : BaseEntityConfiguration<Salary>
{
    public override void Configure(EntityTypeBuilder<Salary> builder)
    {
        builder.ToTable("Salaries");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(d => d.Id).ValueGeneratedOnAdd();
        
        builder.Property(s => s.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
            
        // One-to-One: Employee to Salary
        builder.HasOne(s => s.Employee)
            .WithOne(e => e.Salary)
            .HasForeignKey<Salary>(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
