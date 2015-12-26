using System.IO;
using Konnie.Model.File;
using Newtonsoft.Json;

namespace Konnie.Model
{
	public class KonnieFileParser
	{
		public KFile FromFile(string filePath)
		{
			var text = System.IO.File.ReadAllText(filePath);
			return DeserializeObject(text);
		}

		private static KFile DeserializeObject(string text)
		{
			return JsonConvert.DeserializeObject<KFile>(text);
		}
	}
}