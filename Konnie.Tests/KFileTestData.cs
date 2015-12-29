using System.Collections.Generic;
using Konnie.Model.File;
using Konnie.Model.Tasks.SubTasks;

namespace Konnie.Tests
{
	public class KFileTestData
	{
		public const string ValidTaskName = "ValidTaskName";
		public const string ValidTaskTwoName = "ValidTaskTwoName";
		public const string ValidTaskThreeName = "ValidTaskThreeName";
		public const string InvalidTaskName = "InvalidTaskName";

		public KTask TaskWithNonExistantSubTask = new KTask
		{
			Name = InvalidTaskName,
			SubTasksToRun = new List<string>
			{
				SubTaskTestData.InvalidSubTaskNameOne
			}
		};

		public KTask TaskWithSubstitutionSubTask = new KTask
		{
			Name = InvalidTaskName,
			SubTasksToRun = new List<string>
			{
				SubTaskTestData.SubstitutionTaskWithNonExistantVariableSetName
			}
		};

		public KTask ValidTaskOne = new KTask
		{
			Name = ValidTaskName,
			SubTasksToRun = new List<string>
			{
				SubTaskTestData.ValidTransformTaskName,
				SubTaskTestData.ValidSubstitutionTaskName
			}
		};

		public KTask ValidTaskTwo = new KTask
		{
			Name = ValidTaskTwoName,
			SubTasksToRun = new List<string>
			{
				SubTaskTestData.ValidAssertLackOfXPathTaskName,
				SubTaskTestData.ValidStopServiceTaskName
			}
		};

		public KTask ValidTaskThree = new KTask
		{
			Name = ValidTaskThreeName,
			SubTasksToRun = new List<string>
			{
				SubTaskTestData.ValidStopServiceTaskName,
				SubTaskTestData.ValidStartServiceTaskName
			}
		};

		public KFile ValidKFile
		{
			get
			{
				var subTaskTestData = new SubTaskTestData();
				var variableSetTestData = new VariableSetsTestData();
				return new KFile
				{
					Tasks = new KTasks
					{
						ValidTaskOne
					},
					SubTasks = new KSubTasks
					{
						subTaskTestData.ValidAssertLackOfXPathTask,
						subTaskTestData.ValidAssertNoMoreVariablesInFile,
						subTaskTestData.ValidStartServiceTask,
						subTaskTestData.ValidStopServiceTask,
						subTaskTestData.ValidSubstitutionTask,
						subTaskTestData.ValidTransformTask
					},
					VariableSets = new KVariableSets
					{
						variableSetTestData.ValidVariableSet
					}
				};
			}
		}

		public KFile KFileWithTaskWithNonExistantSubTask
		{
			get
			{
				var subTaskTestData = new SubTaskTestData();
				return new KFile
				{
					Tasks = new KTasks
					{
						TaskWithNonExistantSubTask
					},
					SubTasks = new KSubTasks
					{
						subTaskTestData.ValidTransformTask
					}
				};
			}
		}

		public KFile KFileWithSubstitutionTaskWithNonExistantVariableSet
		{
			get
			{
				var subTaskTestData = new SubTaskTestData();
				return new KFile
				{
					Tasks = new KTasks
					{
						TaskWithNonExistantSubTask
					},
					SubTasks = new KSubTasks
					{
						subTaskTestData.SubstitutionTaskWithNonExistantVariableSet
					}
				};
			}
		}

