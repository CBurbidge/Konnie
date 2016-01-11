using System;
using Konnie.Model.Tasks.SubTasks;
using Konnie.Runner;
using Moq;
using NUnit.Framework;

namespace Konnie.Tests.Model.Tasks.SubTasks
{
	[TestFixture]
	public class StartServiceTaskTests
	{
		[Test]
		public void NeedsToRunReturnsFalse()
		{
			var task = new StartServiceTask();
			var mockFileSystem = new Mock<IFileSystemHandler>();
			mockFileSystem.Setup(f => f.FilesHistory.FileIsDifferent(It.IsAny<string>(), It.IsAny<DateTime>()))
				.Returns(true);

			var result = task.NeedToRun(mockFileSystem.Object);

			Assert.That(result, Is.False);
		}
	}
}