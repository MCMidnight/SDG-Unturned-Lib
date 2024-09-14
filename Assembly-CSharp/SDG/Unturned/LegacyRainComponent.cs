using System;

namespace SDG.Unturned
{
	// Token: 0x020004D0 RID: 1232
	public class LegacyRainComponent : CustomWeatherComponent
	{
		// Token: 0x060025A3 RID: 9635 RVA: 0x00095D98 File Offset: 0x00093F98
		public override void OnBeginTransitionIn()
		{
			this.SetRain(ELightingRain.PRE_DRIZZLE);
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x00095DA1 File Offset: 0x00093FA1
		public override void OnEndTransitionIn()
		{
			this.SetRain(ELightingRain.DRIZZLE);
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x00095DAA File Offset: 0x00093FAA
		public override void OnBeginTransitionOut()
		{
			this.SetRain(ELightingRain.POST_DRIZZLE);
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x00095DB3 File Offset: 0x00093FB3
		public override void OnEndTransitionOut()
		{
			this.SetRain(ELightingRain.NONE);
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x00095DBC File Offset: 0x00093FBC
		private void SetRain(ELightingRain rain)
		{
			LevelLighting.rainyness = rain;
			LightingManager.broadcastRainUpdated(rain);
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x00095DCA File Offset: 0x00093FCA
		private void OnEnable()
		{
			this.puddleWaterLevel = 0.75f;
			this.puddleIntensity = 2f;
		}
	}
}
