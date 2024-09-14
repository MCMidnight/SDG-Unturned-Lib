using System;
using System.Diagnostics;
using Steamworks;

namespace SDG.Unturned
{
	/// <summary>
	/// If using a map or mods from the workshop, this class monitors them for changes so the server can be restarted.
	/// </summary>
	// Token: 0x020002A5 RID: 677
	public class DedicatedWorkshopUpdateMonitor : IDedicatedWorkshopUpdateMonitor
	{
		// Token: 0x06001461 RID: 5217 RVA: 0x0004BE2C File Offset: 0x0004A02C
		public DedicatedWorkshopUpdateMonitor(DedicatedWorkshopUpdateMonitor.MonitoredItem[] monitoredItems, float queryInterval = 900f)
		{
			this.monitoredItems = monitoredItems;
			this.fileIdsForQuery = new PublishedFileId_t[monitoredItems.Length];
			for (int i = 0; i < monitoredItems.Length; i++)
			{
				this.fileIdsForQuery[i] = monitoredItems[i].fileId;
			}
			this.queryInterval = queryInterval;
			this.queryTimer = 0f;
			this.queryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.onQueryCompleted));
			this.queryHandle = UGCQueryHandle_t.Invalid;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0004BEB0 File Offset: 0x0004A0B0
		public void tick(float deltaTime)
		{
			if (this.isFinishedTicking)
			{
				return;
			}
			if (this.shouldDoFinalTick)
			{
				this.shouldDoFinalTick = false;
				this.isFinishedTicking = true;
				this.handleFinalTick();
				return;
			}
			if (this.queryHandle != UGCQueryHandle_t.Invalid)
			{
				return;
			}
			this.queryTimer += deltaTime;
			if (this.queryTimer > this.queryInterval)
			{
				this.queryTimer = 0f;
				this.handleTimerTriggered();
			}
		}

		/// <summary>
		/// Request status of workshop items.
		/// </summary>
		// Token: 0x06001463 RID: 5219 RVA: 0x0004BF24 File Offset: 0x0004A124
		protected void submitQueryRequest(PublishedFileId_t[] fileIds)
		{
			if (fileIds == null || fileIds.Length < 1)
			{
				return;
			}
			if (this.queryHandle != UGCQueryHandle_t.Invalid)
			{
				throw new Exception("Already waiting on a pending query");
			}
			this.queryHandle = SteamGameServerUGC.CreateQueryUGCDetailsRequest(fileIds, (uint)fileIds.Length);
			SteamAPICall_t hAPICall = SteamGameServerUGC.SendQueryUGCRequest(this.queryHandle);
			this.queryCompleted.Set(hAPICall, null);
		}

		/// <summary>
		/// Called the next tick after update(s) detected.
		/// </summary>
		// Token: 0x06001464 RID: 5220 RVA: 0x0004BF80 File Offset: 0x0004A180
		protected virtual void handleFinalTick()
		{
			WorkshopDownloadConfig workshopDownloadConfig = WorkshopDownloadConfig.get();
			ChatManager.say(string.Format(workshopDownloadConfig.Shutdown_Update_Detected_Message, workshopDownloadConfig.Shutdown_Update_Detected_Timer), ChatManager.welcomeColor, true);
			Provider.shutdown(workshopDownloadConfig.Shutdown_Update_Detected_Timer, workshopDownloadConfig.Shutdown_Kick_Message);
		}

		/// <summary>
		/// Called when a queried item's update timestamp is newer than our initially loaded version.
		/// </summary>
		// Token: 0x06001465 RID: 5221 RVA: 0x0004BFC5 File Offset: 0x0004A1C5
		protected virtual void handleUpdateDetected(SteamUGCDetails_t fileDetails)
		{
			CommandWindow.LogFormat("Detected an update to '{0}' ({1})", new object[]
			{
				fileDetails.m_rgchTitle,
				fileDetails.m_nPublishedFileId
			});
			this.shouldDoFinalTick = true;
		}

