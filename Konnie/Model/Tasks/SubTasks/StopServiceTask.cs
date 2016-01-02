using Konnie.Model.FilesHistory;

namespace Konnie.Model.Tasks.SubTasks
{
	public class StopServiceTask : ISubTask
	{
		public string Name { get; set; }
		public string Type => nameof(StopServiceTask);
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