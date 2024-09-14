using System;
using System.Collections;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	// Token: 0x02000086 RID: 134
	public class TimeUtility : MonoBehaviour
	{
		/// <summary>
		/// Equivalent to MonoBehaviour.Update
		/// </summary>
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600033E RID: 830 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		// (remove) Token: 0x0600033F RID: 831 RVA: 0x0000C928 File Offset: 0x0000AB28
		public static event UpdateHandler updated;

		/// <summary>
		/// Equivalent to MonoBehaviour.FixedUpdate
		/// </summary>
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000340 RID: 832 RVA: 0x0000C95C File Offset: 0x0000AB5C
		// (remove) Token: 0x06000341 RID: 833 RVA: 0x0000C990 File Offset: 0x0000AB90
		public static event UpdateHandler physicsUpdated;

		/// <summary>
		/// Useful when caller is not a MonoBehaviour, or coroutine should not be owned by a component which might get
		/// deactivated. For example attached effects destroy timer should happen regardless of parent deactivation.
		/// </summary>
		// Token: 0x06000342 RID: 834 RVA: 0x0000C9C3 File Offset: 0x0000ABC3
		public static Coroutine InvokeAfterDelay(Action callback, float timeSeconds)
		{
			return TimeUtility.singleton.StartCoroutine(TimeUtility.singleton.InternalInvokeAfterDelay(callback, timeSeconds));
		}

		/// <summary>
		/// Stop a coroutine started by InvokeAfterDelay.
		/// </summary>
		// Token: 0x06000343 RID: 835 RVA: 0x0000C9DB File Offset: 0x0000ABDB
		public static void StaticStopCoroutine(Coroutine routine)
		{
			if (TimeUtility.singleton != null)
			{
				TimeUtility.singleton.StopCoroutine(routine);
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000C9F5 File Offset: 0x0000ABF5
		protected virtual void triggerUpdated()
		{
			UpdateHandler updateHandler = TimeUtility.updated;
			if (updateHandler == null)
			{
				return;
			}
			updateHandler();
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000CA06 File Offset: 0x0000AC06
		protected virtual void Update()
		{
			UpdateHandler updateHandler = TimeUtility.updated;
			if (updateHandler == null)
			{
				return;
			}
			updateHandler();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000CA17 File Offset: 0x0000AC17
		protected virtual void FixedUpdate()
		{
			UpdateHandler updateHandler = TimeUtility.physicsUpdated;
			if (updateHandler == null)
			{
				return;
			}
			updateHandler();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000CA28 File Offset: 0x0000AC28
		private IEnumerator InternalInvokeAfterDelay(Action callback, float timeSeconds)
		{
			yield return new WaitForSeconds(timeSeconds);
			callback.Invoke();
			yield break;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000CA3E File Offset: 0x0000AC3E
		private void Awake()
		{
			TimeUtility.singleton = this;
		}

		// Token: 0x0400015F RID: 351
		private static TimeUtility singleton;
	}
}
