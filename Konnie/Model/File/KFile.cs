namespace Konnie.Model.File
{
	public class KFile
	{
		public KTasks Tasks { get; set; }
		public KSubTasks SubTasks { get; set; }
		public KVariableSets VariableSets { get; set; }

		public KFile Merge(KFile otherKFile)
		{
			return this;
		}
	}
}