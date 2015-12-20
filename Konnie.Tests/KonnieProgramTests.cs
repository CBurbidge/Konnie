using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
