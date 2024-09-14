using System;

namespace SDG.Unturned
{
	// Token: 0x020007F1 RID: 2033
	public class UseableWalkieTalkie : Useable
	{
		// Token: 0x060045F7 RID: 17911 RVA: 0x001A232B File Offset: 0x001A052B
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			base.player.voice.hasUseableWalkieTalkie = true;
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x001A2354 File Offset: 0x001A0554
		public override void dequip()
		{
			base.player.voice.hasUseableWalkieTalkie = false;
		}
	}
}
