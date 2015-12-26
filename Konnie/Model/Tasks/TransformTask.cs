namespace Konnie.Model.Tasks
{
	public class TransformTask : ISubTask
	{
		public bool TransformationTaskThing = false;
		public string TaskName => nameof(TransformTask);
		public bool CanRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}