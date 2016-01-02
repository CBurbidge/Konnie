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
		private HistoryFile _historyFile = new HistoryFile();
		private readonly string _taskBeingPerformed;

		public JsonFilePersistedFilesHistory(string historyJsonFilePath, string taskBeingPerformed, IFileSystem fs = null,
			IHistoryFileConverter historyFileConverter = null)
		{
			_fs = fs ?? new FileSystem();
			_historyJsonFilePath = historyJsonFilePath;
			_taskBeingPerformed = taskBeingPerformed;
			_historyFileConverter = historyFileConverter ?? new HistoryFileConverter(_fs);
		}

		public void LoadFileHistory()
		{
			if (_fs.File.Exists(_historyJsonFilePath) == false)
			{
				return;
			}

			try
			{
				_historyFile = _historyFileConverter.LoadHistoryFile(_historyJsonFilePath);
			}
			catch (JsonException)
			{
			}

			if (_historyFile.ContainsKey(_taskBeingPerformed) == false)
			{
				_historyFile[_taskBeingPerformed] = new FileModifiedDateByAbsoluteFilePath();
			}
		}

		/// <summary>
		/// This will return false if the file history is specified and the file exists and the file exists in the task history
		/// and if the last modified date is the same as the one specified. For everything else it will return true.
		/// </summary>
		public bool FileIsDifferent(string absoluteFilePath, DateTime lastModified)
		{
			if (_historyFile.ContainsKey(_taskBeingPerformed) == false)
			{
				return true;
			}

			var taskHistory = _historyFile[_taskBeingPerformed];

			if (taskHistory.ContainsKey(absoluteFilePath) == false)
			{
				return true;
			}

			return taskHistory[absoluteFilePath] != lastModified;
		}

		public void UpdateHistory(string absoluteFilePath, DateTime lastModified)
		{
			var taskHistory = _historyFile[_taskBeingPerformed];

			if (taskHistory.ContainsKey(absoluteFilePath) == false)
			{
				taskHistory[absoluteFilePath] = DateTime.MinValue;
			}

			taskHistory[absoluteFilePath] = lastModified;
		}

		public void CommitChanges()
		{
			_historyFileConverter.SaveHistoryFile(_historyFile, _historyJsonFilePath);
		}
	}
}