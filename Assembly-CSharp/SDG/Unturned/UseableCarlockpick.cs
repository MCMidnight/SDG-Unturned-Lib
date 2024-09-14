using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007DB RID: 2011
	public class UseableCarlockpick : Useable
	{
		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600445F RID: 17503 RVA: 0x0018C1C6 File Offset: 0x0018A3C6
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06004460 RID: 17504 RVA: 0x0018C1DC File Offset: 0x0018A3DC
		private bool isUnlockable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.75f;
			}
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x0018C1F8 File Offset: 0x0018A3F8
		private void jimmy()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0018C249 File Offset: 0x0018A449
		[Obsolete]
		public void askJimmy(CSteamID steamID)
		{
			this.ReceivePlayJimmy();
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0018C251 File Offset: 0x0018A451
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askJimmy")]
		public void ReceivePlayJimmy()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.jimmy();
			}
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x0018C26C File Offset: 0x0018A46C
		private bool fire()
		{
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.vehicle == null || !raycastInfo.vehicle.isEmpty || !raycastInfo.vehicle.isLocked)
				{
					return false;
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Carlockpick);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Carlockpick);
				if (input == null)
				{
					return false;
				}
				if ((input.point - base.player.look.aim.position).sqrMagnitude > 49f)
				{
					return false;
				}
				if (input.type != ERaycastInfoType.VEHICLE)
				{
					return false;
				}
				if (input.vehicle == null || !input.vehicle.isEmpty || !input.vehicle.isLocked)
				{
					return false;
				}
				this.isUnlocking = true;
				this.vehicle = input.vehicle;
			}
			return true;
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x0018C3B4 File Offset: 0x0018A5B4
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (this.isUseable && this.fire())
			{
				base.player.equipment.isBusy = true;
				this.startedUse = Time.realtimeSinceStartup;
				this.isUsing = true;
				this.jimmy();
				if (Provider.isServer)
				{
					base.player.life.markAggressive(true, true);
					UseableCarlockpick.SendPlayJimmy.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x0018C446 File Offset: 0x0018A646
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x0018C47C File Offset: 0x0018A67C
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUnlocking && this.isUnlockable)
			{
				this.isUnlocking = false;
				if (this.vehicle != null && this.vehicle.isEmpty && this.vehicle.isLocked)
				{
					VehicleManager.unlockVehicle(this.vehicle, base.player);
					this.wasSuccessfullyUsed = !this.vehicle.isLocked;
					this.vehicle = null;
				}
				if (Provider.isServer)
				{
					if (this.wasSuccessfullyUsed)
					{
						base.player.equipment.useStepA();
					}
					else
					{
						base.player.equipment.dequip();
					}
				}
			}
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (Provider.isServer && this.wasSuccessfullyUsed)
				{
					base.player.equipment.useStepB();
				}
			}
		}

		// Token: 0x04002DD2 RID: 11730
		private float startedUse;

		// Token: 0x04002DD3 RID: 11731
		private float useTime;

		// Token: 0x04002DD4 RID: 11732
		private bool isUsing;

		// Token: 0x04002DD5 RID: 11733
		private bool isUnlocking;

		// Token: 0x04002DD6 RID: 11734
		private InteractableVehicle vehicle;

		// Token: 0x04002DD7 RID: 11735
		private static readonly ClientInstanceMethod SendPlayJimmy = ClientInstanceMethod.Get(typeof(UseableCarlockpick), "ReceivePlayJimmy");

		// Token: 0x04002DD8 RID: 11736
		private bool wasSuccessfullyUsed;
	}
}
