using System.Collections.Generic;
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
			var inMem = new KFile
			{
				Tasks = new KTasks
				{
					new KTask
					{
						Name = "taskOne",
						SubTasksToRun = new List<string>
						{
							"subTaskOne"
						} 
					}
				},
				SubTasks = new KSubTasks
				{
					new SubstitutionTask
					{
						Name = "subTaskOne"
					},
					new TransformTask
					{
						Name = "subTaskTwo"
					},
					new StartServiceTask
					{
						Name = "subTaskThree",
						ServiceName = "Gazza"
					},
					new StopServiceTask
					{
						Name = "subTaskFour",
						ServiceName = "diggers"
					}
				},
				VariableSets = new KVariableSets
				{
					new VariableSet
					{
						Name = "variablesOne",
						Variables = new Dictionary<string, string>
						{
							{"var1", "val1"}
						}
					}
				}
			};

			var parser = new KFileConverter();
			var text = parser.Serialize(inMem);
			var kFile = parser.DeserializeFromString(text);
		}
	}
}