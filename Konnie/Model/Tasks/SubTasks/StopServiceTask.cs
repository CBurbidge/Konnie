using System.ServiceProcess;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class StopServiceTask : ISubTask
	{
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(StopServiceTask);
		public string ServiceName { get; set; }

		public bool NeedToRun(IFilesHistory history)
		{
			return false;
		}

		public void Run(IFileSystemHandler fileSystemHandler)
		{
			Logger.Terse($"Stopping service {ServiceName}");
			var service = new ServiceController(ServiceName);
			service.Stop();
		}
	}
}