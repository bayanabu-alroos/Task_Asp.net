using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_ejabisoft.Models.Entity;

namespace Task_ejabisoft.Models.EntityConfigurations
{
    public class EmployeeEntityConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Phone).IsUnique();
        }
    }
}
