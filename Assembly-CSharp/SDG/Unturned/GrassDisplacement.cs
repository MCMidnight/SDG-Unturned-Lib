using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004C9 RID: 1225
	public class GrassDisplacement : MonoBehaviour
	{
		// Token: 0x06002583 RID: 9603 RVA: 0x0009541C File Offset: 0x0009361C
		private void Update()
		{
			Shader.SetGlobalVector(this._Grass_Displacement_Point, new Vector4(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z, 0f));
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x00095474 File Offset: 0x00093674
		private void OnEnable()
		{
			if (this._Grass_Displacement_Point == -1)
			{
				this._Grass_Displacement_Point = Shader.PropertyToID("_Grass_Displacement_Point");
			}
		}

		// Token: 0x04001348 RID: 4936
		private int _Grass_Displacement_Point = -1;
	}
}
