using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007ED RID: 2029
	public class UseableTire : Useable
	{
		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x060045CE RID: 17870 RVA: 0x001A13F8 File Offset: 0x0019F5F8
		// (remove) Token: 0x060045CF RID: 17871 RVA: 0x001A142C File Offset: 0x0019F62C
		public static event UseableTire.ModifyTireRequestHandler onModifyTireRequested;

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x060045D0 RID: 17872 RVA: 0x001A145F File Offset: 0x0019F65F
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x060045D1 RID: 17873 RVA: 0x001A1475 File Offset: 0x0019F675
		private bool isAttachable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.75f;
			}
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x001A1494 File Offset: 0x0019F694
		private void attach()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x001A14E5 File Offset: 0x0019F6E5
		[Obsolete]
		public void askAttach(CSteamID steamID)
		{
			this.ReceivePlayAttach();
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x001A14ED File Offset: 0x0019F6ED
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askAttach")]
		public void ReceivePlayAttach()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.attach();
			}
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x001A1508 File Offset: 0x0019F708
		private bool fire()
		{
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.vehicle == null || !raycastInfo.vehicle.isTireReplaceable)
				{
					return false;
				}
				if (((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.ADD && !raycastInfo.vehicle.isTireCompatible(base.player.equipment.itemID))
				{
					return false;
				}
				if (raycastInfo.vehicle.getClosestAliveTireIndex(raycastInfo.point, ((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.REMOVE) == -1)
				{
					return false;
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Tire);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Tire);
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
				if (input.vehicle == null || !input.vehicle.isTireReplaceable)
				{
					return false;
				}
				if (((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.ADD && !input.vehicle.isTireCompatible(base.player.equipment.itemID))
				{
					return false;
				}
				int closestAliveTireIndex = input.vehicle.getClosestAliveTireIndex(input.point, ((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.REMOVE);
				if (closestAliveTireIndex == -1)
				{
					return false;
				}
				if (UseableTire.onModifyTireRequested != null)
				{
					bool flag = true;
					UseableTire.onModifyTireRequested(this, input.vehicle, closestAliveTireIndex, ref flag);
					if (!flag)
					{
						return false;
					}
				}
				this.isAttaching = true;
				this.vehicle = input.vehicle;
				this.vehicleWheelIndex = closestAliveTireIndex;
			}
			return true;
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x001A1740 File Offset: 0x0019F940
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
				this.attach();
				if (Provider.isServer)
				{
					UseableTire.SendPlayAttach.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
				return true;
			}
			return false;
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x001A17C0 File Offset: 0x0019F9C0
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x001A17F4 File Offset: 0x0019F9F4
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isAttaching && this.isAttachable)
			{
				this.isAttaching = false;
				if (this.vehicle != null && this.vehicle.isTireReplaceable && this.vehicleWheelIndex != -1)
				{
					if (((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.ADD)
					{
						if (!this.vehicle.tires[this.vehicleWheelIndex].isAlive)
						{
							this.vehicle.askRepairTire(this.vehicleWheelIndex);
							this.wasSuccessfullyUsed = this.vehicle.tires[this.vehicleWheelIndex].isAlive;
						}
					}
					else if (this.vehicle.tires[this.vehicleWheelIndex].isAlive)
					{
						this.vehicle.askDamageTire(this.vehicleWheelIndex);
						if (!this.vehicle.tires[this.vehicleWheelIndex].isAlive)
						{
							base.player.inventory.forceAddItem(new Item(this.vehicle.asset.tireID, true), false);
						}
					}
					this.vehicle = null;
				}
				if (((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.ADD && Provider.isServer)
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
				if (((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.ADD && Provider.isServer && this.wasSuccessfullyUsed)
				{
					base.player.equipment.useStepB();
				}
			}
		}

		// Token: 0x04002F18 RID: 12056
		private float startedUse;

		// Token: 0x04002F19 RID: 12057
		private float useTime;

		// Token: 0x04002F1A RID: 12058
		private bool isUsing;

		// Token: 0x04002F1B RID: 12059
		private bool isAttaching;

		// Token: 0x04002F1C RID: 12060
		private InteractableVehicle vehicle;

		// Token: 0x04002F1D RID: 12061
		private int vehicleWheelIndex = -1;

		// Token: 0x04002F1E RID: 12062
		private static readonly ClientInstanceMethod SendPlayAttach = ClientInstanceMethod.Get(typeof(UseableTire), "ReceivePlayAttach");

		// Token: 0x04002F1F RID: 12063
		private bool wasSuccessfullyUsed;

		// Token: 0x02000A1D RID: 2589
		// (Invoke) Token: 0x06004D89 RID: 19849
		public delegate void ModifyTireRequestHandler(UseableTire useable, InteractableVehicle vehicle, int wheelIndex, ref bool shouldAllow);
	}
}
