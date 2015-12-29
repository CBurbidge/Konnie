using System.Collections.Generic;

namespace Konnie.Runner
{
	public class KonnieProgramArgs 
	{
		public string Task { get; set; }
		public List<string> Files { get; set; }
		public string HistoryFilePath { get; set; }
	}
}