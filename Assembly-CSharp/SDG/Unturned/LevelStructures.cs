using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004F4 RID: 1268
	public class LevelStructures
	{
		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060027C8 RID: 10184 RVA: 0x000A79A4 File Offset: 0x000A5BA4
		[Obsolete("Was the parent of all structures in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelStructures._models == null)
				{
					LevelStructures._models = new GameObject().transform;
					LevelStructures._models.name = "Structures";
					LevelStructures._models.parent = Level.spawns;
					LevelStructures._models.tag = "Logic";
					LevelStructures._models.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing LevelStructures.models which has been deprecated.", Array.Empty<object>());
				}
				return LevelStructures._models;
			}
		}

		// Token: 0x0400150D RID: 5389
		private static Transform _models;
	}
}
