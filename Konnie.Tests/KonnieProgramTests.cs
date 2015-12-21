using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Konnie.Tests
{
	[TestFixture]
	public class KonnieProgramTests
	{
		readonly List<string> logLines = new List<string>();

		[SetUp]
		public void SetupTests()
		{
			logLines.Clear();
		}

		[Test]
		public void ListsArgumentsOnStartup()
		{
			var konnieProgram = new KonnieProgram(line => logLines.Add(line));
			var arg1 = "arg1";
			var arg2 = "arg2";
			var arg3 = "arg3";
			var args = new []{arg1, arg2, arg3};
			var expectedPart = string.Join(",", args);

			konnieProgram.Run(args);

			Assert.That(logLines.Count(l => l.Contains(expectedPart)) == 1);
		}

		[Test]
		public void TellsUserToAddMoreArgsIfZeroAreAdded()
		{
			var konnieProgram = new KonnieProgram(line => logLines.Add(line));
			var args = new string[]{};
			
			konnieProgram.Run(args);

			Assert.That(logLines.Count(l => l.Contains(KonnieProgram.Wording.ArgumentsDescription)) == 1);
			Assert.That(logLines.Count(l => l.Contains(KonnieProgram.Wording.NeedToPassTwoArgumentsIn)) == 1);
			Assert.That(logLines.Count(l => l.Contains(KonnieProgram.Wording.NeedToPassArgumentsWarning)) == 1);
		}

		[Test]
		public void TellsUserToAddMoreArgsIfOneIsAdded()
		{
			var konnieProgram = new KonnieProgram(line => logLines.Add(line));
			var args = new[]{"arg1"};
			
			konnieProgram.Run(args);

			Assert.That(logLines.Count(l => l.Contains(KonnieProgram.Wording.ArgumentsDescription)) == 1);
			Assert.That(logLines.Count(l => l.Contains(KonnieProgram.Wording.NeedToPassTwoArgumentsIn)) == 1);
			Assert.That(logLines.Count(l => l.Contains(KonnieProgram.Wording.NeedToPassArgumentsWarning)) == 1);
		}

		[Test]
		public void CheckFileArgumentExists()
		{
			var arg1 = "arg1";
			var arg2 = "arg2";
			var args = new[]{arg1, arg2};
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem.Setup(f => f.File.Exists(arg1)).Returns(true);
			var konnieProgram = new KonnieProgram(line => logLines.Add(line), mockFileSystem.Object);

			konnieProgram.Run(args);

			mockFileSystem.Verify(f => f.File.Exists(arg1));
		}

		[Test]
		public void CheckMultipleFileArgumentsExist()
		{
			var arg1 = "arg1";
			var arg2 = "arg2";
			var arg3 = "arg3";
			var args = new[]{arg1, arg2, arg3};
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem.Setup(f => f.File.Exists(arg1)).Returns(true);
			mockFileSystem.Setup(f => f.File.Exists(arg2)).Returns(true);
			var konnieProgram = new KonnieProgram(line => logLines.Add(line), mockFileSystem.Object);

			konnieProgram.Run(args);

			mockFileSystem.Verify(f => f.File.Exists(arg1));
			mockFileSystem.Verify(f => f.File.Exists(arg2));
		}
	}
}
