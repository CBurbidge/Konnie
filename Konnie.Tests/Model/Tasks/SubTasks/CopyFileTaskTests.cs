using Konnie.Model.Tasks.SubTasks;
using Konnie.Runner;
using Konnie.Runner.Logging;
using Moq;
using NUnit.Framework;

namespace Konnie.Tests.Model.Tasks.SubTasks
{
	[TestFixture]
	public class CopyFileTaskTests
	{
		private const string SourcePath = "someSource";
		private const string DestinationPath = "someDestination";

		[Test]
		public void ThrowsIfDestinationFileDoesntExist()
		{
			var task = new CopyFileTask
			{
				Logger = new ConsoleLogger(), Name = "",
				Source = SourcePath,
				Destination = DestinationPath,
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(SourcePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(DestinationPath)).Returns(false);

			Assert.Throws<FileDoesntExist>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void ThrowsIfSourceFileDoesntExist()
		{
			var task = new CopyFileTask
			{
				Logger = new ConsoleLogger(),
				Name = "",
				Source = SourcePath,
				Destination = DestinationPath,
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(SourcePath)).Returns(false);
			mockFileSystemHandler.Setup(f => f.Exists(DestinationPath)).Returns(true);

			Assert.Throws<FileDoesntExist>(() => task.Run(mockFileSystemHandler.Object, null));
		}

		[Test]
		public void CopiesFile()
		{
			var task = new CopyFileTask
			{
				Logger = new ConsoleLogger(),
				Name = "",
				Source = SourcePath,
				Destination = DestinationPath,
			};
			var mockFileSystemHandler = new Mock<IFileSystemHandler>();
			mockFileSystemHandler.Setup(f => f.Exists(SourcePath)).Returns(true);
			mockFileSystemHandler.Setup(f => f.Exists(DestinationPath)).Returns(true);

			task.Run(mockFileSystemHandler.Object, null);

			mockFileSystemHandler.Verify(f => f.Copy(SourcePath, DestinationPath), Times.Once);
		}
	}

}