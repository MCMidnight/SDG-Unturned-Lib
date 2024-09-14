using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000469 RID: 1129
	public class InteractableRainBarrel : Interactable
	{
		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x0008522C File Offset: 0x0008342C
		public bool isFull
		{
			get
			{
				return this._isFull;
			}
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x00085234 File Offset: 0x00083434
		public void updateFull(bool newFull)
		{
			this._isFull = newFull;
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x0008523D File Offset: 0x0008343D
		public override void updateState(Asset asset, byte[] state)
		{
			this._isFull = (state[0] == 1);
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x0008524B File Offset: 0x0008344B
		public override bool checkUseable()
		{
			return this.isFull;
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x00085253 File Offset: 0x00083453
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.VOLUME_WATER;
			text = "";
			color = Color.white;
			return true;
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x0008526C File Offset: 0x0008346C
		private void onRainUpdated(ELightingRain rain)
		{
			if (rain != ELightingRain.POST_DRIZZLE)
			{
				return;
			}
			if (Physics.Raycast(base.transform.position + Vector3.up, Vector3.up, 32f, RayMasks.BLOCK_WIND))
			{
				return;
			}
			this._isFull = true;
			if (Provider.isServer)
			{
				BarricadeManager.updateRainBarrel(base.transform, this.isFull, false);
			}
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000852CA File Offset: 0x000834CA
		private void OnEnable()
		{
			LightingManager.onRainUpdated = (RainUpdated)Delegate.Combine(LightingManager.onRainUpdated, new RainUpdated(this.onRainUpdated));
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000852EC File Offset: 0x000834EC
		private void OnDisable()
		{
			LightingManager.onRainUpdated = (RainUpdated)Delegate.Remove(LightingManager.onRainUpdated, new RainUpdated(this.onRainUpdated));
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x0008530E File Offset: 0x0008350E
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveFull(bool newFull)
		{
			this.updateFull(newFull);
		}

		// Token: 0x040010EC RID: 4332
		private bool _isFull;

		// Token: 0x040010ED RID: 4333
		internal static readonly ClientInstanceMethod<bool> SendFull = ClientInstanceMethod<bool>.Get(typeof(InteractableRainBarrel), "ReceiveFull");
	}
}
