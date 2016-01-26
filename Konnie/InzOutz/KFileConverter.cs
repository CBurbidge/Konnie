using System;
using System.Collections.Generic;
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
			return DeserializeObject(text, new List<string> {filePath}, _fs.Path.GetDirectoryName(filePath));
		}

		public KFile DeserializeFromString(string text)
		{
			return DeserializeObject(text, new List<string>(), null);
		}

		public string Serialize(KFile kFile)
		{
			// want to not serialise if none exist. 
			// Setting to default makes not serialise.
			if (kFile.ExtraFiles.Count == 0)
			{
				kFile.ExtraFiles = null;
			}

			return JsonConvert.SerializeObject(kFile, Formatting.Indented,
				new JsonSerializerSettings
				{
					DefaultValueHandling = DefaultValueHandling.Ignore
				});
		}

		private KFile DeserializeObject(string text, List<string> filesAlreadyMet, string dirPath)
		{
			var deserializeObject = JsonConvert.DeserializeObject<KFile>(text, new SubTaskJsonConverter(_logger));
			deserializeObject.Logger = _logger;

			if (deserializeObject.ExtraFiles.Count > 0)
			{
				foreach (var kFilePath in deserializeObject.ExtraFiles)
				{
					var path = _fs.Path.Combine(dirPath, kFilePath);

					if (filesAlreadyMet.Contains(path))
					{
						throw new KFileAlreadyAdded(kFilePath);
					}

					filesAlreadyMet.Add(path);
					var kFile = DeserializeObject(_fs.File.ReadAllText(path), filesAlreadyMet, dirPath);

					deserializeObject.Merge(kFile);
				}
			}

			return deserializeObject;
		}
	}
}