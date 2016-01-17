using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Konnie.Runner.Logging;

namespace Konnie.Runner
{
	public class KonnieProgramArgs 
	{
		public string ProjectDir { get; set; }
		public string Task { get; set; }
		public bool Verbose { get; set; }
		public List<string> Files { get; set; }
		public string HistoryFile { get; set; }

		public void Validate(IFileSystem fs, ILogger loggerInj = null, Func<string, Stream> getFileStreamFromPath = null)
		{
			var logger = loggerInj ?? new ConsoleLogger(false);
			
			var getFileStream = getFileStreamFromPath ?? (path => new FileStream(path, FileMode.Open, FileAccess.ReadWrite));

			if (fs.Directory.Exists(ProjectDir) == false)
			{
				var absProjDir = fs.Path.GetFullPath(ProjectDir);
				logger.Terse($"Project dir '{absProjDir}' does not exist.");
				throw new ProjectDirectoryDoesntExist(absProjDir);
			}

			if (Files.Count == 0)
			{
				logger.Terse("No konnie files have been specified.");
				throw new InvalidProgramException("No Konnie files.");
			}

			foreach (var filePath in Files)
			{
				if (fs.File.Exists(filePath) == false)
				{
					var absFilePath = fs.Path.GetFullPath(filePath);
					logger.Terse($"Konnie file '{absFilePath}' doesn't exist.");
					throw new KonnieFileDoesntExistOrCantBeAccessed(absFilePath, "Doesn't exist");
				}

				try
				{
					using (var fileStream = getFileStream(filePath))
					{ }
				}
				catch (IOException)
				{
					logger.Terse($"File path '{filePath}' cannot be accessed");
					throw new KonnieFileDoesntExistOrCantBeAccessed(filePath, "Can't access");
				}
			}
		}
	}
}