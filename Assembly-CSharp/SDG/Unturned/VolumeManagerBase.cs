using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200051B RID: 1307
	public abstract class VolumeManagerBase
	{
		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x060028E1 RID: 10465 RVA: 0x000AE4C7 File Offset: 0x000AC6C7
		// (set) Token: 0x060028E2 RID: 10466 RVA: 0x000AE4CF File Offset: 0x000AC6CF
		public string FriendlyName { get; protected set; }

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x060028E3 RID: 10467 RVA: 0x000AE4D8 File Offset: 0x000AC6D8
		// (set) Token: 0x060028E4 RID: 10468 RVA: 0x000AE4E0 File Offset: 0x000AC6E0
		public virtual ELevelVolumeVisibility Visibility { get; set; }

		// Token: 0x060028E5 RID: 10469
		public abstract bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance);

		// Token: 0x060028E6 RID: 10470
		public abstract void InstantiateVolume(Vector3 position, Quaternion rotation, Vector3 scale);

		// Token: 0x060028E7 RID: 10471
		public abstract IEnumerable<VolumeBase> EnumerateAllVolumes();

		/// <summary>
		/// Auto-registering list of volume manager subclasses for level editor.
		/// </summary>
		// Token: 0x040015BF RID: 5567
		internal static List<VolumeManagerBase> allManagers = new List<VolumeManagerBase>();
	}
}
