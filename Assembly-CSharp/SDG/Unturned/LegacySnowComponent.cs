using System;

namespace SDG.Unturned
{
	// Token: 0x020004D1 RID: 1233
	public class LegacySnowComponent : CustomWeatherComponent
	{
		// Token: 0x060025AA RID: 9642 RVA: 0x00095DEA File Offset: 0x00093FEA
		public override void OnBeginTransitionIn()
		{
			this.SetSnow(ELightingSnow.PRE_BLIZZARD);
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x00095DF3 File Offset: 0x00093FF3
		public override void OnEndTransitionIn()
		{
			this.SetSnow(ELightingSnow.BLIZZARD);
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x00095DFC File Offset: 0x00093FFC
		public override void OnBeginTransitionOut()
		{
			this.SetSnow(ELightingSnow.POST_BLIZZARD);
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x00095E05 File Offset: 0x00094005
		public override void OnEndTransitionOut()
		{
			this.SetSnow(ELightingSnow.NONE);
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x00095E0E File Offset: 0x0009400E
		private void SetSnow(ELightingSnow snow)
		{
			LevelLighting.snowyness = snow;
			LightingManager.broadcastSnowUpdated(snow);
		}
	}
}
