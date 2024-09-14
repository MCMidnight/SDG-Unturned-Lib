using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200010C RID: 268
	public class DevkitHierarchyWorldItem : DevkitHierarchyItemBase
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0001A072 File Offset: 0x00018272
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x0001A07F File Offset: 0x0001827F
		public Vector3 inspectablePosition
		{
			get
			{
				return base.transform.localPosition;
			}
			set
			{
				base.transform.position = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0001A08D File Offset: 0x0001828D
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0001A09A File Offset: 0x0001829A
		public Quaternion inspectableRotation
		{
			get
			{
				return base.transform.localRotation;
			}
			set
			{
				base.transform.rotation = value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001A0A8 File Offset: 0x000182A8
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0001A0B5 File Offset: 0x000182B5
		public Vector3 inspectableScale
		{
			get
			{
				return base.transform.localScale;
			}
			set
			{
				base.transform.localScale = value;
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001A0C3 File Offset: 0x000182C3
		public override void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return;
			}
			this.readHierarchyItem(reader);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001A0D8 File Offset: 0x000182D8
		protected virtual void readHierarchyItem(IFormattedFileReader reader)
		{
			base.transform.position = reader.readValue<Vector3>("Position");
			base.transform.SetRotation_RoundIfNearlyAxisAligned(reader.readValue<Quaternion>("Rotation"), 0.05f);
			Vector3 vector = reader.readValue<Vector3>("Scale");
			vector.x = Mathf.Clamp(vector.x, -100000f, 100000f);
			vector.y = Mathf.Clamp(vector.y, -100000f, 100000f);
			vector.z = Mathf.Clamp(vector.z, -100000f, 100000f);
			base.transform.SetLocalScale_RoundIfNearlyEqualToOne(vector, 0.001f);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001A187 File Offset: 0x00018387
		public override void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			this.writeHierarchyItem(writer);
			writer.endObject();
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001A19C File Offset: 0x0001839C
		protected virtual void writeHierarchyItem(IFormattedFileWriter writer)
		{
			writer.writeValue<Vector3>("Position", base.transform.position);
			writer.writeValue<Quaternion>("Rotation", base.transform.rotation);
			writer.writeValue<Vector3>("Scale", base.transform.localScale);
		}
	}
}
