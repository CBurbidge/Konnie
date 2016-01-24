using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class CopyFileTask : ISubTask
	{
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(CopyFileTask);
		public string Source { get; set; }
		public string Destination { get; set; }

		public bool NeedToRun(IFileSystemHandler fileSystemHandler)
		{
			var lastModified = fileSystemHandler.GetLastWriteTime(Source);
			return fileSystemHandler.FilesHistory.FileIsDifferent(Source, lastModified);
		}

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			Logger.Verbose($"Starting task '{Name}'");

			if (fileSystemHandler.Exists(Source) == false)
			{
				Logger.Terse($"Source file '{Source}' doesn't exist, can't continue.");
				throw new FileDoesntExist(fileSystemHandler.GetAbsPath(Source));
			}

			if (fileSystemHandler.Exists(Destination) == false)
			{
				Logger.Terse($"Destination file '{Destination}' doesn't exist, can't continue.");
				throw new FileDoesntExist(fileSystemHandler.GetAbsPath(Destination));
			}

			Logger.Verbose($"Copying file '{Source}' to '{Destination}'.");
			fileSystemHandler.Copy(Source, Destination);
		}
	}
}