using Konnie.Model.File;

namespace Konnie.Model.Tasks
{
	public interface ISubTask
	{
		string Type { get; }
		string Name { get; set; }
		bool CanRun(KVariableSets variableSets, IFilesHistory history);
		void Run();
	}
}