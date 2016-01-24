using System;
using System.Collections.Generic;
using Konnie.Model.File;
using Konnie.Model.Tasks.SubTasks;
using Konnie.Runner;
using Konnie.Runner.Logging;
using Moq;
using NUnit.Framework;

namespace Konnie.Tests.Model.Tasks.SubTasks
{
	[TestFixture]
	public class SubstitutionTaskTests
	{
		private readonly string FilePath = "SomeFilePath";
		private readonly string ConfigFileTemplate = @"<?xml version=""1.0"" encoding=""utf - 8""?>
	<configuration >
		<appSettings >
			<add key = ""SettingOne"" value = ""{0}"" />
			<add key = ""SettingTwo"" value = ""{1}"" />
			<add key = ""SettingThree"" value = ""{2}"" />
		</appSettings >
	</configuration >";
		private const string VariableSetOneName = "VariableSetOne";
		private const string VariableSetTwoName = "VariableSetTwo";
		private const string VariableSetNonOverideName = "VariableSetNonOveride";
		private const string VarOneName = "VOne";
		private const string VSet1VarOneValue = "VOneValue";
		private const string VSet2VarOneValue = "VOneValueLater";
		private const string VarTwoName = "VTwo";
		private const string VSet1VarTwoValue = "VTwoValue";

		private readonly KVariableSet VariableSetOne = new KVariableSet
		{
			Name = VariableSetOneName,
			Variables = new Dictionary<string, string>
			{
				{VarOneName, VSet1VarOneValue},
				{VarTwoName, VSet1VarTwoValue}
			}
		};

		private readonly KVariableSet VariableSetTwo = new KVariableSet
		{
			Name = VariableSetTwoName,
			Variables = new Dictionary<string, string>
			{
				{VarOneName, VSet2VarOneValue}
			}
		};

		private readonly KVariableSet VariableSetNonOveride = new KVariableSet
		{
			Name = VariableSetNonOverideName,
			Variables = new Dictionary<string, string>
			{
				{VarOneName, ""}
			},
			IgnoreBlankValues = true
		};

		private string OptionalOutputFile = "OptionalOutputFilePath";

		private Mock<IFileSystemHandler> GetMockFileSystemHandler(string configFile, string expectedFilePath, string expectedFileContent)
		{
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath))
				.Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllLines(FilePath))
				.Returns(configFile.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries));
			mockFileSystemHandler.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
				.Callback<string, string>(
					(fileContents, filePath) =>
					{
						Assert.That(fileContents, Is.EqualTo(expectedFileContent));
						Assert.That(filePath, Is.EqualTo(expectedFilePath));
					});
			return mockFileSystemHandler;
		}

		[Test]
		public void LaterVariableValuesOveridePreviousOnes()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName, VariableSetTwoName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VarOneName}}}",
				$"#{{{VarTwoName}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VSet2VarOneValue}",
				$"{VSet1VarTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, FilePath, expectedFileContent);

			var sets = new KVariableSets {VariableSetOne, VariableSetTwo};
			task.Run(mockFileSystemHandler.Object, sets);
		}

		[Test]
		public void VariableDoesntOverideIfVariableSetIgnoresBlankValues()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName, VariableSetNonOverideName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VarOneName}}}",
				$"#{{{VarTwoName}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VSet1VarOneValue}",
				$"{VSet1VarTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, FilePath, expectedFileContent);

			var sets = new KVariableSets {VariableSetOne, VariableSetNonOveride};
			task.Run(mockFileSystemHandler.Object, sets);
		}

		[Test]
		public void SubstitutesVariablesIntoFile()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VarOneName}}}",
				$"#{{{VarTwoName}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VSet1VarOneValue}",
				$"{VSet1VarTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, FilePath, expectedFileContent);

			task.Run(mockFileSystemHandler.Object, new KVariableSets {VariableSetOne});
		}

		[Test]
		public void SubstitutionHandlesTwoVariablesOnOneLine()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VarOneName}}}#{{{VarTwoName}}}",
				$"#{{{VarTwoName}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VSet1VarOneValue + VSet1VarTwoValue}",
				$"{VSet1VarTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, FilePath, expectedFileContent);

			task.Run(mockFileSystemHandler.Object, new KVariableSets {VariableSetOne});
		}

		[Test]
		public void WritesToOptionalOutputFileIfItIsNotNull()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				OptionalOutputFile = OptionalOutputFile,
                SubstitutionVariableSets = new List<string> {VariableSetOneName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VarOneName}}}",
				$"#{{{VarTwoName}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VSet1VarOneValue}",
				$"{VSet1VarTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, OptionalOutputFile, expectedFileContent);

			task.Run(mockFileSystemHandler.Object, new KVariableSets {VariableSetOne});
		}

		[Test]
		public void IgnoreVariablesThatArentInVariableSets()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VarOneName}}}",
				$"#{{{VarTwoName}}}",
				"#{SomeOtherVariable}");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VSet1VarOneValue}",
				$"{VSet1VarTwoValue}",
				"#{SomeOtherVariable}");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, FilePath, expectedFileContent);

			task.Run(mockFileSystemHandler.Object, new KVariableSets {VariableSetOne});
		}

		[Test]
		public void SubstitutionIsCaseInsensitive()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VarOneName.ToLower()}}}",
				$"#{{{VarTwoName.ToLower()}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VSet1VarOneValue}",
				$"{VSet1VarTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, FilePath, expectedFileContent);

			task.Run(mockFileSystemHandler.Object, new KVariableSets {VariableSetOne});
		}

		[Test]
		public void SubstitutionWorksWithWhitespaceOnEnds()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{ {VarOneName}  }}",
				$"#{{   {VarTwoName}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VSet1VarOneValue}",
				$"{VSet1VarTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, FilePath, expectedFileContent);

			task.Run(mockFileSystemHandler.Object, new KVariableSets {VariableSetOne});
		}

		[Test]
		public void ThrowsIfFileDoesntExist()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {"VariableSetOne"}
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(false);

			Assert.Throws<FileDoesntExist>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void ThrowsIfNoSubstitutionSetsAreSpecified()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string>()
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(false);

			Assert.Throws<InvalidProgramException>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void NeedToRunReturnsFalse()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(true),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {"VarSet"}
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();

			Assert.That(task.NeedToRun(mockFileSystemHandler.Object), Is.False);
		}
	}
}