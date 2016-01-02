using System;
using System.Collections.Generic;
using Konnie.Model.File;

namespace Konnie
{
	public class KonnieFileDoesntExist : Exception
	{
		private readonly string _filepathKonnie;

		public KonnieFileDoesntExist(string filepathKonnie)
		{
			_filepathKonnie = filepathKonnie;
		}

		public override string Message => $"{_filepathKonnie}";
	}
	public class KFileIsInvalid : Exception
	{
		private readonly List<string> _kFile;

		public KFileIsInvalid(List<string> kFile)
		{
			_kFile = kFile;
		}
	}
	public class ArgsParsingFailed : Exception
	{
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