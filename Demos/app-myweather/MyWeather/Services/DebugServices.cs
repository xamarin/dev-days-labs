using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MyWeather
{
	public static class DebugServices
	{
		#region Methods
		public static void Report(Exception exception,
								  [CallerMemberName] string callerMemberName = "",
								  [CallerLineNumber] int lineNumber = 0,
								  [CallerFilePath] string filePath = "")
		{
			PrintException(exception, callerMemberName, lineNumber, filePath);
		}

		[Conditional("DEBUG")]
		static void PrintException(Exception exception, string callerMemberName, int lineNumber, string filePath)
		{
			var fileName = System.IO.Path.GetFileName(filePath);

			Debug.WriteLine(exception.GetType());
			Debug.WriteLine($"Error: {exception.Message}");
			Debug.WriteLine($"Line Number: {lineNumber}");
			Debug.WriteLine($"Caller Name: {callerMemberName}");
			Debug.WriteLine($"File Name: {fileName}");
		}
		#endregion
	}
}
