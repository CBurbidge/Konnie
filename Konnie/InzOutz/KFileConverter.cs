using System.IO;
using Konnie.Model.File;
using Newtonsoft.Json;

namespace Konnie.InzOutz
{
	public class KFileConverter
	{
		public KFile DeserializeFromFile(string filePath)
		{
			var text = File.ReadAllText(filePath);
			return DeserializeObject(text);
		}

		public KFile DeserializeFromString(string text)
		{
			return DeserializeObject(text);
		}
		public string Serialize(KFile kFile)
		{
			return JsonConvert.SerializeObject(kFile, Formatting.Indented);
		}

		private static KFile DeserializeObject(string text)
		{
			return JsonConvert.DeserializeObject<KFile>(text, new SubTaskJsonConverter());
		}
	}
}