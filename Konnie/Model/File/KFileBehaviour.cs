using System;
using System.Linq;
using Konnie.Model.Tasks;

namespace Konnie.Model.File
{
	public partial class KFile
	{
		public KFile Merge(KFile otherKFile)
		{
			Logger.Verbose("Merging Tasks");
			EnsureNoDuplicates(
				() => Tasks.Select(t => t.Name).ToArray(),
				() => otherKFile.Tasks.Select(t => t.Name).ToArray(),
				nameof(Tasks));

			var newKTasks = new KTasks();
			newKTasks.AddRange(Tasks);
			newKTasks.AddRange(otherKFile.Tasks);
			Tasks = newKTasks;

			Logger.Verbose("Merging SubTasks");
			EnsureNoDuplicates(
				() => SubTasks.Select(t => t.Name).ToArray(),
				() => otherKFile.SubTasks.Select(t => t.Name).ToArray(),
				nameof(SubTasks));

			var newKSubTasks = new KSubTasks();
			newKSubTasks.AddRange(SubTasks);
			newKSubTasks.AddRange(otherKFile.SubTasks);
			SubTasks = newKSubTasks;

			Logger.Verbose("Merging VariableSets");
			EnsureNoDuplicates(
				() => VariableSets.Select(t => t.Name).ToArray(),
				() => otherKFile.VariableSets.Select(t => t.Name).ToArray(),
				nameof(VariableSets));

			var newKVariableSets = new KVariableSets();
			newKVariableSets.AddRange(VariableSets);
			newKVariableSets.AddRange(otherKFile.VariableSets);
			VariableSets = newKVariableSets;

			Logger = otherKFile.Logger;

			return this;
		}

		private static void EnsureNoDuplicates(Func<string[]> getNames, Func<string[]> getOtherNames, string type)
		{
			var names = getNames();
			var otherNames = getOtherNames();
			var allNames = names.Concat(otherNames);
			if (allNames.Distinct().Count() != names.Count() + otherNames.Count())
			{
				throw new KFileDuplication(type, allNames.ToArray());
			}
		}

		public bool IsValid(string taskName)
		{
			Logger.Verbose($"Checking validity of '{taskName}'");

			if (Tasks.Select(t => t.Name).Contains(taskName) == false)
			{
				Logger.Terse($"Doesn't contain task {taskName}");
				return false;
			}

			var taskToRun = Tasks.Single(t => t.Name == taskName);
			var subTaskNames = SubTasks.Select(t => t.Name);
			foreach (var subTask in taskToRun.SubTasksToRun)
			{
				if (subTaskNames.Contains(subTask) == false)
				{
					Logger.Terse($"SubTasks doesn't contain {subTask}");
					return false;
				}
			}

			return CheckAllSubTasksHaveTheRequiredVariableSets(taskToRun);
		}

		private bool CheckAllSubTasksHaveTheRequiredVariableSets(KTask taskToRun)
		{
			var subTasksWithVariableSets = SubTasks
				.Where(st => taskToRun.SubTasksToRun.Contains(st.Name))
				.Where(st => (st as ISubTaskThatUsesVariableSets) != null);

			if (subTasksWithVariableSets.Any() == false)
			{
				Logger.Verbose("No subtasks that require variable sets.");
				return true;
			}

			var variableSetNames = VariableSets.Select(t => t.Name);

			foreach (var subTasksWithVariableSet in subTasksWithVariableSets.Cast<ISubTaskThatUsesVariableSets>())
			{
				if (subTasksWithVariableSet.SubstitutionVariableSets == null)
				{
					Logger.Terse($"SubTask's VariableSets is null, this shouldn't really happen.");
					return false;
				}

				foreach (var subTaskThatUsesVariableSet in subTasksWithVariableSet.SubstitutionVariableSets)
				{
					if (variableSetNames.Contains(subTaskThatUsesVariableSet) == false)
					{
						Logger.Terse($"SubTask '{subTasksWithVariableSet.Name}'s VariableSet '{subTaskThatUsesVariableSet}' doesn't exist.");
						return false;
					}
				}
			}

			Logger.Verbose("All VariableSets required exist.");
			return true;
		}
	}
}