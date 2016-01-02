using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Konnie.InzOutz;
using Konnie.Model.File;
using Konnie.Runner.Logging;

namespace Konnie.Runner
{
	public class KFileCombiner
	{
		private readonly ILogger _logger;
		private readonly KFileConverter _converter;

		public KFileCombiner(ILogger logger, IFileSystem fs)
		{
			_logger = logger;
			_converter = new KFileConverter(logger, fs);
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