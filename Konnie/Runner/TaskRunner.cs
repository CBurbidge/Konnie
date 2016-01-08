using System;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using Konnie.InzOutz;
using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner.Logging;

namespace Konnie.Runner
{
	/// <summary>
	/// This is the concrete class to run stuff.
	/// This should have minimum logic in it, just build up dependencies and run the files.
	/// </summary>
	public class TaskRunner : IKonnieRunner
	{
		public void Run(KonnieProgramArgs args, ILogger logger, IFileSystem fs)
		{
			args.Validate(fs);

			try
			{
				var fileConverter = new HistoryFileConverter(logger, fs);
				var filesHistory = new FilesHistoryFactory(logger, fs, fileConverter)
					.Create(args.HistoryFile, args.Task);
				var fileSystemHandler = new FileSystemHandler(filesHistory);

				var kFileCombiner = new KFileCombiner(logger, fs);
				var kFile = kFileCombiner.Combine(args.Files);
				
				if (kFile.IsValid(args.Task) == false)
				{
					throw new CombinedKFileIsInvalid(args.Files);
				}

				var taskToRun = kFile.Tasks.Single(t => t.Name == args.Task);
				var subTasksToRun = kFile.SubTasks.Where(st => taskToRun.SubTasksToRun.Contains(st.Name));
				var anySubTasksNeedToRun = subTasksToRun.Any(st => st.NeedToRun(filesHistory));

				if (anySubTasksNeedToRun)
				{
					foreach (var subTask in subTasksToRun)
					{
						subTask.Run(fileSystemHandler);
					}

					logger.Verbose("Commiting changes to file history.");
					filesHistory.CommitChanges();
				}
				else
				{
					logger.Terse("Not run as doesn't need to.");
				}
			}
			catch (Exception e) // catch everything and throw out the verbose log.
			{
				logger.Terse("Konnie failed to run task. Exception message is:");
				logger.Terse(e.Message);
				var verboseLog = logger.GetLog(LogType.Verbose);
				throw new Exception(verboseLog);
			}
		}
	}

	

	public interface IKonnieRunner
	{
		void Run(KonnieProgramArgs args, ILogger logger, IFileSystem fs);
	}
}