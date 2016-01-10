using NUnit.Framework;

namespace Konnie.Tests.Model.Tasks.SubTasks
{
	[TestFixture]
	public class TransformTaskTests
	{
		[Test] public void ThrowsIfInputFilePathDoesntExist() {}
		[Test] public void ThrowsIfOutputFilePathDoesntExist() {}
		[Test] public void ThrowsIfAnyTransformFilePathDoesntExist() {}
		[Test] public void ThrowsIfTransformFileListIsEmpty() {}
	}
}