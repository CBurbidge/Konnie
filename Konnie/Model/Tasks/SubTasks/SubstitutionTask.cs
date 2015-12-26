namespace Konnie.Model.Tasks.SubTasks
{
	public class SubstitutionTask : ISubTask
	{
		public string TaskName => nameof(SubstitutionTask);

		public bool CanRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}