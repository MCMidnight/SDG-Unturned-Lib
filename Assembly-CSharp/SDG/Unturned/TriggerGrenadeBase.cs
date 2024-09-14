using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000746 RID: 1862
	public class TriggerGrenadeBase : MonoBehaviour
	{
		// Token: 0x06003CF7 RID: 15607 RVA: 0x00122515 File Offset: 0x00120715
		protected virtual void GrenadeTriggered()
		{
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00122518 File Offset: 0x00120718
		private void OnTriggerEnter(Collider other)
		{
			if (this.isStuck)
			{
				return;
			}
			if (other.isTrigger)
			{
				return;
			}
			if (this.ignoreTransform != null && (other.transform == this.ignoreTransform || other.transform.IsChildOf(this.ignoreTransform)))
			{
				return;
			}
			this.isStuck = true;
			this.GrenadeTriggered();
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x0012257C File Offset: 0x0012077C
		private void Awake()
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				component.isTrigger = true;
				BoxCollider boxCollider = component as BoxCollider;
				if (boxCollider != null)
				{
					boxCollider.size *= 2f;
				}
			}
		}

		// Token: 0x04002653 RID: 9811
		public Transform ignoreTransform;

		// Token: 0x04002654 RID: 9812
		private bool isStuck;
	}
}
