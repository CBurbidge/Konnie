using Konnie.Model.File;

namespace Konnie.Model.Tasks.SubTasks
{
	public class AssertNoMoreVariablesInFile : ISubTask
	{
		public string Name { get; set; }
		public string Type => nameof(AssertNoMoreVariablesInFile);
		
		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}