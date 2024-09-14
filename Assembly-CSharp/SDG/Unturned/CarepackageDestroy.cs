using System;
using System.Collections;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200073E RID: 1854
	public class CarepackageDestroy : MonoBehaviour
	{
		// Token: 0x06003CE1 RID: 15585 RVA: 0x00121EEE File Offset: 0x001200EE
		private IEnumerator cleanup()
		{
			yield return new WaitForSeconds(600f);
			BarricadeManager.damage(base.transform, 65000f, 1f, false, default(CSteamID), EDamageOrigin.Carepackage_Timeout);
			yield break;
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x00121EFD File Offset: 0x001200FD
		private void Start()
		{
			base.StartCoroutine("cleanup");
		}
	}
}
