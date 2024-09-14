using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006D9 RID: 1753
	public class ServerConfigData
	{
		// Token: 0x06003AEE RID: 15086 RVA: 0x00113114 File Offset: 0x00111314
		public ServerConfigData()
		{
			this.VAC_Secure = true;
			this.BattlEye_Secure = true;
			this.Max_Ping_Milliseconds = 750U;
			this.Timeout_Queue_Seconds = 15f;
			this.Timeout_Game_Seconds = 30f;
			this.Max_Packets_Per_Second = 50f;
			this.Rate_Limit_Kick_Threshold = 10;
			this.Fake_Lag_Threshold_Seconds = 3f;
			this.Fake_Lag_Damage_Penalty_Multiplier = 0.1f;
			this.Enable_Kick_Input_Spam = false;
			this.Enable_Kick_Input_Timeout = false;
			this.Validate_EconInfo_Hash = true;
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x00113243 File Offset: 0x00111443
		internal float GetClampedTimeoutQueueSeconds()
		{
			return Mathf.Clamp(this.Timeout_Queue_Seconds, 1f, 25f);
		}

		// Token: 0x040023B6 RID: 9142
		public bool VAC_Secure;

		// Token: 0x040023B7 RID: 9143
		public bool BattlEye_Secure;

		// Token: 0x040023B8 RID: 9144
		public uint Max_Ping_Milliseconds;

		// Token: 0x040023B9 RID: 9145
		public float Timeout_Queue_Seconds;

		// Token: 0x040023BA RID: 9146
		public float Timeout_Game_Seconds;

		// Token: 0x040023BB RID: 9147
		public float Max_Packets_Per_Second;

		/// <summary>
		/// If a rate-limited method is called this many times within cooldown window the client will be kicked.
		/// For example a value of 1 means the client will be kicked the first time they call the method off-cooldown. (not recommended)
		/// </summary>
		// Token: 0x040023BC RID: 9148
		public int Rate_Limit_Kick_Threshold;

		/// <summary>
		/// Ordinarily the server should be receiving multiple input packets per second from a client. If more than this
		/// amount of time passes between input packets we flag the client as potentially using a lag switch, and modify
		/// their stats (e.g. reduce player damage) for a corresponding duration.
		/// Minimum value is PlayerInput.MIN_FAKE_LAG_THRESHOLD_SECONDS.
		/// </summary>
		// Token: 0x040023BD RID: 9149
		public float Fake_Lag_Threshold_Seconds;

		/// <summary>
		/// Whether fake lag detection should log to command output. False positives are relatively likely when client
		/// framerate hitches (e.g. loading dense region), so this is best used for tuning threshold rather than bans.
		/// </summary>
		// Token: 0x040023BE RID: 9150
		public bool Fake_Lag_Log_Warnings;

		/// <summary>
		/// PvP damage multiplier while under fake lag penalty.
		/// </summary>
		// Token: 0x040023BF RID: 9151
		public float Fake_Lag_Damage_Penalty_Multiplier;

		/// <summary>
		/// Should we kick players after detecting spammed calls to askInput?
		/// </summary>
		// Token: 0x040023C0 RID: 9152
		public bool Enable_Kick_Input_Spam;

		/// <summary>
		/// Should we kick players if they do not submit inputs for a long time?
		/// </summary>
		// Token: 0x040023C1 RID: 9153
		public bool Enable_Kick_Input_Timeout;

		/// <summary>
		/// Should the server automatically shutdown at a configured time?
		/// </summary>
		// Token: 0x040023C2 RID: 9154
		public bool Enable_Scheduled_Shutdown;

		/// <summary>
		/// When the server should shutdown if Enable_Scheduled_Shutdown is true.
		/// </summary>
		// Token: 0x040023C3 RID: 9155
		public string Scheduled_Shutdown_Time = "1:30 am";

		/// <summary>
		/// Broadcast "shutting down for scheduled maintenance" warnings at these intervals.
		/// </summary>
		// Token: 0x040023C4 RID: 9156
		public string[] Scheduled_Shutdown_Warnings = new string[]
		{
			"00:30:00",
			"00:15:00",
			"00:05:00",
			"00:01:00",
			"00:00:30",
			"00:00:15",
			"00:00:03",
			"00:00:02",
			"00:00:01"
		};

		/// <summary>
		/// Should the server automatically shutdown when a new version is detected?
		/// </summary>
		// Token: 0x040023C5 RID: 9157
		public bool Enable_Update_Shutdown;

		/// <summary>
		/// Unfortunately the server does not have a way to automatically determine the current beta branch.
		/// </summary>
		// Token: 0x040023C6 RID: 9158
		public string Update_Steam_Beta_Name = "public";

		/// <summary>
		/// Broadcast "shutting down for update" warnings at these intervals.
		/// </summary>
		// Token: 0x040023C7 RID: 9159
		public string[] Update_Shutdown_Warnings = new string[]
		{
			"00:03:00",
			"00:01:00",
			"00:00:30",
			"00:00:15",
			"00:00:03",
			"00:00:02",
			"00:00:01"
		};

		/// <summary>
		/// Should vanilla text chat messages always use rich text?
		/// Servers with plugins may want to enable because IMGUI does not fade out rich text.
		/// Kept because plugins might be setting this directly, but it no longer does anything.
		/// </summary>
		// Token: 0x040023C8 RID: 9160
		[Obsolete("uGUI supports rich text fade out.")]
		[NonSerialized]
		public bool Chat_Always_Use_Rich_Text;

		/// <summary>
		/// Should the EconInfo.json hash be checked by the server?
		/// </summary>
		// Token: 0x040023C9 RID: 9161
		public bool Validate_EconInfo_Hash;

		/// <summary>
		/// If true, opt-in to SteamNetworkingSockets "FakeIP" system.
		/// https://partner.steamgames.com/doc/api/ISteamNetworkingSockets#1
		/// </summary>
		// Token: 0x040023CA RID: 9162
		public bool Use_FakeIP;

		/// <summary>
		/// Limit max queue timeout duration so that if server encounters an error or doesn't
		/// process the request the client can timeout locally.
		/// </summary>
		// Token: 0x040023CB RID: 9163
		internal const float MAX_TIMEOUT_QUEUE_SECONDS = 25f;

		/// <summary>
		/// Longer than server timeout so that ideally more context is logged on the server
		/// rather than just "client disconnected."
		/// </summary>
		// Token: 0x040023CC RID: 9164
		internal const float CLIENT_TIMEOUT_QUEUE_SECONDS = 30f;
	}
}
