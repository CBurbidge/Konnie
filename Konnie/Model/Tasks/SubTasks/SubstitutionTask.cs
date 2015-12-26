namespace Konnie.Model.Tasks.SubTasks
{
	public class SubstitutionTask : ISubTask
	{
		public string Name { get; set; }
		public string Type => nameof(SubstitutionTask);

		public bool CanRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}