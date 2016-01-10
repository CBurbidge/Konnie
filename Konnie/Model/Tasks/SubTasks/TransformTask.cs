using System.Collections.Generic;
using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class TransformTask : ISubTask
	{
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(TransformTask);

		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public string InputFile { get; set; }
		public string OutputFile { get; set; }
		public List<string> TransformFiles { get; set; }

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			
		}
	}
}