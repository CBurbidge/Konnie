using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Konnie.Model.File;
using Konnie.Runner;
using Konnie.Runner.Logging;
using Microsoft.Web.XmlTransform;

namespace Konnie.Model.Tasks.SubTasks
{
	public class TransformTask : ISubTask
	{
		private readonly Func<string, Stream> _getFileStream;

		public TransformTask(Func<string, Stream> getFileStream = null)
		{
			_getFileStream = getFileStream ?? (fp => new FileStream(fp, FileMode.Open, FileAccess.Read));
		}

		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(TransformTask);

		public bool NeedToRun(IFileSystemHandler fileSystemHandler)
		{
			return TransformFiles.Concat(new[] {InputFile}).Any(f =>
			{
				var lastModified = fileSystemHandler.GetLastWriteTime(f);
				return fileSystemHandler.FilesHistory.FileIsDifferent(f, lastModified);
			});
		}

		public string InputFile { get; set; }
		public string OutputFile { get; set; }
		public List<string> TransformFiles { get; set; }

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			Logger.Verbose($"Starting task '{Name}'");

			if (TransformFiles.Count == 0)
			{
				throw new InvalidProgramException("Need to specify Transform file paths.");
			}
			
			CheckFilesExist(fileSystemHandler);

			var input = new XmlDocument();
			var stringReader = new StringReader(fileSystemHandler.ReadAllText(InputFile));
			input.Load(stringReader);

			var logger = new XmlTransformationLogger();
			foreach (var transformFile in TransformFiles)
			{
				Logger.Verbose($"Applying transform '{transformFile}'");
				var stream = _getFileStream(fileSystemHandler.GetAbsPath(transformFile));
				var transform = new XmlTransformation(stream, logger);
				transform.Apply(input);
			}

			Logger.Verbose($"Writing to file '{OutputFile}'.");
			fileSystemHandler.SaveXDocument(input, OutputFile);

			Logger.Verbose("Applied all of the transforms");
		}

		private void CheckFilesExist(IFileSystemHandler fileSystemHandler)
		{
			AssertFileExists(OutputFile, fileSystemHandler);
			AssertFileExists(InputFile, fileSystemHandler);
			foreach (var transformFile in TransformFiles)
			{
				AssertFileExists(transformFile, fileSystemHandler);
			}
		}

		private void AssertFileExists(string file, IFileSystemHandler fileSystemHandler)
		{
			if (fileSystemHandler.Exists(file) == false)
			{
				Logger.Terse($"File '{file}' doesn't exist, can't continue");
				throw new FileDoesntExist(fileSystemHandler.GetAbsPath(file));
			}
		}
	}
}