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
	public class KVariableSets : Dictionary<string, VariableSet>
	{
	}

	public class KTasks : Dictionary<string, SubTaskCollection>
	{
	}

	public class KSubTasks : Dictionary<string, ISubTask>
	{
	}

	public class VariableSet : Dictionary<string, string>
	{
	}
}