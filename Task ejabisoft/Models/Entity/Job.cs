using System.Reflection;
using static Task_ejabisoft.Models.Enum.SystemEnums;

namespace Task_ejabisoft.Models.Entity
{
    public class Job :SharedProp
    {
        public string  Name { get; set; }
        public Category Category { get; set; }
        public virtual List<EmployeeJob> EmployeeJobs { get; set; }

    }
}
