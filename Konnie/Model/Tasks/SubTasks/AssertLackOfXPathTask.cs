namespace Konnie.Model.Tasks.SubTasks
{
	public class AssertLackOfXPathTask : ISubTask
	{
		public string ServiceName { get; set; }
		public string Type => nameof(AssertLackOfXPathTask);
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