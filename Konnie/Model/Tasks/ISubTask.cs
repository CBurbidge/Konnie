using System.Collections.Generic;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;
using Newtonsoft.Json;

namespace Konnie.Model.Tasks
{
	public interface ISubTask
	{
		string Type { get; }
		string Name { get; set; }

		[JsonIgnore]
		ILogger Logger { get; set; }
		bool NeedToRun(IFilesHistory history);
		void Run(IFileSystemHandler fileSystemHandler);
	}

	public interface ISubTaskThatUsesVariableSets : ISubTask
	{
		List<string> SubstitutionVariableSets { get; set; }
	}
}