using System;
using System.IO.Abstractions;
using System.Linq;

namespace Konnie
{
	public class KonnieProgram
	{
		private readonly IFileSystem _fs;
		private readonly Action<string> _logLine;

		public KonnieProgram(Action<string> logLine = null, IFileSystem fs = null)
		{
			_fs = fs ?? new FileSystem();
			_logLine = logLine ?? (Console.WriteLine);
		}

		public void Run(string[] args)
		{
			_logLine("Started Konnie...");
			var argsAsLine = string.Join(",", args);
			_logLine($"Called with args {argsAsLine}");

			var numberOrArguments = args.Length;

			if (numberOrArguments < 2)
			{
				TellUserTherereNotEnoughArguments();
				return;
			}

			var konnieFiles = args.Take(numberOrArguments - 1);
			var konnieTask = args[numberOrArguments - 1];

			foreach (var konnieFile in konnieFiles)
			{
				_logLine($"Checking existance of file '{konnieFile}'");
				if (_fs.File.Exists(konnieFile) == false)
				{
					
				}
			}
		}

		private void TellUserTherereNotEnoughArguments()
		{
			_logLine(Wording.NeedToPassArgumentsWarning);
			_logLine(Wording.NeedToPassTwoArgumentsIn);
			_logLine(Wording.ArgumentsDescription);
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