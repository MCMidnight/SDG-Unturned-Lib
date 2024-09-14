using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007D7 RID: 2007
	public class UseableArrestEnd : Useable
	{
		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06004420 RID: 17440 RVA: 0x001877C9 File Offset: 0x001859C9
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x06004421 RID: 17441 RVA: 0x001877DF File Offset: 0x001859DF
		private void arrest()
		{
			base.player.animator.play("Use", false);
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x001877F7 File Offset: 0x001859F7
		[Obsolete]
		public void askArrest(CSteamID steamID)
		{
			this.ReceivePlayArrest();
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x001877FF File Offset: 0x001859FF
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askArrest")]
		public void ReceivePlayArrest()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.arrest();
			}
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x0018781C File Offset: 0x00185A1C
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.player != null && raycastInfo.player.animator.gesture == EPlayerGesture.ARREST_START)
				{
					base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.ArrestEnd);
					if (!Provider.isServer)
					{
						base.player.equipment.isBusy = true;
						this.startedUse = Time.realtimeSinceStartup;
						this.isUsing = true;
						this.arrest();
					}
				}
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.ArrestEnd);
				if (input == null)
				{
					return false;
				}
				if (input.type == ERaycastInfoType.PLAYER && input.player != null)
				{
					this.enemy = input.player;
					base.player.equipment.isBusy = true;
					this.startedUse = Time.realtimeSinceStartup;
					this.isUsing = true;
					this.arrest();
					UseableArrestEnd.SendPlayArrest.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
			}
			return true;
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x00187991 File Offset: 0x00185B91
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x001879C8 File Offset: 0x00185BC8
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (Provider.isServer)
				{
					if (this.enemy != null && this.enemy.animator.gesture == EPlayerGesture.ARREST_START && this.enemy.animator.captorID == base.channel.owner.playerID.steamID && this.enemy.animator.captorItem == ((ItemArrestEndAsset)base.player.equipment.asset).recover)
					{
						this.enemy.animator.captorID = CSteamID.Nil;
						this.enemy.animator.captorStrength = 0;
						this.enemy.animator.sendGesture(EPlayerGesture.ARREST_STOP, true);
						base.player.inventory.forceAddItem(new Item(((ItemArrestEndAsset)base.player.equipment.asset).recover, EItemOrigin.NATURE), false);
					}
					base.player.equipment.dequip();
				}
			}
		}

		// Token: 0x04002D99 RID: 11673
		private float startedUse;

		// Token: 0x04002D9A RID: 11674
		private float useTime;

		// Token: 0x04002D9B RID: 11675
		private bool isUsing;

		// Token: 0x04002D9C RID: 11676
		private Player enemy;

		// Token: 0x04002D9D RID: 11677
		private static readonly ClientInstanceMethod SendPlayArrest = ClientInstanceMethod.Get(typeof(UseableArrestEnd), "ReceivePlayArrest");
	}
}
