using System;

namespace Konnie
{
	public class KonnieProgram
	{
		private readonly Action<string> _logLine;

		public KonnieProgram(Action<string> logLine)
		{
			_logLine = logLine;
		}

		public void Run(string[] args)
		{
			_logLine("Started Konnie...");
			var argsAsLine = string.Join(",", args);
			_logLine($"Called with args {argsAsLine}");

		}
	}
}