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
		public KTasks Tasks { get; set; }
		public KSubTasks SubTasks { get; set; }
		public KVariableSets VariableSets { get; set; }

		public KFile Merge(KFile otherKFile)
		{
			return this;
		}
		public bool IsValid()
		{
			return false;
		}
	}
}