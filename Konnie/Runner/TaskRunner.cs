using System.IO.Abstractions;

namespace Konnie.Runner
{
	public class TaskRunner : IKonnieRunner
	{
		private readonly KonnieProgramArgs _args;

		public TaskRunner(KonnieProgramArgs args, IFileSystem fs = null)
		{
			_args = args;


		}

		public void Run()
		{
			
		}
	}

	public interface IKonnieRunner
	{
		void Run();
	}
}