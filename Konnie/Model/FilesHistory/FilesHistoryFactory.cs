using System;

namespace Konnie.Model.FilesHistory
{
	public class FilesHistoryFactory
	{
		public IFilesHistory Create(string historyFilePath, string taskName)
		{
			if (string.IsNullOrEmpty(historyFilePath))
			{
				return new UnpersistedHistory();
			}
		}
	}

	public class UnpersistedHistory : IFilesHistory
	{
		public void Initiate()
		{
			
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
			
		}
	}
}