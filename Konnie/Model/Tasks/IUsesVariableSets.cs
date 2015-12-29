using System.Collections.Generic;

namespace Konnie.Model.Tasks
{
	public interface IUsesVariableSets
	{
		List<string> SubstitutionVariableSets { get; set; }
	}
}