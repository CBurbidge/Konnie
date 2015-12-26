using Konnie.Model.File;
using Konnie.Model.Tasks;
using Konnie.Model.Tasks.SubTasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Konnie.Tests
{
	[TestFixture]
	public class KonnieFileParserTests
	{
		[Test]
		public void Test()
		{
			var inMem = new KFile
			{
				Tasks = new KTasks
				{
					{
						"taskOne", new SubTaskCollection
						{
							"subTaskOne"
						}
					}
				},
				SubTasks = new KSubTasks
				{
					{"subTaskOne", new SubstitutionTask()},
					{"subTaskTwo", new TransformTask()}
				},
				VariableSets = new KVariableSets
				{
					{
						"variablesOne",
						new VariableSet
						{
							{"var1", "val1"}
						}
					}
				}
			};

			var thing = JsonConvert.SerializeObject(inMem, Formatting.Indented);
			var thing2 = JsonConvert.DeserializeObject<KFile>(thing, new SubTaskJsonConverter());
			var thing3 = 0;
		}
	}
}