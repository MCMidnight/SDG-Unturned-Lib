using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200010B RID: 267
	public abstract class DevkitHierarchyItemBase : MonoBehaviour, IDevkitHierarchyItem, IFormattedFileReadable, IFormattedFileWritable
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0001A02F File Offset: 0x0001822F
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x0001A037 File Offset: 0x00018237
		public virtual uint instanceID { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0001A040 File Offset: 0x00018240
		public virtual GameObject areaSelectGameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0001A048 File Offset: 0x00018248
		public virtual bool ShouldSave
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0001A04B File Offset: 0x0001824B
		public virtual bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001A04E File Offset: 0x0001824E
		public NetId GetNetIdFromInstanceId()
		{
			if (this.instanceID > 0U)
			{
				return LevelNetIdRegistry.GetDevkitObjectNetId(this.instanceID);
			}
			return NetId.INVALID;
		}

		// Token: 0x060006DD RID: 1757
		public abstract void read(IFormattedFileReader reader);

		// Token: 0x060006DE RID: 1758
		public abstract void write(IFormattedFileWriter writer);
	}
}
