using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000460 RID: 1120
	public class InteractableObjectResource : InteractableObject
	{
		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x0600220E RID: 8718 RVA: 0x00084085 File Offset: 0x00082285
		public ushort amount
		{
			get
			{
				return this._amount;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x0600220F RID: 8719 RVA: 0x0008408D File Offset: 0x0008228D
		public ushort capacity
		{
			get
			{
				return this._capacity;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002210 RID: 8720 RVA: 0x00084095 File Offset: 0x00082295
		public bool isRefillable
		{
			get
			{
				return this.amount < this.capacity;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002211 RID: 8721 RVA: 0x000840A5 File Offset: 0x000822A5
		public bool isSiphonable
		{
			get
			{
				return this.amount > 0;
			}
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x000840B0 File Offset: 0x000822B0
		public bool checkCanReset(float multiplier)
		{
			if (this.amount == this.capacity)
			{
				return false;
			}
			if (base.objectAsset.interactabilityReset < 1f)
			{
				return false;
			}
			if (base.objectAsset.interactability == EObjectInteractability.WATER)
			{
				return Time.realtimeSinceStartup - this.lastUsed > base.objectAsset.interactabilityReset * multiplier;
			}
			return base.objectAsset.interactability == EObjectInteractability.FUEL && Time.realtimeSinceStartup - this.lastUsed > base.objectAsset.interactabilityReset * multiplier;
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x00084136 File Offset: 0x00082336
		public void updateAmount(ushort newAmount)
		{
			this._amount = newAmount;
			this.lastUsed = Time.realtimeSinceStartup;
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x0008414C File Offset: 0x0008234C
		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._amount = BitConverter.ToUInt16(state, 0);
			this._capacity = ((ObjectAsset)asset).interactabilityResource;
			this.lastUsed = Time.realtimeSinceStartup;
			if (base.objectAsset.interactability == EObjectInteractability.WATER)
			{
				if (this.isListeningForRain)
				{
					return;
				}
				this.isListeningForRain = true;
				LightingManager.onRainUpdated = (RainUpdated)Delegate.Combine(LightingManager.onRainUpdated, new RainUpdated(this.onRainUpdated));
			}
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x000841C8 File Offset: 0x000823C8
		public override bool checkUseable()
		{
			return this.amount > 0;
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x000841D4 File Offset: 0x000823D4
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (base.objectAsset.interactability == EObjectInteractability.WATER)
			{
				message = EPlayerMessage.VOLUME_WATER;
				text = this.amount.ToString() + "/" + this.capacity.ToString();
			}
			else
			{
				message = EPlayerMessage.VOLUME_FUEL;
				text = "";
			}
			color = Color.white;
			return true;
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x00084234 File Offset: 0x00082434
		private void onRainUpdated(ELightingRain rain)
		{
			if (rain != ELightingRain.POST_DRIZZLE)
			{
				return;
			}
			this._amount = this.capacity;
			if (Provider.isServer)
			{
				ObjectManager.updateObjectResource(base.transform, this.amount, false);
			}
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x00084260 File Offset: 0x00082460
		private void OnDestroy()
		{
			if (base.objectAsset.interactability == EObjectInteractability.WATER)
			{
				if (!this.isListeningForRain)
				{
					return;
				}
				this.isListeningForRain = false;
				LightingManager.onRainUpdated = (RainUpdated)Delegate.Remove(LightingManager.onRainUpdated, new RainUpdated(this.onRainUpdated));
			}
		}

		// Token: 0x040010C7 RID: 4295
		private ushort _amount;

		// Token: 0x040010C8 RID: 4296
		private ushort _capacity;

		// Token: 0x040010C9 RID: 4297
		private bool isListeningForRain;

		// Token: 0x040010CA RID: 4298
		private float lastUsed = -9999f;
	}
}