		public KFile MergeWholeLeft
		{
			get
			{
				var subTaskTestData = new SubTaskTestData();
				var variableSetTestData = new VariableSetsTestData();
				return new KFile
				{
					Tasks = new KTasks
					{
						ValidTaskOne,
						ValidTaskTwo
					},
					SubTasks = new KSubTasks
					{
						subTaskTestData.ValidStartServiceTask,
						subTaskTestData.ValidAssertLackOfXPathTask
					},
					VariableSets = new KVariableSets
					{
						variableSetTestData.ValidVariableSet
					}
				};
			}
		}
		public KFile MergeWholeRight
		{
			get
			{
				var subTaskTestData = new SubTaskTestData();
				var variableSetTestData = new VariableSetsTestData();
				return new KFile
				{
					Tasks = new KTasks
					{
						ValidTaskThree
					},
					SubTasks = new KSubTasks
					{
						subTaskTestData.ValidTransformTask,
						subTaskTestData.ValidStopServiceTask
					},
					VariableSets = new KVariableSets
					{
						variableSetTestData.ValidVariableTwoSet,
						variableSetTestData.ValidVariableThreeSet
					}
				};
			}
		}
		public KFile MergeWholeLeftThenRight
		{
			get
			{
				var subTaskTestData = new SubTaskTestData();
				var variableSetTestData = new VariableSetsTestData();
				return new KFile
				{
					Tasks = new KTasks
					{
						ValidTaskOne,
						ValidTaskTwo,
						ValidTaskThree
					},
					SubTasks = new KSubTasks
					{
						subTaskTestData.ValidStartServiceTask,
						subTaskTestData.ValidAssertLackOfXPathTask,
                        subTaskTestData.ValidTransformTask,
						subTaskTestData.ValidStopServiceTask
					},
					VariableSets = new KVariableSets
					{
						variableSetTestData.ValidVariableSet,
						variableSetTestData.ValidVariableTwoSet,
						variableSetTestData.ValidVariableThreeSet
					}
				};
			}
		}

		public class VariableSetsTestData
		{
			public const string ValidVariableSetName = "ValidVariableSetName";
			public const string ValidVariableSetTwoName = "ValidVariableSetTwoName";
			public const string ValidVariableSetThreeName = "ValidVariableSetThreeName";

			public KVariableSet ValidVariableSet = new KVariableSet
			{
				Name = ValidVariableSetName,
				Variables = new Dictionary<string, string>
				{
					{"variableName1", "variableVal1"}
				}
			};

			public KVariableSet ValidVariableTwoSet = new KVariableSet
			{
				Name = ValidVariableSetTwoName,
				Variables = new Dictionary<string, string>
				{
					{"variableName2", "variableVal2"}
				}
			};

			public KVariableSet ValidVariableThreeSet = new KVariableSet
			{
				Name = ValidVariableSetThreeName,
				Variables = new Dictionary<string, string>
				{
					{"variableName3", "variableVal3"}
				}
			};
		}

		public class SubTaskTestData
		{
			public const string InvalidSubTaskNameOne = "InvalidSubTaskNameOneName";
			public const string ValidTransformTaskName = "ValidTransformTaskName";
			public const string ValidSubstitutionTaskName = "ValidSubstitutionTaskName";
			public const string ValidAssertLackOfXPathTaskName = "ValidAssertLackOfXPathTaskName";
			public const string ValidAssertNoMoreVariablesInFileName = "ValidAssertNoMoreVariablesInFileName";
			public const string ValidStopServiceTaskName = "ValidStopServiceTaskName";
			public const string ValidStartServiceTaskName = "ValidStartServiceTaskName";
			public const string SubstitutionTaskWithNonExistantVariableSetName = "SubstitutionTaskWithNonExistantVariableSetName";

			public SubstitutionTask SubstitutionTaskWithNonExistantVariableSet = new SubstitutionTask
			{
				Name = SubstitutionTaskWithNonExistantVariableSetName,
				SubstitutionVariableSets = new List<string>
				{
					"NonExistantVariableSet"
				}
			};

			public AssertLackOfXPathTask ValidAssertLackOfXPathTask = new AssertLackOfXPathTask
			{
				Name = ValidAssertLackOfXPathTaskName
			};

			public AssertNoMoreVariablesInFile ValidAssertNoMoreVariablesInFile = new AssertNoMoreVariablesInFile
			{
				Name = ValidAssertNoMoreVariablesInFileName
			};

			public StartServiceTask ValidStartServiceTask = new StartServiceTask
			{
				Name = ValidStartServiceTaskName
			};

			public StopServiceTask ValidStopServiceTask = new StopServiceTask
			{
				Name = ValidStopServiceTaskName
			};

			public SubstitutionTask ValidSubstitutionTask = new SubstitutionTask
			{
				Name = ValidSubstitutionTaskName
			};

			public TransformTask ValidTransformTask = new TransformTask
			{
				Name = ValidTransformTaskName
			};
		}
	}
}