namespace Konnie.Model.Tasks
{
	public interface ISubTask
	{
		string TaskName { get; }
		bool CanRun(IFilesHistory history);
		void Run();
	}
}