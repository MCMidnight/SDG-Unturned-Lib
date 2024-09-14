using System;
using System.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Unturned;
using Unturned.SystemEx;

namespace SDG.Framework.Devkit.Tools
{
	// Token: 0x02000147 RID: 327
	public class DevkitSelectionToolOptions : IFormattedFileReadable, IFormattedFileWritable
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x0001DACD File Offset: 0x0001BCCD
		public static DevkitSelectionToolOptions instance
		{
			get
			{
				return DevkitSelectionToolOptions._instance;
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001DAD4 File Offset: 0x0001BCD4
		public static void load()
		{
			DevkitSelectionToolOptions.wasLoaded = true;
			string text = PathEx.Join(UnturnedPaths.RootDirectory, "Cloud", "Selection.tool");
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
					DevkitSelectionToolOptions.instance.read(reader);
				}
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001DB74 File Offset: 0x0001BD74
		public static void save()
		{
			if (!DevkitSelectionToolOptions.wasLoaded)
			{
				return;
			}
			string text = PathEx.Join(UnturnedPaths.RootDirectory, "Cloud", "Selection.tool");
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(text))
			{
				((IFormattedFileWriter)new KeyValueTableWriter(streamWriter)).writeValue<DevkitSelectionToolOptions>(DevkitSelectionToolOptions.instance);
			}
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001DBE8 File Offset: 0x0001BDE8
		public void read(IFormattedFileReader reader)
		{
			this.snapPosition = reader.readValue<float>("Snap_Position");
			this.snapRotation = reader.readValue<float>("Snap_Rotation");
			this.snapScale = reader.readValue<float>("Snap_Scale");
			this.surfaceOffset = reader.readValue<float>("Surface_Offset");
			this.surfaceAlign = reader.readValue<bool>("Surface_Align");
			this.localSpace = reader.readValue<bool>("Local_Space");
			this.lockHandles = reader.readValue<bool>("Lock_Handles");
			this.selectionMask = reader.readValue<ERayMask>("Selection_Mask");
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001DC80 File Offset: 0x0001BE80
		public void write(IFormattedFileWriter writer)
		{
			writer.writeValue<float>("Snap_Position", this.snapPosition);
			writer.writeValue<float>("Snap_Rotation", this.snapRotation);
			writer.writeValue<float>("Snap_Scale", this.snapScale);
			writer.writeValue<float>("Surface_Offset", this.surfaceOffset);
			writer.writeValue<bool>("Surface_Align", this.surfaceAlign);
			writer.writeValue<bool>("Local_Space", this.localSpace);
			writer.writeValue<bool>("Lock_Handles", this.lockHandles);
			writer.writeValue<ERayMask>("Selection_Mask", this.selectionMask);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001DD15 File Offset: 0x0001BF15
		static DevkitSelectionToolOptions()
		{
			DevkitSelectionToolOptions.load();
		}

		// Token: 0x0400032A RID: 810
		private static DevkitSelectionToolOptions _instance = new DevkitSelectionToolOptions();

		// Token: 0x0400032B RID: 811
		public float snapPosition = 1f;

		// Token: 0x0400032C RID: 812
		public float snapRotation = 15f;

		// Token: 0x0400032D RID: 813
		public float snapScale = 0.1f;

		// Token: 0x0400032E RID: 814
		public float surfaceOffset;

		// Token: 0x0400032F RID: 815
		public bool surfaceAlign;

		// Token: 0x04000330 RID: 816
		public bool localSpace;

		// Token: 0x04000331 RID: 817
		public bool lockHandles;

		// Token: 0x04000332 RID: 818
		public ERayMask selectionMask = ERayMask.LARGE | ERayMask.MEDIUM | ERayMask.SMALL | ERayMask.ENVIRONMENT | ERayMask.GROUND | ERayMask.CLIP | ERayMask.TRAP;

		// Token: 0x04000333 RID: 819
		protected static bool wasLoaded;
	}
}
