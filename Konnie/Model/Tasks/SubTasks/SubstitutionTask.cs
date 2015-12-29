using System.Collections.Generic;

namespace Konnie.Model.Tasks.SubTasks
{
	public class SubstitutionTask : ISubTask, IUsesVariableSets
	{
		public string Name { get; set; }
		public string Type => nameof(SubstitutionTask);

		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
		}

		public List<string> SubstitutionVariableSets { get; set; }
	}
}