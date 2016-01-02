using System.Collections.Generic;
using System.Linq;
using Konnie.InzOutz;
using Konnie.Model.File;
using Konnie.Runner.Logging;

namespace Konnie.Runner
{
	public class KFileCombiner
	{
		private readonly ILogger _logger;
		private readonly KFileConverter _converter = new KFileConverter();

		public KFileCombiner(ILogger logger)
		{
			_logger = logger;
		}

		public KFile Combine(List<string> filePaths)
		{
			_logger.Verbose("Combining KFiles");
			var kFile = new KFile();
			var kFileObjects = filePaths.Select(fp =>
			{
				_logger.Verbose($"Loading from file '{fp}'");
				return _converter.DeserializeFromFile(fp);
			});
			foreach (var kFileObject in kFileObjects)
			{
				kFile.Merge(kFileObject);
			}

			return kFile;
		}
	}
}