using System;

namespace SDG.Unturned
{
	// Token: 0x02000362 RID: 866
	public class NPCRewardsListAssetReward : INPCReward
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001A36 RID: 6710 RVA: 0x0005E36C File Offset: 0x0005C56C
		// (set) Token: 0x06001A37 RID: 6711 RVA: 0x0005E374 File Offset: 0x0005C574
		public AssetReference<Asset> assetRef { get; protected set; }

		// Token: 0x06001A38 RID: 6712 RVA: 0x0005E380 File Offset: 0x0005C580
		public override void GrantReward(Player player)
		{
			Asset asset = this.assetRef.Find();
			if (asset == null)
			{
				UnturnedLog.warn(string.Format("Rewards list asset reward unable to resolve guid ({0})", this.assetRef));
				return;
			}
			SpawnAsset spawnAsset = asset as SpawnAsset;
			if (spawnAsset != null)
			{
				asset = SpawnTableTool.Resolve(spawnAsset, new Func<string>(this.OnGetSpawnTableErrorContext));
				if (asset == null)
				{
					return;
				}
			}
			NPCRewardsAsset npcrewardsAsset = asset as NPCRewardsAsset;
			if (npcrewardsAsset != null)
			{
				if (npcrewardsAsset.AreConditionsMet(player))
				{
					npcrewardsAsset.ApplyConditions(player);
					npcrewardsAsset.GrantRewards(player);
					return;
				}
			}
			else
			{
				UnturnedLog.warn(string.Concat(new string[]
				{
					"Rewards list asset reward unable to grant \"",
					asset.FriendlyName,
					"\" because type is incompatible (",
					asset.GetTypeFriendlyName(),
					")"
				}));
			}
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x0005E437 File Offset: 0x0005C637
		public NPCRewardsListAssetReward(AssetReference<Asset> newAssetRef, string newText) : base(newText)
		{
			this.assetRef = newAssetRef;
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0005E447 File Offset: 0x0005C647
		private string OnGetSpawnTableErrorContext()
		{
			return "NPC rewards list asset reward";
		}
	}
}
