using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Konnie.Model.File;
using Konnie.Model.Tasks.SubTasks;

namespace Konnie.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var kFile = new KFile
			{
				Tasks = new KTasks
				{
					new KTask
					{
						Name = "TransformAndSubs",
						SubTasksToRun = new List<string>
						{
							"TransformWithReleaseFile",
							"SubstituteVariablesIntoTransformedFile"
						}
					}
				},
				SubTasks = new KSubTasks
				{
					new TransformTask
					{
						Name = "TransformWithReleaseFile",
						InputFile = "Config.xml",
						OutputFile = "TransformOutputFile",
						TransformFiles = new List<string>
						{
							"Config.Release.xml"
						}
					},
					new SubstitutionTask
					{
						Name = "SubstituteVariablesIntoTransformedFile",
						FilePath = "TransformOutputFile",
						SubstitutionVariableSets = new List<string>
						{
							"VariableSetOne"
						}
					}
				},
				VariableSets = new KVariableSets
				{
					new KVariableSet
					{
						Name = "VariableSetOne",
						Variables = new Dictionary<string, string>
						{
							{"VariableOne", "VariableOneValue"},
							{"VariableTwo", "VariableTwoValue"},
							{"VariableThree", "VariableThreeValue"}
						}
					}
				}
			};
			return View(kFile);
		}

		//public ActionResult About()
		//{
		//	ViewBag.Message = "Your application description page.";

		//	return View();
		//}

		//public ActionResult Contact()
		//{
		//	ViewBag.Message = "Your contact page.";

		//	return View();
		//}
	}
}