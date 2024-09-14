using System;

namespace SDG.Unturned
{
	// Token: 0x02000499 RID: 1177
	public class Item
	{
		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x00091E1F File Offset: 0x0009001F
		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x00091E27 File Offset: 0x00090027
		// (set) Token: 0x06002493 RID: 9363 RVA: 0x00091E2F File Offset: 0x0009002F
		public byte durability
		{
			get
			{
				return this.quality;
			}
			set
			{
				this.quality = value;
			}
		}

		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002494 RID: 9364 RVA: 0x00091E38 File Offset: 0x00090038
		// (set) Token: 0x06002495 RID: 9365 RVA: 0x00091E40 File Offset: 0x00090040
		public byte[] metadata
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x00091E49 File Offset: 0x00090049
		public ItemAsset GetAsset()
		{
			return Assets.find(EAssetType.ITEM, this.id) as ItemAsset;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x00091E5C File Offset: 0x0009005C
		public T GetAsset<T>() where T : ItemAsset
		{
			return Assets.find(EAssetType.ITEM, this.id) as T;
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x00091E74 File Offset: 0x00090074
		public Item(ushort newID, bool full) : this(newID, full ? EItemOrigin.ADMIN : EItemOrigin.WORLD)
		{
		}

		/// <summary>
		/// Ideally in a future rewrite asset overload will become the default rather than the overload taking legacy ID.
		/// </summary>
		// Token: 0x06002499 RID: 9369 RVA: 0x00091E84 File Offset: 0x00090084
		public Item(ItemAsset asset, EItemOrigin origin) : this((asset != null) ? asset.id : 0, origin)
		{
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x00091E9C File Offset: 0x0009009C
		public Item(ushort newID, EItemOrigin origin)
		{
			this._id = newID;
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, this.id) as ItemAsset;
			if (itemAsset == null)
			{
				this.state = new byte[0];
				return;
			}
			if (origin != EItemOrigin.WORLD)
			{
				this.amount = MathfEx.Max(itemAsset.amount, 1);
			}
			else
			{
				this.amount = MathfEx.Max(itemAsset.count, 1);
			}
			if (origin != EItemOrigin.WORLD || Item.ShouldItemTypeSpawnAtFullQuality(itemAsset.type))
			{
				this.quality = 100;
			}
			else
			{
				this.quality = MathfEx.Clamp(itemAsset.quality, 0, 100);
			}
			this.state = itemAsset.getState(origin);
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x00091F3C File Offset: 0x0009013C
		public Item(ushort newID, bool full, byte newQuality) : this(newID, full ? EItemOrigin.ADMIN : EItemOrigin.WORLD, newQuality)
		{
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x00091F50 File Offset: 0x00090150
		public Item(ushort newID, EItemOrigin origin, byte newQuality)
		{
			this._id = newID;
			this.quality = newQuality;
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, this.id) as ItemAsset;
			if (itemAsset == null)
			{
				this.state = new byte[0];
				return;
			}
			if (origin != EItemOrigin.WORLD)
			{
				this.amount = MathfEx.Max(itemAsset.amount, 1);
			}
			else
			{
				this.amount = MathfEx.Max(itemAsset.count, 1);
			}
			this.state = itemAsset.getState(origin);
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x00091FCC File Offset: 0x000901CC
		public Item(ushort newID, byte newAmount, byte newQuality)
		{
			this._id = newID;
			this.amount = newAmount;
			this.quality = newQuality;
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, this.id) as ItemAsset;
			if (itemAsset == null)
			{
				this.state = new byte[0];
				return;
			}
			this.state = itemAsset.getState();
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x00092022 File Offset: 0x00090222
		public Item(ushort newID, byte newAmount, byte newQuality, byte[] newState)
		{
			this._id = newID;
			this.amount = newAmount;
			this.quality = newQuality;
			this.state = ((newState != null) ? newState : new byte[0]);
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x00092054 File Offset: 0x00090254
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.id.ToString(),
				" ",
				this.amount.ToString(),
				" ",
				this.quality.ToString(),
				" ",
				this.state.Length.ToString()
			});
		}

		/// <summary>
		/// If true, item has 100% quality. If false, item has a random quality.
		/// </summary>
		// Token: 0x060024A0 RID: 9376 RVA: 0x000920C4 File Offset: 0x000902C4
		private static bool ShouldItemTypeSpawnAtFullQuality(EItemType type)
		{
			if (!Provider.modeConfigData.Items.Has_Durability)
			{
				return true;
			}
			switch (type)
			{
			case EItemType.HAT:
			case EItemType.PANTS:
			case EItemType.SHIRT:
			case EItemType.MASK:
			case EItemType.BACKPACK:
			case EItemType.VEST:
			case EItemType.GLASSES:
				return Provider.modeConfigData.Items.Clothing_Spawns_At_Full_Quality;
			case EItemType.GUN:
			case EItemType.MELEE:
				return Provider.modeConfigData.Items.Weapons_Spawn_At_Full_Quality;
			case EItemType.FOOD:
				return Provider.modeConfigData.Items.Food_Spawns_At_Full_Quality;
			case EItemType.WATER:
				return Provider.modeConfigData.Items.Water_Spawns_At_Full_Quality;
			}
			return Provider.modeConfigData.Items.Default_Spawns_At_Full_Quality;
		}

		// Token: 0x040012A8 RID: 4776
		private ushort _id;

		// Token: 0x040012A9 RID: 4777
		public byte amount;

		// Token: 0x040012AA RID: 4778
		public byte quality;

		// Token: 0x040012AB RID: 4779
		public byte[] state;
	}
}
