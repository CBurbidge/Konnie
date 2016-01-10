using System;
using Microsoft.Web.XmlTransform;

namespace Konnie.Model.Tasks.SubTasks
{
	public class XmlTransformationLogger : IXmlTransformationLogger
	{
		public void LogMessage(string message, params object[] messageArgs)
		{
			
		}

		public void LogMessage(MessageType type, string message, params object[] messageArgs)
		{
		}

		public void LogWarning(string message, params object[] messageArgs)
		{
		}

		public void LogWarning(string file, string message, params object[] messageArgs)
		{
		}

		public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
		{
		}

		public void LogError(string message, params object[] messageArgs)
		{
			throw new Exception(message);
		}

		public void LogError(string file, string message, params object[] messageArgs)
		{
			throw new Exception(message);
		}

		public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
		{
			throw new Exception(message);
		}

		public void LogErrorFromException(Exception ex)
		{
			throw new Exception(ex.Message);
		}

		public void LogErrorFromException(Exception ex, string file)
		{
			throw new Exception(ex.Message);
		}

		public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
		{
			throw new Exception(ex.Message);
		}

		public void StartSection(string message, params object[] messageArgs)
		{
		}

		public void StartSection(MessageType type, string message, params object[] messageArgs)
		{
		}

		public void EndSection(string message, params object[] messageArgs)
		{
		}

		public void EndSection(MessageType type, string message, params object[] messageArgs)
		{
		}
	}
}