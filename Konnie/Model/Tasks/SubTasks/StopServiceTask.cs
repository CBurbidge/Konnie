namespace Konnie.Model.Tasks.SubTasks
{
	public class StopServiceTask : ISubTask
	{
		public string ServiceName { get; set; }
		public string Type => nameof(StopServiceTask);
		public string Name { get; set; }
		public bool CanRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}