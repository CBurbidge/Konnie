using System.IO.Abstractions;
using Moq;
using NUnit.Framework;

namespace Konnie.Tests
{
	[TestFixture]
	public class KonnieProgramTests
	{
		[Test]
		public void DoesntThrowIfCorrectArgumentsAreUsed()
		{
			var fileone = "fileOne";
			var taskArg = "thing";
			var konnieProgram = new KonnieProgram();

			Assert.Throws<ProjectDirectoryDoesntExist>(() => konnieProgram.Run(new[] { "--files", fileone, "--task", taskArg }));
		}

		[Test]
		public void ThrowsIfNoArgumentsAreAdded()
		{
			var konnieProgram = new KonnieProgram();
			var args = new string[] { };

			Assert.Throws<ArgsParsingFailed>(() => konnieProgram.Run(args));
		}

		[Test]
		public void ThrowsIfOnlyOneArgumentIsAdded()
		{
			var konnieProgram = new KonnieProgram();
			var args = new[] { "arg1" };

			Assert.Throws<ArgsParsingFailed>(() => konnieProgram.Run(args));
		}
	}
}
