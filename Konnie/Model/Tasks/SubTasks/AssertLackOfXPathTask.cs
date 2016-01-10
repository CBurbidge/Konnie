using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class AssertLackOfXPathTask : ISubTask
	{
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(AssertLackOfXPathTask);
		public string FilePath { get; set; } 
		public List<string> XPaths { get; set; } 
		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			if (fileSystemHandler.Exists(FilePath) == false)
			{
				Logger.Terse($"File '{FilePath}' doesn't exist, can't continue");
				throw new FileDoesntExist(FilePath);
			}


		}
		private static XPathNavigator GetXPathNavResult(string text, string xPath)
		{
			var xPathDocument = new XPathDocument(new StringReader(text));
			var xPathNavigator = xPathDocument.CreateNavigator();
			return xPathNavigator.SelectSingleNode(xPath);
		}

	}
}