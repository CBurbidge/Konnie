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
		[Test]
		public void ThrowsIfFileDoesntExist()
		{
			var filePath = "somePath";
			var task = new AssertLackOfXPathTask
			{
				Logger = new ConsoleLogger(), Name = "",
				FilePath = filePath
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(filePath)).Returns(false);

			Assert.Throws<FileDoesntExist>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		public void ThrowsIfXPathIsInvalid() { }
		public void ThrowsIfXPathElementExists() { }
		public void ValidXPathNotMatchingAnythingDoesntThrow() { }
	}
}