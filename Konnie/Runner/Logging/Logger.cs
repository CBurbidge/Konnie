using System;
using System.Collections.Generic;
using System.Linq;

namespace Konnie.Runner.Logging
{
	public class Logger : ILogger
	{
		private readonly List<LineAndType> _logLines = new List<LineAndType>();
		protected Action<string, LogType> ContinualLogger;

		public Logger(Action<string, LogType> continualLogger = null)
		{
			ContinualLogger = continualLogger;
		}

		public void Verbose(string line)
		{
			_logLines.Add(new LineAndType(line, LogType.Verbose));

			ContinualLogger?.Invoke(line, LogType.Verbose);
		}

		public void Terse(string line)
		{
			_logLines.Add(new LineAndType(line, LogType.Terse));

			ContinualLogger?.Invoke(line, LogType.Terse);
		}

		public string GetLog(LogType logType)
		{
			if (logType == LogType.Terse)
			{
				return string.Join(
					Environment.NewLine,
					_logLines
						.Where(lt => lt.Type == LogType.Terse)
						.Select(lt => lt.Line));
			}

			return string.Join(
				Environment.NewLine,
				_logLines.Select(lt => lt.Line));
		}

		public class LineAndType
		{
			public LineAndType(string line, LogType type)
			{
				Type = type;
				Line = line;
			}

			public string Line { get; }
			public LogType Type { get; }
		}
	}

	public class ConsoleLogger : Logger
	{
		public ConsoleLogger() : base(
			(line, logType) =>
			{
				Console.ForegroundColor =
					logType == LogType.Verbose
						? ConsoleColor.Green
						: ConsoleColor.DarkYellow;
				Console.WriteLine(line);
			})
		{
		}
	}
}