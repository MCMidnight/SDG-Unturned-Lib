using System;

namespace SDG.Unturned
{
	// Token: 0x0200045A RID: 1114
	public class InteractableObject : InteractablePower
	{
		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x00083040 File Offset: 0x00081240
		public ObjectAsset objectAsset
		{
			get
			{
				return this._objectAsset;
			}
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x00083048 File Offset: 0x00081248
		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._objectAsset = (asset as ObjectAsset);
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x0008305E File Offset: 0x0008125E
		private void Start()
		{
			base.RefreshIsConnectedToPower();
		}

		// Token: 0x040010A1 RID: 4257
		protected ObjectAsset _objectAsset;
	}
}
