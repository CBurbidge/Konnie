//using System;
//using System.Collections.Generic;
//using System.IO.Abstractions;
//using System.Linq;
//using Moq;
//using NUnit.Framework;

//namespace Konnie.Tests
//{
//	[TestFixture]
//	public class KonnieProgramTests
//	{
//		readonly List<string> logLines = new List<string>();

//		[SetUp]
//		public void SetupTests()
//		{
//			logLines.Clear();
//		}

//		[Test]
//		public void ListsArgumentsOnStartup()
//		{
//			var fileone = "fileOne";
//			var taskArg = "thing";
//			var mockFileSystem = new Mock<IFileSystem>();
//			mockFileSystem.Setup(f => f.File.Exists(fileone)).Returns(true);
//			var konnieProgram = new KonnieProgram(line => logLines.Add(line), mockFileSystem.Object);

//			konnieProgram.Run(new []{"--files", fileone, "--task", taskArg});

//			Assert.That(logLines.Count(l => l.Contains(fileone)) > 0);
//			Assert.That(logLines.Count(l => l.Contains(taskArg)) > 0);
//		}

//		[Test]
//		public void TellsUserToAddMoreArgsIfZeroAreAdded()
//		{
//			var konnieProgram = new KonnieProgram(line => logLines.Add(line));
//			var args = new string[]{};

//			Assert.Throws<ArgsParsingFailed>(() => konnieProgram.Run(args));
//			Assert.That(logLines.Count(l => l.Contains(KonnieProgram.Wording.ArgumentsDescription)) == 1);
//			Assert.That(logLines.Count(l => l.Contains(KonnieProgram.Wording.NeedToPassArgumentsWarning)) == 1);
//		}

//		[Test]
//		public void TellsUserToAddMoreArgsIfOneIsAdded()
//		{
//			var konnieProgram = new KonnieProgram(line => logLines.Add(line));
//			var args = new[]{"arg1"};

//			Assert.Throws<ArgsParsingFailed>(() => konnieProgram.Run(args));
//			Assert.That(logLines.Count(l => l.Contains(KonnieProgram.Wording.ArgumentsDescription)) == 1);
//			Assert.That(logLines.Count(l => l.Contains(KonnieProgram.Wording.NeedToPassArgumentsWarning)) == 1);
//		}

//		[Test]
//		public void CheckFileArgumentExists()
//		{
//			var fileName = "Thing";
//			var args = new[]{"--files", fileName, "--task", "blah"};
//			var mockFileSystem = new Mock<IFileSystem>();
//			mockFileSystem.Setup(f => f.File.Exists(fileName)).Returns(true);
//			var konnieProgram = new KonnieProgram(line => logLines.Add(line), mockFileSystem.Object);

//			konnieProgram.Run(args);

//			mockFileSystem.Verify(f => f.File.Exists(fileName));
//		}

//		[Test]
//		public void CheckMultipleFileArgumentsExist()
//		{
//			var file1 = "fileOne";
//			var file2 = "fileTwo";
//			var args = new[]{"--files", file1, file2, "--task", "something"};
//			var mockFileSystem = new Mock<IFileSystem>();
//			mockFileSystem.Setup(f => f.File.Exists(file1)).Returns(true);
//			mockFileSystem.Setup(f => f.File.Exists(file2)).Returns(true);
//			var konnieProgram = new KonnieProgram(line => logLines.Add(line), mockFileSystem.Object);

//			konnieProgram.Run(args);

//			mockFileSystem.Verify(f => f.File.Exists(file1));
//			mockFileSystem.Verify(f => f.File.Exists(file2));
//		}

//		[Test]
//		public void WarnsAndThrowsIfFileDoesntExist()
//		{
//			var fileName = "Thing";
//			var args = new[] { "--files", fileName, "--task", "blah" };
//			var mockFileSystem = new Mock<IFileSystem>();
//			mockFileSystem.Setup(f => f.File.Exists(fileName)).Returns(false);
//			var konnieProgram = new KonnieProgram(line => logLines.Add(line), mockFileSystem.Object);

//			Assert.Throws<KonnieFileDoesntExist>(() => konnieProgram.Run(args));
//			mockFileSystem.Verify(f => f.File.Exists(fileName));
//			string expectedString = string.Format(KonnieProgram.Wording.FileDoesntExistFailure, fileName);
//			Assert.That(logLines.Count(l => l.Contains(expectedString)) == 1);
//		}

//	}
//}
