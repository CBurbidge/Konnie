using System;
using System.IO.Abstractions;
using Konnie.InzOutz;
using Konnie.Runner.Logging;
using Newtonsoft.Json;

namespace Konnie.Model.FilesHistory
{
	public class JsonFilePersistedFilesHistory : IFilesHistory
	{
		private readonly IFileSystem _fs;
		private readonly IHistoryFileConverter _historyFileConverter;
		private readonly string _historyJsonFilePath;
		private HistoryFile _historyFile = new HistoryFile();
		private readonly string _taskBeingPerformed;
		private readonly ILogger _logger;

		public JsonFilePersistedFilesHistory(string historyJsonFilePath, string taskBeingPerformed, ILogger logger = null, IFileSystem fs = null, IHistoryFileConverter historyFileConverter = null)
		{
			_fs = fs ?? new FileSystem();
			_historyJsonFilePath = historyJsonFilePath;
			_taskBeingPerformed = taskBeingPerformed;
			_historyFileConverter = historyFileConverter ?? new HistoryFileConverter(_logger, _fs);
			_logger = logger ?? new ConsoleLogger(true);
		}

		public void LoadFileHistory()
		{
			_logger.Terse("Loading the file history");
			if (_fs.File.Exists(_historyJsonFilePath) == false)
			{
				_logger.Verbose($"File doesn't exist at path '{_historyJsonFilePath}', not going to try to load history.");
				return;
			}

			try
			{
				_logger.Verbose($"Trying to load JSON file located at '{_historyJsonFilePath}'");
				_historyFile = _historyFileConverter.LoadHistoryFile(_historyJsonFilePath);
			}
			catch (JsonException e)
			{
				_logger.Terse("Loading and serialising of file failed. Ignore this error and start with a new history to overwrite file when commiting.");
				_logger.Terse($"Error message was '{e.Message}'");
			}

			if (_historyFile.ContainsKey(_taskBeingPerformed) == false)
			{
				_logger.Verbose($"History doesn't contain the task '{_taskBeingPerformed}'.");
				_historyFile[_taskBeingPerformed] = new FileModifiedDateByAbsoluteFilePath();
			}
		}

		/// <summary>
		/// This will return false if the file history is specified and the file exists and the file exists in the task history
		/// and if the last modified date is the same as the one specified. For everything else it will return true.
		/// </summary>
		public bool FileIsDifferent(string filePath, DateTime lastModified)
		{
			_logger.Verbose($"Is modify date of file '{filePath}' different to {lastModified}");
			if (_historyFile.ContainsKey(_taskBeingPerformed) == false)
			{
				_logger.Verbose($"History doesn't contain the task '{_taskBeingPerformed}'. Reporting that file is different.");
				return true;
			}

			var taskHistory = _historyFile[_taskBeingPerformed];

			if (taskHistory.ContainsKey(filePath) == false)
			{
				_logger.Verbose($"Task history doesn't contain the file path '{filePath}'. Reporting that file is different.");
				return true;
			}

			var historyLastModified = taskHistory[filePath];
			_logger.Verbose($"File was last modified at {historyLastModified}");

			return historyLastModified != lastModified;
		}

		public void UpdateHistory(string filePath, DateTime lastModified)
		{
			_logger.Verbose($"UpdateHistory called with file '{filePath}' and time {lastModified}");
			var taskHistory = _historyFile[_taskBeingPerformed];

			if (taskHistory.ContainsKey(filePath) == false)
			{
				_logger.Verbose("Task history doesn't contain the file.");
				taskHistory[filePath] = DateTime.MinValue;
			}

			_logger.Verbose("Updating the time in history.");
			taskHistory[filePath] = lastModified;
		}

		public void CommitChanges()
		{
			_logger.Terse($"CommitChanges called, saving to {_historyJsonFilePath}");
			_historyFileConverter.SaveHistoryFile(_historyFile, _historyJsonFilePath);
		}
	}
}