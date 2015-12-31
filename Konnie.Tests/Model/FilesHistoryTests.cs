using System;
using System.IO.Abstractions;
using Konnie.Model;
using Moq;
using NUnit.Framework;

namespace Konnie.Tests.Model
{
	[TestFixture]
	public class FilesHistoryTests
	{
		
		[Test]
		public void CanBeInstantiatedWithoutAHistoryFile()
		{
			var fileHistory = new FilesHistory(null, "SomeTask");
		}

		[Test]
		public void ThrowsIfTaskNameIsNotGiven()
		{
			Assert.Throws<InvalidProgramException>(() => new FilesHistory(null, null));
		}

		[Test]
		public void DoesntThrowIfHistoryFileDoesntExist()
		{
			var mockFileSystem = new Mock<IFileSystem>();
			var historyFilePath = "filePath";
			mockFileSystem.Setup(fs => fs.File.Exists(historyFilePath)).Returns(false);

			var thing = new FilesHistory(historyFilePath, "SomeTask");
		}

		[Test]
		public void SaysThatFileIsDifferentIfNoHistoryFileIsPresented()
		{
			var filesHistory = new FilesHistory(null, "SomeTask");

			Assert.That(filesHistory.FileIsDifferent("SomeFilePath", DateTime.Now), Is.True);
		}

		[Test]
		public void DoesntThrowIfUpdatingWhenNoHistoryFileIsPresent()
		{
			var filesHistory = new FilesHistory(null, "SomeTask");

			filesHistory.UpdateHistory("SomeFilePath", DateTime.Now);
		}
	
		[Test]
		public void DoesntThrowIfCommitingWhenNoHistoryFileIsPresent()
		{
			var filesHistory = new FilesHistory(null, "SomeTask");
			filesHistory.UpdateHistory("SomeFilePath", DateTime.Now);

			filesHistory.CommitChanges();
		}
	}
}