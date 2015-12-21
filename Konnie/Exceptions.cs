using System;
using System.IO.Abstractions;
using System.Linq;

namespace Konnie
{
	internal class KonnieFileDoesntExist : Exception
	{
		private readonly string _filepathKonnie;

		public KonnieFileDoesntExist(string filepathKonnie)
		{
			_filepathKonnie = filepathKonnie;
		}

		public override string Message { get { return $"{_filepathKonnie}"; } }
	}
}