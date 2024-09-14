using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200032E RID: 814
	public class NPCItemCondition : NPCLogicCondition
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x0600186E RID: 6254 RVA: 0x00058F62 File Offset: 0x00057162
		// (set) Token: 0x0600186F RID: 6255 RVA: 0x00058F6A File Offset: 0x0005716A
		[Obsolete]
		public ushort id { get; protected set; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001870 RID: 6256 RVA: 0x00058F73 File Offset: 0x00057173
		// (set) Token: 0x06001871 RID: 6257 RVA: 0x00058F7B File Offset: 0x0005717B
		public ushort amount { get; protected set; }

		// Token: 0x06001872 RID: 6258 RVA: 0x00058F84 File Offset: 0x00057184
		public ItemAsset GetItemAsset()
		{
			return Assets.FindItemByGuidOrLegacyId<ItemAsset>(this.itemGuid, this.id);
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x00058F98 File Offset: 0x00057198
		public override bool isConditionMet(Player player)
		{
			ItemAsset itemAsset = this.GetItemAsset();
			if (itemAsset == null)
			{
				return false;
			}
			NPCItemCondition.search.Clear();
			player.inventory.search(NPCItemCondition.search, itemAsset.id, false, true);
			ushort num = 0;
			foreach (InventorySearch inventorySearch in NPCItemCondition.search)
			{
				num += (ushort)inventorySearch.jar.item.amount;
			}
			return base.doesLogicPass<ushort>(num, this.amount);
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x00059034 File Offset: 0x00057234
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			ItemAsset itemAsset = this.GetItemAsset();
			if (itemAsset == null)
			{
				return;
			}
			NPCItemCondition.applyConditionSearch.Clear();
			player.inventory.search(NPCItemCondition.applyConditionSearch, itemAsset.id, false, true);
			NPCItemCondition.applyConditionSearch.Sort(NPCItemCondition.qualityAscendingComparator);
			uint num = (uint)this.amount;
			foreach (InventorySearch inventorySearch in NPCItemCondition.applyConditionSearch)
			{
				uint num2 = inventorySearch.deleteAmount(player, num);
				num -= num2;
				if (num == 0U)
				{
					break;
				}
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x000590DC File Offset: 0x000572DC
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.format("Condition_Item");
			}
			ItemAsset itemAsset = this.GetItemAsset();
			if (itemAsset != null)
			{
				string arg = string.Concat(new string[]
				{
					"<color=",
					Palette.hex(ItemTool.getRarityColorUI(itemAsset.rarity)),
					">",
					itemAsset.itemName,
					"</color>"
				});
				NPCItemCondition.search.Clear();
				player.inventory.search(NPCItemCondition.search, itemAsset.id, false, true);
				int num = 0;
				foreach (InventorySearch inventorySearch in NPCItemCondition.search)
				{
					num += (int)inventorySearch.jar.item.amount;
				}
				return Local.FormatText(this.text, num, this.amount, arg);
			}
			return Local.FormatText(this.text, 0, this.amount, "?");
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x00059214 File Offset: 0x00057414
		public override ISleekElement createUI(Player player, Texture2D icon)
		{
			string text = this.formatCondition(player);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			ItemAsset itemAsset = this.GetItemAsset();
			if (itemAsset == null)
			{
				return null;
			}
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			if (itemAsset.size_y == 1)
			{
				sleekBox.SizeOffset_Y = (float)(itemAsset.size_y * 50 + 10);
			}
			else
			{
				sleekBox.SizeOffset_Y = (float)(itemAsset.size_y * 25 + 10);
			}
			sleekBox.SizeScale_X = 1f;
			if (icon != null)
			{
				ISleekImage sleekImage = Glazier.Get().CreateImage(icon);
				sleekImage.PositionOffset_X = 5f;
				sleekImage.PositionOffset_Y = -10f;
				sleekImage.PositionScale_Y = 0.5f;
				sleekImage.SizeOffset_X = 20f;
				sleekImage.SizeOffset_Y = 20f;
				sleekBox.AddChild(sleekImage);
			}
			SleekItemIcon sleekItemIcon = new SleekItemIcon();
			if (icon != null)
			{
				sleekItemIcon.PositionOffset_X = 30f;
			}
			else
			{
				sleekItemIcon.PositionOffset_X = 5f;
			}
			sleekItemIcon.PositionOffset_Y = 5f;
			if (itemAsset.size_y == 1)
			{
				sleekItemIcon.SizeOffset_X = (float)(itemAsset.size_x * 50);
				sleekItemIcon.SizeOffset_Y = (float)(itemAsset.size_y * 50);
			}
			else
			{
				sleekItemIcon.SizeOffset_X = (float)(itemAsset.size_x * 25);
				sleekItemIcon.SizeOffset_Y = (float)(itemAsset.size_y * 25);
			}
			sleekBox.AddChild(sleekItemIcon);
			sleekItemIcon.Refresh(itemAsset.id, 100, itemAsset.getState(false), itemAsset, Mathf.RoundToInt(sleekItemIcon.SizeOffset_X), Mathf.RoundToInt(sleekItemIcon.SizeOffset_Y));
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			if (icon != null)
			{
				sleekLabel.PositionOffset_X = 35f + sleekItemIcon.SizeOffset_X;
				sleekLabel.SizeOffset_X = -40f - sleekItemIcon.SizeOffset_X;
			}
			else
			{
				sleekLabel.PositionOffset_X = 10f + sleekItemIcon.SizeOffset_X;
				sleekLabel.SizeOffset_X = -15f - sleekItemIcon.SizeOffset_X;
			}
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.SizeScale_Y = 1f;
			sleekLabel.TextAlignment = 3;
			sleekLabel.TextColor = 4;
			sleekLabel.TextContrastContext = 1;
			sleekLabel.AllowRichText = true;
			sleekLabel.Text = text;
			sleekBox.AddChild(sleekLabel);
			return sleekBox;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00059441 File Offset: 0x00057641
		public NPCItemCondition(Guid newItemGuid, ushort newID, ushort newAmount, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.itemGuid = newItemGuid;
			this.id = newID;
			this.amount = newAmount;
		}

		// Token: 0x04000B07 RID: 2823
		private static InventorySearchQualityAscendingComparator qualityAscendingComparator = new InventorySearchQualityAscendingComparator();

		/// <summary>
		/// isConditionMet can get called during applyCondition because item consume refreshes the UI.
		/// </summary>
		// Token: 0x04000B08 RID: 2824
		private static List<InventorySearch> search = new List<InventorySearch>();

		// Token: 0x04000B09 RID: 2825
		private static List<InventorySearch> applyConditionSearch = new List<InventorySearch>();

		// Token: 0x04000B0A RID: 2826
		public Guid itemGuid;
	}
}
