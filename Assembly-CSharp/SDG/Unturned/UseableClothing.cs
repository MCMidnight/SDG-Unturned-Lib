using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007DC RID: 2012
	public class UseableClothing : Useable
	{
		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x0600446A RID: 17514 RVA: 0x0018C595 File Offset: 0x0018A795
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x0018C5AB File Offset: 0x0018A7AB
		private void wear()
		{
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0018C5DF File Offset: 0x0018A7DF
		[Obsolete]
		public void askWear(CSteamID steamID)
		{
			this.ReceivePlayWear();
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0018C5E7 File Offset: 0x0018A7E7
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askWear")]
		public void ReceivePlayWear()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.wear();
			}
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x0018C604 File Offset: 0x0018A804
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			base.player.equipment.isBusy = true;
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			if (Provider.isServer)
			{
				UseableClothing.SendPlayWear.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
			}
			this.wear();
			return true;
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0018C672 File Offset: 0x0018A872
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x0018C6A8 File Offset: 0x0018A8A8
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (Provider.isServer)
				{
					ItemAsset asset = base.player.equipment.asset;
					EItemType type = asset.type;
					byte quality = base.player.equipment.quality;
					byte[] state = base.player.equipment.state;
					base.player.equipment.use();
					if (type == EItemType.HAT)
					{
						base.player.clothing.askWearHat(asset as ItemHatAsset, quality, state, true);
						return;
					}
					if (type == EItemType.SHIRT)
					{
						base.player.clothing.askWearShirt(asset as ItemShirtAsset, quality, state, true);
						return;
					}
					if (type == EItemType.PANTS)
					{
						base.player.clothing.askWearPants(asset as ItemPantsAsset, quality, state, true);
						return;
					}
					if (type == EItemType.BACKPACK)
					{
						base.player.clothing.askWearBackpack(asset as ItemBackpackAsset, quality, state, true);
						return;
					}
					if (type == EItemType.VEST)
					{
						base.player.clothing.askWearVest(asset as ItemVestAsset, quality, state, true);
						return;
					}
					if (type == EItemType.MASK)
					{
						base.player.clothing.askWearMask(asset as ItemMaskAsset, quality, state, true);
						return;
					}
					if (type == EItemType.GLASSES)
					{
						base.player.clothing.askWearGlasses(asset as ItemGlassesAsset, quality, state, true);
					}
				}
			}
		}

		// Token: 0x04002DD9 RID: 11737
		private float startedUse;

		// Token: 0x04002DDA RID: 11738
		private float useTime;

		// Token: 0x04002DDB RID: 11739
		private bool isUsing;

		// Token: 0x04002DDC RID: 11740
		private static readonly ClientInstanceMethod SendPlayWear = ClientInstanceMethod.Get(typeof(UseableClothing), "ReceivePlayWear");
	}
}
