using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Compares weather intensity to value.
	/// </summary>
	// Token: 0x0200034D RID: 845
	public class NPCWeatherBlendAlphaCondition : NPCLogicCondition
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x0005AF36 File Offset: 0x00059136
		// (set) Token: 0x0600195A RID: 6490 RVA: 0x0005AF3E File Offset: 0x0005913E
		public AssetReference<WeatherAssetBase> weather { get; private set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x0005AF47 File Offset: 0x00059147
		// (set) Token: 0x0600195C RID: 6492 RVA: 0x0005AF4F File Offset: 0x0005914F
		public float value { get; private set; }

		// Token: 0x0600195D RID: 6493 RVA: 0x0005AF58 File Offset: 0x00059158
		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<float>(LevelLighting.GetWeatherGlobalBlendAlpha(this.weather.Find()), this.value);
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0005AF84 File Offset: 0x00059184
		public NPCWeatherBlendAlphaCondition(AssetReference<WeatherAssetBase> newWeather, float newValue, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.weather = newWeather;
			this.value = newValue;
		}
	}
}
