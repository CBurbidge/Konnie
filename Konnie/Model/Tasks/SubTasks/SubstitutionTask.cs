using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class SubstitutionTask : ISubTaskThatUsesVariableSets
	{
		public string FilePath { get; set; }
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(SubstitutionTask);

		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			Logger.Verbose($"Starting task '{Name}'");
			CheckVariableSets();
			CheckFileExists(fileSystemHandler);

			var subsVals = new Dictionary<string, string>();
			var releventVariableSets = variableSets.Where(v => SubstitutionVariableSets.Contains(v.Name));
			foreach (var variableSet in releventVariableSets)
			{
				foreach (var kvp in variableSet.Variables)
				{
					subsVals[kvp.Key.ToLower().Trim()] = kvp.Value;
				}
			}

			var transformedLines = new List<string>();
			foreach (var line in fileSystemHandler.ReadAllLines(FilePath))
			{
				var lineToAdd = line;

				if (Variable.VariableRegex.IsMatch(line))
				{
					foreach (Match match in Variable.VariableRegex.Matches(line))
					{
						var variableName = match.Groups["name"].Value;

						var lower = variableName.ToLower().Trim();
						if (subsVals.ContainsKey(lower))
						{
							var value = subsVals[lower];
							lineToAdd = line.Replace($"#{{{variableName}}}", value);
						}
					}
				}

				transformedLines.Add(lineToAdd);
			}

			var fileContents = string.Join(Environment.NewLine, transformedLines);
			fileSystemHandler.WriteAllText(fileContents, FilePath);
		}

		private void CheckVariableSets()
		{
			if (SubstitutionVariableSets.Count == 0)
			{
				Logger.Terse("Can't proceed as SubstitutionVariableSets not specified.");
				throw new InvalidProgramException("Can't run as no substitution variable sets are specified.");
			}
		}

		private void CheckFileExists(IFileSystemHandler fileSystemHandler)
		{
			if (fileSystemHandler.Exists(FilePath) == false)
			{
				Logger.Terse($"File '{FilePath}' doesn't exist, can't continue");
				throw new FileDoesntExist(FilePath);
			}
		}

		public List<string> SubstitutionVariableSets { get; set; }
	}
}