namespace Konnie.Model.Tasks.SubTasks
{
	public class TransformTask : ISubTask
	{
		public string Type => nameof(TransformTask);
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