using Konnie.Model.File;

namespace Konnie.Model.Tasks
{
	public interface ISubTask
	{
		string Type { get; }
		string Name { get; set; }
		bool NeedToRun(IFilesHistory history);
		void Run();
	}
}