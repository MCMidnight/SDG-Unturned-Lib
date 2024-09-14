using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000765 RID: 1893
	public class PoolReference : MonoBehaviour
	{
		// Token: 0x06003DED RID: 15853 RVA: 0x0012BF70 File Offset: 0x0012A170
		public void DestroyIntoPool(float t)
		{
			this.CancelDestroyTimer();
			if (this.pool == null)
			{
				Object.Destroy(base.gameObject, t);
				return;
			}
			if (base.gameObject.activeInHierarchy)
			{
				this.invokeAfterDelayCoroutine = TimeUtility.InvokeAfterDelay(new Action(this.DestroyIntoPoolCallback), t);
				return;
			}
			this.pool.Destroy(this);
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x0012BFCA File Offset: 0x0012A1CA
		internal void CancelDestroyTimer()
		{
			if (this.invokeAfterDelayCoroutine != null)
			{
				TimeUtility.StaticStopCoroutine(this.invokeAfterDelayCoroutine);
				this.invokeAfterDelayCoroutine = null;
			}
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x0012BFE6 File Offset: 0x0012A1E6
		private void DestroyIntoPoolCallback()
		{
			this.invokeAfterDelayCoroutine = null;
			if (this.pool == null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			this.pool.Destroy(this);
		}

		/// <summary>
		/// Listen for OnDestroy callback because mods may be destroying themselves in unexpected ways (e.g., Grenade
		/// component) and still need to be cleaned up.
		/// </summary>
		// Token: 0x06003DF0 RID: 15856 RVA: 0x0012C010 File Offset: 0x0012A210
		private void OnDestroy()
		{
			this.CancelDestroyTimer();
			if (Level.isExiting)
			{
				return;
			}
			if (base.transform.parent != null)
			{
				EffectManager.UnregisterAttachment(base.gameObject);
			}
			if (this.pool != null && !this.inPool)
			{
				this.pool.active.RemoveFast(this);
				this.pool = null;
			}
		}

		// Token: 0x040026E0 RID: 9952
		public GameObjectPool pool;

		// Token: 0x040026E1 RID: 9953
		public bool inPool;

		/// <summary>
		/// Enabled for effects held by guns and sentries.
		/// </summary>
		// Token: 0x040026E2 RID: 9954
		public bool excludeFromDestroyAll;

		// Token: 0x040026E3 RID: 9955
		private Coroutine invokeAfterDelayCoroutine;
	}
}
