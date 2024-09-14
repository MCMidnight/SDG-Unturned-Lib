using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200073F RID: 1855
	public class Distraction : MonoBehaviour
	{
		// Token: 0x06003CE4 RID: 15588 RVA: 0x00121F13 File Offset: 0x00120113
		public void Distract()
		{
			AlertTool.alert(base.transform.position, 24f);
			Object.Destroy(this);
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x00121F30 File Offset: 0x00120130
		private void Start()
		{
			base.Invoke("Distract", 2.5f);
		}
	}
}
