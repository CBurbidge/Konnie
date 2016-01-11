using System;
using System.Collections.Generic;
using System.Xml.XPath;
using Konnie.Model.FilesHistory;
using Konnie.Model.Tasks.SubTasks;
using Konnie.Runner;
using Konnie.Runner.Logging;
using Moq;
using NUnit.Framework;

namespace Konnie.Tests.Model.Tasks.SubTasks
{
	[TestFixture]
	public class AssertNoMoreVariablesInFileTaskTests
	{
		private const string FilePath = "somePath";
		private const string ConfigXmlTemplate = @"<?xml version=""1.0"" encoding=""utf - 8""?>
	<configuration >
		<appSettings >
			<add key = ""ThingLocation"" value = ""{0}"" />
			<add key = ""TimeInterval"" value = ""100"" />
		</appSettings >
	</configuration > ";

		[Test]
		public void ThrowsIfFileDoesntExist()
		{
			var task = new AssertNoMoreVariablesInFileTask
			{
				Logger = new Logger(), Name = "",
				FilePath = FilePath
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(false);

			Assert.Throws<FileDoesntExist>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void ThrowsIfThereArePeicesOfVariableSyntaxInFile()
		{
			var task = new AssertNoMoreVariablesInFileTask
			{
				Logger = new Logger(), Name = "",
				FilePath = FilePath
			};
			var fileContents = string.Format(ConfigXmlTemplate, "#{SomeVariable}");
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllText(FilePath)).Returns(fileContents);

			Assert.Throws<VariablesStillExistInFile>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void DoesntThrowIfNoVariablesExistInFile()
		{
			var task = new AssertNoMoreVariablesInFileTask
			{
				Logger = new Logger(), Name = "",
				FilePath = FilePath
			};
			var fileContents = string.Format(ConfigXmlTemplate, "AValueOfSomeSort");
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllText(FilePath)).Returns(fileContents);

			task.Run(mockFileSystemHandler.Object, null);
		}

		[Test]
		public void NeedsToRunReturnsFalse()
		{
			var task = new AssertNoMoreVariablesInFileTask
			{
				Logger = new Logger(),
				Name = "",
				FilePath = FilePath,
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();

			Assert.That(task.NeedToRun(mockFileSystemHandler.Object), Is.False);
		}
	}
}