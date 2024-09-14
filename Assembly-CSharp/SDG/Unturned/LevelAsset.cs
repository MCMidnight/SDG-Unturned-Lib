using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000311 RID: 785
	public class LevelAsset : Asset
	{
		/// <summary>
		/// Audio clip to play in 2D when a player dies.
		/// </summary>
		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x00056FDC File Offset: 0x000551DC
		// (set) Token: 0x060017B2 RID: 6066 RVA: 0x00056FE4 File Offset: 0x000551E4
		public MasterBundleReference<AudioClip> DeathMusicRef { get; private set; }

		// Token: 0x060017B3 RID: 6067 RVA: 0x00056FF0 File Offset: 0x000551F0
		public bool isBlueprintBlacklisted(Blueprint blueprint)
		{
			if (this.craftingBlacklists == null || blueprint == null)
			{
				return false;
			}
			if (this.resolvedCraftingBlacklists == null)
			{
				this.resolvedCraftingBlacklists = new List<CraftingBlacklistAsset>(this.craftingBlacklists.Count);
				foreach (AssetReference<CraftingBlacklistAsset> assetReference in this.craftingBlacklists)
				{
					CraftingBlacklistAsset craftingBlacklistAsset = assetReference.Find();
					if (craftingBlacklistAsset != null)
					{
						this.resolvedCraftingBlacklists.Add(craftingBlacklistAsset);
					}
					else
					{
						Assets.reportError(this, string.Format("unable to find crafting blacklist {0}", assetReference));
					}
				}
			}
			using (List<CraftingBlacklistAsset>.Enumerator enumerator2 = this.resolvedCraftingBlacklists.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.isBlueprintBlacklisted(blueprint))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x000570E4 File Offset: 0x000552E4
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.defaultGameMode = data.ParseStruct<TypeReference<GameMode>>("Default_Game_Mode", default(TypeReference<GameMode>));
			DatList datList;
			if (data.TryGetList("Supported_Game_Modes", out datList))
			{
				this.supportedGameModes = datList.ParseListOfStructs<TypeReference<GameMode>>();
			}
			this.dropshipPrefab = data.ParseStruct<MasterBundleReference<GameObject>>("Dropship", default(MasterBundleReference<GameObject>));
			this.airdropRef = data.ParseStruct<AssetReference<AirdropAsset>>("Airdrop", default(AssetReference<AirdropAsset>));
			DatList datList2;
			if (data.TryGetList("Crafting_Blacklists", out datList2) && datList2.Count > 0)
			{
				this.craftingBlacklists = datList2.ParseListOfStructs<AssetReference<CraftingBlacklistAsset>>();
			}
			DatList datList3;
			if (data.TryGetList("Weather_Types", out datList3))
			{
				List<LevelAsset.SchedulableWeather> list = new List<LevelAsset.SchedulableWeather>(datList3.Count);
				for (int i = 0; i < datList3.Count; i++)
				{
					DatDictionary datDictionary = datList3[i] as DatDictionary;
					if (datDictionary != null)
					{
						LevelAsset.SchedulableWeather schedulableWeather = default(LevelAsset.SchedulableWeather);
						schedulableWeather.assetRef = datDictionary.ParseStruct<AssetReference<WeatherAssetBase>>("Asset", default(AssetReference<WeatherAssetBase>));
						schedulableWeather.minFrequency = Mathf.Max(0f, datDictionary.ParseFloat("Min_Frequency", 0f));
						schedulableWeather.maxFrequency = Mathf.Max(0f, datDictionary.ParseFloat("Max_Frequency", 0f));
						schedulableWeather.minDuration = Mathf.Max(0f, datDictionary.ParseFloat("Min_Duration", 0f));
						schedulableWeather.maxDuration = Mathf.Max(0f, datDictionary.ParseFloat("Max_Duration", 0f));
						if (Mathf.Max(schedulableWeather.minDuration, schedulableWeather.maxDuration) > 0.001f)
						{
							list.Add(schedulableWeather);
						}
						else
						{
							UnturnedLog.warn("Disabling level {0} weather {1} because max duration is zero", new object[]
							{
								this,
								schedulableWeather.assetRef
							});
						}
					}
				}
				if (list.Count > 0)
				{
					this.schedulableWeathers = list.ToArray();
				}
			}
			this.perpetualWeatherRef = data.ParseStruct<AssetReference<WeatherAssetBase>>("Perpetual_Weather_Asset", default(AssetReference<WeatherAssetBase>));
			DatList datList4;
			if (data.TryGetList("Loading_Screen_Music", out datList4))
			{
				this.loadingScreenMusic = new LevelAsset.LoadingScreenMusic[datList4.Count];
				for (int j = 0; j < datList4.Count; j++)
				{
					DatDictionary datDictionary2 = datList4[j] as DatDictionary;
					if (datDictionary2 != null)
					{
						LevelAsset.LoadingScreenMusic loadingScreenMusic = default(LevelAsset.LoadingScreenMusic);
						loadingScreenMusic.loopRef = datDictionary2.ParseStruct<MasterBundleReference<AudioClip>>("Loop", default(MasterBundleReference<AudioClip>));
						loadingScreenMusic.outroRef = datDictionary2.ParseStruct<MasterBundleReference<AudioClip>>("Outro", default(MasterBundleReference<AudioClip>));
						if (datDictionary2.ContainsKey("Loop_Volume"))
						{
							loadingScreenMusic.loopVolume = datDictionary2.ParseFloat("Loop_Volume", 0f);
						}
						else
						{
							loadingScreenMusic.loopVolume = 1f;
						}
						if (datDictionary2.ContainsKey("Outro_Volume"))
						{
							loadingScreenMusic.outroVolume = datDictionary2.ParseFloat("Outro_Volume", 0f);
						}
						else
						{
							loadingScreenMusic.outroVolume = 1f;
						}
						this.loadingScreenMusic[j] = loadingScreenMusic;
					}
				}
			}
			MasterBundleReference<AudioClip> deathMusicRef;
			if (data.TryParseStruct<MasterBundleReference<AudioClip>>("Death_Music", out deathMusicRef))
			{
				this.DeathMusicRef = deathMusicRef;
			}
			else
			{
				this.DeathMusicRef = LevelAsset.DefaultDeathMusicRef;
			}
			this.shouldAnimateBackgroundImage = data.ParseBool("Should_Animate_Background_Image", false);
			if (data.ContainsKey("Global_Weather_Mask"))
			{
				this.globalWeatherMask = data.ParseUInt32("Global_Weather_Mask", 0U);
			}
			else
			{
				this.globalWeatherMask = uint.MaxValue;
			}
			DatList datList5;
			if (data.TryGetList("Skills", out datList5))
			{
				this.skillRules = new LevelAsset.SkillRule[(int)PlayerSkills.SPECIALITIES][];
				this.skillRules[0] = new LevelAsset.SkillRule[7];
				this.skillRules[1] = new LevelAsset.SkillRule[7];
				this.skillRules[2] = new LevelAsset.SkillRule[8];
				for (int k = 0; k < datList5.Count; k++)
				{
					DatDictionary datDictionary3 = datList5[k] as DatDictionary;
					if (datDictionary3 != null)
					{
						string @string = datDictionary3.GetString("Id", null);
						int num;
						int num2;
						if (!PlayerSkills.TryParseIndices(@string, out num, out num2))
						{
							UnturnedLog.warn("Level {0} unable to parse skill index {1} ({2})", new object[]
							{
								this,
								k,
								@string
							});
						}
						else
						{
							LevelAsset.SkillRule skillRule = new LevelAsset.SkillRule();
							skillRule.defaultLevel = datDictionary3.ParseInt32("Default_Level", 0);
							if (datDictionary3.ContainsKey("Max_Unlockable_Level"))
							{
								skillRule.maxUnlockableLevel = datDictionary3.ParseInt32("Max_Unlockable_Level", 0);
							}
							else
							{
								skillRule.maxUnlockableLevel = -1;
							}
							if (datDictionary3.ContainsKey("Cost_Multiplier"))
							{
								skillRule.costMultiplier = datDictionary3.ParseFloat("Cost_Multiplier", 0f);
							}
							else
							{
								skillRule.costMultiplier = 1f;
							}
							this.skillRules[num][num2] = skillRule;
						}
					}
				}
			}
			this.minStealthRadius = data.ParseFloat("Min_Stealth_Radius", 0f);
			this.fallDamageSpeedThreshold = data.ParseFloat("Fall_Damage_Speed_Threshold", 0f);
			if (data.ContainsKey("Enable_Admin_Faster_Salvage_Duration"))
			{
				this.enableAdminFasterSalvageDuration = data.ParseBool("Enable_Admin_Faster_Salvage_Duration", false);
			}
			if (data.ContainsKey("Has_Clouds"))
			{
				this.hasClouds = data.ParseBool("Has_Clouds", false);
			}
			else
			{
				this.hasClouds = true;
			}
			DatList datList6;
			if (data.TryGetList("TerrainColors", out datList6))
			{
				List<LevelAsset.TerrainColorRule> list2 = new List<LevelAsset.TerrainColorRule>(datList6.Count);
				foreach (IDatNode datNode in datList6)
				{
					LevelAsset.TerrainColorRule terrainColorRule = new LevelAsset.TerrainColorRule();
					if (terrainColorRule.TryParse(datNode))
					{
						bool flag = false;
						foreach (Color color in Customization.SKINS)
						{
							float inputHue;
							float inputSaturation;
							float inputValue;
							Color.RGBToHSV(color, out inputHue, out inputSaturation, out inputValue);
							if (terrainColorRule.CompareColors(inputHue, inputSaturation, inputValue) == LevelAsset.TerrainColorRule.EComparisonResult.TooSimilar)
							{
								flag = true;
								string text = Palette.hex(color);
								Assets.reportError("skipping TerrainColor entry because it blocks default skin color " + text);
								break;
							}
						}
						if (!flag)
						{
							list2.Add(terrainColorRule);
						}
					}
					else
					{
						Assets.reportError(this, "unable to parse entry in TerrainColors: " + datNode.DebugDumpToString());
					}
				}
				if (list2.Count > 0)
				{
					this.terrainColorRules = list2;
					return;
				}
				Assets.reportError(this, "TerrainColors list is empty");
			}
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x0005774C File Offset: 0x0005594C
		public LevelAsset()
		{
			this.supportedGameModes = new List<TypeReference<GameMode>>();
		}

		// Token: 0x04000AA7 RID: 2727
		public static AssetReference<LevelAsset> defaultLevel = new AssetReference<LevelAsset>(new Guid("12dc9fdbe9974022afd21158ad54b76a"));

		// Token: 0x04000AA8 RID: 2728
		internal static MasterBundleReference<AudioClip> DefaultDeathMusicRef = new MasterBundleReference<AudioClip>("core.masterbundle", "Music/Death.mp3");

		// Token: 0x04000AA9 RID: 2729
		public TypeReference<GameMode> defaultGameMode;

		// Token: 0x04000AAA RID: 2730
		public List<TypeReference<GameMode>> supportedGameModes;

		// Token: 0x04000AAB RID: 2731
		public MasterBundleReference<GameObject> dropshipPrefab;

		// Token: 0x04000AAC RID: 2732
		public AssetReference<AirdropAsset> airdropRef;

		/// <summary>
		/// Player stealth radius cannot go below this value.
		/// </summary>
		// Token: 0x04000AAD RID: 2733
		public float minStealthRadius;

		/// <summary>
		/// Deal damage and break legs if speed is greater than this value.
		/// </summary>
		// Token: 0x04000AAE RID: 2734
		public float fallDamageSpeedThreshold;

		/// <summary>
		/// By default players in singleplayer and admins in multiplayer have a faster salvage time.
		/// This option was requested for maps with entirely custom balanced salvage times.
		/// </summary>
		// Token: 0x04000AAF RID: 2735
		public bool enableAdminFasterSalvageDuration = true;

		// Token: 0x04000AB0 RID: 2736
		public List<AssetReference<CraftingBlacklistAsset>> craftingBlacklists;

		/// <summary>
		/// Cached result of finding all craftingBlacklists.
		/// </summary>
		// Token: 0x04000AB1 RID: 2737
		private List<CraftingBlacklistAsset> resolvedCraftingBlacklists;

		/// <summary>
		/// Determines which weather can naturally occur in this level.
		/// Null if empty.
		/// </summary>
		// Token: 0x04000AB2 RID: 2738
		public LevelAsset.SchedulableWeather[] schedulableWeathers;

		/// <summary>
		/// If set, this weather will always be active and scheduled weather is disabled.
		/// </summary>
		// Token: 0x04000AB3 RID: 2739
		public AssetReference<WeatherAssetBase> perpetualWeatherRef;

		// Token: 0x04000AB4 RID: 2740
		public LevelAsset.LoadingScreenMusic[] loadingScreenMusic;

		/// <summary>
		/// Defaults to false because some servers have rules and info on the loading screen.
		/// </summary>
		// Token: 0x04000AB6 RID: 2742
		public bool shouldAnimateBackgroundImage;

		/// <summary>
		/// Volume weather mask used while not inside an ambience volume.
		/// </summary>
		// Token: 0x04000AB7 RID: 2743
		public uint globalWeatherMask;

		/// <summary>
		/// Allows level to override skill max levels.
		/// Null if empty, otherwise matches 1:1 with PlayerSkills._skills.
		/// </summary>
		// Token: 0x04000AB8 RID: 2744
		public LevelAsset.SkillRule[][] skillRules;

		/// <summary>
		/// If false, clouds are removed from the skybox.
		/// </summary>
		// Token: 0x04000AB9 RID: 2745
		public bool hasClouds = true;

		/// <summary>
		/// Players are kicked from multiplayer if their skin color is within threshold of any of these rules.
		/// </summary>
		// Token: 0x04000ABA RID: 2746
		internal List<LevelAsset.TerrainColorRule> terrainColorRules;

		// Token: 0x02000921 RID: 2337
		public struct SchedulableWeather
		{
			// Token: 0x04003266 RID: 12902
			public AssetReference<WeatherAssetBase> assetRef;

			// Token: 0x04003267 RID: 12903
			public float minFrequency;

			// Token: 0x04003268 RID: 12904
			public float maxFrequency;

			// Token: 0x04003269 RID: 12905
			public float minDuration;

			// Token: 0x0400326A RID: 12906
			public float maxDuration;
		}

		// Token: 0x02000922 RID: 2338
		public struct LoadingScreenMusic
		{
			// Token: 0x0400326B RID: 12907
			public MasterBundleReference<AudioClip> loopRef;

			// Token: 0x0400326C RID: 12908
			public MasterBundleReference<AudioClip> outroRef;

			// Token: 0x0400326D RID: 12909
			public float loopVolume;

			// Token: 0x0400326E RID: 12910
			public float outroVolume;
		}

		// Token: 0x02000923 RID: 2339
		public class SkillRule
		{
			// Token: 0x0400326F RID: 12911
			public int defaultLevel;

			// Token: 0x04003270 RID: 12912
			public int maxUnlockableLevel;

			// Token: 0x04003271 RID: 12913
			public float costMultiplier;
		}

		// Token: 0x02000924 RID: 2340
		internal class TerrainColorRule : IDatParseable
		{
			// Token: 0x06004A7B RID: 19067 RVA: 0x001B17FC File Offset: 0x001AF9FC
			public LevelAsset.TerrainColorRule.EComparisonResult CompareColors(float inputHue, float inputSaturation, float inputValue)
			{
				float num;
				float num2;
				if (inputHue < this.ruleHue)
				{
					num = this.ruleHue - inputHue;
					num2 = inputHue + 1f - this.ruleHue;
				}
				else
				{
					num = inputHue - this.ruleHue;
					num2 = this.ruleHue + 1f - inputHue;
				}
				if (num > this.hueThreshold && num2 > this.hueThreshold)
				{
					return LevelAsset.TerrainColorRule.EComparisonResult.OutsideHueThreshold;
				}
				if (Mathf.Abs(inputSaturation - this.ruleSaturation) > this.saturationThreshold)
				{
					return LevelAsset.TerrainColorRule.EComparisonResult.OutsideSaturationThreshold;
				}
				if (Mathf.Abs(inputValue - this.ruleValue) > this.valueThreshold)
				{
					return LevelAsset.TerrainColorRule.EComparisonResult.OutsideValueThreshold;
				}
				return LevelAsset.TerrainColorRule.EComparisonResult.TooSimilar;
			}

			// Token: 0x06004A7C RID: 19068 RVA: 0x001B1888 File Offset: 0x001AFA88
			public bool TryParse(IDatNode node)
			{
				DatDictionary datDictionary = node as DatDictionary;
				if (datDictionary != null)
				{
					Color32 c;
					bool flag = datDictionary.TryParseColor32RGB("Color", out c);
					Color.RGBToHSV(c, out this.ruleHue, out this.ruleSaturation, out this.ruleValue);
					return flag & datDictionary.TryParseFloat("HueThreshold", out this.hueThreshold) & datDictionary.TryParseFloat("SaturationThreshold", out this.saturationThreshold) & datDictionary.TryParseFloat("ValueThreshold", out this.valueThreshold);
				}
				return false;
			}

			// Token: 0x04003272 RID: 12914
			public float ruleHue;

			// Token: 0x04003273 RID: 12915
			public float ruleSaturation;

			// Token: 0x04003274 RID: 12916
			public float ruleValue;

			// Token: 0x04003275 RID: 12917
			public float hueThreshold;

			// Token: 0x04003276 RID: 12918
			public float saturationThreshold;

			// Token: 0x04003277 RID: 12919
			public float valueThreshold;

			// Token: 0x02000A36 RID: 2614
			public enum EComparisonResult
			{
				// Token: 0x0400355F RID: 13663
				TooSimilar,
				// Token: 0x04003560 RID: 13664
				OutsideHueThreshold,
				// Token: 0x04003561 RID: 13665
				OutsideSaturationThreshold,
				// Token: 0x04003562 RID: 13666
				OutsideValueThreshold
			}
		}
	}
}
