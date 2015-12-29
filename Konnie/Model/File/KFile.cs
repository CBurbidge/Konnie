using System;
using System.Collections.Generic;
using System.Linq;
using Konnie.Model.Tasks;

namespace Konnie.Model.File
{
	/// <summary>
	/// KFile is responsible for holding all of the data needed to run tasks
	/// 
	/// It can merge itself with another KFile object, in which case it will work 
	/// like a concatenation of all of its properties ensuring that there are no duplicates.
	/// 
	/// It can decide whether it has all of the required objects (sub tasks and variable sets) in a KFile to know 
	/// whether a file can run by calling the IsValid method. IsValid decides using only the data in the KFile object, 
	/// it doesn't know about the file history etc.
	/// </summary>
	public class KFile
	{
		public KTasks Tasks { get; set; } = new KTasks();
		public KSubTasks SubTasks { get; set; } = new KSubTasks();
		public KVariableSets VariableSets { get; set; } = new KVariableSets();

		public KFile Merge(KFile otherKFile)
		{
			EnsureNoDuplicates(
				() => Tasks.Select(t => t.Name).ToArray(),
				() => otherKFile.Tasks.Select(t => t.Name).ToArray(),
				nameof(Tasks));

			var newKTasks = new KTasks();
			newKTasks.AddRange(Tasks);
			newKTasks.AddRange(otherKFile.Tasks);
			Tasks = newKTasks;
			
			EnsureNoDuplicates(
				() => SubTasks.Select(t => t.Name).ToArray(), 
				() => otherKFile.SubTasks.Select(t => t.Name).ToArray(), 
				nameof(SubTasks));

			var newKSubTasks = new KSubTasks();
			newKSubTasks.AddRange(SubTasks);
			newKSubTasks.AddRange(otherKFile.SubTasks);
			SubTasks = newKSubTasks;

			EnsureNoDuplicates(
				() => VariableSets.Select(t => t.Name).ToArray(),
				() => otherKFile.VariableSets.Select(t => t.Name).ToArray(),
				nameof(VariableSets));
			
			var newKVariableSets = new KVariableSets();
			newKVariableSets.AddRange(VariableSets);
			newKVariableSets.AddRange(otherKFile.VariableSets);
			VariableSets = newKVariableSets;

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
			var subTaskNames = SubTasks.Select(t => t.Name);
			var taskToRun = Tasks.Single(t => t.Name == taskName);

			foreach (var subTask in taskToRun.SubTasksToRun)
			{
				if (subTaskNames.Contains(subTask) == false)
				{
					return false;
				}
			}

			var subTasksWithVariableSets = SubTasks
				.Where(st => taskToRun.SubTasksToRun.Contains(st.Name))
				.Where(st => (st as ISubTaskThatUsesVariableSets) != null);

			if (subTasksWithVariableSets.Any() == false)
			{
				return true;
			}

			var variableSetNames = VariableSets.Select(t => t.Name);

			foreach (var subTasksWithVariableSet in subTasksWithVariableSets.Cast<ISubTaskThatUsesVariableSets>())
			{
				if (subTasksWithVariableSet.SubstitutionVariableSets == null)
				{
					return false;
				}

				foreach (var subTaskThatUsesVariableSet in subTasksWithVariableSet.SubstitutionVariableSets)
				{
					if (variableSetNames.Contains(subTaskThatUsesVariableSet) == false)
					{
						return false;
					}
				}
			}
			return true;
		}
	}


	public class KFileDuplication : Exception
	{
		private readonly string _type;
		private readonly IEnumerable<string> _allNames;

		public KFileDuplication(string type, IEnumerable<string> allNames)
		{
			_type = type;
			_allNames = allNames;
		}
	}
}