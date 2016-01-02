namespace Konnie.Runner.Logging
{
	public interface ILogger
	{
		void Verbose(string line);
		void Terse(string line);
		string GetLog(LogType logType);
	}
}