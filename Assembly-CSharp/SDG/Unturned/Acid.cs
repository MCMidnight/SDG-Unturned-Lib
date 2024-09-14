using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200073B RID: 1851
	public class Acid : MonoBehaviour
	{
		// Token: 0x06003CD4 RID: 15572 RVA: 0x00121870 File Offset: 0x0011FA70
		private void OnTriggerEnter(Collider other)
		{
			if (this.isExploded)
			{
				return;
			}
			if (other.isTrigger)
			{
				return;
			}
			if (other.transform.CompareTag("Agent"))
			{
				return;
			}
			this.isExploded = true;
			if (Provider.isServer)
			{
				EffectAsset effectAsset = Assets.FindEffectAssetByGuidOrLegacyId(this.effectGuid, this.effectID);
				if (effectAsset != null)
				{
					EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
					{
						position = this.lastPos,
						relevantDistance = EffectManager.LARGE
					});
				}
			}
			Object.Destroy(base.transform.parent.gameObject);
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x00121901 File Offset: 0x0011FB01
		private void FixedUpdate()
		{
			this.lastPos = base.transform.position;
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x00121914 File Offset: 0x0011FB14
		private void Awake()
		{
			this.lastPos = base.transform.position;
		}

		// Token: 0x0400261C RID: 9756
		private bool isExploded;

		// Token: 0x0400261D RID: 9757
		private Vector3 lastPos;

		// Token: 0x0400261E RID: 9758
		public Guid effectGuid;

		/// <summary>
		/// Kept because lots of modders have been using this script in Unity,
		/// so removing legacy effect id would break their content.
		/// </summary>
		// Token: 0x0400261F RID: 9759
		public ushort effectID;
	}
}
