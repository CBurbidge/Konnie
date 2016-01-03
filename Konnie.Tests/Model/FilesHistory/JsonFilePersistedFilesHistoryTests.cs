using System;
using System.IO.Abstractions;
using Konnie.InzOutz;
using Konnie.Model.FilesHistory;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;


namespace Konnie.Tests.Model.FilesHistory
{
	[TestFixture]
	public class JsonFilePersistedFilesHistoryTests
	{
		public class FileIsDifferentTests
		{
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
				var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, "TaskTwo", null, mockFileSystem.Object,
					mockHistoryFileConverter.Object);
				filesHistory.LoadFileHistory();

				Assert.That(filesHistory.FileIsDifferent("SomeOtherFilePath", DateTime.Now), Is.True);
			}

			[Test]
			public void HistoryIsTaskSpecificAndDoesntConfuseFilesInOtherTasks()
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
				var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, taskName, null, mockFileSystem.Object,
					mockHistoryFileConverter.Object);
				filesHistory.LoadFileHistory();

				Assert.That(filesHistory.FileIsDifferent(filepath, lastModified), Is.True);
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
				var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, taskName, null, mockFileSystem.Object,
					mockHistoryFileConverter.Object);
				filesHistory.LoadFileHistory();

				Assert.That(filesHistory.FileIsDifferent(filepath, DateTime.Now), Is.True);
			}

			[Test]
			public void HistoryWithTaskWithPreviousFileWithDayOldModifiedDateReturnsTrue()
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
				var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, taskName, null, mockFileSystem.Object,
					mockHistoryFileConverter.Object);
				filesHistory.LoadFileHistory();

				Assert.That(filesHistory.FileIsDifferent(filepath, lastModified.AddDays(1)), Is.True);
			}
		}

		public class InitialisingTests
		{
			/// <summary>
			///     If the file exists but can't be serialised into a HistoryFile object then we want to ignore this
			///     error and write over the file when commiting.
			/// </summary>
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
				var jsonFilePersistedHistory = new JsonFilePersistedFilesHistory(historyFilePath, "SomeTask", null, mockFileSystem.Object,
					mockHistoryFileConverter.Object);

				jsonFilePersistedHistory.LoadFileHistory();
			}

			[Test]
			public void DoesntThrowIfHistoryFileDoesntExist()
			{
				var mockFileSystem = new Mock<IFileSystem>();
				var historyFilePath = "filePath";
				mockFileSystem.Setup(fs => fs.File.Exists(historyFilePath)).Returns(false);
				var jsonFilePersistedHistory = new JsonFilePersistedFilesHistory(historyFilePath, "SomeTask", null, mockFileSystem.Object);

				jsonFilePersistedHistory.LoadFileHistory();
			}
		}

		public class UpdateHistoryTests
		{
			[Test]
			public void HistoryUpdatesFileValueItHasSeenBefore()
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
				var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, taskName, null, mockFileSystem.Object,
					mockHistoryFileConverter.Object);
				filesHistory.LoadFileHistory();

				Assert.That(filesHistory.FileIsDifferent(filepath, lastModified.AddDays(1)), Is.True);
				filesHistory.UpdateHistory(filepath, lastModified.AddDays(1));

				Assert.That(filesHistory.FileIsDifferent(filepath, lastModified.AddDays(1)), Is.False);
			}

			[Test]
			public void HistoryUpdatesFileValueItHasntSeenBefore()
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
							taskName, new FileModifiedDateByAbsoluteFilePath()
						}
					});
				var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, taskName, null, mockFileSystem.Object,
					mockHistoryFileConverter.Object);
				filesHistory.LoadFileHistory();

				Assert.That(filesHistory.FileIsDifferent(filepath, lastModified.AddDays(1)), Is.True);
				filesHistory.UpdateHistory(filepath, lastModified.AddDays(1));

				Assert.That(filesHistory.FileIsDifferent(filepath, lastModified.AddDays(1)), Is.False);
			}
		}

		public class CommitChangesTests
		{
			[Test]
			public void CallsToHistoryFileConverterToSaveChanges()
			{
				var historyJsonFilePath = "thing";
				var mockFileSystem = new Mock<IFileSystem>();
				mockFileSystem
					.Setup(fs => fs.File.Exists(historyJsonFilePath))
					.Returns(true);
				var mockHistoryFileConverter = new Mock<IHistoryFileConverter>();
				var taskName = "TaskOne";
				var historyFile = new HistoryFile
				{
					{
						taskName, new FileModifiedDateByAbsoluteFilePath
						{
							{"SomeFile", DateTime.Now }
						}
					}
				};
				mockHistoryFileConverter
					.Setup(h => h.LoadHistoryFile(historyJsonFilePath))
					.Returns(historyFile);
				var filesHistory = new JsonFilePersistedFilesHistory(historyJsonFilePath, taskName, null, mockFileSystem.Object,
					mockHistoryFileConverter.Object);
				filesHistory.LoadFileHistory();

				filesHistory.CommitChanges();

				mockHistoryFileConverter.Verify(h => h.SaveHistoryFile(historyFile, historyJsonFilePath));
			}
		}
	}
}