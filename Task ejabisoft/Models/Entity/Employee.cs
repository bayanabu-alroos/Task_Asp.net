﻿using static Task_ejabisoft.Models.Enum.SystemEnums;

namespace Task_ejabisoft.Models.Entity
{
    public class Employee : SharedProp
    {
        public string Name { get; set; }
        public DateOnly Birthdate { get; set; }
        public string Phone { get; set; }
        public DateOnly Employment_Date { get; set; }
        public string Personal_Photo { get; set; }
        public bool Under_Probation { get; set; }
        public Governorate Governorate { get; set; }
        public virtual List<EmployeeJob> EmployeeJobs { get; set; }

    }
}
