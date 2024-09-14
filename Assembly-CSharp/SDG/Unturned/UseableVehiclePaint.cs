using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007F0 RID: 2032
	public class UseableVehiclePaint : Useable
	{
		// Token: 0x140000FA RID: 250
		// (add) Token: 0x060045EA RID: 17898 RVA: 0x001A1DA8 File Offset: 0x0019FFA8
		// (remove) Token: 0x060045EB RID: 17899 RVA: 0x001A1DDC File Offset: 0x0019FFDC
		public static event PaintVehicleRequestHandler OnPaintVehicleRequested;

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x060045EC RID: 17900 RVA: 0x001A1E0F File Offset: 0x001A000F
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x060045ED RID: 17901 RVA: 0x001A1E25 File Offset: 0x001A0025
		private bool isReplaceable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.85f;
			}
		}

		// Token: 0x060045EE RID: 17902 RVA: 0x001A1E44 File Offset: 0x001A0044
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

		// Token: 0x060045EF RID: 17903 RVA: 0x001A1E95 File Offset: 0x001A0095
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceivePlayReplace()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.replace();
			}
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x001A1EB0 File Offset: 0x001A00B0
		private bool IsVehicleAlreadySameColorAsPaint(InteractableVehicle vehicle)
		{
			ItemVehiclePaintToolAsset itemVehiclePaintToolAsset = base.player.equipment.asset as ItemVehiclePaintToolAsset;
			if (itemVehiclePaintToolAsset != null)
			{
				Color32 paintColor = itemVehiclePaintToolAsset.PaintColor;
				paintColor.a = byte.MaxValue;
				return vehicle.PaintColor.Equals(paintColor);
			}
			return false;
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x001A1F08 File Offset: 0x001A0108
		private bool fire()
		{
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.vehicle == null)
				{
					return false;
				}
				if (!raycastInfo.vehicle.asset.IsPaintable)
				{
					PlayerUI.message(EPlayerMessage.NOT_PAINTABLE, string.Empty, 2f);
					return false;
				}
				if (!raycastInfo.vehicle.checkEnter(base.player))
				{
					return false;
				}
				if (this.IsVehicleAlreadySameColorAsPaint(raycastInfo.vehicle))
				{
					return false;
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Paint);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Paint);
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
				if (input.vehicle == null || !input.vehicle.asset.IsPaintable)
				{
					return false;
				}
				if (!input.vehicle.checkEnter(base.player))
				{
					return false;
				}
				if (this.IsVehicleAlreadySameColorAsPaint(input.vehicle))
				{
					return false;
				}
				this.isReplacing = true;
				this.vehicle = input.vehicle;
			}
			return true;
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x001A20A0 File Offset: 0x001A02A0
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
					UseableVehiclePaint.SendPlayReplace.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
				return true;
			}
			return false;
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x001A2120 File Offset: 0x001A0320
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x001A2154 File Offset: 0x001A0354
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isReplacing && this.isReplaceable)
			{
				this.isReplacing = false;
				if (this.vehicle != null && this.vehicle.asset.IsPaintable && this.vehicle.checkEnter(base.player))
				{
					Color32 paintColor = new Color32(0, 0, 0, byte.MaxValue);
					ItemVehiclePaintToolAsset itemVehiclePaintToolAsset = base.player.equipment.asset as ItemVehiclePaintToolAsset;
					if (itemVehiclePaintToolAsset != null)
					{
						paintColor = itemVehiclePaintToolAsset.PaintColor;
						paintColor.a = byte.MaxValue;
					}
					else
					{
						paintColor.r = (byte)Random.Range(0, 256);
						paintColor.g = (byte)Random.Range(0, 256);
						paintColor.b = (byte)Random.Range(0, 256);
					}
					bool flag = true;
					try
					{
						PaintVehicleRequestHandler onPaintVehicleRequested = UseableVehiclePaint.OnPaintVehicleRequested;
						if (onPaintVehicleRequested != null)
						{
							onPaintVehicleRequested(this.vehicle, base.player, ref flag, ref paintColor);
						}
					}
					catch (Exception e)
					{
						UnturnedLog.exception(e, "Caught exception invoking OnPaintVehicleRequested:");
					}
					if (flag)
					{
						this.vehicle.ServerSetPaintColor(paintColor);
						this.wasSuccessfullyUsed = true;
					}
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

		// Token: 0x04002F28 RID: 12072
		private float startedUse;

		// Token: 0x04002F29 RID: 12073
		private float useTime;

		// Token: 0x04002F2A RID: 12074
		private bool isUsing;

		// Token: 0x04002F2B RID: 12075
		private bool isReplacing;

		// Token: 0x04002F2C RID: 12076
		private InteractableVehicle vehicle;

		// Token: 0x04002F2D RID: 12077
		private static readonly ClientInstanceMethod SendPlayReplace = ClientInstanceMethod.Get(typeof(UseableVehiclePaint), "ReceivePlayReplace");

		// Token: 0x04002F2E RID: 12078
		private bool wasSuccessfullyUsed;
	}
}
