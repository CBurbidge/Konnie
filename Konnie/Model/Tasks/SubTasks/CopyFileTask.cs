using Konnie.Model.FilesHistory;

namespace Konnie.Model.Tasks.SubTasks
{
	public class CopyFileTask : ISubTask
	{
		public string Name { get; set; }
		public string Type => nameof(TransformTask);
		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}