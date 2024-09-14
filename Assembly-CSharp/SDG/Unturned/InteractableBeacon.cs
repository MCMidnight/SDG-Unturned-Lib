using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200044A RID: 1098
	public class InteractableBeacon : MonoBehaviour, IManualOnDestroy
	{
		// Token: 0x060020EF RID: 8431 RVA: 0x0007F185 File Offset: 0x0007D385
		public void updateState(ItemBarricadeAsset asset)
		{
			this.asset = (ItemBeaconAsset)asset;
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060020F0 RID: 8432 RVA: 0x0007F193 File Offset: 0x0007D393
		public bool IsChildOfVehicle
		{
			get
			{
				return base.transform.parent != null && base.transform.parent.CompareTag("Vehicle");
			}
		}

		/// <summary>
		/// Number of players inside the navmesh when the beacon was placed.
		/// Clamped to 1 if ShouldScaleWithNumberOfParticipants is false.
		/// </summary>
		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060020F1 RID: 8433 RVA: 0x0007F1BF File Offset: 0x0007D3BF
		// (set) Token: 0x060020F2 RID: 8434 RVA: 0x0007F1C7 File Offset: 0x0007D3C7
		public int initialParticipants { get; private set; }

		// Token: 0x060020F3 RID: 8435 RVA: 0x0007F1D0 File Offset: 0x0007D3D0
		public void init(int amount)
		{
			if (this.wasInit)
			{
				return;
			}
			if (amount >= (int)this.asset.wave)
			{
				this.remaining = 0;
				this.alive = (int)this.asset.wave;
			}
			else
			{
				this.remaining = (int)this.asset.wave - amount;
				this.alive = amount;
			}
			this.wasInit = true;
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x0007F22F File Offset: 0x0007D42F
		public int getRemaining()
		{
			return this.remaining;
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x0007F237 File Offset: 0x0007D437
		public void spawnRemaining()
		{
			if (this.remaining <= 0)
			{
				return;
			}
			this.remaining--;
			this.alive++;
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x0007F25F File Offset: 0x0007D45F
		public int getAlive()
		{
			return this.alive;
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x0007F268 File Offset: 0x0007D468
		public void despawnAlive()
		{
			if (this.alive <= 0)
			{
				return;
			}
			this.alive--;
			if (this.remaining == 0 && this.alive == 0)
			{
				BarricadeManager.damage(base.transform, 10000f, 1f, false, default(CSteamID), EDamageOrigin.Horde_Beacon_Self_Destruct);
			}
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x0007F2C0 File Offset: 0x0007D4C0
		private void Update()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (Time.realtimeSinceStartup - this.started < 3f)
			{
				return;
			}
			if (this.isRegistered)
			{
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					SteamPlayer steamPlayer = Provider.clients[i];
					if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead && steamPlayer.player.movement.nav == this.nav)
					{
						return;
					}
				}
			}
			BarricadeManager.damage(base.transform, 10000f, 1f, false, default(CSteamID), EDamageOrigin.Horde_Beacon_Self_Destruct);
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x0007F394 File Offset: 0x0007D594
		private void Start()
		{
			this.started = Time.realtimeSinceStartup;
			Transform transform = base.transform.Find("Engine");
			if (transform != null)
			{
				transform.gameObject.SetActive(true);
			}
			if (!Provider.isServer)
			{
				return;
			}
			if (this.isRegistered)
			{
				return;
			}
			if (this.IsChildOfVehicle)
			{
				return;
			}
			if (!LevelNavigation.checkNavigation(base.transform.position))
			{
				return;
			}
			LevelNavigation.tryGetNavigation(base.transform.position, out this.nav);
			if (this.asset.ShouldScaleWithNumberOfParticipants)
			{
				this.initialParticipants = BeaconManager.getParticipants(this.nav);
			}
			else
			{
				this.initialParticipants = 1;
			}
			BeaconManager.registerBeacon(this.nav, this);
			this.isRegistered = true;
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x0007F450 File Offset: 0x0007D650
		public void ManualOnDestroy()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (!this.isRegistered)
			{
				return;
			}
			BeaconManager.deregisterBeacon(this.nav, this);
			this.isRegistered = false;
			if (!this.wasInit)
			{
				return;
			}
			if (this.remaining > 0 || this.alive > 0)
			{
				return;
			}
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].player != null && Provider.clients[i].player.life.IsAlive && Provider.clients[i].player.movement.nav == this.nav)
				{
					Provider.clients[i].player.quests.trackHordeKill();
				}
			}
			int num = (int)this.asset.rewards;
			int num2 = Mathf.Max(1, this.initialParticipants);
			uint beacon_Max_Participants = Provider.modeConfigData.Zombies.Beacon_Max_Participants;
			if (beacon_Max_Participants > 0U)
			{
				num2 = Mathf.Min(this.initialParticipants, (int)beacon_Max_Participants);
			}
			float num3 = Mathf.Sqrt((float)num2);
			num = Mathf.CeilToInt((float)num * num3);
			num = Mathf.CeilToInt((float)num * Provider.modeConfigData.Zombies.Beacon_Rewards_Multiplier);
			uint beacon_Max_Rewards = Provider.modeConfigData.Zombies.Beacon_Max_Rewards;
			if (beacon_Max_Rewards > 0U)
			{
				num = Mathf.Min(num, (int)beacon_Max_Rewards);
			}
			num = Mathf.Min(num, 256);
			for (int j = 0; j < num; j++)
			{
				ushort num4 = SpawnTableTool.ResolveLegacyId(this.asset.rewardID, EAssetType.ITEM, new Func<string>(this.OnGetRewardErrorContext));
				if (num4 != 0)
				{
					ItemManager.dropItem(new Item(num4, EItemOrigin.NATURE), base.transform.position, false, true, true);
				}
			}
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x0007F60A File Offset: 0x0007D80A
		private string OnGetRewardErrorContext()
		{
			string text = "Horde beacon reward ";
			ItemBeaconAsset itemBeaconAsset = this.asset;
			return text + ((itemBeaconAsset != null) ? itemBeaconAsset.FriendlyName : null);
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060020FC RID: 8444 RVA: 0x0007F628 File Offset: 0x0007D828
		[Obsolete("Renamed to IsChildOfVehicle")]
		public bool isPlant
		{
			get
			{
				return this.IsChildOfVehicle;
			}
		}

		// Token: 0x04001027 RID: 4135
		private ItemBeaconAsset asset;

		// Token: 0x04001029 RID: 4137
		private byte nav;

		// Token: 0x0400102A RID: 4138
		private bool wasInit;

		// Token: 0x0400102B RID: 4139
		private float started;

		// Token: 0x0400102C RID: 4140
		private int remaining;

		// Token: 0x0400102D RID: 4141
		private int alive;

		// Token: 0x0400102E RID: 4142
		private bool isRegistered;
	}
}
