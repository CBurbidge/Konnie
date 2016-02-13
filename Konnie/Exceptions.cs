using System;
using System.Collections.Generic;
//
// The thought behind putting all of the exceptions in one file is that so it is easy to see what can go wrong in one place.
// Not sure if it is that much clearer.
//
namespace Konnie
{
	internal class KFileAlreadyAdded : Exception
	{
		private readonly string _kFilePath;

		public KFileAlreadyAdded(string kFilePath)
		{
			_kFilePath = kFilePath;
		}
	}
	public class KonnieFileDoesntExistOrCantBeAccessed : Exception
	{
		private readonly string _filePath;
		private readonly string _filepathKonnie;

		public KonnieFileDoesntExistOrCantBeAccessed(string filePath, string filepathKonnie)
		{
			_filePath = filePath;
			_filepathKonnie = filepathKonnie;
		}

		public override string Message => $"File '{_filePath}' {_filepathKonnie}";
	}
	public class FileDoesntExist : Exception
	{
		private readonly string _source;

		public FileDoesntExist(string source)
		{
			_source = source;
		}

		public override string Message => $"Cannot find file at path: '{_source}'";
	}

	public class ElementExistsAtXPath : Exception
	{
		private readonly string _xPath;

		public ElementExistsAtXPath(string xPath)
		{
			_xPath = xPath;
		}
	}

	public class VariablesStillExistInFile : Exception
	{
	}
	public class CombinedKFileIsInvalid : Exception
	{
		private readonly List<string> _kFile;

		public CombinedKFileIsInvalid(List<string> kFile)
		{
			_kFile = kFile;
		}
	}
	public class ArgsParsingFailed : Exception
	{
		private readonly string _errorText;

		public ArgsParsingFailed(string errorText)
		{
			_errorText = errorText;
		}
	}

	public class ProjectDirectoryDoesntExist : Exception
	{
		private readonly string _projectDir;

		public ProjectDirectoryDoesntExist(string projectDir)
		{
			_projectDir = projectDir;
		}
	}

	public class KFileDuplication : Exception
	{
		private readonly IEnumerable<string> _allNames;
		private readonly string _type;

		public KFileDuplication(string type, string[] allNames)
		{
			_type = type;
			_allNames = allNames;
		}
	}
}