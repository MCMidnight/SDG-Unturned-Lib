using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007E0 RID: 2016
	public class UseableFilter : Useable
	{
		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x0600449D RID: 17565 RVA: 0x0018DE44 File Offset: 0x0018C044
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x0018DE5A File Offset: 0x0018C05A
		private void filter()
		{
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x0018DE8E File Offset: 0x0018C08E
		[Obsolete]
		public void askFilter(CSteamID steamID)
		{
			this.ReceivePlayFilter();
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x0018DE96 File Offset: 0x0018C096
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askFilter")]
		public void ReceivePlayFilter()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.filter();
			}
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x0018DEB0 File Offset: 0x0018C0B0
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (base.player.clothing.maskAsset == null || !base.player.clothing.maskAsset.proofRadiation || base.player.clothing.maskQuality == 100)
			{
				return false;
			}
			base.player.equipment.isBusy = true;
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			if (Provider.isServer)
			{
				UseableFilter.SendPlayFilter.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
			}
			this.filter();
			return true;
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x0018DF5D File Offset: 0x0018C15D
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x0018DF94 File Offset: 0x0018C194
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (Provider.isServer)
				{
					if (base.player.clothing.maskAsset != null && base.player.clothing.maskAsset.proofRadiation && base.player.clothing.maskQuality < 100)
					{
						base.player.equipment.use();
						base.player.clothing.maskQuality = 100;
						base.player.clothing.sendUpdateMaskQuality();
						return;
					}
					base.player.equipment.dequip();
				}
			}
		}

		// Token: 0x04002DF4 RID: 11764
		private float startedUse;

		// Token: 0x04002DF5 RID: 11765
		private float useTime;

		// Token: 0x04002DF6 RID: 11766
		private bool isUsing;

		// Token: 0x04002DF7 RID: 11767
		private static readonly ClientInstanceMethod SendPlayFilter = ClientInstanceMethod.Get(typeof(UseableFilter), "ReceivePlayFilter");
	}
}
