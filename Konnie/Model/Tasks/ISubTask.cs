namespace Konnie.Model.Tasks
{
	public interface ISubTask
	{
		string Type { get; }
		string Name { get; set; }
		bool CanRun(IFilesHistory history);
		void Run();
	}
}