using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007E3 RID: 2019
	public class UseableGrower : Useable
	{
		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x060044CE RID: 17614 RVA: 0x0018FB73 File Offset: 0x0018DD73
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x0018FB8C File Offset: 0x0018DD8C
		private void grow()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x0018FBDD File Offset: 0x0018DDDD
		[Obsolete]
		public void askGrow(CSteamID steamID)
		{
			this.ReceivePlayGrow();
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x0018FBE5 File Offset: 0x0018DDE5
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askGrow")]
		public void ReceivePlayGrow()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.grow();
			}
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x0018FC00 File Offset: 0x0018DE00
		private bool fire()
		{
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.transform == null || !raycastInfo.transform.CompareTag("Barricade"))
				{
					return false;
				}
				InteractableFarm component = raycastInfo.transform.GetComponent<InteractableFarm>();
				if (component == null)
				{
					return false;
				}
				if (!component.canFertilize)
				{
					return false;
				}
				if (component.IsFullyGrown)
				{
					return false;
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Grower);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Grower);
				if (input == null)
				{
					return false;
				}
				if ((input.point - base.player.look.aim.position).sqrMagnitude > 49f)
				{
					return false;
				}
				if (input.type != ERaycastInfoType.BARRICADE)
				{
					return false;
				}
				if (input.transform == null || !input.transform.CompareTag("Barricade"))
				{
					return false;
				}
				this.farm = input.transform.GetComponent<InteractableFarm>();
				if (this.farm == null)
				{
					return false;
				}
				if (!this.farm.canFertilize)
				{
					return false;
				}
				if (this.farm.IsFullyGrown)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x0018FD94 File Offset: 0x0018DF94
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
				this.grow();
				if (Provider.isServer)
				{
					UseableGrower.SendPlayGrow.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
				return true;
			}
			return false;
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x0018FE14 File Offset: 0x0018E014
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x0018FE48 File Offset: 0x0018E048
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (Provider.isServer)
				{
					if (this.farm != null && this.farm.canFertilize && !this.farm.IsFullyGrown)
					{
						BarricadeManager.updateFarm(this.farm.transform, 1U, true);
						base.player.equipment.use();
						return;
					}
					base.player.equipment.dequip();
				}
			}
		}

		// Token: 0x04002E1E RID: 11806
		private float startedUse;

		// Token: 0x04002E1F RID: 11807
		private float useTime;

		// Token: 0x04002E20 RID: 11808
		private bool isUsing;

		// Token: 0x04002E21 RID: 11809
		private InteractableFarm farm;

		// Token: 0x04002E22 RID: 11810
		private static readonly ClientInstanceMethod SendPlayGrow = ClientInstanceMethod.Get(typeof(UseableGrower), "ReceivePlayGrow");
	}
}
