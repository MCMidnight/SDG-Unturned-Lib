using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002CB RID: 715
	public class ItemArrestEndAsset : ItemAsset
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x0004D644 File Offset: 0x0004B844
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x0004D64C File Offset: 0x0004B84C
		public ushort recover
		{
			get
			{
				return this._recover;
			}
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0004D654 File Offset: 0x0004B854
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this._recover != 0)
			{
				ItemArrestStartAsset itemArrestStartAsset = Assets.find(EAssetType.ITEM, this._recover) as ItemArrestStartAsset;
				if (itemArrestStartAsset != null)
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ArrestEnd_UnlocksItem", string.Concat(new string[]
					{
						"<color=",
						Palette.hex(ItemTool.getRarityColorUI(itemArrestStartAsset.rarity)),
						">",
						itemArrestStartAsset.itemName,
						"</color>"
					})), 2000);
				}
			}
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0004D6EE File Offset: 0x0004B8EE
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = bundle.load<AudioClip>("Use");
			this._recover = data.ParseUInt16("Recover", 0);
		}

		// Token: 0x0400085E RID: 2142
		protected AudioClip _use;

		// Token: 0x0400085F RID: 2143
		protected ushort _recover;
	}
}
