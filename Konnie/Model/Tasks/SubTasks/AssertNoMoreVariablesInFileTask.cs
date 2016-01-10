using System.Text.RegularExpressions;
using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class AssertNoMoreVariablesInFileTask : ISubTask
	{
		public Regex VariableRegex = new Regex(@"#\{[a-zA-Z0-9_\-\s]+\}");
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(AssertNoMoreVariablesInFileTask);
		public string FilePath { get; set; }
		
		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			if (fileSystemHandler.Exists(FilePath) == false)
			{
				Logger.Terse($"File '{FilePath}' doesn't exist, can't continue");
				throw new FileDoesntExist(FilePath);
			}

			var wholeFile = fileSystemHandler.ReadAllText(FilePath);
			if (VariableRegex.IsMatch(wholeFile))
			{
				throw new VariablesStillExistInFile();
			}
		}
	}
}