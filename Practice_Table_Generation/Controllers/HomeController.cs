using System.Diagnostics;
using System.Xml;
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
            return RedirectToAction("EnterSubjectHours", model);
        }

        [HttpGet]
        public IActionResult EnterSubjectHours(TableGenetation model)
        {
            model.SubjectHoursList = new List<SubjectHours>();
            for (int i = 0; i < model.TotalSubjects; i++)
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
                return RedirectToAction("EnterSubjectHours", model);
            }

            if (ModelState.IsValid)
            {
                var timetable = GenerateTimetableFromInput(model);
                return RedirectToAction("Timetable", timetable);
            }

            return View("EnterSubjectHours", model.TotalSubjects);
        }

        public IActionResult Timetable(string[,] timetable)
        {
            return View(timetable);
        }

        private string[,] GenerateTimetableFromInput(TableGenetation model)
        {
            var timetable = new string[(int)model.WorkingDays, (int)model.SubjectsPerDay];

            var subjectsWithHours = model.SubjectHoursList.Select(x => new { x.SubjectName, x.Hours }).ToList();
            List<string> subjectList = new List<string>();

            foreach (var subject in subjectsWithHours)
            {
                for (int i = 0; i < subject.Hours; i++)
                {
                    subjectList.Add(subject.SubjectName);
                }
            }

            Random rand = new Random();
            subjectList = subjectList.OrderBy(x => rand.Next()).ToList();


            int subjectIndex = 0;
            for (int day = 0; day < model.WorkingDays; day++)
            {
                for (int period = 0; period < model.SubjectsPerDay; period++)
                {
                    timetable[day, period] = subjectList[subjectIndex];
                    subjectIndex++;
                }
            }

            return timetable;
        }
    }
}
