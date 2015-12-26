namespace Konnie.Model.Tasks.SubTasks
{
	public class StartServiceTask : ISubTask
	{
		public string ServiceName { get; set; }
		public string Type => nameof(StartServiceTask);
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