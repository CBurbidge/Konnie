using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Xml;
using Konnie.Model.FilesHistory;
using Konnie.Runner.Logging;

namespace Konnie.Runner
{
	public class FileSystemHandler : IFileSystemHandler
	{
		private readonly IFilesHistory _filesHistory;
		private readonly IFileSystem _fs;
		private readonly ILogger _logger;
		private readonly string _projectDirectory;

		public FileSystemHandler(string projectDirectory, IFilesHistory filesHistory, ILogger logger, IFileSystem fs = null)
		{
			_projectDirectory = projectDirectory;
			_logger = logger;
			_fs = fs ?? new FileSystem();
			_filesHistory = filesHistory;
		}

		public string GetAbsPath(string filePath)
		{
			return _fs.Path.Combine(_projectDirectory, filePath);
		}

		public DateTime GetLastWriteTime(string filePath)
		{
			return _fs.File.GetLastWriteTime(GetAbsPath(filePath));
		}

		public IFilesHistory FilesHistory => _filesHistory;

		public void Copy(string source, string destination)
		{
			_fs.File.Copy(GetAbsPath(source), GetAbsPath(destination), true);

			UpdateHistory(destination);
		}

		public bool Exists(string filePath)
		{
			return _fs.File.Exists(GetAbsPath(filePath));
		}

		public IEnumerable<string> ReadAllLines(string filePath)
		{
			return _fs.File.ReadAllLines(GetAbsPath(filePath));
		}

		public string ReadAllText(string filePath)
		{
			return _fs.File.ReadAllText(GetAbsPath(filePath));
		}

		public void WriteAllText(string text, string filePath)
		{
			_fs.File.WriteAllText(GetAbsPath(filePath), text);

			UpdateHistory(filePath);
		}

		public void SaveXDocument(XmlDocument doc, string filePath)
		{
			// might be better way of doing this.
			using (var stringWriter = new StringWriter())
			{
				var xmlWriterSettings = new XmlWriterSettings
				{
					Indent = true
				};
				using (var xmlTextWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
				{
					doc.WriteTo(xmlTextWriter);
					xmlTextWriter.Flush();
					var xmlString = stringWriter.GetStringBuilder().ToString();
					_fs.File.WriteAllText(GetAbsPath(filePath), xmlString);
				}
			}

			UpdateHistory(filePath);
		}
		
		private void UpdateHistory(string filePath)
		{
			var timeModified = _fs.File.GetLastWriteTime(filePath);
			_filesHistory.UpdateHistory(filePath, timeModified);
		}
	}

	/// <summary>
	///     IFileSystemHandler handles everything to do with the file system that the Run method needs.
	///     It is a facade around the IO methods that Konnie needs and it also ensures that the history is updated on writes.
	///     Konnie is called with the location of the project, all files are relative to this location.
	/// </summary>
	public interface IFileSystemHandler
	{
		IFilesHistory FilesHistory { get; }
		void Copy(string source, string destination);
		bool Exists(string filePath);
		IEnumerable<string> ReadAllLines(string filePath);
		string ReadAllText(string filePath);
		void WriteAllText(string text, string filePath);
		void SaveXDocument(XmlDocument doc, string filePath);
		string GetAbsPath(string filePath);
		DateTime GetLastWriteTime(string filePath);
	}
}