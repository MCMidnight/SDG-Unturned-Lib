using System;

namespace SDG.Unturned
{
	// Token: 0x0200044D RID: 1101
	public class InteractableClaim : Interactable
	{
		// Token: 0x0600211D RID: 8477 RVA: 0x0007FC5B File Offset: 0x0007DE5B
		public void updateState(ItemBarricadeAsset asset)
		{
			this.deregisterClaim();
			this.registerClaim();
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x0007FC69 File Offset: 0x0007DE69
		public override bool checkInteractable()
		{
			return false;
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x0007FC6C File Offset: 0x0007DE6C
		private void registerClaim()
		{
			if (base.IsChildOfVehicle)
			{
				if (this.plant == null)
				{
					this.plant = ClaimManager.registerPlant(base.transform.parent, this.owner, this.group);
					return;
				}
			}
			else if (this.bubble == null)
			{
				this.bubble = ClaimManager.registerBubble(base.transform.position, 32f, this.owner, this.group);
			}
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x0007FCDB File Offset: 0x0007DEDB
		private void deregisterClaim()
		{
			if (this.bubble != null)
			{
				ClaimManager.deregisterBubble(this.bubble);
				this.bubble = null;
			}
			if (this.plant != null)
			{
				ClaimManager.deregisterPlant(this.plant);
				this.plant = null;
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x0007FD11 File Offset: 0x0007DF11
		private void OnDisable()
		{
			this.deregisterClaim();
		}

		// Token: 0x04001044 RID: 4164
		public ulong owner;

		// Token: 0x04001045 RID: 4165
		public ulong group;

		// Token: 0x04001046 RID: 4166
		private ClaimBubble bubble;

		// Token: 0x04001047 RID: 4167
		private ClaimPlant plant;
	}
}
