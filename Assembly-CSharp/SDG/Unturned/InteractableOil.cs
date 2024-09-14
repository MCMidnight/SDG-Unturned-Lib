using System;
using System.Collections;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000465 RID: 1125
	public class InteractableOil : InteractablePower
	{
		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x00084A57 File Offset: 0x00082C57
		public ushort fuel
		{
			get
			{
				return this._fuel;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002233 RID: 8755 RVA: 0x00084A5F File Offset: 0x00082C5F
		// (set) Token: 0x06002234 RID: 8756 RVA: 0x00084A67 File Offset: 0x00082C67
		public ushort capacity { get; protected set; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002235 RID: 8757 RVA: 0x00084A70 File Offset: 0x00082C70
		public bool isRefillable
		{
			get
			{
				return this.fuel < this.capacity;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x00084A80 File Offset: 0x00082C80
		public bool isSiphonable
		{
			get
			{
				return this.fuel > 0;
			}
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x00084A8B File Offset: 0x00082C8B
		public void tellFuel(ushort newFuel)
		{
			this._fuel = newFuel;
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x00084A94 File Offset: 0x00082C94
		public void askBurn(ushort amount)
		{
			if (amount == 0)
			{
				return;
			}
			if (amount >= this.fuel)
			{
				this._fuel = 0;
			}
			else
			{
				this._fuel -= amount;
			}
			if (Provider.isServer)
			{
				this.updateState();
			}
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x00084AC8 File Offset: 0x00082CC8
		public void askFill(ushort amount)
		{
			if (amount == 0)
			{
				return;
			}
			if (amount >= this.capacity - this.fuel)
			{
				this._fuel = this.capacity;
			}
			else
			{
				this._fuel += amount;
			}
			if (Provider.isServer)
			{
				this.updateState();
			}
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x00084B08 File Offset: 0x00082D08
		private void UpdateVisual()
		{
			if (this.engine != null)
			{
				this.engine.gameObject.SetActive(base.isWired);
			}
			if (this.root != null)
			{
				if (base.isWired)
				{
					this.root.Play();
					using (IEnumerator enumerator = this.root.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							((AnimationState)obj).normalizedTime = Random.value;
						}
						return;
					}
				}
				this.root.Stop();
			}
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x00084BB4 File Offset: 0x00082DB4
		protected override void updateWired()
		{
			this.UpdateVisual();
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x00084BBC File Offset: 0x00082DBC
		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this.capacity = ((ItemOilPumpAsset)asset).fuelCapacity;
			this._fuel = BitConverter.ToUInt16(state, 0);
			this.engine = base.transform.Find("Engine");
			if (Provider.isServer)
			{
				this.metadata = state;
			}
			base.RefreshIsConnectedToPowerWithoutNotify();
			this.UpdateVisual();
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x00084C1F File Offset: 0x00082E1F
		public override bool checkUseable()
		{
			return this.fuel > 0;
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x00084C2A File Offset: 0x00082E2A
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.VOLUME_FUEL;
			text = "";
			color = Color.white;
			return true;
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x00084C43 File Offset: 0x00082E43
		private void updateState()
		{
			if (this.metadata == null)
			{
				return;
			}
			BitConverter.GetBytes(this.fuel).CopyTo(this.metadata, 0);
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x00084C68 File Offset: 0x00082E68
		private void Update()
		{
			if (!base.isWired)
			{
				this.lastDrilled = Time.realtimeSinceStartup;
				return;
			}
			if (Time.realtimeSinceStartup - this.lastDrilled > 2f)
			{
				this.lastDrilled = Time.realtimeSinceStartup;
				if (this.fuel < this.capacity)
				{
					this.askFill(1);
				}
			}
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x00084CBC File Offset: 0x00082EBC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveFuel(ushort newFuel)
		{
			this.tellFuel(newFuel);
		}

		// Token: 0x040010DB RID: 4315
		private ushort _fuel;

		// Token: 0x040010DD RID: 4317
		private byte[] metadata;

		// Token: 0x040010DE RID: 4318
		private Transform engine;

		// Token: 0x040010DF RID: 4319
		private Animation root;

		// Token: 0x040010E0 RID: 4320
		private float lastDrilled;

		// Token: 0x040010E1 RID: 4321
		internal static readonly ClientInstanceMethod<ushort> SendFuel = ClientInstanceMethod<ushort>.Get(typeof(InteractableOil), "ReceiveFuel");
	}
}
