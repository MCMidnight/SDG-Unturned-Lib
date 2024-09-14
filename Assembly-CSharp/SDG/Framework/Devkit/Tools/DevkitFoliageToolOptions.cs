using System;
using System.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Unturned;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Framework.Devkit.Tools
{
	// Token: 0x02000140 RID: 320
	public class DevkitFoliageToolOptions : IFormattedFileReadable, IFormattedFileWritable
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x0001CEAC File Offset: 0x0001B0AC
		public static DevkitFoliageToolOptions instance
		{
			get
			{
				return DevkitFoliageToolOptions._instance;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0001CEB3 File Offset: 0x0001B0B3
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x0001CEBC File Offset: 0x0001B0BC
		public static float addSensitivity
		{
			get
			{
				return DevkitFoliageToolOptions._addSensitivity;
			}
			set
			{
				DevkitFoliageToolOptions._addSensitivity = value;
				UnturnedLog.info("Set add_sensitivity to: " + DevkitFoliageToolOptions.addSensitivity.ToString());
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0001CEEB File Offset: 0x0001B0EB
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x0001CEF4 File Offset: 0x0001B0F4
		public static float removeSensitivity
		{
			get
			{
				return DevkitFoliageToolOptions._removeSensitivity;
			}
			set
			{
				DevkitFoliageToolOptions._removeSensitivity = value;
				UnturnedLog.info("Set remove_sensitivity to: " + DevkitFoliageToolOptions.removeSensitivity.ToString());
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x0001CF23 File Offset: 0x0001B123
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x0001CF2B File Offset: 0x0001B12B
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

		// Token: 0x0600082A RID: 2090 RVA: 0x0001CF44 File Offset: 0x0001B144
		public static void load()
		{
			DevkitFoliageToolOptions.wasLoaded = true;
			string text = PathEx.Join(UnturnedPaths.RootDirectory, "Cloud", "Foliage.tool");
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
					DevkitFoliageToolOptions.instance.read(reader);
				}
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		public static void save()
		{
			if (!DevkitFoliageToolOptions.wasLoaded)
			{
				return;
			}
			string text = PathEx.Join(UnturnedPaths.RootDirectory, "Cloud", "Foliage.tool");
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(text))
			{
				((IFormattedFileWriter)new KeyValueTableWriter(streamWriter)).writeValue<DevkitFoliageToolOptions>(DevkitFoliageToolOptions.instance);
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001D058 File Offset: 0x0001B258
		public void read(IFormattedFileReader reader)
		{
			this.bakeInstancedMeshes = reader.readValue<bool>("Bake_Instanced_Meshes");
			this.bakeResources = reader.readValue<bool>("Bake_Resources");
			this.bakeObjects = reader.readValue<bool>("Bake_Objects");
			this.bakeClear = reader.readValue<bool>("Bake_Clear");
			this.bakeApplyScale = reader.readValue<bool>("Bake_Apply_Scale");
			this.brushRadius = reader.readValue<float>("Brush_Radius");
			this.brushFalloff = reader.readValue<float>("Brush_Falloff");
			this.brushStrength = reader.readValue<float>("Brush_Strength");
			this.densityTarget = reader.readValue<float>("Density_Target");
			this.surfaceMask = reader.readValue<ERayMask>("Surface_Mask");
			this.maxPreviewSamples = reader.readValue<uint>("Max_Preview_Samples");
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001D120 File Offset: 0x0001B320
		public void write(IFormattedFileWriter writer)
		{
			writer.writeValue<bool>("Bake_Instanced_Meshes", this.bakeInstancedMeshes);
			writer.writeValue<bool>("Bake_Resources", this.bakeResources);
			writer.writeValue<bool>("Bake_Objects", this.bakeObjects);
			writer.writeValue<bool>("Bake_Clear", this.bakeClear);
			writer.writeValue<bool>("Bake_Apply_Scale", this.bakeApplyScale);
			writer.writeValue<float>("Brush_Radius", this.brushRadius);
			writer.writeValue<float>("Brush_Falloff", this.brushFalloff);
			writer.writeValue<float>("Brush_Strength", this.brushStrength);
			writer.writeValue<float>("Density_Target", this.densityTarget);
			writer.writeValue<ERayMask>("Surface_Mask", this.surfaceMask);
			writer.writeValue<uint>("Max_Preview_Samples", this.maxPreviewSamples);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001D1E8 File Offset: 0x0001B3E8
		static DevkitFoliageToolOptions()
		{
			DevkitFoliageToolOptions._instance = new DevkitFoliageToolOptions();
			DevkitFoliageToolOptions.load();
		}

		// Token: 0x040002EB RID: 747
		private static DevkitFoliageToolOptions _instance;

		// Token: 0x040002EC RID: 748
		protected static float _addSensitivity = 1f;

		// Token: 0x040002ED RID: 749
		protected static float _removeSensitivity = 1f;

		// Token: 0x040002EE RID: 750
		public bool bakeInstancedMeshes = true;

		// Token: 0x040002EF RID: 751
		public bool bakeResources = true;

		// Token: 0x040002F0 RID: 752
		public bool bakeObjects = true;

		// Token: 0x040002F1 RID: 753
		public bool bakeClear;

		// Token: 0x040002F2 RID: 754
		public bool bakeApplyScale;

		// Token: 0x040002F3 RID: 755
		private float _brushRadius = 16f;

		// Token: 0x040002F4 RID: 756
		public float brushFalloff = 0.5f;

		// Token: 0x040002F5 RID: 757
		public float brushStrength = 0.05f;

		// Token: 0x040002F6 RID: 758
		public float densityTarget = 1f;

		// Token: 0x040002F7 RID: 759
		public ERayMask surfaceMask = ERayMask.LARGE | ERayMask.MEDIUM | ERayMask.SMALL | ERayMask.ENVIRONMENT | ERayMask.GROUND;

		// Token: 0x040002F8 RID: 760
		public uint maxPreviewSamples = 1024U;

		// Token: 0x040002F9 RID: 761
		protected static bool wasLoaded;
	}
}
