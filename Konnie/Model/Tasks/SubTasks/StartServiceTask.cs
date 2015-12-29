using Konnie.Model.File;

namespace Konnie.Model.Tasks.SubTasks
{
	public class StartServiceTask : ISubTask
	{
		public string Name { get; set; }
		public string Type => nameof(StartServiceTask);
		public string ServiceName { get; set; }

		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}