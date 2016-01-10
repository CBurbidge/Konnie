using System;
using System.Collections.Generic;
using System.IO;
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
		private string InputFilePath = "InputFile";
		private string OutputFilePath = "OutputFile";
		private string TransformFilePathOne = "TransformFileOne";
		private string TransformFilePathTwo = "TransformFileTwo";

		[Test]
		public void ThrowsIfAnyTransformFilePathDoesntExist()
		{
			var task = new TransformTask
			{
				Logger = new ConsoleLogger(),
				Name = "",
				InputFile = InputFilePath,
				OutputFile = OutputFilePath,
				TransformFiles = new List<string> { TransformFilePathOne }
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
				InputFile = InputFilePath, OutputFile = OutputFilePath,
				TransformFiles = new List<string> { TransformFilePathOne }
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
				TransformFiles = new List<string> { TransformFilePathOne }
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
			string inputXmlFile = 
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
				Logger = new ConsoleLogger(), Name = "",
				InputFile = InputFilePath,
				OutputFile = OutputFilePath,
				TransformFiles = new List<string> { TransformFilePathOne }
			};

			// This is super specific on whitespace etc.
			var expectedOutputFile = @"<?xml version=""1.0"" encoding=""utf - 8""?>
<configuration>
  <appSettings>
    <add key=""SettingOne"" value=""SettingValueOne"" />
    <add key=""SettingThree"" value=""SomeOtherValue"" />
  </appSettings>
</configuration>";
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(InputFilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllText(InputFilePath))
				.Returns(inputXmlFile);
			mockFileSystemHandler.Setup(f => f.Exists(OutputFilePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(TransformFilePathOne)).Returns(true);
			mockFileSystemHandler.Setup(f => f.ReadAllText(TransformFilePathOne))
				.Returns(transformFile);

			task.Run(mockFileSystemHandler.Object, null);

			mockFileSystemHandler.Verify(f => f.WriteAllText(expectedOutputFile, OutputFilePath));
		}
		public Stream GenerateStreamFromString(string s)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
	}
}