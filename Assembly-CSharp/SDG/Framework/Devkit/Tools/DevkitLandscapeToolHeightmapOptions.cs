using System;
using System.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Unturned;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Framework.Devkit.Tools
{
	// Token: 0x02000143 RID: 323
	public class DevkitLandscapeToolHeightmapOptions : IFormattedFileReadable, IFormattedFileWritable
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0001D27A File Offset: 0x0001B47A
		public static DevkitLandscapeToolHeightmapOptions instance
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions._instance;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0001D281 File Offset: 0x0001B481
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0001D288 File Offset: 0x0001B488
		public static float adjustSensitivity
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions._adjustSensitivity;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions._adjustSensitivity = value;
				UnturnedLog.info("Set adjust_sensitivity to: " + DevkitLandscapeToolHeightmapOptions.adjustSensitivity.ToString());
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0001D2B7 File Offset: 0x0001B4B7
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0001D2C0 File Offset: 0x0001B4C0
		public static float flattenSensitivity
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions._flattenSensitivity;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions._flattenSensitivity = value;
				UnturnedLog.info("Set flatten_sensitivity to: " + DevkitLandscapeToolHeightmapOptions.flattenSensitivity.ToString());
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0001D2EF File Offset: 0x0001B4EF
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x0001D2F7 File Offset: 0x0001B4F7
		public float brushRadius
		{
			get
			{
				return this._brushRadius;
			}
			set
			{
				this._brushRadius = Mathf.Clamp(value, 0f, 2048f);
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001D310 File Offset: 0x0001B510
		public static void load()
		{
			DevkitLandscapeToolHeightmapOptions.wasLoaded = true;
			string text = PathEx.Join(UnturnedPaths.RootDirectory, "Cloud", "Heightmap.tool");
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (!File.Exists(text))
			{
				return;
			}
			using (FileStream fileStream = new FileStream(text, 3, 1, 1))
			{
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					IFormattedFileReader reader = new KeyValueTableReader(streamReader);
					DevkitLandscapeToolHeightmapOptions.instance.read(reader);
				}
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001D3B0 File Offset: 0x0001B5B0
		public static void save()
		{
			if (!DevkitLandscapeToolHeightmapOptions.wasLoaded)
			{
				return;
			}
			string text = PathEx.Join(UnturnedPaths.RootDirectory, "Cloud", "Heightmap.tool");
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(text))
			{
				((IFormattedFileWriter)new KeyValueTableWriter(streamWriter)).writeValue<DevkitLandscapeToolHeightmapOptions>(DevkitLandscapeToolHeightmapOptions.instance);
			}
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001D424 File Offset: 0x0001B624
		public void read(IFormattedFileReader reader)
		{
			this.brushRadius = reader.readValue<float>("Brush_Radius");
			this.brushFalloff = reader.readValue<float>("Brush_Falloff");
			this.brushStrength = reader.readValue<float>("Brush_Strength");
			this.flattenStrength = reader.readValue<float>("Flatten_Strength");
			this.smoothStrength = reader.readValue<float>("Smooth_Strength");
			this.flattenTarget = reader.readValue<float>("Flatten_Target");
			this.maxPreviewSamples = reader.readValue<uint>("Max_Preview_Samples");
			this.smoothMethod = reader.readValue<EDevkitLandscapeToolHeightmapSmoothMethod>("Smooth_Method");
			this.flattenMethod = reader.readValue<EDevkitLandscapeToolHeightmapFlattenMethod>("Flatten_Method");
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001D4CC File Offset: 0x0001B6CC
		public void write(IFormattedFileWriter writer)
		{
			writer.writeValue<float>("Brush_Radius", this.brushRadius);
			writer.writeValue<float>("Brush_Falloff", this.brushFalloff);
			writer.writeValue<float>("Brush_Strength", this.brushStrength);
			writer.writeValue<float>("Flatten_Strength", this.flattenStrength);
			writer.writeValue<float>("Smooth_Strength", this.smoothStrength);
			writer.writeValue<float>("Flatten_Target", this.flattenTarget);
			writer.writeValue<uint>("Max_Preview_Samples", this.maxPreviewSamples);
			writer.writeValue<EDevkitLandscapeToolHeightmapSmoothMethod>("Smooth_Method", this.smoothMethod);
			writer.writeValue<EDevkitLandscapeToolHeightmapFlattenMethod>("Flatten_Method", this.flattenMethod);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001D572 File Offset: 0x0001B772
		static DevkitLandscapeToolHeightmapOptions()
		{
			DevkitLandscapeToolHeightmapOptions._instance = new DevkitLandscapeToolHeightmapOptions();
			DevkitLandscapeToolHeightmapOptions.load();
		}

		// Token: 0x04000301 RID: 769
		private static DevkitLandscapeToolHeightmapOptions _instance;

		// Token: 0x04000302 RID: 770
		protected static float _adjustSensitivity = 0.1f;

		// Token: 0x04000303 RID: 771
		protected static float _flattenSensitivity = 1f;

		// Token: 0x04000304 RID: 772
		private float _brushRadius = 16f;

		// Token: 0x04000305 RID: 773
		public float brushFalloff = 0.5f;

		// Token: 0x04000306 RID: 774
		public float brushStrength = 0.05f;

		// Token: 0x04000307 RID: 775
		public float flattenStrength = 1f;

		// Token: 0x04000308 RID: 776
		public float smoothStrength = 1f;

		// Token: 0x04000309 RID: 777
		public float flattenTarget;

		// Token: 0x0400030A RID: 778
		public uint maxPreviewSamples = 1024U;

		// Token: 0x0400030B RID: 779
		public EDevkitLandscapeToolHeightmapSmoothMethod smoothMethod = EDevkitLandscapeToolHeightmapSmoothMethod.PIXEL_AVERAGE;

		// Token: 0x0400030C RID: 780
		public EDevkitLandscapeToolHeightmapFlattenMethod flattenMethod;

		// Token: 0x0400030D RID: 781
		protected static bool wasLoaded;
	}
}
