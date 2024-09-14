using System;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to any GameObject with a Trigger to receive events.
	/// Ensure that Layer will detect player overlaps. Trap is a good candidate.
	/// </summary>
	// Token: 0x020005C3 RID: 1475
	[AddComponentMenu("Unturned/Collision Event Hook")]
	public class CollisionEventHook : MonoBehaviour
	{
		// Token: 0x06002FD7 RID: 12247 RVA: 0x000D3864 File Offset: 0x000D1A64
		private void OnTriggerEnter(Collider other)
		{
			if (other == null)
			{
				return;
			}
			if (!other.CompareTag("Player"))
			{
				return;
			}
			this.OnPlayerEnter.Invoke();
			int numOverlappingPlayers = this.numOverlappingPlayers + 1;
			this.numOverlappingPlayers = numOverlappingPlayers;
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x000D38A4 File Offset: 0x000D1AA4
		private void OnTriggerExit(Collider other)
		{
			if (other == null)
			{
				return;
			}
			if (!other.CompareTag("Player"))
			{
				return;
			}
			this.OnPlayerExit.Invoke();
			int numOverlappingPlayers = this.numOverlappingPlayers - 1;
			this.numOverlappingPlayers = numOverlappingPlayers;
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002FD9 RID: 12249 RVA: 0x000D38E4 File Offset: 0x000D1AE4
		// (set) Token: 0x06002FDA RID: 12250 RVA: 0x000D38EC File Offset: 0x000D1AEC
		private int numOverlappingPlayers
		{
			get
			{
				return this._numOverlappingPlayers;
			}
			set
			{
				value = Mathf.Max(0, value);
				if (this._numOverlappingPlayers == value)
				{
					return;
				}
				this._numOverlappingPlayers = value;
				if (value == 0)
				{
					this.OnAllPlayersExit.Invoke();
					return;
				}
				if (value != 1)
				{
					return;
				}
				this.OnFirstPlayerEnter.Invoke();
			}
		}

		/// <summary>
		/// Invoked when a player enters the trigger.
		/// Called before OnFirstPlayerEnter.
		/// </summary>
		// Token: 0x040019D2 RID: 6610
		public UnityEvent OnPlayerEnter;

		/// <summary>
		/// Invoked when a player exits the trigger.
		/// Called before OnAllPlayersExit.
		/// </summary>
		// Token: 0x040019D3 RID: 6611
		public UnityEvent OnPlayerExit;

		/// <summary>
		/// Invoked when first player enters the trigger, and not again until all players have left.
		/// Called after OnPlayerEnter.
		/// </summary>
		// Token: 0x040019D4 RID: 6612
		public UnityEvent OnFirstPlayerEnter;

		/// <summary>
		/// Invoked when last player exits the trigger.
		/// Called after OnPlayerExit.
		/// </summary>
		// Token: 0x040019D5 RID: 6613
		public UnityEvent OnAllPlayersExit;

		// Token: 0x040019D6 RID: 6614
		private int _numOverlappingPlayers;
	}
}
