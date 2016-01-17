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
			var projDir = "SomeDir";
			var args= new KonnieProgramArgs
			{
				ProjectDir = projDir,
				Task = "SomeTask",
				Files = new List<string>()
			};
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem.Setup(fs => fs.Directory.Exists(projDir)).Returns(true);

			Assert.Throws<InvalidProgramException>(() => args.Validate(mockFileSystem.Object));
		}

		[Test]
		public void ThrowsIfFileDoesntExist()
		{
			var fileName = "SomeFile";
			var projDir = "SomeDir";
			var args = new KonnieProgramArgs
			{
				ProjectDir = projDir,
				Task = "SomeTask",
				Files = new List<string>
				{
					fileName
				}
			};
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem.Setup(fs => fs.Path.GetFullPath(It.IsAny<string>())).Returns("Thing");
			mockFileSystem.Setup(fs => fs.File.Exists(fileName)).Returns(false);
			mockFileSystem.Setup(fs => fs.Directory.Exists(projDir)).Returns(true);

			Assert.Throws<KonnieFileDoesntExistOrCantBeAccessed>(() => args.Validate(mockFileSystem.Object));
		}
		
		
		[Test]
		public void ThrowsIfProjectDirDoesntExist()
		{
			var projDir = "SOmePLace";
			var args = new KonnieProgramArgs
			{
				ProjectDir = projDir,
				Task = "SomeTask",
				Files = new List<string>
				{
					"SomeFile"
				}
			};
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem.Setup(fs => fs.Directory.Exists(projDir))
				.Returns(false);
			mockFileSystem.Setup(fs => fs.Path.GetFullPath(It.IsAny<string>())).Returns("Thing");

			Assert.Throws<ProjectDirectoryDoesntExist>(() => args.Validate(mockFileSystem.Object));
		}
		
		[Test]
		public void ThrowsIfFileCantBeAccessed()
		{
			var fileName = "SomeFile";
			var projDir = "SomePlace";
			var args = new KonnieProgramArgs
			{
				ProjectDir = projDir,
				Task = "SomeTask",
				Files = new List<string>
				{
					fileName
				}
			};
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem.Setup(fs => fs.Directory.Exists(projDir)).Returns(true);
			mockFileSystem.Setup(fs => fs.File.Exists(fileName))
				.Returns(false);
			mockFileSystem.Setup(fs => fs.Path.GetFullPath(It.IsAny<string>())).Returns("Thing");
			Func<string, Stream> getFileStreamFromPath = path => {throw new IOException("Can't access file");};

			Assert.Throws<KonnieFileDoesntExistOrCantBeAccessed>(() => args.Validate(mockFileSystem.Object, null, getFileStreamFromPath));
		}
		
		[Test]
		public void DoesntThrowIfEverythingIsFine()
		{
			var projDir = "SomePlace";
			var fileName = "SomeFile";
			var fileNameTwo = "SomeFileTwo";
			var args = new KonnieProgramArgs
			{
				ProjectDir = projDir,
				Task = "SomeTask",
				Files = new List<string>
				{
					fileName,
					fileNameTwo
				}
			};
			var mockFileSystem = new Mock<IFileSystem>();
			mockFileSystem.Setup(fs => fs.Directory.Exists(projDir)).Returns(true);
			mockFileSystem.Setup(fs => fs.File.Exists(fileName)).Returns(true);
			mockFileSystem.Setup(fs => fs.File.Exists(fileNameTwo)).Returns(true);
			Func<string, Stream> getFileStreamFromPath = path => null;

			args.Validate(mockFileSystem.Object, null, getFileStreamFromPath);
		}
	}
}