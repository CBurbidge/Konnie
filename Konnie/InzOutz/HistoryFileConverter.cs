using System.IO.Abstractions;
using Konnie.Model;
using Newtonsoft.Json;

namespace Konnie.InzOutz
{
	public class HistoryFileConverter : IHistoryFileConverter
	{
		private readonly IFileSystem _fs;

		public HistoryFileConverter(IFileSystem fs = null)
		{
			_fs = fs ?? new FileSystem();
		}

		public HistoryFile LoadHistoryFile(string historyJsonFilePath)
		{
			var jsonText = _fs.File.ReadAllText(historyJsonFilePath);
			return JsonConvert.DeserializeObject<HistoryFile>(jsonText);
		}
	}

	public interface IHistoryFileConverter
	{
		HistoryFile LoadHistoryFile(string historyJsonFilePath);
	}
}