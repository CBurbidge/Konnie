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
		public bool NeedToRun(IFileSystemHandler fileSystemHandler)
		{
			return false;
		}

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			if (fileSystemHandler.Exists(FilePath) == false)
			{
				Logger.Terse($"File '{FilePath}' doesn't exist, can't continue");
				throw new FileDoesntExist(FilePath);
			}

			var xmlFileReader = new StringReader(fileSystemHandler.ReadAllText(FilePath));
			var xPathDocument = new XPathDocument(xmlFileReader);
			var xPathNavigator = xPathDocument.CreateNavigator();

			foreach (var xPath in XPaths)
			{
				Logger.Verbose($"Testing XPath '{xPath}'");
				var node = xPathNavigator.SelectSingleNode(xPath);
				if (node != null)
				{
					Logger.Terse($"XPath '{xPath}' finds an element. Cannot continue.");
					throw new ElementExistsAtXPath(xPath);
				}
			}

			Logger.Verbose("All XPaths are fine.");
		}
	}
}