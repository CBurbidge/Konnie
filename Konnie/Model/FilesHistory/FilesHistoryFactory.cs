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

			return new JsonFilePersistedFilesHistory(historyFilePath, taskName);
		}
	}
}