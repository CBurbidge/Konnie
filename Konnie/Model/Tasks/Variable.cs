using System.Text.RegularExpressions;

namespace Konnie.Model.Tasks
{
	public class Variable
	{
		public static readonly Regex VariableRegex = new Regex(@"#\{[a-zA-Z0-9_\-\s]+\}");
	}
}