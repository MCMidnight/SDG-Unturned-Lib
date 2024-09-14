using System;
using System.Collections.Generic;
using SDG.Provider;
using Steamworks;

namespace SDG.Unturned
{
	/// <summary>
	/// Static functions for creating monitor instance on server.
	/// </summary>
	// Token: 0x020002A6 RID: 678
	public static class DedicatedWorkshopUpdateMonitorFactory
	{
		// Token: 0x14000073 RID: 115
		// (add) Token: 0x0600146C RID: 5228 RVA: 0x0004C150 File Offset: 0x0004A350
		// (remove) Token: 0x0600146D RID: 5229 RVA: 0x0004C184 File Offset: 0x0004A384
		public static event DedicatedWorkshopUpdateMonitorFactory.CreateHandler onCreateForLevel;

		/// <summary>
		/// Entry point called by dedicated server after loading level.
		/// </summary>
		// Token: 0x0600146E RID: 5230 RVA: 0x0004C1B7 File Offset: 0x0004A3B7
		public static IDedicatedWorkshopUpdateMonitor createForLevel(LevelInfo level)
		{
			if (!WorkshopDownloadConfig.get().Should_Monitor_Updates)
			{
				return null;
			}
			if (DedicatedWorkshopUpdateMonitorFactory.onCreateForLevel == null)
			{
				return DedicatedWorkshopUpdateMonitorFactory.createDefaultForLevel(level);
			}
			return DedicatedWorkshopUpdateMonitorFactory.onCreateForLevel(level);
		}

		/// <summary>
		/// Create vanilla update monitor that watches for changes to workshop level file and any other mods.
		/// </summary>
		// Token: 0x0600146F RID: 5231 RVA: 0x0004C1E0 File Offset: 0x0004A3E0
		public static IDedicatedWorkshopUpdateMonitor createDefaultForLevel(LevelInfo level)
		{
			List<DedicatedWorkshopUpdateMonitor.MonitoredItem> list = new List<DedicatedWorkshopUpdateMonitor.MonitoredItem>();
			DedicatedWorkshopUpdateMonitor.MonitoredItem monitoredItem;
			if (DedicatedWorkshopUpdateMonitorFactory.createMonitoredItemForLevel(level, out monitoredItem))
			{
				CommandWindow.LogFormat("Monitoring workshop map \"{0}\" ({1}) for changes", new object[]
				{
					level.name,
					level.publishedFileId
				});
				list.Add(monitoredItem);
			}
			else if (level.isFromWorkshop)
			{
				UnturnedLog.info(string.Format("Unable to monitor workshop map \"{0}\" ({1}) for changes", level.name, level.publishedFileId));
			}
			foreach (ulong num in Provider.getServerWorkshopFileIDs())
			{
				if (num != level.publishedFileId)
				{
					DedicatedWorkshopUpdateMonitor.MonitoredItem monitoredItem2;
					if (DedicatedWorkshopUpdateMonitorFactory.createMonitoredItem(new PublishedFileId_t(num), out monitoredItem2))
					{
						CommandWindow.LogFormat("Monitoring workshop file {0} for changes", new object[]
						{
							num
						});
						list.Add(monitoredItem2);
					}
					else
					{
						UnturnedLog.info(string.Format("Unable to monitor workshop file {0} for changes", num));
					}
				}
			}
			if (list.Count < 1)
			{
				UnturnedLog.info("No workshop items to monitor for updates");
				return null;
			}
			return new DedicatedWorkshopUpdateMonitor(list.ToArray(), 900f);
		}

		/// <summary>
		/// Helper to get updated timestamp from workshop items loaded by DedicatedUGC.
		/// </summary>
		// Token: 0x06001470 RID: 5232 RVA: 0x0004C308 File Offset: 0x0004A508
		public static bool getCachedInitialTimestamp(PublishedFileId_t fileId, out uint timestamp)
		{
			CachedUGCDetails cachedUGCDetails;
			if (TempSteamworksWorkshop.getCachedDetails(fileId, out cachedUGCDetails))
			{
				timestamp = cachedUGCDetails.updateTimestamp;
				return true;
			}
			timestamp = 0U;
			return false;
		}

		/// <summary>
		/// Helper to create monitored item for use with default DedicatedWorkshopUpdateMonitor implementation.
		/// </summary>
		// Token: 0x06001471 RID: 5233 RVA: 0x0004C330 File Offset: 0x0004A530
		public static bool createMonitoredItem(PublishedFileId_t fileId, out DedicatedWorkshopUpdateMonitor.MonitoredItem monitoredItem)
		{
			uint initialTimestamp;
			if (DedicatedWorkshopUpdateMonitorFactory.getCachedInitialTimestamp(fileId, out initialTimestamp))
			{
				monitoredItem = default(DedicatedWorkshopUpdateMonitor.MonitoredItem);
				monitoredItem.fileId = fileId;
				monitoredItem.initialTimestamp = initialTimestamp;
				return true;
			}
			monitoredItem = default(DedicatedWorkshopUpdateMonitor.MonitoredItem);
			return false;
		}

		/// <summary>
		/// For use with default DedicatedWorkshopUpdateMonitor implementation.
		/// </summary>
		// Token: 0x06001472 RID: 5234 RVA: 0x0004C368 File Offset: 0x0004A568
		public static bool createMonitoredItemForLevel(LevelInfo level, out DedicatedWorkshopUpdateMonitor.MonitoredItem monitoredItem)
		{
			if (level != null && level.isFromWorkshop)
			{
				PublishedFileId_t publishedFileId_t = new PublishedFileId_t(level.publishedFileId);
				if (DedicatedWorkshopUpdateMonitorFactory.createMonitoredItem(publishedFileId_t, out monitoredItem))
				{
					return true;
				}
				CommandWindow.LogWarningFormat("Unable to monitor level '{0}' ({1}) for changes because no details were cached", new object[]
				{
					level.name,
					publishedFileId_t
				});
			}
			monitoredItem = default(DedicatedWorkshopUpdateMonitor.MonitoredItem);
			return false;
		}

		// Token: 0x0200091B RID: 2331
		// (Invoke) Token: 0x06004A72 RID: 19058
		public delegate IDedicatedWorkshopUpdateMonitor CreateHandler(LevelInfo level);
	}
}
