using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000350 RID: 848
	public class NPCZombieKillsCondition : INPCCondition
	{
		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001965 RID: 6501 RVA: 0x0005B0B4 File Offset: 0x000592B4
		// (set) Token: 0x06001966 RID: 6502 RVA: 0x0005B0BC File Offset: 0x000592BC
		public ushort id { get; protected set; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x0005B0C5 File Offset: 0x000592C5
		// (set) Token: 0x06001968 RID: 6504 RVA: 0x0005B0CD File Offset: 0x000592CD
		public short value { get; protected set; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001969 RID: 6505 RVA: 0x0005B0D6 File Offset: 0x000592D6
		// (set) Token: 0x0600196A RID: 6506 RVA: 0x0005B0DE File Offset: 0x000592DE
		public EZombieSpeciality zombie { get; protected set; }

		/// <summary>
		/// Should zombie(s) of the required type be spawned when player enters the area?
		/// </summary>
		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x0600196B RID: 6507 RVA: 0x0005B0E7 File Offset: 0x000592E7
		// (set) Token: 0x0600196C RID: 6508 RVA: 0x0005B0EF File Offset: 0x000592EF
		public bool spawn { get; protected set; }

		/// <summary>
		/// How many to spawn if spawning <see cref="P:SDG.Unturned.NPCZombieKillsCondition.spawn" /> is enabled.
		/// </summary>
		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x0005B0F8 File Offset: 0x000592F8
		// (set) Token: 0x0600196E RID: 6510 RVA: 0x0005B100 File Offset: 0x00059300
		public int spawnQuantity { get; protected set; }

		/// <summary>
		/// If greater than zero, find this zombie type configured in the level editor. For example, if the level editor
		/// lists "0 Fire (4)", then 4 is the unique ID, and if assigned to this condition a zombie from the "Fire"
		/// table will spawn.
		/// </summary>
		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x0005B109 File Offset: 0x00059309
		// (set) Token: 0x06001970 RID: 6512 RVA: 0x0005B111 File Offset: 0x00059311
		public int LevelTableUniqueId { get; private set; }

		/// <summary>
		/// Navmesh index player must be within. If set to byte.MaxValue then anywhere on the map is eligible.
		/// </summary>
		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x0005B11A File Offset: 0x0005931A
		// (set) Token: 0x06001972 RID: 6514 RVA: 0x0005B122 File Offset: 0x00059322
		public byte nav { get; protected set; }

		/// <summary>
		/// Only kills within this radius around the player are tracked.
		/// </summary>
		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x0005B12B File Offset: 0x0005932B
		// (set) Token: 0x06001974 RID: 6516 RVA: 0x0005B133 File Offset: 0x00059333
		public float sqrRadius { get; protected set; }

		/// <summary>
		/// If spawning is enabled, whether to use the timer between spawns.
		/// </summary>
		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001975 RID: 6517 RVA: 0x0005B13C File Offset: 0x0005933C
		// (set) Token: 0x06001976 RID: 6518 RVA: 0x0005B144 File Offset: 0x00059344
		public bool usesBossInterval { get; protected set; }

		// Token: 0x06001977 RID: 6519 RVA: 0x0005B150 File Offset: 0x00059350
		public override bool isConditionMet(Player player)
		{
			short num;
			return player.quests.getFlag(this.id, out num) && num >= this.value;
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x0005B180 File Offset: 0x00059380
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			player.quests.sendRemoveFlag(this.id);
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x0005B19C File Offset: 0x0005939C
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.format("Condition_ZombieKills");
			}
			short num;
			if (!player.quests.getFlag(this.id, out num))
			{
				num = 0;
			}
			return Local.FormatText(this.text, num, this.value);
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0005B1FE File Offset: 0x000593FE
		public override bool isAssociatedWithFlag(ushort flagID)
		{
			return flagID == this.id;
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x0005B209 File Offset: 0x00059409
		internal override void GatherAssociatedFlags(HashSet<ushort> associatedFlags)
		{
			associatedFlags.Add(this.id);
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x0005B218 File Offset: 0x00059418
		public NPCZombieKillsCondition(ushort newID, short newValue, EZombieSpeciality newZombie, bool newSpawn, int newSpawnQuantity, byte newNav, float newRadius, float newMinRadius, int newLevelTableUniqueId, string newText, bool newShouldReset) : base(newText, newShouldReset)
		{
			this.id = newID;
			this.value = newValue;
			this.zombie = newZombie;
			this.spawn = newSpawn;
			this.spawnQuantity = newSpawnQuantity;
			this.nav = newNav;
			this.sqrRadius = MathfEx.Square(newRadius);
			this.sqrMinRadius = MathfEx.Square(newMinRadius);
			this.LevelTableUniqueId = newLevelTableUniqueId;
			this.usesBossInterval = (this.spawnQuantity < 2);
		}

		/// <summary>
		/// Only kills outside this radius around the player are tracked.
		/// NSTM requested this for a sniping zombies quest.
		/// </summary>
		// Token: 0x04000B5C RID: 2908
		public float sqrMinRadius;
	}
}
