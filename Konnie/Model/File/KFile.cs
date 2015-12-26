using System.Collections.Generic;
using Konnie.Model.Tasks;

namespace Konnie.Model.File
{
	public class KFile
	{
		public KTasks Tasks { get; set; }
		public KSubTasks SubTasks { get; set; }
		public KVariableSets VariableSets { get; set; }

		public KFile Merge(KFile otherKFile)
		{
			return this;
		}
	}

	public class KTasks : List<KTask>
	{
	}

	public class KSubTasks : List<ISubTask>
	{
	}

	public class KVariableSets : List<VariableSet>
	{
	}

	public class KTask
	{
		public string Name { get; set; }
		public List<string> SubTasksToRun { get; set; }
	}

	public class VariableSet
	{
		public string Name { get; set; }
		public Dictionary<string, string> Variables { get; set; }
	}
}