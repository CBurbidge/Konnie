namespace Konnie.Model.Tasks.SubTasks
{
	public class TransformTask : ISubTask
	{
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