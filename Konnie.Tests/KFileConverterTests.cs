using System.Collections.Generic;
using System.Linq;
using Konnie.Model;
using Konnie.Model.File;
using Konnie.Model.Tasks.SubTasks;
using NUnit.Framework;

namespace Konnie.Tests
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
				Name = "subTaskTwo"
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
				Name = "subTaskFive"
			};
			var assertNoMoreVariablesInFile = new AssertNoMoreVariablesInFile
			{
				Name = "subTaskSix"
			};

			var variableSetOne = new VariableSet
			{
				Name = "variablesOne",
				Variables = new Dictionary<string, string>
				{
					{"var1", "val1"},
					{"var2", "val2"},
				}
			};
			var variableSetTwo = new VariableSet
			{
				Name = "variablesTwo",
				Variables = new Dictionary<string, string>
				{
					{"var10", "val10"},
					{"var11", "val11"},
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

			var parser = new KFileConverter();
			var text = parser.Serialize(original);

			var result = parser.DeserializeFromString(text);


			KFileEqualityAsserter.AssertTasksAreEqual(original.Tasks, result.Tasks);
			KFileEqualityAsserter.AssertSubTasksAreEqual(original.SubTasks, result.SubTasks);
			KFileEqualityAsserter.AssertVariableSetsAreEqual(original.VariableSets, result.VariableSets);
		}
	}

	public class KFileEqualityAsserter
	{
		public static void AssertVariableSetsAreEqual(KVariableSets original, KVariableSets result)
		{
			var count = original.Count;
			Assert.That(count, Is.EqualTo(result.Count));
			foreach (var i in Enumerable.Range(0, count))
			{
				var orig = original[i];
				var res = result[i];
				Assert.That(orig.Name, Is.EqualTo(res.Name));
				Assert.That(orig.Variables.Keys, Is.EqualTo(res.Variables.Keys));
				Assert.That(orig.Variables.Values, Is.EqualTo(res.Variables.Values));
			}
		}

		public static void AssertTasksAreEqual(KTasks original, KTasks result)
		{
			Assert.That(original.Count, Is.EqualTo(result.Count));
			for (var i = 0; i < result.Count; i++)
			{
				var orig = original[i];
				var res = result[i];
				Assert.That(orig.Name, Is.EqualTo(res.Name));
				Assert.That(orig.SubTasksToRun, Is.EqualTo(res.SubTasksToRun));
			}
		}

		public static void AssertSubTasksAreEqual(KSubTasks original, KSubTasks result)
		{
			var count = original.Count;
			foreach (var i in Enumerable.Range(0, count))
			{
				var orig = original[i];
				var res = result[i];
				if (orig is AssertLackOfXPathTask)
				{
					CheckAssertLackOfXPathTask((AssertLackOfXPathTask) orig, (AssertLackOfXPathTask) res);
					continue;
				}
				if (orig is StartServiceTask)
				{
					CheckStartServiceTask((StartServiceTask) orig, (StartServiceTask) res);
					continue;
				}
				if (orig is StopServiceTask)
				{
					CheckStopServiceTask((StopServiceTask) orig, (StopServiceTask) res);
					continue;
				}
				if (orig is SubstitutionTask)
				{
					CheckSubstitutionTask((SubstitutionTask) orig, (SubstitutionTask) res);
					continue;
				}
				if (orig is TransformTask)
				{
					CheckTransformTask((TransformTask) orig, (TransformTask) res);
					continue;
				}
			}
		}

		private static void CheckTransformTask(TransformTask original, TransformTask result)
		{
			Assert.That(original.Name, Is.EqualTo(result.Name));
		}

		private static void CheckSubstitutionTask(SubstitutionTask original, SubstitutionTask result)
		{
			Assert.That(original.Name, Is.EqualTo(result.Name));
		}

		private static void CheckStopServiceTask(StopServiceTask original, StopServiceTask result)
		{
			Assert.That(original.Name, Is.EqualTo(result.Name));
		}

		private static void CheckStartServiceTask(StartServiceTask original, StartServiceTask result)
		{
			Assert.That(original.Name, Is.EqualTo(result.Name));
		}

		private static void CheckAssertNoMoreVariablesInFile(AssertNoMoreVariablesInFile original, AssertNoMoreVariablesInFile result)
		{
			Assert.That(original.Name, Is.EqualTo(result.Name));
		}

		private static void CheckAssertLackOfXPathTask(AssertLackOfXPathTask original, AssertLackOfXPathTask result)
		{
			Assert.That(original.Name, Is.EqualTo(result.Name));
		}
	}
}