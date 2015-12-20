using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Konnie.Tests
{
	[TestFixture]
	public class KonnieProgramTests
	{
		List<string> logLines = new List<string>();

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
			var args = new []{arg1, arg2};
			var expectedPart = string.Join(",", args);

			konnieProgram.Run(args);

			Assert.That(logLines.Count(l => l.Contains(expectedPart)) == 1);
		}
	}
}
