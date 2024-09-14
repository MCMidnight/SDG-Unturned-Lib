using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007D8 RID: 2008
	public class UseableArrestStart : Useable
	{
		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06004429 RID: 17449 RVA: 0x00187B2D File Offset: 0x00185D2D
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x00187B43 File Offset: 0x00185D43
		private void arrest()
		{
			base.player.animator.play("Use", false);
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x00187B5B File Offset: 0x00185D5B
		public void askArrest(CSteamID steamID)
		{
			this.ReceivePlayArrest();
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x00187B63 File Offset: 0x00185D63
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askArrest")]
		public void ReceivePlayArrest()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.arrest();
			}
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x00187B80 File Offset: 0x00185D80
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.player != null && raycastInfo.player.animator.gesture == EPlayerGesture.SURRENDER_START)
				{
					base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.ArrestStart);
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
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.ArrestStart);
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
					UseableArrestStart.SendPlayArrest.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
			}
			return true;
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x00187CF4 File Offset: 0x00185EF4
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x00187D28 File Offset: 0x00185F28
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (Provider.isServer)
				{
					if (this.enemy != null && this.enemy.animator.gesture == EPlayerGesture.SURRENDER_START)
					{
						this.enemy.animator.captorID = base.channel.owner.playerID.steamID;
						this.enemy.animator.captorItem = base.player.equipment.itemID;
						this.enemy.animator.captorStrength = ((ItemArrestStartAsset)base.player.equipment.asset).strength;
						this.enemy.animator.sendGesture(EPlayerGesture.ARREST_START, true);
						base.player.equipment.use();
						return;
					}
					base.player.equipment.dequip();
				}
			}
		}

		// Token: 0x04002D9E RID: 11678
		private float startedUse;

		// Token: 0x04002D9F RID: 11679
		private float useTime;

		// Token: 0x04002DA0 RID: 11680
		private bool isUsing;

		// Token: 0x04002DA1 RID: 11681
		private Player enemy;

		// Token: 0x04002DA2 RID: 11682
		private static readonly ClientInstanceMethod SendPlayArrest = ClientInstanceMethod.Get(typeof(UseableArrestStart), "ReceivePlayArrest");
	}
}
