namespace Konnie.Runner
{
	public class KonnieRunnerFactory
	{
		public IKonnieRunner CreateRunner(KonnieProgramArgs args)
		{
			return new TaskRunner(args);
		}
	}
}