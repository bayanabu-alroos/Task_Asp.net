using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;
using Task_ejabisoft.Models.Entity;

namespace Task_ejabisoft.Models.EntityConfigurations
{
    public class JobEntityConfigurations : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
