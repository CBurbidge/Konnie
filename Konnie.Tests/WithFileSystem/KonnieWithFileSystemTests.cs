using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Konnie.InzOutz;
using Konnie.Model.File;
using Konnie.Model.Tasks.SubTasks;
using Konnie.Runner;
using Konnie.Runner.Logging;
using NUnit.Framework;

namespace Konnie.Tests.WithFileSystem
{
	[TestFixture]
	public class KonnieWithFileSystemTests
	{
		[Test]
		public void TransformsAndSubstitutionsWork()
		{
			var currentDirectory = Environment.CurrentDirectory;
			var baseFolderPath = Path.Combine(currentDirectory, nameof(WithFileSystem));

			var transformOutputFile = "ConfigTransformed.xml";
			var transformOutputFilePath = Path.Combine(baseFolderPath, transformOutputFile);
			File.WriteAllText(transformOutputFilePath, ""); // This file needs to exist for konnie to run.
			var variableSetName = "VariableSetOne";
			var transformTaskName = "TransformWithReleaseFile";
			var subsTaskName = "SubstituteVariablesIntoTransformedFile";
			var taskName = "TransformAndSubs";
			var kFile = new KFile
			{
				Tasks = new KTasks
				{
					new KTask
					{
						Name = taskName,
						SubTasksToRun = new List<string>
						{
							transformTaskName,
							subsTaskName
						}
					}
				},
				SubTasks = new KSubTasks
				{
					new TransformTask
					{
						Name = transformTaskName,
						InputFile = "Config.xml",
						OutputFile = transformOutputFile,
						TransformFiles = new List<string>
						{
							"Config.Release.xml"
						}
					},
					new SubstitutionTask
					{
						Name = subsTaskName,
						FilePath = transformOutputFile,
						SubstitutionVariableSets = new List<string>
						{
							variableSetName
						}
					}
				},
				VariableSets = new KVariableSets
				{
					new KVariableSet
					{
						Name = variableSetName,
						Variables = new Dictionary<string, string>
						{
							{"VariableOne", "VariableOneValue" },
							{"VariableTwo", "VariableTwoValue" },
							{"VariableThree", "VariableThreeValue" },
						}
					}
				}
			};
			var converter = new KFileConverter(new ConsoleLogger(), new FileSystem());
			var kFileName = nameof(TransformsAndSubstitutionsWork) + ".konnie";
			var kFilePath = Path.Combine(baseFolderPath, kFileName);
			File.WriteAllText(kFilePath, converter.Serialize(kFile));

			var args = new KonnieProgramArgs
			{
				Files = new List<string> { kFilePath},
				ProjectDir = baseFolderPath,
				Task = taskName
			};
			var runner = new TaskRunner();
			runner.Run(args);

			var result = File.ReadAllText(transformOutputFilePath);
			var expected = File.ReadAllText(Path.Combine(baseFolderPath, "ConfigExpected.xml"));
			AssertEqualityIgnoringWhitespace(result, expected);
		}

		private void AssertEqualityIgnoringWhitespace(string result, string expected)
		{
			Assert.That(result
							.Replace(" ", "") 
							.Replace("\n", "") 
							.Replace("\r", "") 
							.Replace("\t", "")
							, Is.EqualTo(
						expected
							.Replace(" ", "")
							.Replace("\n", "")
							.Replace("\r", "")
							.Replace("\t", "")));
		}
	}
}
