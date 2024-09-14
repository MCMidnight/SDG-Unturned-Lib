using System;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Single percentage randomness with two outcomes.
	/// </summary>
	// Token: 0x020005C1 RID: 1473
	[AddComponentMenu("Unturned/Binary Random")]
	public class BinaryRandomComponent : MonoBehaviour
	{
		// Token: 0x06002FD0 RID: 12240 RVA: 0x000D37E4 File Offset: 0x000D19E4
		public void TriggerDefault()
		{
			this.TriggerWithProbability(this.DefaultProbability);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000D37F2 File Offset: 0x000D19F2
		public void TriggerWithProbability(float Probability)
		{
			if (this.AuthorityOnly && !Provider.isServer)
			{
				return;
			}
			if (Random.value < Probability)
			{
				this.OnTrue.Invoke();
				return;
			}
			this.OnFalse.Invoke();
		}

		/// <summary>
		/// If true the event will only be invoked in offline mode and on the server.
		/// </summary>
		// Token: 0x040019CC RID: 6604
		public bool AuthorityOnly;

		/// <summary>
		/// Percentage chance of event occurring.
		/// </summary>
		// Token: 0x040019CD RID: 6605
		[Range(0f, 1f)]
		public float DefaultProbability;

		/// <summary>
		/// Invoked when random event occurs.
		/// </summary>
		// Token: 0x040019CE RID: 6606
		public UnityEvent OnTrue;

		/// <summary>
		/// Invoked when random event does NOT occur.
		/// </summary>
		// Token: 0x040019CF RID: 6607
		public UnityEvent OnFalse;
	}
}
