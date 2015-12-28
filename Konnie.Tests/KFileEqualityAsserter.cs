using System.Linq;
using Konnie.Model.File;
using Konnie.Model.Tasks.SubTasks;
using NUnit.Framework;

namespace Konnie.Tests
{
	public class KFileEqualityAsserter
	{
		public static void AssertAreEqual(KFile original, KFile result)
		{
			AssertTasksAreEqual(original.Tasks, result.Tasks);
			AssertSubTasksAreEqual(original.SubTasks, result.SubTasks);
			AssertVariableSetsAreEqual(original.VariableSets, result.VariableSets);
		}
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