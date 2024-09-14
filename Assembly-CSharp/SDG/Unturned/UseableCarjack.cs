using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007DA RID: 2010
	public class UseableCarjack : Useable
	{
		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06004453 RID: 17491 RVA: 0x0018BD95 File Offset: 0x00189F95
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06004454 RID: 17492 RVA: 0x0018BDAB File Offset: 0x00189FAB
		private bool isJackable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.75f;
			}
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0018BDC8 File Offset: 0x00189FC8
		private void pull()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0018BE19 File Offset: 0x0018A019
		[Obsolete]
		public void askPull(CSteamID steamID)
		{
			this.ReceivePlayPull();
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x0018BE21 File Offset: 0x0018A021
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askPull")]
		public void ReceivePlayPull()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.pull();
			}
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x0018BE3C File Offset: 0x0018A03C
		private bool isVehicleValid(InteractableVehicle testVehicle)
		{
			return testVehicle.isEmpty && (base.channel.owner.isAdmin || !base.player.movement.isSafe || base.player.movement.isSafeInfo == null || !base.player.movement.isSafeInfo.noWeapons || testVehicle.checkEnter(base.player));
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x0018BEB0 File Offset: 0x0018A0B0
		private bool fire()
		{
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.vehicle == null || !this.isVehicleValid(raycastInfo.vehicle))
				{
					return false;
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Carjack);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Carjack);
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
				if (input.vehicle == null || !this.isVehicleValid(input.vehicle))
				{
					return false;
				}
				this.isJacking = true;
				this.vehicle = input.vehicle;
			}
			return true;
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x0018BFE0 File Offset: 0x0018A1E0
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
				this.pull();
				if (Provider.isServer)
				{
					UseableCarjack.SendPlayPull.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x0018C060 File Offset: 0x0018A260
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x0018C094 File Offset: 0x0018A294
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isJacking && this.isJackable)
			{
				this.isJacking = false;
				if (this.vehicle != null && this.isVehicleValid(this.vehicle))
				{
					Vector3 force = new Vector3(Random.Range(-32f, 32f), Random.Range(480f, 544f) * (float)((base.player.skills.boost == EPlayerBoost.FLIGHT) ? 4 : 1), Random.Range(-32f, 32f));
					Vector3 torque = new Vector3(Random.Range(-64f, 64f), Random.Range(-64f, 64f), Random.Range(-64f, 64f));
					VehicleManager.carjackVehicle(this.vehicle, base.player, force, torque);
					this.vehicle = null;
				}
			}
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
			}
		}

		// Token: 0x04002DCC RID: 11724
		private float startedUse;

		// Token: 0x04002DCD RID: 11725
		private float useTime;

		// Token: 0x04002DCE RID: 11726
		private bool isUsing;

		// Token: 0x04002DCF RID: 11727
		private bool isJacking;

		// Token: 0x04002DD0 RID: 11728
		private InteractableVehicle vehicle;

		// Token: 0x04002DD1 RID: 11729
		private static readonly ClientInstanceMethod SendPlayPull = ClientInstanceMethod.Get(typeof(UseableCarjack), "ReceivePlayPull");
	}
}
