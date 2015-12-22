﻿using System.Collections.Generic;
using System.IO;
using Konnie.Model.Tasks;
using Newtonsoft.Json;

namespace Konnie.Model
{
	public class KonnieFileParser
	{
		public KonnieFile FromFile(string filePath)
		{
			var text = File.ReadAllText(filePath);
			return DeserializeObject(text);
		}

		private static KonnieFile DeserializeObject(string text)
		{
			return JsonConvert.DeserializeObject<KonnieFile>(text);
		}
	}
}