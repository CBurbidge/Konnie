using System;
using System.Collections.Generic;
using Konnie.Model.File;
using Konnie.Model.Tasks.SubTasks;
using NUnit.Framework;

namespace Konnie.Tests.Model.File
{
	[TestFixture]
	public class KFileTests
	{
		public class KFileMergeTests
		{
			[Test]
			public void FilesMergeSuccessfully()
			{
				var kFileTestData = new KFileTestData();
				var left = kFileTestData.MergeWholeLeft;
				var right = kFileTestData.MergeWholeRight;

				var merged = left.Merge(right);

				KFileEqualityAsserter.AssertAreEqual(merged, kFileTestData.MergeWholeLeftThenRight);
			}

			[Test]
			public void FilesFailMergeIfHaveSubTasksWithSameName()
			{
				var taskName = "Thing";
				var left = new KFile
				{
					SubTasks = new KSubTasks
					{
						new SubstitutionTask
						{
							Name = taskName
						}
					}

				};
				var right = new KFile
				{
					SubTasks = new KSubTasks
					{
						new TransformTask
						{
							Name = taskName
						}
					}
				};

				Assert.Throws<KFileDuplication>(() => left.Merge(right));
			}

			[Test]
			public void FilesFailMergeIfHaveTasksWithSameName()
			{
				var taskName = "Thing";
				var left = new KFile
				{
					Tasks = new KTasks
					{
						new KTask
						{
							Name = taskName,
							SubTasksToRun = new List<string>
							{
								"SomeSubTask"
							}
						}
					}
				};
				var right = new KFile
				{
					Tasks = new KTasks
					{
						new KTask
						{
							Name = taskName,
							SubTasksToRun = new List<string>
							{
								"SomeOtherSubTask"
							}
						}
					}
				};

				Assert.Throws<KFileDuplication>(() => left.Merge(right));
			}

			[Test]
			public void FilesFailMergeIfHaveVariableSetsWithSameName()
			{
				var variableSetName = "Thing";
				var left = new KFile
				{
					VariableSets = new KVariableSets
					{
						new KVariableSet
						{
							Name = variableSetName,
							Variables = new Dictionary<string, string>
							{
								{ "Thing1", "Thing1Val"} 
							}
						}
					}
				};
				var right = new KFile
				{
					VariableSets = new KVariableSets
					{
						new KVariableSet
						{
							Name = variableSetName,
							Variables = new Dictionary<string, string>
							{
								{ "Thing2", "Thing2Val"}
							}
						}
					}
				};

				Assert.Throws<KFileDuplication>(() => left.Merge(right));
			}
		}

		public class KFileTaskValidityTests
		{
			[Test]
			public void KFileWithRunnableTaskReturnsTrue()
			{
				var kFile = new KFileTestData().ValidKFile;

				var result = kFile.IsValid(KFileTestData.ValidTaskName);

				Assert.That(result, Is.True);
			}

			[Test]
			public void KFileWithTaskWithMissingSubTaskReturnFalse()
			{
				var kFile = new KFileTestData().KFileWithTaskWithNonExistantSubTask;

				var result = kFile.IsValid(KFileTestData.InvalidTaskName);

				Assert.That(result, Is.False);
			}

			[Test]
			public void KFileWithSubstitutionTaskWithoutAllRequiredVariableSetsReturnsFalse()
			{
				var kFile = new KFileTestData().KFileWithSubstitutionTaskWithNonExistantVariableSet;

				var result = kFile.IsValid(KFileTestData.InvalidTaskName);

				Assert.That(result, Is.False);
			}
		}
	}

}