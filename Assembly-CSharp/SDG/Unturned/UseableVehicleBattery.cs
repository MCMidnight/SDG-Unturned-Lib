using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007EE RID: 2030
	public class UseableVehicleBattery : Useable
	{
		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x060045DB RID: 17883 RVA: 0x001A19FE File Offset: 0x0019FBFE
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x060045DC RID: 17884 RVA: 0x001A1A14 File Offset: 0x0019FC14
		private bool isReplaceable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.75f;
			}
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x001A1A30 File Offset: 0x0019FC30
		private void replace()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x001A1A81 File Offset: 0x0019FC81
		[Obsolete]
		public void askReplace(CSteamID steamID)
		{
			this.ReceivePlayReplace();
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x001A1A89 File Offset: 0x0019FC89
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askReplace")]
		public void ReceivePlayReplace()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.replace();
			}
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x001A1AA4 File Offset: 0x0019FCA4
		private bool fire()
		{
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.vehicle == null || !raycastInfo.vehicle.isBatteryReplaceable)
				{
					return false;
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Battery);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Battery);
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
				if (input.vehicle == null || !input.vehicle.isBatteryReplaceable)
				{
					return false;
				}
				this.isReplacing = true;
				this.vehicle = input.vehicle;
			}
			return true;
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x001A1BD0 File Offset: 0x0019FDD0
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
				this.replace();
				if (Provider.isServer)
				{
					UseableVehicleBattery.SendPlayReplace.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
				return true;
			}
			return false;
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x001A1C50 File Offset: 0x0019FE50
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x001A1C84 File Offset: 0x0019FE84
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isReplacing && this.isReplaceable)
			{
				this.isReplacing = false;
				if (this.vehicle != null && this.vehicle.isBatteryReplaceable)
				{
					this.vehicle.replaceBattery(base.player, base.player.equipment.quality, base.player.equipment.asset.GUID);
					this.wasSuccessfullyUsed = true;
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

		// Token: 0x04002F20 RID: 12064
		private float startedUse;

		// Token: 0x04002F21 RID: 12065
		private float useTime;

		// Token: 0x04002F22 RID: 12066
		private bool isUsing;

		// Token: 0x04002F23 RID: 12067
		private bool isReplacing;

		// Token: 0x04002F24 RID: 12068
		private InteractableVehicle vehicle;

		// Token: 0x04002F25 RID: 12069
		private static readonly ClientInstanceMethod SendPlayReplace = ClientInstanceMethod.Get(typeof(UseableVehicleBattery), "ReceivePlayReplace");

		// Token: 0x04002F26 RID: 12070
		private bool wasSuccessfullyUsed;
	}
}
