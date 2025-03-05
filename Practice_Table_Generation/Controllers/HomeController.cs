using System.Diagnostics;
using System.Reflection;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
			if (HttpContext.Session.GetString("TimeTable") != null)
			{
				var timeTable = JsonConvert.DeserializeObject<TableGenetation>(HttpContext.Session.GetString("TimeTable"));
				HttpContext.Session.Clear();
				return View(timeTable);
			}
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
			if (model.SubjectHoursList.Any(x => x.SubjectName == null || x.Hours == null))
			{
				TempData["ValidationMessage"] = $"Please fill the details for subjects and hours";
				return View("EnterSubjectHours", model);
			}
			if (model.SubjectHoursList.Any(x => x.SubjectName.Equals(string.Empty)))
			{
				TempData["ValidationMessage"] = $"Please enter all subject name";
				return View("EnterSubjectHours", model);
			}
			if (model.SubjectHoursList.Any(x => x.Hours <= 0))
			{
				TempData["ValidationMessage"] = $"Please enter hours for all subjects";
				return View("EnterSubjectHours", model);
			}
			if (model.SubjectHoursList.Sum(x => x.Hours) != model.TotalHours)
			{
				TempData["ValidationMessage"] = $"Total hours entered for subjects must equal to total hours: {model.TotalHours} for the week.";
				return View("EnterSubjectHours", model);
			}
			HttpContext.Session.SetString("TimeTableModel", JsonConvert.SerializeObject(model));
			return RedirectToAction("TimeTable", "Home");
		}

		public IActionResult TimeTable()
		{
			TableGenetation timeTableModel = new TableGenetation();
			if (HttpContext.Session.GetString("TimeTableModel") != null)
			{
				timeTableModel = JsonConvert.DeserializeObject<TableGenetation>(HttpContext.Session.GetString("TimeTableModel"));
			}
			if (HttpContext.Session.GetString("TimeTable") != null)
			{
				var existingTimeTable = JsonConvert.DeserializeObject<TableGenetation>(HttpContext.Session.GetString("TimeTable"));
				return View(existingTimeTable);
			}
			if (timeTableModel != null && timeTableModel.SubjectHoursList != null && timeTableModel.SubjectHoursList.Count() > 0)
			{
				var timetable = GenerateTimetableFromInput(timeTableModel);
				HttpContext.Session.SetString("TimeTable", JsonConvert.SerializeObject(timetable));
				return View(timetable);
			}
			TempData["InitialMessage"] = "Enter the hours details first";
			return View("index");
		}

		private TableGenetation GenerateTimetableFromInput(TableGenetation model)
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
			model.TimeTable = timetable;
			return model;
		}
	}
}
