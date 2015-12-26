﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Fclp;

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

			var argParser = new FluentCommandLineParser<CommandArgs>();
			
			argParser.Setup(arg => arg.Files)
				.As("files")
				.Required();
			argParser.Setup(arg => arg.Task)
				.As("task")
				.Required();

			var result = argParser.Parse(args);

			var commandArgs = CheckAndLogCommandArguments(result, argParser);

			EnsureFilesExistOnDisk(commandArgs);


		}

		private CommandArgs CheckAndLogCommandArguments(ICommandLineParserResult result, FluentCommandLineParser<CommandArgs> argParser)
		{
			if (result.HasErrors)
			{
				TellUserThereAreNotEnoughArguments();
				throw new ArgsParsingFailed();
			}

			var commandArgs = argParser.Object;
			_logLine($"Files: '{string.Join("','", commandArgs.Files)}'");
			_logLine($"Task: '{commandArgs.Task}'");
			return commandArgs;
		}

		private void EnsureFilesExistOnDisk(CommandArgs commandArgs)
		{
			foreach (var file in commandArgs.Files)
			{
				if (_fs.File.Exists(file) == false)
				{
					_logLine(string.Format(Wording.FileDoesntExistFailure, file));
					throw new KonnieFileDoesntExist(file);
				}
				_logLine($"File '{file}' exists.");
			}
		}

		private void TellUserThereAreNotEnoughArguments()
		{
			_logLine(Wording.NeedToPassArgumentsWarning);
			_logLine(Wording.ArgumentsDescription);
		}

		public class Wording
		{
			public const string NeedToPassArgumentsWarning = "Need to pass in arguments to Konnie";
			public const string ArgumentsDescription = "Can pass in multiple .konnie files with --files and then a single task to run with --task";
			public const string FileDoesntExistFailure = "File {0} in argument doesn't exist.";
		}

		public class CommandArgs
		{
			public string Task { get; set; }
			public List<string> Files { get; set; }
		}
	}

}