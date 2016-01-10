using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using Konnie.Model.Tasks.SubTasks;
using Konnie.Runner;
using Konnie.Runner.Logging;
using Moq;
using NUnit.Framework;

namespace Konnie.Tests.Model.Tasks.SubTasks
{
	[TestFixture]
	public class AssertLackOfXPathTaskTests
	{
		private const string FilePath = "somePath";
		private const string ConfigXml = @"<?xml version=""1.0"" encoding=""utf - 8""?>
	<configuration >
		<appSettings >
			<add key = ""ThingLocation"" value = ""c:\Dev\temp\"" />
			<add key = ""TimeInterval"" value = ""2400"" />
		</appSettings >
	</configuration > ";

		[Test]
		public void ThrowsIfFileDoesntExist()
		{
			var task = new AssertLackOfXPathTask
			{
				Logger = new ConsoleLogger(), Name = "",
				FilePath = FilePath
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(false);

			Assert.Throws<FileDoesntExist>(() => task.Run(mockFileSystemHandler.Object, null));
		}
		
		[Test]
		public void ThrowsIfXPathIsInvalid()
		{
			var invalidXPath = "//appSettings/add { @key = 'ThingLocation' } ";
			var task = new AssertLackOfXPathTask
			{
				Logger = new ConsoleLogger(),
				Name = "",
				FilePath = FilePath,
				XPaths = new List<string> { invalidXPath }
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllText(FilePath)).Returns(ConfigXml);

			Assert.Throws<XPathException>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void ThrowsIfXPathElementExists()
		{
			var xPathOfExistingElement = "//appSettings/add[@key = 'ThingLocation']";
			var task = new AssertLackOfXPathTask
			{
				Logger = new ConsoleLogger(),
				Name = "",
				FilePath = FilePath,
				XPaths = new List<string> {xPathOfExistingElement}
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllText(FilePath)).Returns(ConfigXml);

			Assert.Throws<ElementExistsAtXPath>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void ValidXPathNotMatchingAnythingDoesntThrow()
		{
			var xPathOfNonExistingElement = "//appSettings/add[@key = 'SomethingElse']";
			var task = new AssertLackOfXPathTask
			{
				Logger = new ConsoleLogger(),
				Name = "",
				FilePath = FilePath,
				XPaths = new List<string> { xPathOfNonExistingElement }
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(FilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllText(FilePath)).Returns(ConfigXml);

			task.Run(mockFileSystemHandler.Object, null);
		}
	}

}