using System;
using System.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Unturned;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Framework.Devkit.Tools
{
	// Token: 0x02000146 RID: 326
	public class DevkitLandscapeToolSplatmapOptions : IFormattedFileReadable, IFormattedFileWritable
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0001D5F4 File Offset: 0x0001B7F4
		public static DevkitLandscapeToolSplatmapOptions instance
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions._instance;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0001D5FB File Offset: 0x0001B7FB
		// (set) Token: 0x0600083F RID: 2111 RVA: 0x0001D604 File Offset: 0x0001B804
		public static float paintSensitivity
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions._paintSensitivity;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions._paintSensitivity = value;
				UnturnedLog.info("Set paint_sensitivity to: " + DevkitLandscapeToolSplatmapOptions.paintSensitivity.ToString());
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0001D633 File Offset: 0x0001B833
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x0001D63B File Offset: 0x0001B83B
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

		// Token: 0x06000842 RID: 2114 RVA: 0x0001D654 File Offset: 0x0001B854
		public static void load()
		{
			DevkitLandscapeToolSplatmapOptions.wasLoaded = true;
			string text = PathEx.Join(UnturnedPaths.RootDirectory, "Cloud", "Splatmap.tool");
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
					DevkitLandscapeToolSplatmapOptions.instance.read(reader);
				}
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0001D6F4 File Offset: 0x0001B8F4
		public static void save()
		{
			if (!DevkitLandscapeToolSplatmapOptions.wasLoaded)
			{
				return;
			}
			string text = PathEx.Join(UnturnedPaths.RootDirectory, "Cloud", "Splatmap.tool");
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(text))
			{
				((IFormattedFileWriter)new KeyValueTableWriter(streamWriter)).writeValue<DevkitLandscapeToolSplatmapOptions>(DevkitLandscapeToolSplatmapOptions.instance);
			}
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001D768 File Offset: 0x0001B968
		public void read(IFormattedFileReader reader)
		{
			this.brushRadius = reader.readValue<float>("Brush_Radius");
			this.brushFalloff = reader.readValue<float>("Brush_Falloff");
			this.brushStrength = reader.readValue<float>("Brush_Strength");
			this.autoStrength = reader.readValue<float>("Auto_Strength");
			this.smoothStrength = reader.readValue<float>("Smooth_Strength");
			this.useWeightTarget = reader.readValue<bool>("Use_Weight_Target");
			this.weightTarget = reader.readValue<float>("Weight_Target");
			this.maxPreviewSamples = reader.readValue<uint>("Max_Preview_Samples");
			this.smoothMethod = reader.readValue<EDevkitLandscapeToolSplatmapSmoothMethod>("Smooth_Method");
			this.previewMethod = reader.readValue<EDevkitLandscapeToolSplatmapPreviewMethod>("Preview_Method");
			this.useAutoSlope = reader.readValue<bool>("Use_Auto_Slope");
			this.autoMinAngleBegin = reader.readValue<float>("Auto_Min_Angle_Begin");
			this.autoMinAngleEnd = reader.readValue<float>("Auto_Min_Angle_End");
			this.autoMaxAngleBegin = reader.readValue<float>("Auto_Max_Angle_Begin");
			this.autoMaxAngleEnd = reader.readValue<float>("Auto_Max_Angle_End");
			this.useAutoFoundation = reader.readValue<bool>("Use_Auto_Foundation");
			this.autoRayRadius = reader.readValue<float>("Auto_Ray_Radius");
			this.autoRayLength = reader.readValue<float>("Auto_Ray_Length");
			this.autoRayMask = reader.readValue<ERayMask>("Auto_Ray_Mask");
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001D8B8 File Offset: 0x0001BAB8
		public void write(IFormattedFileWriter writer)
		{
			writer.writeValue<float>("Brush_Radius", this.brushRadius);
			writer.writeValue<float>("Brush_Falloff", this.brushFalloff);
			writer.writeValue<float>("Brush_Strength", this.brushStrength);
			writer.writeValue<float>("Auto_Strength", this.autoStrength);
			writer.writeValue<float>("Smooth_Strength", this.smoothStrength);
			writer.writeValue<bool>("Use_Weight_Target", this.useWeightTarget);
			writer.writeValue<float>("Weight_Target", this.weightTarget);
			writer.writeValue<uint>("Max_Preview_Samples", this.maxPreviewSamples);
			writer.writeValue<EDevkitLandscapeToolSplatmapSmoothMethod>("Smooth_Method", this.smoothMethod);
			writer.writeValue<EDevkitLandscapeToolSplatmapPreviewMethod>("Preview_Method", this.previewMethod);
			writer.writeValue<bool>("Use_Auto_Slope", this.useAutoSlope);
			writer.writeValue<float>("Auto_Min_Angle_Begin", this.autoMinAngleBegin);
			writer.writeValue<float>("Auto_Min_Angle_End", this.autoMinAngleEnd);
			writer.writeValue<float>("Auto_Max_Angle_Begin", this.autoMaxAngleBegin);
			writer.writeValue<float>("Auto_Max_Angle_End", this.autoMaxAngleEnd);
			writer.writeValue<bool>("Use_Auto_Foundation", this.useAutoFoundation);
			writer.writeValue<float>("Auto_Ray_Radius", this.autoRayRadius);
			writer.writeValue<float>("Auto_Ray_Length", this.autoRayLength);
			writer.writeValue<ERayMask>("Auto_Ray_Mask", this.autoRayMask);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001DA08 File Offset: 0x0001BC08
		static DevkitLandscapeToolSplatmapOptions()
		{
			DevkitLandscapeToolSplatmapOptions._instance = new DevkitLandscapeToolSplatmapOptions();
			DevkitLandscapeToolSplatmapOptions.load();
		}

		// Token: 0x04000314 RID: 788
		private static DevkitLandscapeToolSplatmapOptions _instance;

		// Token: 0x04000315 RID: 789
		protected static float _paintSensitivity = 1f;

		// Token: 0x04000316 RID: 790
		private float _brushRadius = 16f;

		// Token: 0x04000317 RID: 791
		public float brushFalloff = 0.5f;

		// Token: 0x04000318 RID: 792
		public float brushStrength = 1f;

		// Token: 0x04000319 RID: 793
		public float autoStrength = 1f;

		// Token: 0x0400031A RID: 794
		public float smoothStrength = 1f;

		// Token: 0x0400031B RID: 795
		public bool useWeightTarget;

		// Token: 0x0400031C RID: 796
		public float weightTarget;

		// Token: 0x0400031D RID: 797
		public uint maxPreviewSamples = 1024U;

		// Token: 0x0400031E RID: 798
		public EDevkitLandscapeToolSplatmapSmoothMethod smoothMethod = EDevkitLandscapeToolSplatmapSmoothMethod.PIXEL_AVERAGE;

		// Token: 0x0400031F RID: 799
		public EDevkitLandscapeToolSplatmapPreviewMethod previewMethod;

		// Token: 0x04000320 RID: 800
		public bool useAutoSlope;

		// Token: 0x04000321 RID: 801
		public float autoMinAngleBegin = 50f;

		// Token: 0x04000322 RID: 802
		public float autoMinAngleEnd = 70f;

		// Token: 0x04000323 RID: 803
		public float autoMaxAngleBegin = 90f;

		// Token: 0x04000324 RID: 804
		public float autoMaxAngleEnd = 90f;

		// Token: 0x04000325 RID: 805
		public bool useAutoFoundation;

		// Token: 0x04000326 RID: 806
		public float autoRayRadius = 1f;

		// Token: 0x04000327 RID: 807
		public float autoRayLength = 512f;

		// Token: 0x04000328 RID: 808
		public ERayMask autoRayMask = (ERayMask)RayMasks.BLOCK_GRASS;

		// Token: 0x04000329 RID: 809
		protected static bool wasLoaded;
	}
}
