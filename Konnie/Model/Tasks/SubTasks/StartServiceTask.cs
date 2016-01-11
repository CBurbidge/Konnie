using System.ServiceProcess;
using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class StartServiceTask : ISubTask
	{
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(StartServiceTask);
		public string ServiceName { get; set; }

		public bool NeedToRun(IFileSystemHandler fileSystemHandler)
		{
			return false;
		}

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			Logger.Terse($"Starting service {ServiceName}");
			var service = new ServiceController(ServiceName);
			service.Start();
		}
	}
}