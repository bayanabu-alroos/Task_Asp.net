namespace Task_ejabisoft.Models.Entity
{
    public class EmployeeJob : SharedProp
    {
        public virtual Job Job { get; set; }
        public virtual Employee Employee { get; set; }

    }
}