		/// <summary>
		/// Called when results from a call to submitQueryRequest are available.
		/// </summary>
		// Token: 0x06001466 RID: 5222 RVA: 0x0004BFF8 File Offset: 0x0004A1F8
		protected virtual void handleQueryResponse(SteamUGCQueryCompleted_t callback)
		{
			for (uint num = 0U; num < callback.m_unNumResultsReturned; num += 1U)
			{
				SteamUGCDetails_t steamUGCDetails_t;
				if (SteamGameServerUGC.GetQueryUGCResult(callback.m_handle, num, out steamUGCDetails_t))
				{
					uint num2;
					if (this.getInitialTimestamp(steamUGCDetails_t.m_nPublishedFileId, out num2))
					{
						if (steamUGCDetails_t.m_rtimeUpdated > num2)
						{
							this.handleUpdateDetected(steamUGCDetails_t);
						}
					}
					else
					{
						UnturnedLog.warn("Unable to find local timestamp for monitored workshop item '{0}' ({1})", new object[]
						{
							steamUGCDetails_t.m_rgchTitle,
							steamUGCDetails_t.m_nPublishedFileId
						});
					}
				}
			}
		}

		/// <summary>
		/// Called once timer reaches interval.
		/// </summary>
		// Token: 0x06001467 RID: 5223 RVA: 0x0004C070 File Offset: 0x0004A270
		protected virtual void handleTimerTriggered()
		{
			this.submitQueryRequest(this.fileIdsForQuery);
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0004C080 File Offset: 0x0004A280
		protected bool getInitialTimestamp(PublishedFileId_t fileId, out uint timestamp)
		{
			foreach (DedicatedWorkshopUpdateMonitor.MonitoredItem monitoredItem in this.monitoredItems)
			{
				if (monitoredItem.fileId == fileId)
				{
					timestamp = monitoredItem.initialTimestamp;
					return true;
				}
			}
			timestamp = 0U;
			return false;
		}

		/// <summary>
		/// Callback from Steam when results from a call to submitQueryRequest are available.
		/// </summary>
		// Token: 0x06001469 RID: 5225 RVA: 0x0004C0C8 File Offset: 0x0004A2C8
		private void onQueryCompleted(SteamUGCQueryCompleted_t callback, bool ioFailure)
		{
			if (callback.m_handle != this.queryHandle)
			{
				return;
			}
			if (ioFailure)
			{
				UnturnedLog.warn("Encountered IO error when monitoring workshop changes");
			}
			else if (callback.m_eResult == EResult.k_EResultOK)
			{
				this.handleQueryResponse(callback);
			}
			else
			{
				UnturnedLog.warn("Encountered error '{0}' when monitoring workshop changes", new object[]
				{
					callback.m_eResult
				});
			}
			SteamGameServerUGC.ReleaseQueryUGCRequest(this.queryHandle);
			this.queryHandle = UGCQueryHandle_t.Invalid;
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0004C13F File Offset: 0x0004A33F
		[Conditional("LOG_WORKSHOP_UPDATE_MONITOR")]
		private void debugMessage(string message)
		{
			CommandWindow.Log(message);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0004C147 File Offset: 0x0004A347
		[Conditional("LOG_WORKSHOP_UPDATE_MONITOR")]
		private void debugMessage(string format, params object[] args)
		{
			CommandWindow.LogFormat(format, args);
		}

		/// <summary>
		/// Were update(s) detected that should be handled on next tick?
		/// </summary>
		// Token: 0x04000706 RID: 1798
		protected bool shouldDoFinalTick;

		/// <summary>
		/// Are we done monitoring?
		/// Default finished once an update is detected.
		/// </summary>
		// Token: 0x04000707 RID: 1799
		protected bool isFinishedTicking;

		// Token: 0x04000708 RID: 1800
		protected DedicatedWorkshopUpdateMonitor.MonitoredItem[] monitoredItems;

		// Token: 0x04000709 RID: 1801
		protected PublishedFileId_t[] fileIdsForQuery;

		/// <summary>
		/// Interval between query submissions.
		/// </summary>
		// Token: 0x0400070A RID: 1802
		protected float queryInterval;

		/// <summary>
		/// Accumulated time before submitting query after passing interval.
		/// </summary>
		// Token: 0x0400070B RID: 1803
		protected float queryTimer;

		// Token: 0x0400070C RID: 1804
		private CallResult<SteamUGCQueryCompleted_t> queryCompleted;

		// Token: 0x0400070D RID: 1805
		private UGCQueryHandle_t queryHandle;

		// Token: 0x0200091A RID: 2330
		public struct MonitoredItem
		{
			// Token: 0x04003253 RID: 12883
			public PublishedFileId_t fileId;

			// Token: 0x04003254 RID: 12884
			public uint initialTimestamp;
		}
	}
}
