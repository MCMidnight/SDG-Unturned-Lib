using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200037F RID: 895
	public class VendorAsset : Asset
	{
		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x000634DD File Offset: 0x000616DD
		// (set) Token: 0x06001BBF RID: 7103 RVA: 0x000634E5 File Offset: 0x000616E5
		public string vendorName { get; protected set; }

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x000634EE File Offset: 0x000616EE
		public override string FriendlyName
		{
			get
			{
				return this.vendorName;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x000634F6 File Offset: 0x000616F6
		// (set) Token: 0x06001BC2 RID: 7106 RVA: 0x000634FE File Offset: 0x000616FE
		public string vendorDescription { get; protected set; }

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x00063507 File Offset: 0x00061707
		// (set) Token: 0x06001BC4 RID: 7108 RVA: 0x0006350F File Offset: 0x0006170F
		public VendorBuying[] buying { get; protected set; }

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x00063518 File Offset: 0x00061718
		// (set) Token: 0x06001BC6 RID: 7110 RVA: 0x00063520 File Offset: 0x00061720
		public VendorSellingBase[] selling { get; protected set; }

		/// <summary>
		/// Should the buying and selling lists be alphabetically sorted?
		/// </summary>
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x00063529 File Offset: 0x00061729
		// (set) Token: 0x06001BC8 RID: 7112 RVA: 0x00063531 File Offset: 0x00061731
		public bool enableSorting { get; protected set; }

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x0006353A File Offset: 0x0006173A
		// (set) Token: 0x06001BCA RID: 7114 RVA: 0x00063542 File Offset: 0x00061742
		public AssetReference<ItemCurrencyAsset> currency { get; protected set; }

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x0006354B File Offset: 0x0006174B
		// (set) Token: 0x06001BCC RID: 7116 RVA: 0x00063553 File Offset: 0x00061753
		public byte? faceOverride { get; private set; }

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x0006355C File Offset: 0x0006175C
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.NPC;
			}
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x00063560 File Offset: 0x00061760
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (this.id < 2000 && !base.OriginAllowsVanillaLegacyId && !data.ContainsKey("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			this.vendorName = localization.format("Name");
			this.vendorName = ItemTool.filterRarityRichText(this.vendorName);
			string text = localization.format("Description");
			text = ItemTool.filterRarityRichText(text);
			RichTextUtil.replaceNewlineMarkup(ref text);
			this.vendorDescription = text;
			if (data.ContainsKey("FaceOverride"))
			{
				this.faceOverride = new byte?(data.ParseUInt8("FaceOverride", 0));
			}
			else
			{
				this.faceOverride = default(byte?);
			}
			this.buying = new VendorBuying[(int)data.ParseUInt8("Buying", 0)];
			byte b = 0;
			while ((int)b < this.buying.Length)
			{
				Guid newTargetAssetGuid;
				ushort newTargetAssetLegacyId;
				data.ParseGuidOrLegacyId("Buying_" + b.ToString() + "_ID", out newTargetAssetGuid, out newTargetAssetLegacyId);
				uint newCost = data.ParseUInt32("Buying_" + b.ToString() + "_Cost", 0U);
				INPCCondition[] array = new INPCCondition[(int)data.ParseUInt8("Buying_" + b.ToString() + "_Conditions", 0)];
				NPCTool.readConditions(data, localization, "Buying_" + b.ToString() + "_Condition_", array, this);
				NPCRewardsList newRewardsList = default(NPCRewardsList);
				newRewardsList.Parse(data, localization, this, "Buying_" + b.ToString() + "_Rewards", "Buying_" + b.ToString() + "_Reward_");
				this.buying[(int)b] = new VendorBuying(this, b, newTargetAssetGuid, newTargetAssetLegacyId, newCost, array, newRewardsList);
				b += 1;
			}
			this.selling = new VendorSellingBase[(int)data.ParseUInt8("Selling", 0)];
			byte b2 = 0;
			while ((int)b2 < this.selling.Length)
			{
				string text2 = null;
				if (data.ContainsKey("Selling_" + b2.ToString() + "_Type"))
				{
					text2 = data.GetString("Selling_" + b2.ToString() + "_Type", null);
				}
				Guid newTargetAssetGuid2;
				ushort newTargetAssetLegacyId2;
				data.ParseGuidOrLegacyId("Selling_" + b2.ToString() + "_ID", out newTargetAssetGuid2, out newTargetAssetLegacyId2);
				uint newCost2 = data.ParseUInt32("Selling_" + b2.ToString() + "_Cost", 0U);
				INPCCondition[] array2 = new INPCCondition[(int)data.ParseUInt8("Selling_" + b2.ToString() + "_Conditions", 0)];
				NPCTool.readConditions(data, localization, "Selling_" + b2.ToString() + "_Condition_", array2, this);
				NPCRewardsList newRewardsList2 = default(NPCRewardsList);
				newRewardsList2.Parse(data, localization, this, "Selling_" + b2.ToString() + "_Rewards", "Selling_" + b2.ToString() + "_Reward_");
				if (text2 == null || text2.Equals("Item", 3))
				{
					this.selling[(int)b2] = new VendorSellingItem(this, b2, newTargetAssetGuid2, newTargetAssetLegacyId2, newCost2, array2, newRewardsList2);
				}
				else
				{
					if (!text2.Equals("Vehicle", 3))
					{
						throw new NotSupportedException("unknown selling type: '" + text2 + "'");
					}
					string text3 = "Selling_" + b2.ToString() + "_Spawnpoint";
					string @string = data.GetString(text3, null);
					if (string.IsNullOrEmpty(@string))
					{
						Assets.reportError(this, "missing \"" + text3 + "\" for vehicle");
					}
					Color32? newPaintColor = default(Color32?);
					Color32 color;
					if (data.TryParseColor32RGB("Selling_" + b2.ToString() + "_PaintColor", out color))
					{
						newPaintColor = new Color32?(color);
					}
					this.selling[(int)b2] = new VendorSellingVehicle(this, b2, newTargetAssetGuid2, newTargetAssetLegacyId2, newCost2, @string, newPaintColor, array2, newRewardsList2);
				}
				b2 += 1;
			}
			this.enableSorting = !data.ContainsKey("Disable_Sorting");
			this.currency = data.readAssetReference("Currency");
		}
	}
}
