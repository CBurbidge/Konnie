namespace Konnie.Model.Tasks.SubTasks
{
	public class TransformTask : ISubTask
	{
		public string Name { get; set; }
		public string Type => nameof(TransformTask);
		public bool CanRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}