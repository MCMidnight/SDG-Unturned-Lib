using System;
using System.Collections.Generic;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace SDG.Unturned
{
	/// <summary>
	/// Disables Unity native systems unused by Unturned.
	/// </summary>
	// Token: 0x0200052E RID: 1326
	public static class UnturnedPlayerLoop
	{
		// Token: 0x0600297B RID: 10619 RVA: 0x000B09F4 File Offset: 0x000AEBF4
		public static void initialize()
		{
			HashSet<Type> hashSet = new HashSet<Type>();
			hashSet.Add(typeof(EarlyUpdate.AnalyticsCoreStatsUpdate));
			hashSet.Add(typeof(EarlyUpdate.ARCoreUpdate));
			hashSet.Add(typeof(EarlyUpdate.DeliverIosPlatformEvents));
			hashSet.Add(typeof(EarlyUpdate.UpdateKinect));
			hashSet.Add(typeof(EarlyUpdate.XRUpdate));
			hashSet.Add(typeof(FixedUpdate.NewInputFixedUpdate));
			hashSet.Add(typeof(FixedUpdate.Physics2DFixedUpdate));
			hashSet.Add(typeof(FixedUpdate.XRFixedUpdate));
			hashSet.Add(typeof(Initialization.XREarlyUpdate));
			hashSet.Add(typeof(PostLateUpdate.EnlightenRuntimeUpdate));
			hashSet.Add(typeof(PostLateUpdate.ExecuteGameCenterCallbacks));
			hashSet.Add(typeof(PostLateUpdate.UpdateLightProbeProxyVolumes));
			hashSet.Add(typeof(PostLateUpdate.UpdateSubstance));
			hashSet.Add(typeof(PostLateUpdate.XRPostLateUpdate));
			hashSet.Add(typeof(PostLateUpdate.XRPostPresent));
			hashSet.Add(typeof(PostLateUpdate.XRPreEndFrame));
			hashSet.Add(typeof(PreLateUpdate.AIUpdatePostScript));
			hashSet.Add(typeof(PreLateUpdate.Physics2DLateUpdate));
			hashSet.Add(typeof(PreLateUpdate.UNetUpdate));
			hashSet.Add(typeof(PreLateUpdate.UpdateMasterServerInterface));
			hashSet.Add(typeof(PreLateUpdate.UpdateNetworkManager));
			hashSet.Add(typeof(PreUpdate.AIUpdate));
			hashSet.Add(typeof(PreUpdate.NewInputUpdate));
			hashSet.Add(typeof(PreUpdate.Physics2DUpdate));
			hashSet.Add(typeof(PreUpdate.SendMouseEvents));
			PlayerLoopSystem defaultPlayerLoop = PlayerLoop.GetDefaultPlayerLoop();
			UnturnedPlayerLoop.recursiveTidyPlayerLoop(hashSet, ref defaultPlayerLoop);
			PlayerLoop.SetPlayerLoop(defaultPlayerLoop);
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000B0BC4 File Offset: 0x000AEDC4
		private static void recursiveTidyPlayerLoop(HashSet<Type> disabledSystems, ref PlayerLoopSystem system)
		{
			int num = system.subSystemList.Length;
			List<PlayerLoopSystem> list = new List<PlayerLoopSystem>(num);
			for (int i = 0; i < num; i++)
			{
				PlayerLoopSystem playerLoopSystem = system.subSystemList[i];
				if (!disabledSystems.Contains(playerLoopSystem.type))
				{
					if (playerLoopSystem.subSystemList != null && playerLoopSystem.subSystemList.Length != 0)
					{
						UnturnedPlayerLoop.recursiveTidyPlayerLoop(disabledSystems, ref playerLoopSystem);
					}
					list.Add(playerLoopSystem);
				}
			}
			system.subSystemList = list.ToArray();
		}
	}
}
