using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Konnie
{
	class Program
	{
		static void Main(string[] args)
		{
			var konnieProgram = new KonnieProgram(line => Console.WriteLine(line));
			konnieProgram.Run(args);

			Thread.Sleep(2000);
		}

		
	}
}
