using static Task_ejabisoft.Models.Enum.SystemEnums;

namespace Task_ejabisoft.Models.ViewModel
{
    public class JobEditViewModel
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
