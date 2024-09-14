using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SDG.Unturned
{
	// Token: 0x020004E2 RID: 1250
	public class LevelInfoConfigData
	{
		// Token: 0x06002683 RID: 9859 RVA: 0x0009D4A8 File Offset: 0x0009B6A8
		public LevelInfoConfigData()
		{
			this.Creators = new string[0];
			this.Collaborators = new string[0];
			this.Thanks = new string[0];
			this.Item = 0;
			this.Associated_Stockpile_Items = new int[0];
			this.Feedback = null;
			this.Asset = AssetReference<LevelAsset>.invalid;
			this.Trains = new List<LevelTrainAssociation>();
			this.Mode_Config_Overrides = new Dictionary<string, object>();
			this.Allow_Underwater_Features = false;
			this.Terrain_Snow_Sparkle = false;
			this.Use_Legacy_Clip_Borders = true;
			this.Use_Legacy_Ground = true;
			this.Use_Legacy_Water = true;
			this.Use_Vanilla_Bubbles = true;
			this.Use_Legacy_Snow_Height = true;
			this.Use_Legacy_Oxygen_Height = true;
			this.Use_Rain_Volumes = false;
			this.Use_Snow_Volumes = false;
			this.Is_Aurora_Borealis_Visible = false;
			this.Snow_Affects_Temperature = true;
			this.Has_Atmosphere = true;
			this.Allow_Crafting = true;
			this.Allow_Skills = true;
			this.Allow_Information = true;
			this.Gravity = -9.81f;
			this.Blimp_Altitude = 150f;
			this.Max_Walkable_Slope = -1f;
			this.Prevent_Building_Near_Spawnpoint_Radius = 16f;
			this.Category = ESingleplayerMapCategory.MISC;
			this.Use_Arena_Compactor = true;
			this.Arena_Loadouts = new List<ArenaLoadout>();
			this.Spawn_Loadouts = new List<ArenaLoadout>();
			this.Version = "3.0.0.0";
		}

		// Token: 0x040013FB RID: 5115
		public string[] Creators;

		// Token: 0x040013FC RID: 5116
		public string[] Collaborators;

		// Token: 0x040013FD RID: 5117
		public string[] Thanks;

		// Token: 0x040013FE RID: 5118
		public int Item;

		// Token: 0x040013FF RID: 5119
		public int[] Associated_Stockpile_Items;

		// Token: 0x04001400 RID: 5120
		public string Feedback;

		// Token: 0x04001401 RID: 5121
		public AssetReference<LevelAsset> Asset;

		// Token: 0x04001402 RID: 5122
		public List<LevelTrainAssociation> Trains;

		// Token: 0x04001403 RID: 5123
		public Dictionary<string, object> Mode_Config_Overrides;

		// Token: 0x04001404 RID: 5124
		public bool Allow_Underwater_Features;

		// Token: 0x04001405 RID: 5125
		public bool Terrain_Snow_Sparkle;

		// Token: 0x04001406 RID: 5126
		public bool Use_Legacy_Clip_Borders;

		// Token: 0x04001407 RID: 5127
		public bool Use_Legacy_Ground;

		// Token: 0x04001408 RID: 5128
		public bool Use_Legacy_Water;

		/// <summary>
		/// Should underwater bubble particles be activated?
		/// </summary>
		// Token: 0x04001409 RID: 5129
		public bool Use_Vanilla_Bubbles;

		/// <summary>
		/// Should positions underground be clamped above ground?
		/// Underground volumes are used to whitelist valid positions.
		/// </summary>
		// Token: 0x0400140A RID: 5130
		public bool Use_Underground_Whitelist;

		// Token: 0x0400140B RID: 5131
		public bool Use_Legacy_Snow_Height;

		// Token: 0x0400140C RID: 5132
		public bool Use_Legacy_Fog_Height;

		// Token: 0x0400140D RID: 5133
		public bool Use_Legacy_Oxygen_Height;

		// Token: 0x0400140E RID: 5134
		public bool Use_Rain_Volumes;

		// Token: 0x0400140F RID: 5135
		public bool Use_Snow_Volumes;

		// Token: 0x04001410 RID: 5136
		public bool Is_Aurora_Borealis_Visible;

		// Token: 0x04001411 RID: 5137
		public bool Snow_Affects_Temperature;

		// Token: 0x04001412 RID: 5138
		public ELevelWeatherOverride Weather_Override;

		// Token: 0x04001413 RID: 5139
		public bool Has_Atmosphere;

		// Token: 0x04001414 RID: 5140
		public bool Allow_Crafting;

		// Token: 0x04001415 RID: 5141
		public bool Allow_Skills;

		// Token: 0x04001416 RID: 5142
		public bool Allow_Information;

		/// <summary>
		/// If true, certain objects redirect to load others in-game.
		/// </summary>
		// Token: 0x04001417 RID: 5143
		public bool Allow_Holiday_Redirects;

		/// <summary>
		/// If true, electric objects are always powered, and generators have no effect.
		/// </summary>
		// Token: 0x04001418 RID: 5144
		public bool Has_Global_Electricity;

		// Token: 0x04001419 RID: 5145
		public float Gravity;

		// Token: 0x0400141A RID: 5146
		public float Blimp_Altitude;

		// Token: 0x0400141B RID: 5147
		public float Max_Walkable_Slope;

		// Token: 0x0400141C RID: 5148
		public float Prevent_Building_Near_Spawnpoint_Radius;

		// Token: 0x0400141D RID: 5149
		public ESingleplayerMapCategory Category;

		// Token: 0x0400141E RID: 5150
		public bool PlayerUI_HealthVisible = true;

		// Token: 0x0400141F RID: 5151
		public bool PlayerUI_FoodVisible = true;

		// Token: 0x04001420 RID: 5152
		public bool PlayerUI_WaterVisible = true;

		// Token: 0x04001421 RID: 5153
		public bool PlayerUI_VirusVisible = true;

		// Token: 0x04001422 RID: 5154
		public bool PlayerUI_StaminaVisible = true;

		// Token: 0x04001423 RID: 5155
		public bool PlayerUI_OxygenVisible = true;

		// Token: 0x04001424 RID: 5156
		public bool PlayerUI_GunVisible = true;

		/// <summary>
		/// Display version in the format "a.b.c.d".
		/// </summary>
		// Token: 0x04001425 RID: 5157
		public string Version;

		/// <summary>
		/// Version string packed into integer.
		/// </summary>
		// Token: 0x04001426 RID: 5158
		[JsonIgnore]
		public uint PackedVersion;

		/// <summary>
		/// Number of custom tips defined in per-level localization file.
		/// Tip keys are read as Tip_#
		/// </summary>
		// Token: 0x04001427 RID: 5159
		public int Tips;

		/// <summary>
		/// LevelBatching is currently only enabled if map creator has verified it works properly.
		/// </summary>
		// Token: 0x04001428 RID: 5160
		public int Batching_Version;

		// Token: 0x04001429 RID: 5161
		public bool Use_Arena_Compactor;

		// Token: 0x0400142A RID: 5162
		public List<ArenaLoadout> Arena_Loadouts;

		// Token: 0x0400142B RID: 5163
		public List<ArenaLoadout> Spawn_Loadouts;

		// Token: 0x0400142C RID: 5164
		[JsonIgnore]
		public byte[] Hash;
	}
}
