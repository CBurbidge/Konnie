using System;
using System.IO.Abstractions;
using Konnie.Model.File;
using Konnie.Runner.Logging;
using Newtonsoft.Json;

namespace Konnie.InzOutz
{
	public class KFileConverter
	{
		private readonly IFileSystem _fs;
		private readonly ILogger _logger;

		public KFileConverter(ILogger logger, IFileSystem fs)
		{
			if (logger == null)
			{
				throw new ArgumentException("Logger needs to be non-null", nameof(logger));
			}

			_logger = logger;
			_fs = fs ?? new FileSystem();
		}

		public KFileConverter()
		{
			_logger = new ConsoleLogger(true);
			_fs = new FileSystem();
		}

		public KFile DeserializeFromFile(string filePath)
		{
			var text = _fs.File.ReadAllText(filePath);
			return DeserializeObject(text);
		}

		public KFile DeserializeFromString(string text)
		{
			return DeserializeObject(text);
		}

		public string Serialize(KFile kFile)
		{
			return JsonConvert.SerializeObject(kFile, Formatting.Indented,
				new JsonSerializerSettings
				{
					DefaultValueHandling = DefaultValueHandling.Ignore
				});
		}

		private KFile DeserializeObject(string text)
		{
			var deserializeObject = JsonConvert.DeserializeObject<KFile>(text, new SubTaskJsonConverter(_logger));
			deserializeObject.Logger = _logger;
			return deserializeObject;
		}
	}
}