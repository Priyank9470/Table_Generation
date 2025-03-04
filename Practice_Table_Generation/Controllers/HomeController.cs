using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Practice_Table_Generation.Models;

namespace Practice_Table_Generation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new TableGenetation
            {
                SubjectHoursList = new List<SubjectHours>()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult GenerateTotalHours(TableGenetation model)
        {
            if (model.NoOfWorkingDays < 1 || model.NoOfWorkingDays > 7)
            {
                ModelState.AddModelError("NoOfWorkingDays", "Please enter a number between 1 and 7.");
            }

            if (model.NoOfSubjectsPerDay < 1 || model.NoOfSubjectsPerDay > 8)
            {
                ModelState.AddModelError("NoOfSubjectsPerDay", "Please enter a number between 1 and 8.");
            }

            if (model.TotalSubjects <= 0)
            {
                ModelState.AddModelError("TotalSubjects", "Please enter a positive number for total subjects.");
            }

            // Calculate the total hours for the week
            model.TotalHours = model.NoOfWorkingDays * model.NoOfSubjectsPerDay;

            if (ModelState.IsValid)
            {
                // Redirect to the second form to get subject hours
                return RedirectToAction("EnterSubjectHours", new { totalSubjects = model.TotalSubjects });
            }

            //return View("Index", model);
            return RedirectToAction("EnterSubjectHours", new { totalSubjects = model.TotalSubjects });
        }

        [HttpGet]
        public IActionResult EnterSubjectHours(int totalSubjects)
        {
            var model = new TableGenetation
            {
                TotalSubjects = totalSubjects,
                SubjectHoursList = new List<SubjectHours>()
            };

            for (int i = 0; i < totalSubjects; i++)
            {
                model.SubjectHoursList.Add(new SubjectHours());
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult GenerateTimetable(TableGenetation model)
        {
            if (model.SubjectHoursList.Sum(x => x.Hours) != model.TotalHours)
            {
                ModelState.AddModelError("SubjectHoursList", "Total hours entered for subjects must equal total hours for the week.");
            }

            if (ModelState.IsValid)
            {
                // Generate timetable based on entered subject hours
                var timetable = GenerateTimetableFromInput(model);
                return View("GeneratedTimetable", timetable);
            }

            return View("EnterSubjectHours", model.TotalSubjects);
        }

        private string[,] GenerateTimetableFromInput(TableGenetation model)
        {
            var timetable = new string[model.NoOfWorkingDays, model.NoOfSubjectsPerDay];

            // Distribute subjects based on entered hours
            var subjectsWithHours = model.SubjectHoursList.Select(x => new { x.SubjectName, x.Hours }).ToList();
            int subjectIndex = 0;

            // Create the timetable by placing subjects in their respective hours
            for (int day = 0; day < model.NoOfWorkingDays; day++)
            {
                for (int period = 0; period < model.NoOfSubjectsPerDay; period++)
                {
                    timetable[day, period] = subjectsWithHours[subjectIndex % subjectsWithHours.Count].SubjectName;
                    subjectIndex++;
                }
            }

            return timetable;
        }
    }
}
