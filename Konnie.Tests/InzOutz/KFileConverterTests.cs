using System.Collections.Generic;
using System.Linq;
using Konnie.InzOutz;
using Konnie.Model.File;
using Konnie.Model.Tasks.SubTasks;
using Konnie.Runner.Logging;
using NUnit.Framework;

namespace Konnie.Tests.InzOutz
{
	[TestFixture]
	public class KFileConverterTests
	{
		[Test]
		public void SerialisesToTextAndBack()
		{
			var kTaskOne = new KTask
			{
				Name = "taskOne",
				SubTasksToRun = new List<string>
				{
					"subTaskOne",
					"subTaskOneOne"
				}
			};

			var kTaskTwo = new KTask
			{
				Name = "taskTwo",
				SubTasksToRun = new List<string>
				{
					"subTaskTwo",
					"subTaskTwoTwo"
				}
			};

			var substitutionTask = new SubstitutionTask
			{
				Name = "subTaskOne"
			};

			var transformTask = new TransformTask
			{
				Name = "subTaskTwo",
				InputFile = "SomeInputFile.xml",
				OutputFile = "OutToThisFile.xml",
				TransformFiles = new List<string>
				{
					"TransformMe.xml", "AndMe.xml"
				}
			};
			var startServiceTask = new StartServiceTask
			{
				Name = "subTaskThree",
				ServiceName = "Gazza"
			};

			var stopServiceTask = new StopServiceTask
			{
				Name = "subTaskFour",
				ServiceName = "diggers"
			};

			var assertLackOfXPathTask = new AssertLackOfXPathTask
			{
				Name = "subTaskFive",
				FilePath = "CheckThisFile.xml",
				XPaths = new List<string>
				{
					"//some/path",
					"//some/other/path/where[@thing = 'value']"
				}
			};
			var assertNoMoreVariablesInFile = new AssertNoMoreVariablesInFileTask
			{
				Name = "subTaskSix",
				FilePath = "NoMoreVariablesFile.xml"
			};

			var variableSetOne = new KVariableSet
			{
				Name = "variablesOne",
				Variables = new Dictionary<string, string>
				{
					{"var1", "val1"},
					{"var2", "val2"}
				}
			};
			var variableSetTwo = new KVariableSet
			{
				Name = "variablesTwo",
				Variables = new Dictionary<string, string>
				{
					{"var10", "val10"},
					{"var11", "val11"}
				}
			};

			var original = new KFile
			{
				Tasks = new KTasks
				{
					kTaskOne,
					kTaskTwo
				},
				SubTasks = new KSubTasks
				{
					substitutionTask,
					transformTask,
					startServiceTask,
					stopServiceTask,
					assertLackOfXPathTask,
					assertNoMoreVariablesInFile
				},
				VariableSets = new KVariableSets
				{
					variableSetOne,
					variableSetTwo
				}
			};

			var consoleLogger = new Logger();
			consoleLogger.Verbose("Testing 1 2 3.");
			var parser = new KFileConverter(consoleLogger, null);
			var text = parser.Serialize(original);

			var result = parser.DeserializeFromString(text);

			KFileEqualityAsserter.AssertAreEqual(original, result);
			Assert.That(result.SubTasks.First().Logger, Is.EqualTo(consoleLogger));
			Assert.That(result.Logger, Is.EqualTo(consoleLogger));
		}
	}
}