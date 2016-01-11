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
		private string FilePath = "SomeFilePath";
		private string ConfigFileTemplate = @"<?xml version=""1.0"" encoding=""utf - 8""?>
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
		private const string VTwoName = "VTwo";
		private const string VTwoValue = "VTwoValue";
		private KVariableSet VariableSetOne = new KVariableSet
		{
			Name = VariableSetOneName,
			Variables = new Dictionary<string, string>
			{
				{VOneName, VOneValue },
				{VTwoName, VTwoValue },
			}
		};
		[Test]
		public void LaterVariableValuesOveridePreviousOnes()
		{
			throw new NotImplementedException();
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
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(true);
			var configFile = string.Format(
				ConfigFileTemplate,
				$"#{{{VOneName}}}",
				$"#{{{VTwoName}}}",
				"SomeValue");
			mockFileSystemHandler.Setup(f => f.ReadAllLines(FilePath)).Returns(configFile.Split('\n'));
			var varSets = new KVariableSets { VariableSetOne };

			task.Run(mockFileSystemHandler.Object, varSets);

			string expectedFileContent = string.Format(
				ConfigFileTemplate, 
				$"{VOneValue}", 
				$"{VTwoValue}", 
				"SomeValue");
			mockFileSystemHandler.Verify(f => f.WriteAllText(expectedFileContent, FilePath));
		}

		[Test]
		public void SubstitutionIsCaseInsensitive()
		{
			throw new NotImplementedException();
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

		[Test]
		public void ThrowsIfFileDoesntExist()
		{
			var task = new SubstitutionTask
			{
				Name = "SomeName", Logger = new ConsoleLogger(),
				FilePath = FilePath,
				SubstitutionVariableSets = new List<string> { "VariableSetOne" }
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(false);

			Assert.Throws<FileDoesntExist>(() => task.Run(mockFileSystemHandler.Object, null));
		}
	}
}