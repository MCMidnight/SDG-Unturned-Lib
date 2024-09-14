using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Configuration for DedicatedUGC.
	/// </summary>
	// Token: 0x020006BA RID: 1722
	public class WorkshopDownloadConfig
	{
		// Token: 0x06003980 RID: 14720 RVA: 0x0010DD30 File Offset: 0x0010BF30
		public WorkshopDownloadConfig()
		{
			this.File_IDs = new List<ulong>();
			this.Ignore_Children_File_IDs = new List<ulong>();
			this.Query_Cache_Max_Age_Seconds = 600U;
			this.Max_Query_Retries = 2U;
			this.Use_Cached_Downloads = true;
			this.Should_Monitor_Updates = true;
			this.Shutdown_Update_Detected_Timer = 600;
			this.Shutdown_Update_Detected_Message = "Workshop file update detected, shutdown in: {0}";
			this.Shutdown_Kick_Message = "Shutdown for Workshop file update.";
		}

		/// <summary>
		/// Get instance if loaded, but do not load.
		/// </summary>
		// Token: 0x06003981 RID: 14721 RVA: 0x0010DD9A File Offset: 0x0010BF9A
		public static WorkshopDownloadConfig get()
		{
			return WorkshopDownloadConfig.instance;
		}

		/// <summary>
		/// Get instance, or load if not yet loaded.
		/// </summary>
		// Token: 0x06003982 RID: 14722 RVA: 0x0010DDA1 File Offset: 0x0010BFA1
		public static WorkshopDownloadConfig getOrLoad()
		{
			if (WorkshopDownloadConfig.instance == null)
			{
				WorkshopDownloadConfig.instance = WorkshopDownloadConfig.load();
			}
			return WorkshopDownloadConfig.instance;
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x0010DDBC File Offset: 0x0010BFBC
		private static WorkshopDownloadConfig load()
		{
			WorkshopDownloadConfig workshopDownloadConfig;
			if (ServerSavedata.fileExists("/WorkshopDownloadConfig.json"))
			{
				workshopDownloadConfig = WorkshopDownloadConfig.loadFromConfig();
			}
			else if (ServerSavedata.fileExists("/WorkshopDownloadIDs.json"))
			{
				workshopDownloadConfig = WorkshopDownloadConfig.loadFromLegacyFormat();
			}
			else
			{
				workshopDownloadConfig = null;
			}
			if (workshopDownloadConfig == null)
			{
				workshopDownloadConfig = new WorkshopDownloadConfig();
			}
			ServerSavedata.serializeJSON<WorkshopDownloadConfig>("/WorkshopDownloadConfig.json", workshopDownloadConfig);
			return workshopDownloadConfig;
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x0010DE08 File Offset: 0x0010C008
		private static WorkshopDownloadConfig loadFromConfig()
		{
			WorkshopDownloadConfig result;
			try
			{
				result = ServerSavedata.deserializeJSON<WorkshopDownloadConfig>("/WorkshopDownloadConfig.json");
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Unable to parse WorkshopDownloadConfig.json! consider validating with a JSON linter");
				result = null;
			}
			return result;
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x0010DE44 File Offset: 0x0010C044
		private static WorkshopDownloadConfig loadFromLegacyFormat()
		{
			WorkshopDownloadConfig workshopDownloadConfig;
			try
			{
				workshopDownloadConfig = new WorkshopDownloadConfig();
				workshopDownloadConfig.File_IDs = ServerSavedata.deserializeJSON<List<ulong>>("/WorkshopDownloadIDs.json");
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Unable to parse WorkshopDownloadIDs.json! consider validating with a JSON linter");
				workshopDownloadConfig = null;
			}
			return workshopDownloadConfig;
		}

		/// <summary>
		/// Published workshop file IDs to download.
		/// </summary>
		// Token: 0x04002230 RID: 8752
		public List<ulong> File_IDs;

		/// <summary>
		/// Published workshop file IDs whose children (dependencies) should be skipped.
		/// Useful if workshop author lists dependencies as a way of advertising.
		/// </summary>
		// Token: 0x04002231 RID: 8753
		public List<ulong> Ignore_Children_File_IDs;

		/// <summary>
		/// Controls SetAllowCachedResponse. Disabled when set to zero.
		/// Balance between item change frequency and allowing cached results when query fails.
		/// </summary>
		// Token: 0x04002232 RID: 8754
		public uint Query_Cache_Max_Age_Seconds;

		/// <summary>
		/// Number of total times to try re-submitting failed workshop queries before aborting.
		/// </summary>
		// Token: 0x04002233 RID: 8755
		public uint Max_Query_Retries;

		/// <summary>
		/// Should items already installed be loaded?
		/// </summary>
		// Token: 0x04002234 RID: 8756
		public bool Use_Cached_Downloads;

		/// <summary>
		/// Should used items be monitored for updates?
		/// </summary>
		// Token: 0x04002235 RID: 8757
		public bool Should_Monitor_Updates;

		/// <summary>
		/// Seconds to wait before shutting down after an update is detected.
		/// </summary>
		// Token: 0x04002236 RID: 8758
		public int Shutdown_Update_Detected_Timer;

		/// <summary>
		/// Message broadcasted when shutdown timer begins.
		/// </summary>
		// Token: 0x04002237 RID: 8759
		public string Shutdown_Update_Detected_Message;

		/// <summary>
		/// Message sent to players when shutdown timer completes.
		/// </summary>
		// Token: 0x04002238 RID: 8760
		public string Shutdown_Kick_Message;

		// Token: 0x04002239 RID: 8761
		private static WorkshopDownloadConfig instance;
	}
}
