using System;

namespace SDG.Unturned
{
	// Token: 0x0200034F RID: 847
	public class NPCWeatherStatusCondition : NPCLogicCondition
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x0005AF9E File Offset: 0x0005919E
		// (set) Token: 0x06001960 RID: 6496 RVA: 0x0005AFA6 File Offset: 0x000591A6
		public AssetReference<WeatherAssetBase> weather { get; private set; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001961 RID: 6497 RVA: 0x0005AFAF File Offset: 0x000591AF
		// (set) Token: 0x06001962 RID: 6498 RVA: 0x0005AFB7 File Offset: 0x000591B7
		public ENPCWeatherStatus value { get; private set; }

		// Token: 0x06001963 RID: 6499 RVA: 0x0005AFC0 File Offset: 0x000591C0
		public override bool isConditionMet(Player player)
		{
			switch (this.value)
			{
			case ENPCWeatherStatus.Active:
				return base.doesLogicPass<bool>(LevelLighting.IsWeatherActive(this.weather.Find()), true);
			case ENPCWeatherStatus.Transitioning_In:
				return base.doesLogicPass<bool>(LevelLighting.IsWeatherTransitioningIn(this.weather.Find()), true);
			case ENPCWeatherStatus.Fully_Transitioned_In:
				return base.doesLogicPass<bool>(LevelLighting.IsWeatherFullyTransitionedIn(this.weather.Find()), true);
			case ENPCWeatherStatus.Transitioning_Out:
				return base.doesLogicPass<bool>(LevelLighting.IsWeatherTransitioningOut(this.weather.Find()), true);
			case ENPCWeatherStatus.Fully_Transitioned_Out:
				return base.doesLogicPass<bool>(LevelLighting.IsWeatherFullyTransitionedOut(this.weather.Find()), true);
			case ENPCWeatherStatus.Transitioning:
				return base.doesLogicPass<bool>(LevelLighting.IsWeatherTransitioning(this.weather.Find()), true);
			default:
				return false;
			}
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0005B09A File Offset: 0x0005929A
		public NPCWeatherStatusCondition(AssetReference<WeatherAssetBase> newWeather, ENPCWeatherStatus newValue, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.weather = newWeather;
			this.value = newValue;
		}
	}
}
