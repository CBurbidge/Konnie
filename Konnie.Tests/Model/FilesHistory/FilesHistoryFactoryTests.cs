using System;
using Konnie.Model.FilesHistory;
using NUnit.Framework;

namespace Konnie.Tests.Model.FilesHistory
{
	[TestFixture]
	public class FilesHistoryFactoryTests
	{
		[Test]
		public void ReturnsUnpersistedIfFileNameIsNullOrEmpty()
		{
			var filesHistoryFactory = new FilesHistoryFactory();

			Assert.That(filesHistoryFactory.Create(null, "SomeTask"), Is.TypeOf<UnpersistedHistory>());
		}

		[Test]
		public void ReturnsJsonFilePersistedIfFileNameIsNotNullOrEmpty()
		{
			var filesHistoryFactory = new FilesHistoryFactory();

			Assert.That(filesHistoryFactory.Create("Some file path", "SomeTask"), Is.TypeOf<JsonFilePersistedFilesHistory>());
		}
	}
}