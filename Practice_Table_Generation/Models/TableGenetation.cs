namespace Practice_Table_Generation.Models
{
    public class TableGenetation
    {
        public int NoOfWorkingDays { get; set; } // 1 to 7
        public int NoOfSubjectsPerDay { get; set; } // 1 to 8
        public int TotalSubjects { get; set; } // Positive number
        public int TotalHours { get; set; } // Calculated hours for the week

        // List of subjects and their respective hours
        public List<SubjectHours> SubjectHoursList { get; set; }

    }
    public class SubjectHours
    {
        public string SubjectName { get; set; }
        public int Hours { get; set; }
    }
}
