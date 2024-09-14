using System;
using System.IO;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Responsible for the per-process .log file in the Logs directory.
	/// Kept multiple log files in the past, but now consolidates all information
	/// into a single file named Client.log or Server_{Identifier}.log.
	/// </summary>
	// Token: 0x0200057E RID: 1406
	public class Logs : MonoBehaviour
	{
		// Token: 0x06002CFB RID: 11515 RVA: 0x000C322C File Offset: 0x000C142C
		public static void printLine(string message)
		{
			if (Logs.debugLog != null && !string.IsNullOrEmpty(message))
			{
				string text = message.Trim();
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
					Logs.debugLog.writeLine(string.Format("[{0}] {1}", text2, text));
				}
			}
		}

		/// <summary>
		/// Get logging to path.
		/// </summary>
		// Token: 0x06002CFC RID: 11516 RVA: 0x000C3280 File Offset: 0x000C1480
		public static string getLogFilePath()
		{
			if (Logs.debugLog == null)
			{
				return null;
			}
			return Logs.debugLog.path;
		}

		/// <summary>
		/// Set path to log to.
		/// </summary>
		// Token: 0x06002CFD RID: 11517 RVA: 0x000C3298 File Offset: 0x000C1498
		public static void setLogFilePath(string logFilePath)
		{
			if (!logFilePath.EndsWith(".log"))
			{
				throw new ArgumentException("should be a .log file", "logFilePath");
			}
			Logs.closeLogFile();
			try
			{
				string directoryName = Path.GetDirectoryName(logFilePath);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			try
			{
				if (File.Exists(logFilePath))
				{
					string text = logFilePath.Insert(logFilePath.Length - 4, "_Prev");
					if (File.Exists(text))
					{
						File.Delete(text);
					}
					File.Move(logFilePath, text);
				}
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
			try
			{
				Logs.debugLog = new LogFile(logFilePath);
			}
			catch (Exception exception3)
			{
				Debug.LogException(exception3);
			}
		}

		/// <summary>
		/// Close current log file.
		/// </summary>
		// Token: 0x06002CFE RID: 11518 RVA: 0x000C3360 File Offset: 0x000C1560
		public static void closeLogFile()
		{
			if (Logs.debugLog != null)
			{
				Logs.debugLog.close();
				Logs.debugLog = null;
			}
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x000C337C File Offset: 0x000C157C
		public void awake()
		{
			if (Logs.noDefaultLog)
			{
				return;
			}
			string text = ReadWrite.PATH;
			text = text + "/Logs/Server_" + Dedicator.serverID.Replace(' ', '_') + ".log";
			double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
			Logs.setLogFilePath(text);
			double num = Time.realtimeSinceStartupAsDouble - realtimeSinceStartupAsDouble;
			if (num > 0.1)
			{
				UnturnedLog.info(string.Format("Initializing logging took {0}s", num));
			}
			NetReflection.SetLogCallback(new Action<string>(UnturnedLog.info));
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x000C3401 File Offset: 0x000C1601
		private void OnDestroy()
		{
			Logs.closeLogFile();
		}

		/// <summary>
		/// Should setup of the default *.log file be disabled?
		/// </summary>
		// Token: 0x04001850 RID: 6224
		public static CommandLineFlag noDefaultLog = new CommandLineFlag(false, "-NoDefaultLog");

		// Token: 0x04001851 RID: 6225
		private static LogFile debugLog = null;
	}
}
