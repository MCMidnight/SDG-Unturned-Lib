using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004DB RID: 1243
	public class LevelBarricades
	{
		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06002622 RID: 9762 RVA: 0x00098DAC File Offset: 0x00096FAC
		[Obsolete("Was the parent of all barricades in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelBarricades._models == null)
				{
					LevelBarricades._models = new GameObject().transform;
					LevelBarricades._models.name = "Barricades";
					LevelBarricades._models.parent = Level.spawns;
					LevelBarricades._models.tag = "Logic";
					LevelBarricades._models.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing LevelBarricades.models which has been deprecated.", Array.Empty<object>());
				}
				return LevelBarricades._models;
			}
		}

		// Token: 0x040013AC RID: 5036
		private static Transform _models;
	}
}
