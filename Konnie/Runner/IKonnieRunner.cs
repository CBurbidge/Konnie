namespace Konnie.Runner
{
	public interface IKonnieRunner 
	{
		
	}

	public class TaskRunner : IKonnieRunner
	{
		private readonly KonnieProgramArgs _args;

		public TaskRunner(KonnieProgramArgs args)
		{
			_args = args;
		}
	}

	public class IKonnieRunnerFactory
	{
		public IKonnieRunner CreateRunner(KonnieProgramArgs args)
		{
			return new TaskRunner(args);
		}
	}

	/// <summary>
	/// FileSystemHandler is a wrapper around an IFileSystem object.
	/// This will load the IFileHistory object and makes sure that any changes to files are kept in sync.
	/// </summary>
	public class FileSystemHandler
	{
		
	}
}