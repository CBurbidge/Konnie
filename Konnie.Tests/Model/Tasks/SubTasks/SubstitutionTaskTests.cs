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
		private const string VOneName = "VOne";
		private const string VOneValue = "VOneValue";
		private const string VOneValueLater = "VOneValueLater";
		private const string VTwoName = "VTwo";
		private const string VTwoValue = "VTwoValue";

		private readonly KVariableSet VariableSetOne = new KVariableSet
		{
			Name = VariableSetOneName,
			Variables = new Dictionary<string, string>
			{
				{VOneName, VOneValue},
				{VTwoName, VTwoValue}
			}
		};

		private readonly KVariableSet VariableSetTwo = new KVariableSet
		{
			Name = VariableSetTwoName,
			Variables = new Dictionary<string, string>
			{
				{VOneName, VOneValueLater}
			}
		};

		private Mock<IFileSystemHandler> GetMockFileSystemHandler(string configFile, string expectedFileContent)
		{
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath))
				.Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllLines(FilePath))
				.Returns(configFile.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries));
			mockFileSystemHandler.Setup(f => f.WriteAllText(It.IsAny<string>(), FilePath))
				.Callback<string, string>(
					(fileContents, filePath) => { Assert.That(fileContents, Is.EqualTo(expectedFileContent)); });
			return mockFileSystemHandler;
		}

		[Test]
		public void LaterVariableValuesOveridePreviousOnes()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName, VariableSetTwoName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VOneName}}}",
				$"#{{{VTwoName}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VOneValueLater}",
				$"{VTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, expectedFileContent);

			var sets = new KVariableSets {VariableSetOne, VariableSetTwo};
			task.Run(mockFileSystemHandler.Object, sets);
		}

		[Test]
		public void SubstitutesVariablesIntoFile()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VOneName}}}",
				$"#{{{VTwoName}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VOneValue}",
				$"{VTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, expectedFileContent);

			task.Run(mockFileSystemHandler.Object, new KVariableSets {VariableSetOne});
		}

		[Test]
		public void IgnoreVariablesThatArentInVariableSets()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VOneName}}}",
				$"#{{{VTwoName}}}",
				"#{SomeOtherVariable}");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VOneValue}",
				$"{VTwoValue}",
				"#{SomeOtherVariable}");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, expectedFileContent);

			task.Run(mockFileSystemHandler.Object, new KVariableSets {VariableSetOne});
		}

		[Test]
		public void SubstitutionIsCaseInsensitive()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VOneName.ToLower()}}}",
				$"#{{{VTwoName.ToLower()}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VOneValue}",
				$"{VTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, expectedFileContent);

			task.Run(mockFileSystemHandler.Object, new KVariableSets {VariableSetOne});
		}

		[Test]
		public void SubstitutionWorksWithWhitespaceOnEnds()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> {VariableSetOneName}
			};
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{ {VOneName}  }}",
				$"#{{   {VTwoName}}}",
				"SomeValue");
			var expectedFileContent = string.Format(
				ConfigFileTemplate,
				$"{VOneValue}",
				$"{VTwoValue}",
				"SomeValue");
			var mockFileSystemHandler = GetMockFileSystemHandler(configFile, expectedFileContent);

			task.Run(mockFileSystemHandler.Object, new KVariableSets {VariableSetOne});
		}

		[Test]
		public void ThrowsIfFileDoesntExist()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName",
				Logger = new ConsoleLogger(),
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
				Logger = new ConsoleLogger(),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string>()
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(false);

			Assert.Throws<InvalidProgramException>(() => task.Run(mockFileSystemHandler.Object, null));
		}
	}
}