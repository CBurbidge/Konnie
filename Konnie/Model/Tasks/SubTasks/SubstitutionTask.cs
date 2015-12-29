using System.Collections.Generic;
using Konnie.Model.File;

namespace Konnie.Model.Tasks.SubTasks
{
	public class SubstitutionTask : ISubTask
	{
		public string Name { get; set; }
		public string Type => nameof(SubstitutionTask);
		public List<string> SubstitutionVariableSets { get; set; } 
		public bool CanRun(KVariableSets variableSets, IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}