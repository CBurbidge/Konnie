using System.Collections.Generic;
using Konnie.Model.Tasks;

namespace Konnie.Model
{
	public class KonnieFile
	{
		public Dictionary<string, SubTaskCollection> Tasks { get; set; }
		public Dictionary<string, ISubTask> SubTasks { get; set; }
		public Dictionary<string, VariableSet> VariableSets { get; set; }

		public KonnieFile Merge(KonnieFile otherKonnieFile)
		{
			return this;
		}
	}

	public class VariableSet
	{
		public List<Variable> Variables { get; set; }  
	}

	public class Variable
	{
		public string Key { get; set; }
		public string Value { get; set; }
	}
}