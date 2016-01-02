using System;
using System.IO.Abstractions;
using Konnie.InzOutz;
using Konnie.Model.FilesHistory;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Konnie.Tests.Model
{
	[TestFixture]
	public class JsonFilePersistedFilesHistoryTests
	{
		[Test]
		public void DoesntThrowIfHistoryFileDoesntExist()
		{
			var mockFileSystem = new Mock<IFileSystem>();
			var historyFilePath = "filePath";
			mockFileSystem.Setup(fs => fs.File.Exists(historyFilePath)).Returns(false);

			var thing = new JsonFilePersistedFilesHistory(historyFilePath, "SomeTask", mockFileSystem.Object);
		}

		[Test]
		public void HistoryIsTaskSpecificAndDoesntConfuseFilesInOtherTaskHistory()
		{
			var historyJsonFilePath = "thing";
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem
				.Setup(fs => fs.File.Exists(historyJsonFilePath))
				.Returns(true);
			var mockHistoryFileConverter = new Mock<IHistoryFileConverter>();
			var taskName = "TaskOne";
			var filepath = "FilePath";
			var lastModified = DateTime.Now;
			mockHistoryFileConverter
				.Setup(h => h.LoadHistoryFile(historyJsonFilePath))
				.Returns(new HistoryFile
				{
					{
						taskName, new FileModifiedDateByAbsoluteFilePath
						{
							{"OtherFilePath", lastModified}
						}
					},
					{
						"OtherTaskName", new FileModifiedDateByAbsoluteFilePath
						{
							{filepath, lastModified}
						}
					}
				});
			var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, taskName, mockFileSystem.Object,
				mockHistoryFileConverter.Object);

			Assert.That(filesHistory.FileIsDifferent(filepath, lastModified), Is.True);
		}

		[Test]
		public void HistoryWithPreviousTaskReturnsFalseUntilUpdatesAreCommited()
		{
			var historyJsonFilePath = "thing";
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem
				.Setup(fs => fs.File.Exists(historyJsonFilePath))
				.Returns(true);
			var mockHistoryFileConverter = new Mock<IHistoryFileConverter>();
			var taskName = "TaskOne";
			var filepath = "FilePath";
			var lastModified = DateTime.Now;
			mockHistoryFileConverter
				.Setup(h => h.LoadHistoryFile(historyJsonFilePath))
				.Returns(new HistoryFile
				{
					{
						taskName, new FileModifiedDateByAbsoluteFilePath
						{
							{filepath, lastModified}
						}
					}
				});
			var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, taskName, mockFileSystem.Object,
				mockHistoryFileConverter.Object);

			Assert.That(filesHistory.FileIsDifferent(filepath, lastModified.AddDays(1)), Is.True);
			filesHistory.UpdateHistory(filepath, lastModified.AddDays(1));

			Assert.That(filesHistory.FileIsDifferent(filepath, lastModified.AddDays(1)), Is.True);
			filesHistory.CommitChanges();

			Assert.That(filesHistory.FileIsDifferent(filepath, lastModified.AddDays(1)), Is.False);
		}

		[Test]
		public void HistoryWithPreviousTaskWithNoRecordOfFileReturnsTrue()
		{
			var historyJsonFilePath = "thing";
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem
				.Setup(fs => fs.File.Exists(historyJsonFilePath))
				.Returns(true);
			var mockHistoryFileConverter = new Mock<IHistoryFileConverter>();
			var taskName = "TaskOne";
			var filepath = "FilePath";
			mockHistoryFileConverter
				.Setup(h => h.LoadHistoryFile(historyJsonFilePath))
				.Returns(new HistoryFile
				{
					{
						taskName, new FileModifiedDateByAbsoluteFilePath
						{
							{"SomeOtherPath", DateTime.Now}
						}
					}
				});
			var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, taskName, mockFileSystem.Object,
				mockHistoryFileConverter.Object);

			Assert.That(filesHistory.FileIsDifferent(filepath, DateTime.Now), Is.True);
		}

		[Test]
		public void HistoryWithTaskWithPreviousFileReturnsTrue()
		{
			var historyJsonFilePath = "thing";
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem
				.Setup(fs => fs.File.Exists(historyJsonFilePath))
				.Returns(true);
			var mockHistoryFileConverter = new Mock<IHistoryFileConverter>();
			var taskName = "TaskOne";
			var filepath = "FilePath";
			var lastModified = DateTime.Now;
			mockHistoryFileConverter
				.Setup(h => h.LoadHistoryFile(historyJsonFilePath))
				.Returns(new HistoryFile
				{
					{
						taskName, new FileModifiedDateByAbsoluteFilePath
						{
							{filepath, lastModified}
						}
					}
				});
			var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, taskName, mockFileSystem.Object,
				mockHistoryFileConverter.Object);

			Assert.That(filesHistory.FileIsDifferent(filepath, lastModified.AddDays(1)), Is.True);
		}

		[Test]
		public void JsonExceptionThrownIsIgnored()
		{
			var historyFilePath = "filePath";
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem
				.Setup(fs => fs.File.Exists(historyFilePath))
				.Returns(true);
			var mockHistoryFileConverter = new Mock<IHistoryFileConverter>();
			mockHistoryFileConverter
				.Setup(h => h.LoadHistoryFile(historyFilePath))
				.Throws<JsonReaderException>();

			var thing = new JsonFilePersistedFilesHistory(historyFilePath, "SomeTask", mockFileSystem.Object, mockHistoryFileConverter.Object);
		}

		[Test]
		public void NoPreviousTaskHistoryReturnsFileIsDifferent()
		{
			var historyJsonFilePath = "thing";
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem
				.Setup(fs => fs.File.Exists(historyJsonFilePath))
				.Returns(true);
			var mockHistoryFileConverter = new Mock<IHistoryFileConverter>();
			mockHistoryFileConverter
				.Setup(h => h.LoadHistoryFile(historyJsonFilePath))
				.Returns(new HistoryFile
				{
					{
						"TaskOne", new FileModifiedDateByAbsoluteFilePath
						{
							{"FilePath", DateTime.Now}
						}
					}
				});
			var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, "TaskTwo", mockFileSystem.Object,
				mockHistoryFileConverter.Object);

			Assert.That(filesHistory.FileIsDifferent("SomeOtherFilePath", DateTime.Now), Is.True);
		}
	}
}