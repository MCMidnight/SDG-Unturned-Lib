using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000451 RID: 1105
	public class InteractableFarm : Interactable
	{
		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06002140 RID: 8512 RVA: 0x000805CC File Offset: 0x0007E7CC
		// (remove) Token: 0x06002141 RID: 8513 RVA: 0x00080600 File Offset: 0x0007E800
		public static event InteractableFarm.HarvestRequestHandler OnHarvestRequested_Global;

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002142 RID: 8514 RVA: 0x00080633 File Offset: 0x0007E833
		public uint planted
		{
			get
			{
				return this._planted;
			}
		}

		/// <summary>
		/// Number of seconds to finish growing.
		/// </summary>
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002143 RID: 8515 RVA: 0x0008063B File Offset: 0x0007E83B
		public uint growth
		{
			get
			{
				ItemFarmAsset itemFarmAsset = this.farmAsset;
				if (itemFarmAsset == null)
				{
					return 1U;
				}
				return itemFarmAsset.growth;
			}
		}

		/// <summary>
		/// Item legacy ID to grant the player.
		/// </summary>
		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002144 RID: 8516 RVA: 0x0008064E File Offset: 0x0007E84E
		public ushort grow
		{
			get
			{
				ItemFarmAsset itemFarmAsset = this.farmAsset;
				if (itemFarmAsset == null)
				{
					return 0;
				}
				return itemFarmAsset.grow;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x00080661 File Offset: 0x0007E861
		public bool canFertilize
		{
			get
			{
				ItemFarmAsset itemFarmAsset = this.farmAsset;
				return itemFarmAsset != null && itemFarmAsset.canFertilize;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002146 RID: 8518 RVA: 0x00080674 File Offset: 0x0007E874
		public bool IsFullyGrown
		{
			get
			{
				return this.planted > 0U && Provider.time > this.planted && Provider.time - this.planted > this.growth;
			}
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x000806A2 File Offset: 0x0007E8A2
		public void updatePlanted(uint newPlanted)
		{
			this._planted = newPlanted;
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x000806AC File Offset: 0x0007E8AC
		public override void updateState(Asset asset, byte[] state)
		{
			this.farmAsset = (asset as ItemFarmAsset);
			ItemFarmAsset itemFarmAsset = this.farmAsset;
			this.harvestRewardExperience = ((itemFarmAsset != null) ? itemFarmAsset.harvestRewardExperience : 0U);
			if (state.Length >= 4)
			{
				this._planted = BitConverter.ToUInt32(state, 0);
			}
			else
			{
				this._planted = 0U;
			}
			if (this.isGrown)
			{
				this.isGrown = false;
				Transform transform = base.transform.Find("Foliage_0");
				if (transform != null)
				{
					transform.gameObject.SetActive(true);
				}
				Transform transform2 = base.transform.Find("Foliage_1");
				if (transform2 == null)
				{
					return;
				}
				transform2.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x00080749 File Offset: 0x0007E949
		public bool checkFarm()
		{
			return this.IsFullyGrown;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x00080751 File Offset: 0x0007E951
		public override bool checkUseable()
		{
			return this.checkFarm();
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x00080759 File Offset: 0x0007E959
		public override void use()
		{
			this.ClientHarvest();
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x00080761 File Offset: 0x0007E961
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				message = EPlayerMessage.FARM;
			}
			else
			{
				message = EPlayerMessage.GROW;
			}
			text = "";
			color = Color.white;
			return true;
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x00080788 File Offset: 0x0007E988
		private void onRainUpdated(ELightingRain rain)
		{
			if (rain != ELightingRain.POST_DRIZZLE)
			{
				return;
			}
			if (this.farmAsset != null && !this.farmAsset.shouldRainAffectGrowth)
			{
				return;
			}
			if (Physics.Raycast(base.transform.position + Vector3.up, Vector3.up, 32f, RayMasks.BLOCK_WIND))
			{
				return;
			}
			this.updatePlanted(1U);
			if (Provider.isServer)
			{
				BarricadeManager.updateFarm(base.transform, this.planted, false);
			}
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x000807FC File Offset: 0x0007E9FC
		private void Update()
		{
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x00080809 File Offset: 0x0007EA09
		private void OnEnable()
		{
			LightingManager.onRainUpdated = (RainUpdated)Delegate.Combine(LightingManager.onRainUpdated, new RainUpdated(this.onRainUpdated));
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x0008082B File Offset: 0x0007EA2B
		private void OnDisable()
		{
			LightingManager.onRainUpdated = (RainUpdated)Delegate.Remove(LightingManager.onRainUpdated, new RainUpdated(this.onRainUpdated));
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x0008084D File Offset: 0x0007EA4D
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceivePlanted(uint newPlanted)
		{
			this.updatePlanted(newPlanted);
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00080856 File Offset: 0x0007EA56
		public void ClientHarvest()
		{
			InteractableFarm.SendHarvestRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x0008086C File Offset: 0x0007EA6C
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 10)]
		public void ReceiveHarvestRequest(in ServerInvocationContext context)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if ((base.transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out barricadeRegion))
			{
				return;
			}
			bool flag = true;
			if (BarricadeManager.onHarvestPlantRequested != null)
			{
				ushort index = (ushort)barricadeRegion.IndexOfBarricadeByRootTransform(base.transform);
				BarricadeManager.onHarvestPlantRequested(player.channel.owner.playerID.steamID, x, y, plant, index, ref flag);
			}
			InteractableFarm.HarvestRequestHandler onHarvestRequested_Global = InteractableFarm.OnHarvestRequested_Global;
			if (onHarvestRequested_Global != null)
			{
				onHarvestRequested_Global(this, context.GetCallingPlayer(), ref flag);
			}
			if (!flag)
			{
				return;
			}
			if (this.checkFarm())
			{
				if (this.farmAsset != null)
				{
					ushort num = this.farmAsset.grow;
					if (num == 0)
					{
						num = SpawnTableTool.ResolveLegacyId(this.farmAsset.growSpawnTableGuid, EAssetType.ITEM, new Func<string>(this.OnGetGrowSpawnTableErrorContext));
					}
					player.inventory.forceAddItem(new Item(num, EItemOrigin.NATURE), true);
					if (this.farmAsset.isAffectedByAgricultureSkill && Random.value < player.skills.mastery(2, 5))
					{
						player.inventory.forceAddItem(new Item(num, EItemOrigin.NATURE), true);
					}
					this.farmAsset.harvestRewardsList.Grant(player);
				}
				BarricadeManager.damage(base.transform, 2f, 1f, false, default(CSteamID), EDamageOrigin.Plant_Harvested);
				player.sendStat(EPlayerStat.FOUND_PLANTS);
				player.skills.askPay(this.harvestRewardExperience);
			}
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x00080A12 File Offset: 0x0007EC12
		private string OnGetGrowSpawnTableErrorContext()
		{
			string text = "Farmable ";
			ItemFarmAsset itemFarmAsset = this.farmAsset;
			return text + ((itemFarmAsset != null) ? itemFarmAsset.FriendlyName : null);
		}

		// Token: 0x04001059 RID: 4185
		private uint _planted;

		// Token: 0x0400105A RID: 4186
		private bool isGrown;

		// Token: 0x0400105B RID: 4187
		public uint harvestRewardExperience;

		// Token: 0x0400105C RID: 4188
		internal static readonly ClientInstanceMethod<uint> SendPlanted = ClientInstanceMethod<uint>.Get(typeof(InteractableFarm), "ReceivePlanted");

		// Token: 0x0400105D RID: 4189
		private static readonly ServerInstanceMethod SendHarvestRequest = ServerInstanceMethod.Get(typeof(InteractableFarm), "ReceiveHarvestRequest");

		// Token: 0x0400105E RID: 4190
		private ItemFarmAsset farmAsset;

		// Token: 0x02000946 RID: 2374
		// (Invoke) Token: 0x06004AD1 RID: 19153
		public delegate void HarvestRequestHandler(InteractableFarm harvestable, SteamPlayer instigatorPlayer, ref bool shouldAllow);
	}
}
