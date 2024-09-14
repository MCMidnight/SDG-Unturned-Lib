using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000745 RID: 1861
	public class StickyGrenade : TriggerGrenadeBase
	{
		// Token: 0x06003CF5 RID: 15605 RVA: 0x001224F2 File Offset: 0x001206F2
		protected override void GrenadeTriggered()
		{
			base.GrenadeTriggered();
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.useGravity = false;
			component.isKinematic = true;
		}
	}
}
