using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	// Token: 0x020005D6 RID: 1494
	[AddComponentMenu("Unturned/Timer Event Hook")]
	public class TimerEventHook : MonoBehaviour
	{
		// Token: 0x06003019 RID: 12313 RVA: 0x000D456C File Offset: 0x000D276C
		public void SetTimer(float duration)
		{
			this.SetTimer(duration, false);
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x000D4578 File Offset: 0x000D2778
		public void SetTimer(float duration, bool looping)
		{
			if (this.coroutine != null)
			{
				base.StopCoroutine(this.coroutine);
				this.coroutine = null;
			}
			this.shouldTimerLoop = looping;
			if (base.gameObject.activeInHierarchy)
			{
				this.coroutine = this.InternalStartTimer(duration);
				base.StartCoroutine(this.coroutine);
			}
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x000D45CE File Offset: 0x000D27CE
		public void SetDefaultTimer()
		{
			this.SetTimer(this.DefaultDuration, this.DefaultLooping);
		}

		/// <summary>
		/// Stop pending timer from triggering.
		/// </summary>
		// Token: 0x0600301C RID: 12316 RVA: 0x000D45E2 File Offset: 0x000D27E2
		public void CancelTimer()
		{
			if (this.coroutine != null)
			{
				base.StopCoroutine(this.coroutine);
				this.coroutine = null;
			}
			this.shouldTimerLoop = false;
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x000D4606 File Offset: 0x000D2806
		private IEnumerator InternalStartTimer(float duration)
		{
			yield return new WaitForSeconds(duration);
			this.coroutine = null;
			this.OnTimerTriggered.Invoke();
			if (this.shouldTimerLoop && this.coroutine == null && base.gameObject.activeInHierarchy)
			{
				this.coroutine = this.InternalStartTimer(duration);
				base.StartCoroutine(this.coroutine);
			}
			yield break;
		}

		/// <summary>
		/// Invoked when timer expires.
		/// </summary>
		// Token: 0x04001A13 RID: 6675
		public UnityEvent OnTimerTriggered;

		/// <summary>
		/// Number of seconds to use when SetDefaultTimer is invoked.
		/// </summary>
		// Token: 0x04001A14 RID: 6676
		public float DefaultDuration;

		/// <summary>
		/// Should timer loop when SetDefaultTimer is invoked?
		/// </summary>
		// Token: 0x04001A15 RID: 6677
		public bool DefaultLooping;

		/// <summary>
		/// Handle to stop the coroutine.
		/// </summary>
		// Token: 0x04001A16 RID: 6678
		private IEnumerator coroutine;

		// Token: 0x04001A17 RID: 6679
		private bool shouldTimerLoop;
	}
}
