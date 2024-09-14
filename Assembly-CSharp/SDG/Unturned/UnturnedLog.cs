using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Unturned wrapper for Debug.Log, Debug.LogWarning, Debug.LogError, etc.
	/// </summary>
	// Token: 0x02000822 RID: 2082
	public static class UnturnedLog
	{
		// Token: 0x060046FD RID: 18173 RVA: 0x001A802C File Offset: 0x001A622C
		public static void info(string message)
		{
			if (UnturnedLog.insideLog)
			{
				return;
			}
			try
			{
				UnturnedLog.insideLog = true;
				Logs.printLine(message);
			}
			finally
			{
				UnturnedLog.insideLog = false;
			}
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x001A8068 File Offset: 0x001A6268
		public static void warn(string message)
		{
			if (UnturnedLog.insideLog)
			{
				return;
			}
			try
			{
				UnturnedLog.insideLog = true;
				Logs.printLine(message);
			}
			finally
			{
				UnturnedLog.insideLog = false;
			}
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x001A80A4 File Offset: 0x001A62A4
		public static void error(string message)
		{
			if (UnturnedLog.insideLog)
			{
				return;
			}
			try
			{
				UnturnedLog.insideLog = true;
				Logs.printLine(message);
				CommandWindow.LogError(message);
			}
			finally
			{
				UnturnedLog.insideLog = false;
			}
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x001A80E8 File Offset: 0x001A62E8
		public static void exception(Exception e)
		{
			if (e == null)
			{
				UnturnedLog.error("UnturnedLog.exception called with null argument");
				return;
			}
			if (UnturnedLog.insideLog)
			{
				return;
			}
			try
			{
				UnturnedLog.insideLog = true;
				UnturnedLog.internalException(e);
			}
			finally
			{
				UnturnedLog.insideLog = false;
			}
		}

		/// <summary>
		/// Log an exception with message providing context.
		/// </summary>
		// Token: 0x06004701 RID: 18177 RVA: 0x001A8134 File Offset: 0x001A6334
		public static void exception(Exception e, string message)
		{
			UnturnedLog.error(message);
			UnturnedLog.exception(e);
		}

		/// <summary>
		/// Recursively logs inner exception.
		///
		/// Should only be called by itself and exception because notifications
		/// to CommandWindow would otherwise get re-sent here as errors.
		/// </summary>
		// Token: 0x06004702 RID: 18178 RVA: 0x001A8144 File Offset: 0x001A6344
		private static void internalException(Exception e)
		{
			string text = e.Message;
			if (string.IsNullOrEmpty(text))
			{
				text = "(empty exception message)";
			}
			string text2 = e.StackTrace;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "(empty stack trace)";
			}
			Logs.printLine(text);
			Logs.printLine(text2);
			CommandWindow.LogError(text);
			CommandWindow.LogError(text2);
			if (e.InnerException != null)
			{
				UnturnedLog.internalException(e.InnerException);
			}
		}

		/// <summary>
		/// This is the ONLY place Unturned should be binding logMessageReceived.
		///
		/// This gives us greater control over how logging is handled. In particular, Unity's
		/// headless builds route logs (including stack traces) through stdout which is undesirable
		/// for dedicated servers, so we only call Debug.Log* in the editor and development builds. 
		/// </summary>
		// Token: 0x06004703 RID: 18179 RVA: 0x001A81A6 File Offset: 0x001A63A6
		private static void onBuiltinUnityLogMessageReceived(string text, string stack, LogType type)
		{
			if (UnturnedLog.insideLog)
			{
				return;
			}
			Logs.printLine(text);
			if (type <= LogType.Warning || type == LogType.Exception)
			{
				Logs.printLine(stack);
			}
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x001A81C4 File Offset: 0x001A63C4
		static UnturnedLog()
		{
			Application.logMessageReceived += UnturnedLog.onBuiltinUnityLogMessageReceived;
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x001A81D7 File Offset: 0x001A63D7
		public static void info(object message)
		{
			if (message != null)
			{
				UnturnedLog.info(message.ToString());
			}
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x001A81E7 File Offset: 0x001A63E7
		public static void warn(object message)
		{
			if (message != null)
			{
				UnturnedLog.warn(message.ToString());
			}
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x001A81F7 File Offset: 0x001A63F7
		public static void error(object message)
		{
			if (message != null)
			{
				UnturnedLog.error(message.ToString());
			}
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x001A8207 File Offset: 0x001A6407
		public static void info(string format, params object[] args)
		{
			UnturnedLog.info(string.Format(format, args));
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x001A8215 File Offset: 0x001A6415
		public static void warn(string format, params object[] args)
		{
			UnturnedLog.warn(string.Format(format, args));
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x001A8223 File Offset: 0x001A6423
		public static void error(string format, params object[] args)
		{
			UnturnedLog.error(string.Format(format, args));
		}

		/// <summary>
		/// Log an exception with message providing context.
		/// </summary>
		// Token: 0x0600470B RID: 18187 RVA: 0x001A8234 File Offset: 0x001A6434
		public static void exception(Exception e, string format, params object[] args)
		{
			try
			{
				UnturnedLog.error(string.Format(format, args));
			}
			catch
			{
			}
			UnturnedLog.exception(e);
		}

		// Token: 0x0400304E RID: 12366
		private static bool insideLog;
	}
}
