using System.Collections.Generic;

namespace Konnie.Model.File
{
	public class KTask
	{
		public string Name { get; set; }
		public List<string> SubTasksToRun { get; set; }
	}
}