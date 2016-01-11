using System.IO.Abstractions;
using Konnie.InzOutz;
using Konnie.Runner.Logging;

namespace Konnie.Model.FilesHistory
{
	public class FilesHistoryFactory
	{
		private readonly IHistoryFileConverter _fileConverter;
		private readonly IFileSystem _fs;
		private readonly ILogger _logger;

		public FilesHistoryFactory(ILogger logger = null, IFileSystem fs = null, IHistoryFileConverter fileConverter = null)
		{
			var fileSystem = fs ?? new FileSystem();
			_fs = fileSystem;
			_fileConverter = fileConverter ?? new HistoryFileConverter(_logger, fileSystem);
			_logger = logger ?? new Logger();
		}

		public IFilesHistory Create(string historyFilePath, string taskName)
		{
			if (string.IsNullOrEmpty(historyFilePath))
			{
				_logger.Verbose("Loading Unpersisting file history");
				return new UnpersistedHistory();
			}

			return new JsonFilePersistedFilesHistory(historyFilePath, taskName, _logger, _fs, _fileConverter);
		}
	}
}