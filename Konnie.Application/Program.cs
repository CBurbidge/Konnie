using System;
using System.Threading;

namespace Konnie.Application
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var konnieProgram = new KonnieProgram();
			konnieProgram.Run(args);
		}
	}
}