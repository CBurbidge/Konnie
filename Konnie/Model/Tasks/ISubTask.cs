using System.Collections.Generic;

namespace Konnie.Model.Tasks
{
	public interface ISubTask
	{
		string Type { get; }
		string Name { get; set; }
		bool NeedToRun(IFilesHistory history);
		void Run();
	}
	public interface IUsesVariableSets
	{
		List<string> SubstitutionVariableSets { get; set; }
	}
	public interface ISubTaskThatUsesVariableSets : ISubTask, IUsesVariableSets
	{
	}
}