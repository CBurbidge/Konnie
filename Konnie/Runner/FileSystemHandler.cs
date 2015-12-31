using System.IO;
using System.IO.Abstractions;
using System.Xml.Linq;
using Konnie.Model;

namespace Konnie.Runner
{
	/// <summary>
	/// FileSystemHandler is a wrapper around an IFileSystem object.
	/// This makes sure that any changes to files are kept in sync.
	/// </summary>
	public class FileSystemHandler : IFileSystemHandler
	{
		private readonly IFileSystem _fs;

		public FileSystemHandler(IFilesHistory filesHistory, IFileSystem fs = null)
		{
			_fs = fs ?? new FileSystem();
			FilesHistory = filesHistory;
		}

		public IFilesHistory FilesHistory { get; }
		public string ReadAllText(string filePath)
		{
			throw new System.NotImplementedException();
		}

		public string WriteAllText(string text, string filePath)
		{
			throw new System.NotImplementedException();
		}

		public XDocument LoadXDocument(string filePath)
		{
			throw new System.NotImplementedException();
		}

		public void SaveXDocument(XDocument doc, string filePath)
		{
			throw new System.NotImplementedException();
		}
	}

	public interface IFileSystemHandler
	{
		IFilesHistory FilesHistory { get; }
		string ReadAllText(string filePath);
		string WriteAllText(string text, string filePath);
		XDocument LoadXDocument(string filePath);
		void SaveXDocument(XDocument doc, string filePath);
	}
}