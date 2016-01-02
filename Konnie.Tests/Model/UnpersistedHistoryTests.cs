using System;
using Konnie.Model.FilesHistory;
using NUnit.Framework;

namespace Konnie.Tests.Model
{
	[TestFixture]
	public class UnpersistedHistoryTests
	{
		public void FilesAreDifferentReturnTrue()
		{
			var filesHistory = new UnpersistedHistory();

			Assert.That(filesHistory.FileIsDifferent("Some file", DateTime.Now), Is.True);
		}

		public void UpdateDoesntThrow()
		{
			var filesHistory = new UnpersistedHistory();

			filesHistory.UpdateHistory("Some file", DateTime.Now);
		}

		public void CommitChangesDoesntThrow()
		{
			var filesHistory = new UnpersistedHistory();

			filesHistory.CommitChanges();
		}

		[Test]
		public void InitialiseDoesntThrow()
		{
			var filesHistory = new UnpersistedHistory();

			filesHistory.Initiate();
		}
	}
}