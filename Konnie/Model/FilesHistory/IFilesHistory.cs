using System;

namespace Konnie.Model.FilesHistory
{
	/// <summary>
	///     IFileHistory objects are task specific, it doesn't make sense for them to know about other tasks.
	///     They only care about konnie and input files (xml files and transforms etc)`.
	///     They know how to check to see if a file is different (newer or older) to the last time it was seen and return true
	///     or false
	///     They also know how to update the history if a file is changed for whatever reason
	///     They can commit changes to the file history, this should be done when the task is finishing.
	/// </summary>
	public interface IFilesHistory
	{
		void Initiate();
		bool FileIsDifferent(string absoluteFilePath, DateTime lastModified);
		void UpdateHistory(string absoluteFilePath, DateTime lastModified);
		void CommitChanges();
	}
}