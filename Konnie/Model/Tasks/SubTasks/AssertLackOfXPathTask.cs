using Konnie.Model.File;

namespace Konnie.Model.Tasks.SubTasks
{
	public class AssertLackOfXPathTask : ISubTask
	{
		public string Name { get; set; }
		public string Type => nameof(AssertLackOfXPathTask);
		
		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}