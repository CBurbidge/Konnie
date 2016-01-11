using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Konnie.Model.Tasks.SubTasks;
using Konnie.Runner;
using Konnie.Runner.Logging;
using Moq;
using NUnit.Framework;

namespace Konnie.Tests.Model.Tasks.SubTasks
{
	[TestFixture]
	public class TransformTaskTests
	{
		private readonly string InputFilePath = "InputFile";
		private readonly string OutputFilePath = "OutputFile";
		private readonly string TransformFilePathOne = "TransformFileOne";
		private string TransformFilePathTwo = "TransformFileTwo";

		public Stream GenerateStreamFromString(string s)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		[Test]
		public void ThrowsIfAnyTransformFilePathDoesntExist()
		{
			var task = new TransformTask
			{
				Logger = new ConsoleLogger(),
				Name = "",
				InputFile = InputFilePath,
				OutputFile = OutputFilePath,
				TransformFiles = new List<string> {TransformFilePathOne}
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(InputFilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(OutputFilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(TransformFilePathOne)).Returns(false);

			Assert.Throws<FileDoesntExist>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void ThrowsIfInputFilePathDoesntExist()
		{
			var task = new TransformTask
			{
				Logger = new ConsoleLogger(),
				Name = "",
				InputFile = InputFilePath,
				OutputFile = OutputFilePath,
				TransformFiles = new List<string> {TransformFilePathOne}
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(OutputFilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(TransformFilePathOne)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(InputFilePath)).Returns(false);

			Assert.Throws<FileDoesntExist>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void ThrowsIfOutputFilePathDoesntExist()
		{
			var task = new TransformTask
			{
				Logger = new ConsoleLogger(),
				Name = "",
				InputFile = InputFilePath,
				OutputFile = OutputFilePath,
				TransformFiles = new List<string> {TransformFilePathOne}
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(TransformFilePathOne)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(InputFilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(OutputFilePath)).Returns(false);

			Assert.Throws<FileDoesntExist>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void ThrowsIfTransformFileListIsEmpty()
		{
			var task = new TransformTask
			{
				Logger = new ConsoleLogger(),
				Name = "",
				InputFile = InputFilePath,
				OutputFile = OutputFilePath,
				TransformFiles = new List<string>()
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(InputFilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(OutputFilePath)).Returns(true);

			Assert.Throws<InvalidProgramException>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void TransformsFile()
		{
			var inputXmlFile =
				@"<?xml version=""1.0"" encoding=""utf - 8""?>
	<configuration >
		<appSettings >
			<add key = ""SettingOne"" value = ""SettingValueOne"" />
			<add key = ""SettingTwo"" value = ""SettingValueTwo"" />
			<add key = ""SettingThree"" value = ""SettingValueThree"" />
		</appSettings >
	</configuration > ";
			var transformFile =
				@"﻿<?xml version=""1.0"" encoding=""utf-8""?>

	<configuration xmlns:xdt = ""http://schemas.microsoft.com/XML-Document-Transform"" >
		<appSettings>
			<add key=""SettingTwo"" xdt:Transform=""RemoveAll"" xdt:Locator=""Match(key)"" />
			<add key=""SettingThree"" xdt:Transform=""Replace"" xdt:Locator=""Match(key)"" value=""SomeOtherValue"" />
		</appSettings>
	</configuration> ";
			Func<string, Stream> getTransformFile = f => GenerateStreamFromString(transformFile);
			var task = new TransformTask(getTransformFile)
			{
				Logger = new ConsoleLogger(),
				Name = "",
				InputFile = InputFilePath,
				OutputFile = OutputFilePath,
				TransformFiles = new List<string> {TransformFilePathOne}
			};

			var expectedOutputFile = @"<?xml version=""1.0"" encoding=""utf - 8""?>
<configuration>
  <appSettings>
    <add key=""SettingOne"" value=""SettingValueOne"" />
    <add key=""SettingThree"" value=""SomeOtherValue"" />
  </appSettings>
</configuration>";
			var expectedXmlDoc = new XmlDocument();
			expectedXmlDoc.Load(new StringReader(expectedOutputFile));

			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(InputFilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllText(InputFilePath))
				.Returns(inputXmlFile);
			mockFileSystemHandler.Setup(f => f.Exists(OutputFilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(TransformFilePathOne)).Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllText(TransformFilePathOne))
				.Returns(transformFile);
			mockFileSystemHandler.Setup(f => f.SaveXDocument(It.IsAny<XmlDocument>(), OutputFilePath))
				.Callback<XmlDocument, string>((doc, outputPath) =>
				{
					// Make sure the xml content is the same without getting the fragility of whitespace etc.
					Assert.That(doc.OuterXml, Is.EqualTo(expectedXmlDoc.OuterXml));
				});

			task.Run(mockFileSystemHandler.Object, null);
		}
	}
}