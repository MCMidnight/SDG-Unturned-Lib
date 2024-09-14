using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000543 RID: 1347
	public class BeaconManager : MonoBehaviour
	{
		// Token: 0x06002AC7 RID: 10951 RVA: 0x000B7040 File Offset: 0x000B5240
		public static int getParticipants(byte nav)
		{
			int num = 0;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead && steamPlayer.player.movement.nav == nav)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x000B70CE File Offset: 0x000B52CE
		public static InteractableBeacon checkBeacon(byte nav)
		{
			if (BeaconManager.beacons[(int)nav].Count > 0)
			{
				return BeaconManager.beacons[(int)nav][0];
			}
			return null;
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x000B70EE File Offset: 0x000B52EE
		public static void registerBeacon(byte nav, InteractableBeacon beacon)
		{
			if (!LevelNavigation.checkSafe(nav))
			{
				return;
			}
			BeaconManager.beacons[(int)nav].Add(beacon);
			BeaconUpdated beaconUpdated = BeaconManager.onBeaconUpdated;
			if (beaconUpdated == null)
			{
				return;
			}
			beaconUpdated(nav, BeaconManager.beacons[(int)nav].Count > 0);
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x000B7125 File Offset: 0x000B5325
		public static void deregisterBeacon(byte nav, InteractableBeacon beacon)
		{
			if (!LevelNavigation.checkSafe(nav))
			{
				return;
			}
			BeaconManager.beacons[(int)nav].Remove(beacon);
			BeaconUpdated beaconUpdated = BeaconManager.onBeaconUpdated;
			if (beaconUpdated == null)
			{
				return;
			}
			beaconUpdated(nav, BeaconManager.beacons[(int)nav].Count > 0);
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x000B7160 File Offset: 0x000B5360
		private void onLevelLoaded(int level)
		{
			if (LevelNavigation.bounds == null)
			{
				return;
			}
			BeaconManager.beacons = new List<InteractableBeacon>[LevelNavigation.bounds.Count];
			for (int i = 0; i < BeaconManager.beacons.Length; i++)
			{
				BeaconManager.beacons[i] = new List<InteractableBeacon>();
			}
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000B71A7 File Offset: 0x000B53A7
		private void Start()
		{
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		// Token: 0x040016C9 RID: 5833
		private static List<InteractableBeacon>[] beacons;

		// Token: 0x040016CA RID: 5834
		public static BeaconUpdated onBeaconUpdated;
	}
}
