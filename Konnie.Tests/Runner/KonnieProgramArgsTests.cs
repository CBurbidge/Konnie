using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Konnie.Runner;
using Moq;
using NUnit.Framework;

namespace Konnie.Tests.Runner
{
	[TestFixture]
	public class KonnieProgramArgsTests
	{
		[Test]
		public void ValidatesThatThereAreKonnieFiles()
		{
			var args= new KonnieProgramArgs
			{
				Task = "SomeTask",
				Files = new List<string>()
			};

			Assert.Throws<InvalidProgramException>(() => args.Validate(new FileSystem()));
		}

		[Test]
		public void ThrowsIfFileDoesntExist()
		{
			var fileName = "SomeFile";
			var args = new KonnieProgramArgs
			{
				Task = "SomeTask",
				Files = new List<string>
				{
					fileName
				}
			};
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem.Setup(fs => fs.File.Exists(fileName)).Returns(false);

			Assert.Throws<KonnieFileDoesntExistOrCantBeAccessed>(() => args.Validate(mockFileSystem.Object));
		}
		
		[Test]
		public void ThrowsIfFileCantBeAccessed()
		{
			var fileName = "SomeFile";
			var args = new KonnieProgramArgs
			{
				Task = "SomeTask",
				Files = new List<string>
				{
					fileName
				}
			};
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem.Setup(fs => fs.File.Exists(fileName))
				.Returns(false);
			Func<string, Stream> getFileStreamFromPath = path => {throw new IOException("Can't access file");};

			Assert.Throws<KonnieFileDoesntExistOrCantBeAccessed>(() => args.Validate(mockFileSystem.Object, getFileStreamFromPath));
		}
		
	}
}