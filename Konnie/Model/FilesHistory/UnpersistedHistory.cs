using System;

namespace Konnie.Model.FilesHistory
{
	public class UnpersistedHistory : IFilesHistory
	{
		public void LoadFileHistory()
		{
			
		}

		public bool FileIsDifferent(string filePath, DateTime lastModified)
		{
			return true;
		}

		public void UpdateHistory(string filePath, DateTime lastModified)
		{
			
		}

		public void CommitChanges()
		{
			
		}
	}
}