using System;
using System.Collections.Generic;
using System.Text;
using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class SubstitutionTask : ISubTaskThatUsesVariableSets
	{
		public string FilePath { get; set; }
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(SubstitutionTask);

		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			Logger.Verbose($"Starting task '{Name}'");
			CheckVariableSets();
			CheckFileExists(fileSystemHandler);

			var sb = new StringBuilder();
			foreach (var line in fileSystemHandler.ReadAllLines(FilePath))
			{
				
			}
		}

		private void CheckVariableSets()
		{
			if (SubstitutionVariableSets.Count == 0)
			{
				Logger.Terse("Can't proceed as SubstitutionVariableSets not specified.");
				throw new InvalidProgramException("Can't run as no substitution variable sets are specified.");
			}
		}

		private void CheckFileExists(IFileSystemHandler fileSystemHandler)
		{
			if (fileSystemHandler.Exists(FilePath) == false)
			{
				Logger.Terse($"File '{FilePath}' doesn't exist, can't continue");
				throw new FileDoesntExist(FilePath);
			}
		}

		public List<string> SubstitutionVariableSets { get; set; }
	}
}