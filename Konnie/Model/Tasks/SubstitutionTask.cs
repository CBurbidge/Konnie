namespace Konnie.Model.Tasks
{
	public class SubstitutionTask : ISubTask
	{
		public bool SubstitutionTaskThing = false;
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