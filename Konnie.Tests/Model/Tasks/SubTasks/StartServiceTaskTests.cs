using Konnie.Model.FilesHistory;
using Konnie.Model.Tasks.SubTasks;
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

			var result = task.NeedToRun(new UnpersistedHistory());

			Assert.That(result, Is.False);
		}
	}
}