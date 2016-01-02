using System;

namespace Konnie.Model.FilesHistory
{
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