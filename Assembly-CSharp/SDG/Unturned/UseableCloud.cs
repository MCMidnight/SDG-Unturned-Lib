using System;

namespace SDG.Unturned
{
	// Token: 0x020007DD RID: 2013
	public class UseableCloud : Useable
	{
		// Token: 0x06004473 RID: 17523 RVA: 0x0018C82A File Offset: 0x0018AA2A
		public override void equip()
		{
			base.player.animator.play("Equip", true);
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x0018C842 File Offset: 0x0018AA42
		public override void dequip()
		{
			base.player.movement.itemGravityMultiplier = 1f;
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x0018C859 File Offset: 0x0018AA59
		public override void tick()
		{
			if (!base.player.equipment.IsEquipAnimationFinished)
			{
				return;
			}
			base.player.movement.itemGravityMultiplier = ((ItemCloudAsset)base.player.equipment.asset).gravity;
		}
	}
}
