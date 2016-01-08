using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace Konnie.Runner
{
	public class KonnieProgramArgs 
	{
		public string ProjectDir { get; set; }
		public string Task { get; set; }
		public List<string> Files { get; set; }
		public string HistoryFile { get; set; }

		public void Validate(IFileSystem fs, Func<string, Stream> getFileStreamFromPath = null)
		{
			var getFileStream = getFileStreamFromPath ?? (path => new FileStream(path, FileMode.Open, FileAccess.ReadWrite));

			if (fs.Directory.Exists(ProjectDir) == false)
			{
				throw new ProjectDirectoryDoesntExist(ProjectDir);
			}

			if (Files.Count == 0)
			{
				throw new InvalidProgramException("No Konnie files.");
			}

			foreach (var filePath in Files)
			{
				if (fs.File.Exists(filePath) == false)
				{
					throw new KonnieFileDoesntExistOrCantBeAccessed(filePath, "Doesn't exist");
				}

				try
				{
					using (var fileStream = getFileStream(filePath))
					{ }
				}
				catch (IOException e)
				{
					throw new KonnieFileDoesntExistOrCantBeAccessed(filePath, "Can't access");
				}
			}
		}
	}
}