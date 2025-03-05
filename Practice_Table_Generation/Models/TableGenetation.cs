using System.ComponentModel.DataAnnotations;

namespace Practice_Table_Generation.Models
{
    public class TableGenetation
    {
        [Required(ErrorMessage = "Please enter working days")]
        [RegularExpression(@"[1-7]", ErrorMessage = "Please enter working days in between 1 to 7")]
        public int? WorkingDays { get; set; }

        [Required(ErrorMessage = "Please enter Subject per day")]
        [RegularExpression(@"[1-9]", ErrorMessage = "Please enter Subject per day in between 1 to 9")]
        public int? SubjectsPerDay { get; set; }
        
        [Required(ErrorMessage = "Please enter total subjects")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Please enter total Subject greater than 0")]
        public int? TotalSubjects { get; set; }
        
        public int? TotalHours { get; set; }

        public List<SubjectHours> SubjectHoursList { get; set; }

        public string[,] TimeTable { get; set; }

    }
    public class SubjectHours
    {
        [Required(ErrorMessage = "Please enter subject name")]
        public string? SubjectName { get; set; }
        
        [Required(ErrorMessage = "Please enter hour for subject")]
        [RegularExpression(@"[1-9]", ErrorMessage = "Minimum value for subject hour is 1")]
        public int? Hours { get; set; }
    }
}
