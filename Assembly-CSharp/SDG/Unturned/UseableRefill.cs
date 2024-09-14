using System;
using SDG.Framework.Water;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007EA RID: 2026
	public class UseableRefill : Useable
	{
		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06004599 RID: 17817 RVA: 0x0019FC50 File Offset: 0x0019DE50
		private bool isUseable
		{
			get
			{
				if (this.refillMode == ERefillMode.USE)
				{
					return Time.realtimeSinceStartup - this.startedUse > this.useTime;
				}
				return this.refillMode == ERefillMode.REFILL && Time.realtimeSinceStartup - this.startedUse > this.refillTime;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x0600459A RID: 17818 RVA: 0x0019FC8E File Offset: 0x0019DE8E
		private ERefillWaterType waterType
		{
			get
			{
				if (base.player.equipment.state != null && base.player.equipment.state.Length != 0)
				{
					return (ERefillWaterType)base.player.equipment.state[0];
				}
				return ERefillWaterType.EMPTY;
			}
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x0019FCCC File Offset: 0x0019DECC
		private void use()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x0019FD1D File Offset: 0x0019DF1D
		[Obsolete]
		public void askUse(CSteamID steamID)
		{
			this.ReceivePlayUse();
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x0019FD25 File Offset: 0x0019DF25
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askUse")]
		public void ReceivePlayUse()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.use();
			}
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x0019FD3F File Offset: 0x0019DF3F
		private void refill()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Refill", false);
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x0019FD69 File Offset: 0x0019DF69
		[Obsolete]
		public void askRefill(CSteamID steamID)
		{
			this.ReceivePlayRefill();
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x0019FD71 File Offset: 0x0019DF71
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askRefill")]
		public void ReceivePlayRefill()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.refill();
			}
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x0019FD8C File Offset: 0x0019DF8C
		private bool fire(bool mode, out ERefillWaterType newWaterType)
		{
			newWaterType = ERefillWaterType.EMPTY;
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				if (!(raycastInfo.transform != null))
				{
					return false;
				}
				InteractableRainBarrel component = raycastInfo.transform.GetComponent<InteractableRainBarrel>();
				InteractableTank component2 = raycastInfo.transform.GetComponent<InteractableTank>();
				InteractableObjectResource component3 = raycastInfo.transform.GetComponent<InteractableObjectResource>();
				WaterVolume waterVolume;
				if (WaterUtility.isPointUnderwater(raycastInfo.point, out waterVolume))
				{
					if (mode)
					{
						return false;
					}
					if (this.waterType != ERefillWaterType.EMPTY)
					{
						return false;
					}
					if (waterVolume == null)
					{
						newWaterType = ERefillWaterType.SALTY;
					}
					else
					{
						newWaterType = waterVolume.waterType;
					}
				}
				else if (component != null)
				{
					if (mode)
					{
						if (this.waterType != ERefillWaterType.CLEAN)
						{
							return false;
						}
						if (component.isFull)
						{
							return false;
						}
						newWaterType = ERefillWaterType.EMPTY;
					}
					else
					{
						if (this.waterType == ERefillWaterType.CLEAN)
						{
							return false;
						}
						if (!component.isFull)
						{
							return false;
						}
						newWaterType = ERefillWaterType.CLEAN;
					}
				}
				else if (component2 != null)
				{
					if (component2.source != ETankSource.WATER)
					{
						return false;
					}
					if (mode)
					{
						if (this.waterType != ERefillWaterType.CLEAN)
						{
							return false;
						}
						if (component2.amount == component2.capacity)
						{
							return false;
						}
						newWaterType = ERefillWaterType.EMPTY;
					}
					else
					{
						if (this.waterType == ERefillWaterType.CLEAN)
						{
							return false;
						}
						if (component2.amount == 0)
						{
							return false;
						}
						newWaterType = ERefillWaterType.CLEAN;
					}
				}
				else
				{
					if (!(component3 != null))
					{
						return false;
					}
					if (component3.objectAsset.interactability != EObjectInteractability.WATER)
					{
						return false;
					}
					if (mode)
					{
						if (this.waterType == ERefillWaterType.EMPTY)
						{
							return false;
						}
						if (component3.amount == component3.capacity)
						{
							return false;
						}
						newWaterType = ERefillWaterType.EMPTY;
					}
					else
					{
						if (this.waterType == ERefillWaterType.CLEAN || this.waterType == ERefillWaterType.DIRTY)
						{
							return false;
						}
						if (component3.amount == 0)
						{
							return false;
						}
						newWaterType = ERefillWaterType.DIRTY;
					}
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Refill);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Refill);
				if (input == null)
				{
					return false;
				}
				if ((input.point - base.player.look.aim.position).sqrMagnitude > 49f)
				{
					return false;
				}
				WaterVolume waterVolume2;
				if (WaterUtility.isPointUnderwater(input.point, out waterVolume2))
				{
					if (mode)
					{
						return false;
					}
					if (this.waterType != ERefillWaterType.EMPTY)
					{
						return false;
					}
					if (waterVolume2 == null)
					{
						newWaterType = ERefillWaterType.SALTY;
					}
					else
					{
						newWaterType = waterVolume2.waterType;
					}
				}
				else if (input.type == ERaycastInfoType.BARRICADE)
				{
					if (input.transform == null || !input.transform.CompareTag("Barricade"))
					{
						return false;
					}
					InteractableRainBarrel component4 = input.transform.GetComponent<InteractableRainBarrel>();
					InteractableTank component5 = input.transform.GetComponent<InteractableTank>();
					if (component4 != null)
					{
						if (mode)
						{
							if (this.waterType != ERefillWaterType.CLEAN)
							{
								return false;
							}
							if (component4.isFull)
							{
								return false;
							}
							BarricadeManager.updateRainBarrel(component4.transform, true, true);
							newWaterType = ERefillWaterType.EMPTY;
						}
						else
						{
							if (this.waterType == ERefillWaterType.CLEAN)
							{
								return false;
							}
							if (!component4.isFull)
							{
								return false;
							}
							BarricadeManager.updateRainBarrel(component4.transform, false, true);
							newWaterType = ERefillWaterType.CLEAN;
						}
					}
					else
					{
						if (!(component5 != null))
						{
							return false;
						}
						if (component5.source != ETankSource.WATER)
						{
							return false;
						}
						if (mode)
						{
							if (this.waterType != ERefillWaterType.CLEAN)
							{
								return false;
							}
							if (component5.amount == component5.capacity)
							{
								return false;
							}
							component5.ServerSetAmount(component5.amount + 1);
							newWaterType = ERefillWaterType.EMPTY;
						}
						else
						{
							if (this.waterType == ERefillWaterType.CLEAN)
							{
								return false;
							}
							if (component5.amount == 0)
							{
								return false;
							}
							component5.ServerSetAmount(component5.amount - 1);
							newWaterType = ERefillWaterType.CLEAN;
						}
					}
				}
				else if (input.type == ERaycastInfoType.OBJECT)
				{
					if (input.transform == null)
					{
						return false;
					}
					InteractableObjectResource component6 = input.transform.GetComponent<InteractableObjectResource>();
					if (component6 == null || component6.objectAsset.interactability != EObjectInteractability.WATER)
					{
						return false;
					}
					if (mode)
					{
						if (this.waterType == ERefillWaterType.EMPTY)
						{
							return false;
						}
						if (component6.amount == component6.capacity)
						{
							return false;
						}
						ObjectManager.updateObjectResource(component6.transform, (ushort)((byte)(component6.amount + 1)), true);
						newWaterType = ERefillWaterType.EMPTY;
					}
					else
					{
						if (this.waterType == ERefillWaterType.CLEAN || this.waterType == ERefillWaterType.DIRTY)
						{
							return false;
						}
						if (component6.amount == 0)
						{
							return false;
						}
						ObjectManager.updateObjectResource(component6.transform, (ushort)((byte)(component6.amount - 1)), true);
						newWaterType = ERefillWaterType.DIRTY;
					}
				}
			}
			return true;
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x001A0204 File Offset: 0x0019E404
		private void msg()
		{
			EPlayerMessage message;
			switch (this.waterType)
			{
			case ERefillWaterType.EMPTY:
				message = EPlayerMessage.EMPTY;
				break;
			case ERefillWaterType.CLEAN:
				message = EPlayerMessage.CLEAN;
				break;
			case ERefillWaterType.SALTY:
				message = EPlayerMessage.SALTY;
				break;
			case ERefillWaterType.DIRTY:
				message = EPlayerMessage.DIRTY;
				break;
			default:
				message = EPlayerMessage.FULL;
				break;
			}
			PlayerUI.message(message, "", 2f);
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x001A0258 File Offset: 0x0019E458
		private void start(ERefillWaterType newWaterType)
		{
			base.player.equipment.isBusy = true;
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			this.refillMode = ERefillMode.REFILL;
			this.refill();
			base.player.equipment.quality = ((newWaterType == ERefillWaterType.DIRTY) ? 0 : 100);
			base.player.equipment.updateQuality();
			base.player.equipment.state[0] = (byte)newWaterType;
			base.player.equipment.updateState();
			if (Provider.isServer)
			{
				UseableRefill.SendPlayRefill.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
			}
			if (base.channel.IsLocalPlayer)
			{
				this.msg();
			}
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x001A031C File Offset: 0x0019E51C
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (this.isUseable)
			{
				ERefillWaterType erefillWaterType;
				if (this.fire(true, out erefillWaterType))
				{
					this.start(erefillWaterType);
				}
				else if (this.waterType != ERefillWaterType.EMPTY)
				{
					base.player.equipment.isBusy = true;
					this.startedUse = Time.realtimeSinceStartup;
					this.isUsing = true;
					this.refillMode = ERefillMode.USE;
					this.refillWaterType = this.waterType;
					this.use();
					base.player.equipment.quality = ((erefillWaterType == ERefillWaterType.DIRTY) ? 0 : 100);
					base.player.equipment.updateQuality();
					base.player.equipment.state[0] = 0;
					base.player.equipment.updateState();
					if (Provider.isServer)
					{
						UseableRefill.SendPlayUse.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
					}
					if (base.channel.IsLocalPlayer)
					{
						this.msg();
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x001A042C File Offset: 0x0019E62C
		public override bool startSecondary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			ERefillWaterType newWaterType;
			if (this.isUseable && this.fire(false, out newWaterType))
			{
				this.start(newWaterType);
				return true;
			}
			return false;
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x001A046C File Offset: 0x0019E66C
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
			this.refillTime = base.player.animator.GetAnimationLength("Refill", true);
			if (base.channel.IsLocalPlayer)
			{
				this.msg();
			}
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x001A04DC File Offset: 0x0019E6DC
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (this.refillMode == ERefillMode.USE)
				{
					ItemRefillAsset itemRefillAsset = base.player.equipment.asset as ItemRefillAsset;
					float delta;
					float delta2;
					switch (this.refillWaterType)
					{
					case ERefillWaterType.CLEAN:
						delta = itemRefillAsset.cleanStamina;
						delta2 = itemRefillAsset.cleanOxygen;
						break;
					case ERefillWaterType.SALTY:
						delta = itemRefillAsset.saltyStamina;
						delta2 = itemRefillAsset.saltyOxygen;
						break;
					case ERefillWaterType.DIRTY:
						delta = itemRefillAsset.dirtyStamina;
						delta2 = itemRefillAsset.dirtyOxygen;
						break;
					default:
						delta = 0f;
						delta2 = 0f;
						break;
					}
					base.player.life.simulatedModifyStamina(delta);
					base.player.life.simulatedModifyOxygen(delta2);
					if (Provider.isServer)
					{
						float delta3;
						float delta4;
						float delta5;
						float delta6;
						switch (this.refillWaterType)
						{
						case ERefillWaterType.CLEAN:
							delta3 = itemRefillAsset.cleanHealth;
							delta4 = itemRefillAsset.cleanFood;
							delta5 = itemRefillAsset.cleanWater;
							delta6 = itemRefillAsset.cleanVirus;
							break;
						case ERefillWaterType.SALTY:
							delta3 = itemRefillAsset.saltyHealth;
							delta4 = itemRefillAsset.saltyFood;
							delta5 = itemRefillAsset.saltyWater;
							delta6 = itemRefillAsset.saltyVirus;
							break;
						case ERefillWaterType.DIRTY:
							delta3 = itemRefillAsset.dirtyHealth;
							delta4 = itemRefillAsset.dirtyFood;
							delta5 = itemRefillAsset.dirtyWater;
							delta6 = itemRefillAsset.dirtyVirus;
							break;
						default:
							delta3 = 0f;
							delta4 = 0f;
							delta5 = 0f;
							delta6 = 0f;
							break;
						}
						base.player.life.serverModifyHealth(delta3);
						base.player.life.serverModifyFood(delta4);
						base.player.life.serverModifyWater(delta5);
						base.player.life.serverModifyVirus(delta6);
					}
				}
			}
		}

		// Token: 0x04002EF6 RID: 12022
		private float startedUse;

		// Token: 0x04002EF7 RID: 12023
		private float useTime;

		// Token: 0x04002EF8 RID: 12024
		private float refillTime;

		// Token: 0x04002EF9 RID: 12025
		private bool isUsing;

		// Token: 0x04002EFA RID: 12026
		private ERefillMode refillMode;

		// Token: 0x04002EFB RID: 12027
		private ERefillWaterType refillWaterType;

		// Token: 0x04002EFC RID: 12028
		private static readonly ClientInstanceMethod SendPlayUse = ClientInstanceMethod.Get(typeof(UseableRefill), "ReceivePlayUse");

		// Token: 0x04002EFD RID: 12029
		private static readonly ClientInstanceMethod SendPlayRefill = ClientInstanceMethod.Get(typeof(UseableRefill), "ReceivePlayRefill");
	}
}
