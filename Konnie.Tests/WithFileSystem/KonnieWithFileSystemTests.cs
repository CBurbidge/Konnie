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
		private readonly string _baseFolderPath = Path.Combine(Environment.CurrentDirectory, nameof(WithFileSystem));

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

		[Test]
		public void CopyFileAndAssertNoMoreVariablesInFileFails()
		{
			var taskName = "CopyAndAssertVariables";
			var copyTaskName = "CopyTaskName";
			var assertNoMoreVariablesTaskName = "AssertNoMoreVariablesTaskName";
			var destinationConfig = "ConfigCopied.xml";
			var destinationConfigFilePath = Path.Combine(_baseFolderPath, destinationConfig);
			File.WriteAllText(destinationConfigFilePath, "");
			var kFileCopyTaskName = nameof(CopyFileTask) + ".konnie";
			var kFile = new KFile
			{
				Tasks = new KTasks
				{
					new KTask
					{
						Name = taskName,
						SubTasksToRun = new List<string>
						{
							copyTaskName,
							assertNoMoreVariablesTaskName
						}
					}
				},
				SubTasks = new KSubTasks
				{
					new AssertNoMoreVariablesInFileTask
					{
						Name = assertNoMoreVariablesTaskName,
						FilePath = destinationConfig
					}
				},
				ExtraFiles = new List<string> { kFileCopyTaskName}
			};

			var copyTaskFile = new KFile
			{
				SubTasks = new KSubTasks
				{
					new CopyFileTask
					{
						Name = copyTaskName,
						Source = "Config.xml",
						Destination = destinationConfig
					}
				}
			};

			var converter = new KFileConverter(new ConsoleLogger(true), new FileSystem());
			var kFileName = nameof(CopyFileAndAssertNoMoreVariablesInFileFails) + ".konnie";
			var kFilePath = Path.Combine(_baseFolderPath, kFileName);
			if (File.Exists(kFilePath) == false)
			{
				File.WriteAllText(kFilePath, converter.Serialize(kFile));
			}
			var kFileCopyTaskPath = Path.Combine(_baseFolderPath, kFileCopyTaskName);
			if (File.Exists(kFileCopyTaskPath) == false)
			{
				File.WriteAllText(kFileCopyTaskPath, converter.Serialize(copyTaskFile));
			}
			var args = new KonnieProgramArgs
			{
				Files = new List<string> {kFilePath},
				ProjectDir = _baseFolderPath,
				Task = taskName
			};
			var runner = new TaskRunner();

			Assert.Throws<VariablesStillExistInFile>(() => runner.Run(args));
		}

		[Test]
		public void AssertLackOfXPathFails()
		{
			var taskName = "AssertLackOfXPathFails";
			var assertLackOfXPathTask = "AssertLackOfXPathTaskName";
			var kFile = new KFile
			{
				Tasks = new KTasks
				{
					new KTask
					{
						Name = taskName,
						SubTasksToRun = new List<string>
						{
							assertLackOfXPathTask
						}
					}
				},
				SubTasks = new KSubTasks
				{
					new AssertLackOfXPathTask
					{
						Name = assertLackOfXPathTask,
						FilePath = "Config.xml",
						XPaths = new List<string>
						{
							"//appSettings/add[@key = 'SettingOne']"
						}
					}
				}
			};
			var converter = new KFileConverter(new ConsoleLogger(true), new FileSystem());
			var kFileName = nameof(AssertLackOfXPathFails) + ".konnie";
			var kFilePath = Path.Combine(_baseFolderPath, kFileName);
			if (File.Exists(kFilePath) == false)
			{
				File.WriteAllText(kFilePath, converter.Serialize(kFile));
			}
			var args = new KonnieProgramArgs
			{
				Files = new List<string> {kFilePath},
				ProjectDir = _baseFolderPath,
				Task = taskName
			};
			var runner = new TaskRunner();

			Assert.Throws<ElementExistsAtXPath>(() => runner.Run(args));
		}

		[Test]
		public void TransformsAndSubstitutionsWork()
		{
			var transformOutputFile = "ConfigTransformed.xml";
			var transformOutputFilePath = Path.Combine(_baseFolderPath, transformOutputFile);
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
							{"VariableOne", "VariableOneValue"},
							{"VariableTwo", "VariableTwoValue"},
							{"VariableThree", "VariableThreeValue"}
						}
					}
				}
			};
			var converter = new KFileConverter(new ConsoleLogger(true), new FileSystem());
			var kFileName = nameof(TransformsAndSubstitutionsWork) + ".konnie";
			var kFilePath = Path.Combine(_baseFolderPath, kFileName);
			if (File.Exists(kFilePath) == false)
			{
				File.WriteAllText(kFilePath, converter.Serialize(kFile));
			}

			var args = new KonnieProgramArgs
			{
				Files = new List<string> {kFilePath},
				ProjectDir = _baseFolderPath,
				Task = taskName
			};
			var runner = new TaskRunner();
			runner.Run(args);

			var result = File.ReadAllText(transformOutputFilePath);
			var expected = File.ReadAllText(Path.Combine(_baseFolderPath, "ConfigExpected.xml"));
			AssertEqualityIgnoringWhitespace(result, expected);
		}
	}
}