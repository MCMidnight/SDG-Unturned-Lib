using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000445 RID: 1093
	public class Interactable : MonoBehaviour
	{
		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060020D8 RID: 8408 RVA: 0x0007EFF3 File Offset: 0x0007D1F3
		public bool IsChildOfVehicle
		{
			get
			{
				return base.transform.parent != null && base.transform.parent.CompareTag("Vehicle");
			}
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x0007F01F File Offset: 0x0007D21F
		public virtual void updateState(Asset asset, byte[] state)
		{
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x0007F021 File Offset: 0x0007D221
		public virtual bool checkInteractable()
		{
			return true;
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x0007F024 File Offset: 0x0007D224
		public virtual bool checkUseable()
		{
			return true;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x0007F027 File Offset: 0x0007D227
		public virtual bool checkHighlight(out Color color)
		{
			color = Color.white;
			return false;
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x0007F035 File Offset: 0x0007D235
		public virtual bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.NONE;
			text = "";
			color = Color.white;
			return false;
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x0007F04D File Offset: 0x0007D24D
		public virtual void use()
		{
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x0007F04F File Offset: 0x0007D24F
		public NetId GetNetId()
		{
			return this._netId;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x0007F057 File Offset: 0x0007D257
		internal void AssignNetId(NetId netId)
		{
			this._netId = netId;
			NetIdRegistry.Assign(netId, this);
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x0007F067 File Offset: 0x0007D267
		internal void ReleaseNetId()
		{
			NetIdRegistry.Release(this._netId);
			this._netId.Clear();
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060020E2 RID: 8418 RVA: 0x0007F080 File Offset: 0x0007D280
		[Obsolete("Renamed to IsChildOfVehicle")]
		public bool isPlant
		{
			get
			{
				return this.IsChildOfVehicle;
			}
		}

		// Token: 0x0400101E RID: 4126
		private NetId _netId;
	}
}
