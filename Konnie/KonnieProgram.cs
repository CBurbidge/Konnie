﻿using System.IO.Abstractions;
using Fclp;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie
{
	public class KonnieProgram
	{
		public void Run(string[] args)
		{
			var argParser = new FluentCommandLineParser<KonnieProgramArgs>();
			
			argParser.Setup(arg => arg.Files)
				.As("files")
				.Required();
			argParser.Setup(arg => arg.Task)
				.As("task")
				.Required();

			var result = argParser.Parse(args);

			if (result.HasErrors)
			{
				throw new ArgsParsingFailed(result.ErrorText);
			}

			var commandArgs = argParser.Object;
			
			var taskRunner = new TaskRunner();
			taskRunner.Run(commandArgs, new ConsoleLogger(), new FileSystem());
		}
	}
}