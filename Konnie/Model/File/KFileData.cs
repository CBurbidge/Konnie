using System.Collections.Generic;
using Konnie.Model.Tasks;
using Konnie.Runner.Logging;
using Newtonsoft.Json;

namespace Konnie.Model.File
{
	/// <summary>
	///     KFile is responsible for holding all of the data needed to run tasks
	///     It can merge itself with another KFile object, in which case it will work
	///     like a concatenation of all of its properties ensuring that there are no duplicates.
	///     It can decide whether it has all of the required objects (sub tasks and variable sets) in a KFile to know
	///     whether a file can run by calling the IsValid method. IsValid decides using only the data in the KFile object,
	///     it doesn't know about the file history etc.
	/// </summary>
	public partial class KFile
	{
		public KFile(ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger(false);
		}

		public KTasks Tasks { get; set; } = new KTasks();
		public KSubTasks SubTasks { get; set; } = new KSubTasks();
		public KVariableSets VariableSets { get; set; } = new KVariableSets();

		[JsonIgnore]
		public ILogger Logger { get; set; }

		public List<string> ExtraFiles { get; set; } = new List<string>();
	}

	public class KTasks : List<KTask>
	{
	}

	public class KSubTasks : List<ISubTask>
	{
	}

	public class KTask
	{
		public string Name { get; set; }
		public List<string> SubTasksToRun { get; set; }
	}

	public class KVariableSet
	{
		public string Name { get; set; }
		public Dictionary<string, string> Variables { get; set; }
		public bool IgnoreBlankValues { get; set; } = false;
	}
	
	public class KVariableSets : List<KVariableSet>
	{
	}	
}