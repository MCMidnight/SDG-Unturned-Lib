using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200032F RID: 815
	public class NPCItemReward : INPCReward
	{
		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001879 RID: 6265 RVA: 0x00059484 File Offset: 0x00057684
		// (set) Token: 0x0600187A RID: 6266 RVA: 0x0005948C File Offset: 0x0005768C
		[Obsolete]
		public ushort id { get; protected set; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x0600187B RID: 6267 RVA: 0x00059495 File Offset: 0x00057695
		// (set) Token: 0x0600187C RID: 6268 RVA: 0x0005949D File Offset: 0x0005769D
		public byte amount { get; protected set; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x000594A6 File Offset: 0x000576A6
		// (set) Token: 0x0600187E RID: 6270 RVA: 0x000594AE File Offset: 0x000576AE
		public bool shouldAutoEquip { get; protected set; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x000594B7 File Offset: 0x000576B7
		// (set) Token: 0x06001880 RID: 6272 RVA: 0x000594BF File Offset: 0x000576BF
		public int sight { get; protected set; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001881 RID: 6273 RVA: 0x000594C8 File Offset: 0x000576C8
		// (set) Token: 0x06001882 RID: 6274 RVA: 0x000594D0 File Offset: 0x000576D0
		public int tactical { get; protected set; }

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001883 RID: 6275 RVA: 0x000594D9 File Offset: 0x000576D9
		// (set) Token: 0x06001884 RID: 6276 RVA: 0x000594E1 File Offset: 0x000576E1
		public int grip { get; protected set; }

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x000594EA File Offset: 0x000576EA
		// (set) Token: 0x06001886 RID: 6278 RVA: 0x000594F2 File Offset: 0x000576F2
		public int barrel { get; protected set; }

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x000594FB File Offset: 0x000576FB
		// (set) Token: 0x06001888 RID: 6280 RVA: 0x00059503 File Offset: 0x00057703
		public int magazine { get; protected set; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x0005950C File Offset: 0x0005770C
		// (set) Token: 0x0600188A RID: 6282 RVA: 0x00059514 File Offset: 0x00057714
		public int ammo { get; protected set; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x0005951D File Offset: 0x0005771D
		// (set) Token: 0x0600188C RID: 6284 RVA: 0x00059525 File Offset: 0x00057725
		public EItemOrigin origin { get; protected set; }

		// Token: 0x0600188D RID: 6285 RVA: 0x0005952E File Offset: 0x0005772E
		public ItemAsset GetItemAsset()
		{
			return Assets.FindItemByGuidOrLegacyId<ItemAsset>(this.itemGuid, this.id);
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x00059544 File Offset: 0x00057744
		public override void GrantReward(Player player)
		{
			ItemAsset itemAsset = this.GetItemAsset();
			if (itemAsset == null)
			{
				return;
			}
			for (byte b = 0; b < this.amount; b += 1)
			{
				Item item;
				if (this.sight > -1 || this.tactical > -1 || this.grip > -1 || this.barrel > -1 || this.magazine > -1 || this.ammo > -1)
				{
					ItemGunAsset itemGunAsset = itemAsset as ItemGunAsset;
					if (itemGunAsset != null)
					{
						ushort sight = (this.sight > -1) ? MathfEx.ClampToUShort(this.sight) : itemGunAsset.sightID;
						ushort tactical = (this.tactical > -1) ? MathfEx.ClampToUShort(this.tactical) : itemGunAsset.tacticalID;
						ushort grip = (this.grip > -1) ? MathfEx.ClampToUShort(this.grip) : itemGunAsset.gripID;
						ushort barrel = (this.barrel > -1) ? MathfEx.ClampToUShort(this.barrel) : itemGunAsset.barrelID;
						ushort magazine = (this.magazine > -1) ? MathfEx.ClampToUShort(this.magazine) : itemGunAsset.getMagazineID();
						byte ammo = (this.ammo > -1) ? MathfEx.ClampToByte(this.ammo) : itemGunAsset.ammoMax;
						byte[] state = itemGunAsset.getState(sight, tactical, grip, barrel, magazine, ammo);
						item = new Item(itemAsset.id, 1, 100, state);
					}
					else
					{
						item = new Item(itemAsset.id, this.origin);
					}
				}
				else
				{
					item = new Item(itemAsset.id, this.origin);
				}
				player.inventory.forceAddItem(item, this.shouldAutoEquip, false);
			}
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x000596D0 File Offset: 0x000578D0
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Item");
			}
			ItemAsset itemAsset = this.GetItemAsset();
			string arg;
			if (itemAsset != null)
			{
				arg = string.Concat(new string[]
				{
					"<color=",
					Palette.hex(ItemTool.getRarityColorUI(itemAsset.rarity)),
					">",
					itemAsset.itemName,
					"</color>"
				});
			}
			else
			{
				arg = "?";
			}
			return Local.FormatText(this.text, this.amount, arg);
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00059770 File Offset: 0x00057970
		public override ISleekElement createUI(Player player)
		{
			string text = this.formatReward(player);
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
			SleekItemIcon sleekItemIcon = new SleekItemIcon();
			sleekItemIcon.PositionOffset_X = 5f;
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
			sleekLabel.PositionOffset_X = 10f + sleekItemIcon.SizeOffset_X;
			sleekLabel.SizeOffset_X = -15f - sleekItemIcon.SizeOffset_X;
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

		// Token: 0x06001891 RID: 6289 RVA: 0x000598FC File Offset: 0x00057AFC
		public NPCItemReward(Guid newItemGuid, ushort newID, byte newAmount, bool newShouldAutoEquip, int newSight, int newTactical, int newGrip, int newBarrel, int newMagazine, int newAmmo, EItemOrigin origin, string newText) : base(newText)
		{
			this.itemGuid = newItemGuid;
			this.id = newID;
			this.amount = newAmount;
			this.shouldAutoEquip = newShouldAutoEquip;
			this.sight = newSight;
			this.tactical = newTactical;
			this.grip = newGrip;
			this.barrel = newBarrel;
			this.magazine = newMagazine;
			this.ammo = newAmmo;
			this.origin = origin;
		}

		// Token: 0x04000B0D RID: 2829
		public Guid itemGuid;
	}
}
