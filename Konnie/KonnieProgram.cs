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

			if (args.Length < 2)
			{
				_logLine(Wording.NeedToPassArgumentsWarning);
				_logLine(Wording.NeedToPassTwoArgumentsIn);
				_logLine(Wording.ArgumentsDescription);
				return;
			}


		}

		public class Wording
		{
			public const string NeedToPassArgumentsWarning = "Need to pass in arguments to Konnie";
			public const string ArgumentsDescription = "Can pass in multiple .konnie files and then a single task to run";
			public const string NeedToPassTwoArgumentsIn = "Need to pass in at least 2 arguments";
			public const string FileDoesntExistFailure = "File {0} in argument doesn't exist.";
		}
	}
}