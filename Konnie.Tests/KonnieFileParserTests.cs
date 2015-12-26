using System.Collections.Generic;
using Konnie.Model;
using Konnie.Model.Tasks;
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
			var inMem = new KonnieFile
			{
				Tasks = new Dictionary<string, SubTaskCollection>
				{
					{
						"taskOne", new SubTaskCollection
						{
							SubTasks = new List<string>
							{
								"subTaskOne"
							}
						}
					}
				},
				SubTasks = new Dictionary<string, ISubTask>
				{
					{"subTaskOne", new SubstitutionTask()},
					{"subTaskTwo", new TransformTask()}
				},
				VariableSets = new Dictionary<string, VariableSet>
				{
					{
						"variablesOne", new VariableSet
						{
							Variables = new List<Variable>
							{
								new Variable {Key = "var1", Value = "val1"}
							}
						}
					}
				}
			};

			var thing = JsonConvert.SerializeObject(inMem);
			var thing2 = JsonConvert.DeserializeObject<KonnieFile>(thing, new SubTaskJsonConverter());
			var thing3 = 0;
		}
	}
}