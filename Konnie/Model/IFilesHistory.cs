using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace Konnie.Model
{
	/// <summary>
	/// IFileHistory objects are task specific, it doesn't make sense for them to know about other tasks.
	/// They only care about konnie and input files (xml files and transforms etc)`.
	/// They know how to check to see if a file is different (newer or older) to the last time it was seen and return true or false
	/// They also know how to update the history if a file is changed for whatever reason
	/// They can commit changes to the file history, this should be done when the task is finishing.
	/// </summary>
	public interface IFilesHistory
	{
		bool FileIsDifferent(string absoluteFilePath, DateTime lastModified);
		void UpdateHistory(string absoluteFilePath, DateTime lastModified);
		void CommitChanges();
	}

	public class FilesHistory : IFilesHistory
	{
		private IFileSystem _fs;
		private string _taskBeingPerformed;
		private string _historyJsonFilePath;
		private HistoryFile _historyFile;

		public FilesHistory(string historyJsonFilePath, string taskBeingPerformed, IFileSystem fs = null)
		{
			_historyJsonFilePath = historyJsonFilePath;
			_taskBeingPerformed = taskBeingPerformed;
			_fs = fs ?? new FileSystem();
		}

		public bool FileIsDifferent(string absoluteFilePath, DateTime lastModified)
		{
			return true;
		}

		public void UpdateHistory(string absoluteFilePath, DateTime lastModified)
		{
			
		}

		public void CommitChanges()
		{
			// save file to wherever.
		}
	}

	internal class HistoryFile : Dictionary<string, FileModifiedDateByAbsoluteFilePath>
	{
	}
	internal class FileModifiedDateByAbsoluteFilePath : Dictionary<string, DateTime>
	{
	}
}