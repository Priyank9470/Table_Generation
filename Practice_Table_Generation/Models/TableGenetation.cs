using System.ComponentModel.DataAnnotations;

namespace Practice_Table_Generation.Models
{
    public class TableGenetation
    {
        [Required]
        [RegularExpression(@"[0-9]{1,7}")]
        public int? WorkingDays { get; set; }
		[Required]
		[RegularExpression(@"[0-9]{1,9}")]
		public int? SubjectsPerDay { get; set; }
		[Required]
		public int? TotalSubjects { get; set; }
        public int? TotalHours { get; set; }

        public List<SubjectHours> SubjectHoursList { get; set; }

    }
    public class SubjectHours
    {
        public string SubjectName { get; set; }
        public int Hours { get; set; }
    }
}
