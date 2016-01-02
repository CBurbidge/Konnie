using System;
using System.IO.Abstractions;
using Konnie.InzOutz;
using Newtonsoft.Json;

namespace Konnie.Model.FilesHistory
{
	public class JsonFilePersistedFilesHistory : IFilesHistory
	{
		private readonly IFileSystem _fs;
		private readonly IHistoryFileConverter _historyFileConverter;
		private readonly string _historyJsonFilePath;
		private HistoryFile _historyFileInitial = new HistoryFile();
		private HistoryFile _historyFileUpdated = new HistoryFile();
		private string _taskBeingPerformed;

		public JsonFilePersistedFilesHistory(string historyJsonFilePath, string taskBeingPerformed, IFileSystem fs = null,
			IHistoryFileConverter historyFileConverter = null)
		{
			_fs = fs ?? new FileSystem();
			_historyJsonFilePath = historyJsonFilePath;
			_taskBeingPerformed = taskBeingPerformed;
			_historyFileConverter = historyFileConverter ?? new HistoryFileConverter(_fs);

			if (string.IsNullOrEmpty(taskBeingPerformed))
			{
				throw new InvalidProgramException("Need to specify a task.");
			}

			if (_fs.File.Exists(historyJsonFilePath) == false)
			{
				return;
			}

			try
			{
				_historyFileInitial = _historyFileConverter.LoadHistoryFile(historyJsonFilePath);
				_historyFileUpdated = _historyFileConverter.LoadHistoryFile(historyJsonFilePath);
			}
			catch (JsonException)
			{
			}
		}

		public void Initiate()
		{
			
		}

		/// <summary>
		/// This will return false if the file history is specified and the file exists and the file exists in the task history
		/// and if the last modified date is the same as the one specified. For everything else it will return true.
		/// </summary>
		public bool FileIsDifferent(string absoluteFilePath, DateTime lastModified)
		{
			if (_historyFileInitial.ContainsKey(_taskBeingPerformed) == false)
			{
				return true;
			}

			var taskHistory = _historyFileInitial[_taskBeingPerformed];

			if (taskHistory.ContainsKey(absoluteFilePath) == false)
			{
				return true;
			}

			return taskHistory[absoluteFilePath] != lastModified;
		}

		public void UpdateHistory(string absoluteFilePath, DateTime lastModified)
		{
			
		}

		public void CommitChanges()
		{
			// save file to wherever.
		}
	}
}