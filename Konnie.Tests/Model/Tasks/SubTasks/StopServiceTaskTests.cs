using Konnie.Model.Tasks.SubTasks;
using NUnit.Framework;

namespace Konnie.Tests.Model.Tasks.SubTasks
{
	[TestFixture]
	public class StopServiceTaskTests
	{
		[Test]
		public void NeedsToRunReturnsFalse()
		{
			var task = new StopServiceTask();

			var result = task.NeedToRun(null);

			Assert.That(result, Is.False);
		}
	}
}