using System;
using System.Collections.Generic;
using Konnie.Model.File;

namespace Konnie
{
	public class KonnieFileDoesntExistOrCantBeAccessed : Exception
	{
		private readonly string _filepathKonnie;

		public KonnieFileDoesntExistOrCantBeAccessed(string filePath, string filepathKonnie)
		{
			_filepathKonnie = filepathKonnie;
		}

		public override string Message => $"{_filepathKonnie}";
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