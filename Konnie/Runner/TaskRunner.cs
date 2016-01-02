using System;
using System.IO.Abstractions;
using System.Net;
using Konnie.InzOutz;
using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner.Logging;

namespace Konnie.Runner
{
	/// <summary>
	/// This is the concrete class to run stuff.
	/// This should have no logic in it, just build up dependencies and run the files.
	/// </summary>
	public class TaskRunner : IKonnieRunner
	{
		public void Run(KonnieProgramArgs args, ILogger logger)
		{
			try
			{
				IFileSystem fs = new FileSystem();
				var fileConverter = new HistoryFileConverter(logger, fs);
				var filesHistory = new FilesHistoryFactory(logger, fs, fileConverter)
					.Create(args.HistoryFilePath, args.Task);
				var fileSystemHandler = new FileSystemHandler(filesHistory);
				var kFileCombiner = new KFileCombiner(logger);
				var kFile = kFileCombiner.Combine(args.Files);
				if (kFile.IsValid(args.Task) == false)
				{
					throw new CombinedKFileIsInvalid(args.Files);
				}

			}
			catch (Exception e) // catch everything and throw out the verbose log.
			{
				logger.Terse("Konnie failed to run task. Exception message is:");
				logger.Terse(e.Message);
				throw new Exception(logger.GetLog(LogType.Verbose));
			}
		}
	}

	

	public interface IKonnieRunner
	{
		void Run(KonnieProgramArgs args, ILogger logger);
	}
}