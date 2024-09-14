using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000118 RID: 280
	public interface IDevkitHierarchyItem : IFormattedFileReadable, IFormattedFileWritable
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000739 RID: 1849
		// (set) Token: 0x0600073A RID: 1850
		uint instanceID { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600073B RID: 1851
		GameObject areaSelectGameObject { get; }

		/// <summary>
		/// If true, write to LevelHierarchy file.
		/// False for externally managed objects like legacy lighting WaterVolume.
		/// </summary>
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600073C RID: 1852
		bool ShouldSave { get; }

		/// <summary>
		/// If true, editor tools can select and transform.
		/// False for items like the object-owned culling volumes.
		/// </summary>
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600073D RID: 1853
		bool CanBeSelected { get; }
	}
}
