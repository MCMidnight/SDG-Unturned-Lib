using System;

namespace SDG.Unturned
{
	// Token: 0x02000340 RID: 832
	public class NPCRandomItemReward : INPCReward
	{
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x0005A247 File Offset: 0x00058447
		// (set) Token: 0x060018FA RID: 6394 RVA: 0x0005A24F File Offset: 0x0005844F
		public ushort id { get; protected set; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x0005A258 File Offset: 0x00058458
		// (set) Token: 0x060018FC RID: 6396 RVA: 0x0005A260 File Offset: 0x00058460
		public byte amount { get; protected set; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x0005A269 File Offset: 0x00058469
		// (set) Token: 0x060018FE RID: 6398 RVA: 0x0005A271 File Offset: 0x00058471
		public bool shouldAutoEquip { get; protected set; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x060018FF RID: 6399 RVA: 0x0005A27A File Offset: 0x0005847A
		// (set) Token: 0x06001900 RID: 6400 RVA: 0x0005A282 File Offset: 0x00058482
		public EItemOrigin origin { get; protected set; }

		// Token: 0x06001901 RID: 6401 RVA: 0x0005A28C File Offset: 0x0005848C
		public override void GrantReward(Player player)
		{
			for (byte b = 0; b < this.amount; b += 1)
			{
				ushort num = SpawnTableTool.ResolveLegacyId(this.id, EAssetType.ITEM, new Func<string>(this.OnGetSpawnTableErrorContext));
				if (num != 0)
				{
					player.inventory.forceAddItem(new Item(num, this.origin), this.shouldAutoEquip, false);
				}
			}
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0005A2E5 File Offset: 0x000584E5
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Item_Random");
			}
			return Local.FormatText(this.text, this.amount);
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0005A31F File Offset: 0x0005851F
		public NPCRandomItemReward(ushort newID, byte newAmount, bool newShouldAutoEquip, EItemOrigin origin, string newText) : base(newText)
		{
			this.id = newID;
			this.amount = newAmount;
			this.shouldAutoEquip = newShouldAutoEquip;
			this.origin = origin;
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x0005A346 File Offset: 0x00058546
		private string OnGetSpawnTableErrorContext()
		{
			return "NPC random item reward";
		}
	}
}
