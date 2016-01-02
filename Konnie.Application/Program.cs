using System;
using System.Threading;

namespace Konnie.Application
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			//Console.SetError(new StringWriter(new StringBuilder("Thing")));
			//throw new KonnieFileDoesntExist("FilePath.konnie");
			var konnieProgram = new KonnieProgram(Console.WriteLine);
			konnieProgram.Run(args);

			Thread.Sleep(2000);
		}
	}
}