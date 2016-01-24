using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Konnie.Model.File;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class SubstitutionTask : ISubTaskThatUsesVariableSets
	{
		public string FilePath { get; set; }
		public string OptionalOutputFile { get; set; }
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(SubstitutionTask);

		public bool NeedToRun(IFileSystemHandler fileSystemHandler)
		{
			// Todo, do this properly
			return false;
		}

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			Logger.Verbose($"Starting task '{Name}'");
			CheckVariableSets();
			CheckFileExists(fileSystemHandler);

			Func<string, string> transformToComparableState = s => s.ToLower().Trim();

			var subsVals = new Dictionary<string, string>();
			var releventVariableSets = variableSets.Where(v => SubstitutionVariableSets.Contains(v.Name));
			foreach (var variableSet in releventVariableSets)
			{
				foreach (var kvp in variableSet.Variables)
				{
					if (string.IsNullOrWhiteSpace(kvp.Value) && variableSet.IgnoreBlankValues)
					{
						continue;
					}

					subsVals[transformToComparableState(kvp.Key)] = kvp.Value;
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

						var comparableState =  transformToComparableState(variableName);
						if (subsVals.ContainsKey(comparableState))
						{
							var value = subsVals[comparableState];
							lineToAdd = line.Replace($"#{{{variableName}}}", value);
						}
					}
				}

				transformedLines.Add(lineToAdd);
			}

			var fileContents = string.Join(Environment.NewLine, transformedLines);

			var filePathToWriteTo = string.IsNullOrWhiteSpace(OptionalOutputFile) 
				? FilePath 
				: OptionalOutputFile;

			fileSystemHandler.WriteAllText(fileContents, filePathToWriteTo);
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
				throw new FileDoesntExist(fileSystemHandler.GetAbsPath(FilePath));
			}
		}

		public List<string> SubstitutionVariableSets { get; set; }
	}
}