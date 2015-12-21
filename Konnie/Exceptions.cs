using System;

namespace Konnie
{
	public class KonnieFileDoesntExist : Exception
	{
		private readonly string _filepathKonnie;

		public KonnieFileDoesntExist(string filepathKonnie)
		{
			_filepathKonnie = filepathKonnie;
		}

		public override string Message
		{
			get { return $"{_filepathKonnie}"; }
		}
	}

	public class ArgsParsingFailed : Exception
	{
	}
}