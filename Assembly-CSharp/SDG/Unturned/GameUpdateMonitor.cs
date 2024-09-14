using System;

namespace SDG.Unturned
{
	// Token: 0x02000665 RID: 1637
	public static class GameUpdateMonitor
	{
		/// <summary>
		/// Event for plugins to be notified when a server update is detected.
		///
		/// Pandahut requested this because they run the game as a Windows service and need to shutdown
		/// through their central management system rather than per-process.
		/// </summary>
		// Token: 0x140000CF RID: 207
		// (add) Token: 0x060036AB RID: 13995 RVA: 0x00100624 File Offset: 0x000FE824
		// (remove) Token: 0x060036AC RID: 13996 RVA: 0x00100658 File Offset: 0x000FE858
		public static event GameUpdateMonitor.GameUpdateDetectedHandler OnGameUpdateDetected;

		// Token: 0x060036AD RID: 13997 RVA: 0x0010068C File Offset: 0x000FE88C
		internal static void NotifyGameUpdateDetected(string newVersion, ref bool shouldShutdown)
		{
			try
			{
				GameUpdateMonitor.GameUpdateDetectedHandler onGameUpdateDetected = GameUpdateMonitor.OnGameUpdateDetected;
				if (onGameUpdateDetected != null)
				{
					onGameUpdateDetected(newVersion, ref shouldShutdown);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught plugin exception during OnGameUpdateDetected:");
			}
		}

		// Token: 0x020009B3 RID: 2483
		// (Invoke) Token: 0x06004C1C RID: 19484
		public delegate void GameUpdateDetectedHandler(string newVersion, ref bool shouldShutdown);
	}
}
