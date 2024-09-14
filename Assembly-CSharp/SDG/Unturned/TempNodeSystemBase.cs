using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200015C RID: 348
	public abstract class TempNodeSystemBase
	{
		// Token: 0x060008BA RID: 2234 RVA: 0x0001E7D1 File Offset: 0x0001C9D1
		internal void Instantiate(Vector3 position)
		{
			DevkitTypeFactory.instantiate(this.GetComponentType(), position, Quaternion.identity, Vector3.one);
		}

		// Token: 0x060008BB RID: 2235
		internal abstract Type GetComponentType();

		// Token: 0x060008BC RID: 2236
		internal abstract IEnumerable<GameObject> EnumerateGameObjects();
	}
}
