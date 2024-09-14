using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000667 RID: 1639
	public class Dedicator : MonoBehaviour
	{
		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060036B4 RID: 14004 RVA: 0x00100BCE File Offset: 0x000FEDCE
		// (set) Token: 0x060036B5 RID: 14005 RVA: 0x00100BD5 File Offset: 0x000FEDD5
		public static CommandWindow commandWindow { get; protected set; }

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060036B6 RID: 14006 RVA: 0x00100BDD File Offset: 0x000FEDDD
		[Obsolete("Server plugins do not need to check this because they run on the dedicated-server-only builds.")]
		public static bool isDedicated
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Are we currently running the standalone dedicated server app?
		/// </summary>
		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060036B7 RID: 14007 RVA: 0x00100BE0 File Offset: 0x000FEDE0
		public static bool isStandaloneDedicatedServer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060036B8 RID: 14008 RVA: 0x00100BE3 File Offset: 0x000FEDE3
		public static bool hasBattlEye
		{
			get
			{
				return Dedicator._hasBattlEye;
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060036B9 RID: 14009 RVA: 0x00100BEA File Offset: 0x000FEDEA
		public static bool isVR
		{
			get
			{
				return Dedicator._isVR;
			}
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x00100BF1 File Offset: 0x000FEDF1
		private void Update()
		{
			if (Dedicator.commandWindow != null)
			{
				Dedicator.commandWindow.update();
			}
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x00100C04 File Offset: 0x000FEE04
		public void awake()
		{
			int num = CommandLine.tryGetServer(out Dedicator.serverVisibility, out Dedicator.serverID) ? 1 : 0;
			Dedicator._hasBattlEye = (Environment.CommandLine.IndexOf("-BattlEye", 5) != -1);
			Dedicator._isVR = false;
			bool flag = num == 0;
			if (flag)
			{
				Dedicator.serverVisibility = ESteamServerVisibility.LAN;
				Dedicator.serverID = "Default";
			}
			UnturnedMasterVolume.mutedByDedicatedServer = true;
			Dedicator.commandWindow = new CommandWindow();
			int num2 = 50;
			Application.targetFrameRate = num2;
			UnturnedLog.info(string.Format("Dedicated server set target update rate to {0}", num2));
			if (flag)
			{
				CommandWindow.Log("Running standalone dedicated server, but launch arguments were not specified on the command-line.");
				CommandWindow.LogFormat("Defaulting to {0} {1}. Valid command-line dedicated server launch arguments are:", new object[]
				{
					Dedicator.serverID,
					Dedicator.serverVisibility
				});
				CommandWindow.Log("+InternetServer/{ID}");
				CommandWindow.Log("+LANServer/{ID}");
			}
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x00100CCC File Offset: 0x000FEECC
		private void OnApplicationQuit()
		{
			if (Dedicator.commandWindow != null)
			{
				Dedicator.commandWindow.shutdown();
			}
		}

		// Token: 0x04001FB3 RID: 8115
		public static ESteamServerVisibility serverVisibility;

		// Token: 0x04001FB4 RID: 8116
		public static string serverID;

		// Token: 0x04001FB5 RID: 8117
		public const bool IsDedicatedServer = true;

		/// <summary>
		/// Should dedicated server disable requests to internet?
		/// While in LAN mode skips the Steam backend connection and workshop item queries.
		/// Needs a non-Steam networking implementation before it will be truly offline only.
		/// </summary>
		// Token: 0x04001FB6 RID: 8118
		public static CommandLineFlag offlineOnly = new CommandLineFlag(false, "-OfflineOnly");

		// Token: 0x04001FB7 RID: 8119
		private static bool _hasBattlEye;

		// Token: 0x04001FB8 RID: 8120
		private static bool _isVR;
	}
}
