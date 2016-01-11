using System.IO.Abstractions;
using Konnie.Model.FilesHistory;
using Konnie.Runner.Logging;
using Newtonsoft.Json;

namespace Konnie.InzOutz
{
	public class HistoryFileConverter : IHistoryFileConverter
	{
		private readonly ILogger _logger;
		private readonly IFileSystem _fs;

		public HistoryFileConverter(ILogger logger, IFileSystem fs = null)
		{
			_logger = logger ?? new Logger();
			_fs = fs ?? new FileSystem();
		}

		public HistoryFile LoadHistoryFile(string historyJsonFilePath)
		{
			_logger.Verbose($"Loading history file from {historyJsonFilePath}");
			var jsonText = _fs.File.ReadAllText(historyJsonFilePath);
			_logger.Verbose("Deserialising HistoryFile object.");
			return JsonConvert.DeserializeObject<HistoryFile>(jsonText);
		}

		public void SaveHistoryFile(HistoryFile historyFile, string filePath)
		{
			_logger.Terse($"Saving History file to '{filePath}'");
			_logger.Verbose("Serialising object");
			var text = JsonConvert.SerializeObject(historyFile, Formatting.Indented);
			_logger.Verbose("Writing to file.");
			_fs.File.WriteAllText(filePath, text);
		}
	}

	public interface IHistoryFileConverter
	{
		HistoryFile LoadHistoryFile(string historyJsonFilePath);
		void SaveHistoryFile(HistoryFile historyFile, string filePath);
	}
}