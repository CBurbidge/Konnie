using System.Collections.Generic;
using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class SubstitutionTask : ISubTaskThatUsesVariableSets
	{
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(SubstitutionTask);

		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{ 
		}

		public List<string> SubstitutionVariableSets { get; set; }
	}
}