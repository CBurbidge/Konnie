﻿using Fclp;
using Konnie.Runner;

namespace Konnie.Application
{
	public class KonnieProgram
	{
		public void Run(string[] args)
		{
			var argParser = new FluentCommandLineParser<KonnieProgramArgs>();
			
			argParser.Setup(arg => arg.ProjectDir)
				.As("proj-dir")
				.Required();
			argParser.Setup(arg => arg.Files)
				.As("files")
				.Required();
			argParser.Setup(arg => arg.Task)
				.As("task")
				.Required();
			argParser.Setup(arg => arg.HistoryFile)
				.As("history");
			argParser.Setup(arg => arg.Verbose)
				.As("verbose");

			var result = argParser.Parse(args);

			if (result.HasErrors)
			{
				throw new ArgsParsingFailed(result.ErrorText);
			}

			var commandArgs = argParser.Object;
			
			var taskRunner = new TaskRunner();
			taskRunner.Run(commandArgs);
		}
	}
}