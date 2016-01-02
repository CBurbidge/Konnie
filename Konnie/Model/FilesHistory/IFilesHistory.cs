using System;

namespace Konnie.Model.FilesHistory
{
	/// <summary>
	///     IFileHistory objects are task specific, it doesn't make sense for them to know about other tasks.
	///     They only care about konnie and input files (xml files and transforms etc)`.
	///     They know how to check to see if a file is different (newer or older) to the last time it was seen and return true
	///     or false
	///     They also know how to update the history if a file is changed for whatever reason
	///     They can persist changes to the file history, this should be done when the task is finishing.
	/// </summary>
	public interface IFilesHistory
	{
		/// <summary>
		/// Want to have an initial loading of data into history
		/// </summary>
		void LoadFileHistory();
		bool FileIsDifferent(string absoluteFilePath, DateTime lastModified);
		
		/// <summary>
		/// The UpdateHistory method updates the in memory version of the history this will not be persisted until
		/// CommitChanges is called. Most likely at the end of the task.
		/// </summary>
		void UpdateHistory(string absoluteFilePath, DateTime lastModified);
		void CommitChanges();
	}
}