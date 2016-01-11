using System;
using System.IO.Abstractions;
using System.Linq;
using Konnie.InzOutz;
using Konnie.Model.FilesHistory;
using Konnie.Runner.Logging;

namespace Konnie.Runner
{
	/// <summary>
	///     This is the concrete class to run stuff.
	///     This should have minimum logic in it, just build up dependencies and run the files.
	/// </summary>
	public class TaskRunner : IKonnieRunner
	{
		public void Run(KonnieProgramArgs args, ILogger loggerInj = null, IFileSystem fsInj = null)
		{
			var fs = fsInj ?? new FileSystem();
			var logger = loggerInj ?? new Logger();

			args.Validate(fs);

			try
			{
				var historyFileConverter = new HistoryFileConverter(logger, fs);
				var filesHistoryFactory = new FilesHistoryFactory(logger, fs, historyFileConverter);
				var kFileCombiner = new KFileCombiner(logger, fs);

				var filesHistory = filesHistoryFactory.Create(args.HistoryFile, args.Task);
				var fileSystemHandler = new FileSystemHandler(args.ProjectDir, filesHistory, logger);

				var kFile = kFileCombiner.Combine(args.Files);

				if (kFile.IsValid(args.Task) == false)
				{
					throw new CombinedKFileIsInvalid(args.Files);
				}

				var taskToRun = kFile.Tasks.Single(t => t.Name == args.Task);
				var subTasksToRun = kFile.SubTasks.Where(st => taskToRun.SubTasksToRun.Contains(st.Name));
				var anySubTasksNeedToRun = subTasksToRun.Any(st => st.NeedToRun(filesHistory));

				var anyOfTheKonnieFilesAreDifferent = args.Files.Any(f =>
				{
					var lastModified = fs.File.GetLastWriteTime(f);
					logger.Verbose($"LastWriteTime of file '{f}' is {lastModified}");
					return filesHistory.FileIsDifferent(f, lastModified);
				});

				var runKonnie = anySubTasksNeedToRun || anyOfTheKonnieFilesAreDifferent;

				if (runKonnie)
				{
					logger.Terse("Running Konnie.");

					foreach (var subTask in subTasksToRun)
					{
						logger.Verbose($"Running sub task {subTask.Name}");
						subTask.Run(fileSystemHandler, kFile.VariableSets);
					}

					logger.Verbose("Commiting changes to file history.");
					filesHistory.CommitChanges();

					logger.Terse("All done.");
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