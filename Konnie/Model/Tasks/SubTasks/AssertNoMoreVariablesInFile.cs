namespace Konnie.Model.Tasks.SubTasks
{
	public class AssertNoMoreVariablesInFile : ISubTask
	{
		public string Name { get; set; }
		public string Type => nameof(AssertNoMoreVariablesInFile);
		public string ServiceName { get; set; }

		public bool CanRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}