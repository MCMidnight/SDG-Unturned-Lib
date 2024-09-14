using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007E2 RID: 2018
	public class UseableFuel : Useable
	{
		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x060044C1 RID: 17601 RVA: 0x0018EF7D File Offset: 0x0018D17D
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x0018EF94 File Offset: 0x0018D194
		private void glug()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x0018EFE5 File Offset: 0x0018D1E5
		[Obsolete]
		public void askGlug(CSteamID steamID)
		{
			this.ReceivePlayGlug();
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x0018EFED File Offset: 0x0018D1ED
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askGlug")]
		public void ReceivePlayGlug()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.glug();
			}
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x0018F008 File Offset: 0x0018D208
		private bool fire(UseableFuel.EUseMode mode)
		{
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.vehicle != null)
				{
					if (!raycastInfo.vehicle.checkEnter(base.player))
					{
						return false;
					}
					if (mode == UseableFuel.EUseMode.Deposit)
					{
						if (this.fuel == 0)
						{
							return false;
						}
						if (!raycastInfo.vehicle.isRefillable)
						{
							return false;
						}
					}
					else
					{
						if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
						{
							return false;
						}
						if (!raycastInfo.vehicle.isSiphonable)
						{
							return false;
						}
					}
				}
				else
				{
					if (!(raycastInfo.transform != null))
					{
						return false;
					}
					InteractableGenerator component = raycastInfo.transform.GetComponent<InteractableGenerator>();
					InteractableOil component2 = raycastInfo.transform.GetComponent<InteractableOil>();
					InteractableTank component3 = raycastInfo.transform.GetComponent<InteractableTank>();
					InteractableObjectResource component4 = raycastInfo.transform.GetComponent<InteractableObjectResource>();
					if (component != null)
					{
						if (mode == UseableFuel.EUseMode.Deposit)
						{
							if (this.fuel == 0)
							{
								return false;
							}
							if (!component.isRefillable)
							{
								return false;
							}
						}
						else
						{
							if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
							{
								return false;
							}
							if (!component.isSiphonable)
							{
								return false;
							}
						}
					}
					else if (!(component2 != null))
					{
						if (component3 != null)
						{
							if (component3.source != ETankSource.FUEL)
							{
								return false;
							}
							if (mode == UseableFuel.EUseMode.Deposit)
							{
								if (this.fuel == 0)
								{
									return false;
								}
								if (!component3.isRefillable)
								{
									return false;
								}
							}
							else
							{
								if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
								{
									return false;
								}
								if (!component3.isSiphonable)
								{
									return false;
								}
							}
						}
						else
						{
							if (!(component4 != null))
							{
								return false;
							}
							if (component4.objectAsset.interactability != EObjectInteractability.FUEL)
							{
								return false;
							}
							if (mode == UseableFuel.EUseMode.Deposit)
							{
								if (this.fuel == 0)
								{
									return false;
								}
								if (component4.amount == component4.capacity)
								{
									return false;
								}
							}
							else
							{
								if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
								{
									return false;
								}
								if (component4.amount == 0)
								{
									return false;
								}
							}
						}
					}
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Fuel);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Fuel);
				if (input == null)
				{
					return false;
				}
				if ((input.point - base.player.look.aim.position).sqrMagnitude > 49f)
				{
					return false;
				}
				if (input.type == ERaycastInfoType.VEHICLE)
				{
					if (input.vehicle == null)
					{
						return false;
					}
					if (!input.vehicle.checkEnter(base.player))
					{
						return false;
					}
					if (mode == UseableFuel.EUseMode.Deposit)
					{
						if (this.fuel == 0)
						{
							return false;
						}
						if (!input.vehicle.isRefillable)
						{
							return false;
						}
						ushort num = (ushort)Mathf.Min((int)this.fuel, (int)(input.vehicle.asset.fuel - input.vehicle.fuel));
						input.vehicle.askFillFuel(num);
						this.fuel -= num;
					}
					else
					{
						if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
						{
							return false;
						}
						if (!input.vehicle.isSiphonable)
						{
							return false;
						}
						ushort desiredAmount = ((ItemFuelAsset)base.player.equipment.asset).fuel - this.fuel;
						this.fuel += VehicleManager.siphonFromVehicle(input.vehicle, base.player, desiredAmount);
					}
				}
				else if (input.type == ERaycastInfoType.BARRICADE)
				{
					if (input.transform == null || !input.transform.CompareTag("Barricade"))
					{
						return false;
					}
					InteractableGenerator component5 = input.transform.GetComponent<InteractableGenerator>();
					InteractableOil component6 = input.transform.GetComponent<InteractableOil>();
					InteractableTank component7 = input.transform.GetComponent<InteractableTank>();
					if (component5 != null)
					{
						if (mode == UseableFuel.EUseMode.Deposit)
						{
							if (this.fuel == 0)
							{
								return false;
							}
							if (!component5.isRefillable)
							{
								return false;
							}
							ushort num2 = (ushort)Mathf.Min((int)this.fuel, (int)(component5.capacity - component5.fuel));
							component5.askFill(num2);
							BarricadeManager.sendFuel(input.transform, component5.fuel);
							this.fuel -= num2;
						}
						else
						{
							if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
							{
								return false;
							}
							if (!component5.isSiphonable)
							{
								return false;
							}
							ushort num3 = (ushort)Mathf.Min((int)component5.fuel, (int)(((ItemFuelAsset)base.player.equipment.asset).fuel - this.fuel));
							component5.askBurn(num3);
							BarricadeManager.sendFuel(input.transform, component5.fuel);
							this.fuel += num3;
						}
					}
					else if (component6 != null)
					{
						if (mode == UseableFuel.EUseMode.Deposit)
						{
							if (this.fuel == 0)
							{
								return false;
							}
							if (!component6.isRefillable)
							{
								return false;
							}
							ushort num4 = (ushort)Mathf.Min((int)this.fuel, (int)(component6.capacity - component6.fuel));
							component6.askFill(num4);
							BarricadeManager.sendOil(input.transform, component6.fuel);
							this.fuel -= num4;
						}
						else
						{
							if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
							{
								return false;
							}
							if (!component6.isSiphonable)
							{
								return false;
							}
							ushort num5 = (ushort)Mathf.Min((int)component6.fuel, (int)(((ItemFuelAsset)base.player.equipment.asset).fuel - this.fuel));
							component6.askBurn(num5);
							BarricadeManager.sendOil(input.transform, component6.fuel);
							this.fuel += num5;
						}
					}
					else
					{
						if (!(component7 != null))
						{
							return false;
						}
						if (component7.source != ETankSource.FUEL)
						{
							return false;
						}
						if (mode == UseableFuel.EUseMode.Deposit)
						{
							if (this.fuel == 0)
							{
								return false;
							}
							if (!component7.isRefillable)
							{
								return false;
							}
							ushort num6 = (ushort)Mathf.Min((int)this.fuel, (int)(component7.capacity - component7.amount));
							component7.ServerSetAmount(component7.amount + num6);
							this.fuel -= num6;
						}
						else
						{
							if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
							{
								return false;
							}
							if (!component7.isSiphonable)
							{
								return false;
							}
							ushort num7 = (ushort)Mathf.Min((int)component7.amount, (int)(((ItemFuelAsset)base.player.equipment.asset).fuel - this.fuel));
							component7.ServerSetAmount(component7.amount - num7);
							this.fuel += num7;
						}
					}
				}
				else if (input.type == ERaycastInfoType.OBJECT)
				{
					if (input.transform == null)
					{
						return false;
					}
					InteractableObjectResource component8 = input.transform.GetComponent<InteractableObjectResource>();
					if (component8 == null || component8.objectAsset.interactability != EObjectInteractability.FUEL)
					{
						return false;
					}
					if (mode == UseableFuel.EUseMode.Deposit)
					{
						if (this.fuel == 0)
						{
							return false;
						}
						if (!component8.isRefillable)
						{
							return false;
						}
						ushort num8 = (ushort)Mathf.Min((int)this.fuel, (int)(component8.capacity - component8.amount));
						ObjectManager.updateObjectResource(component8.transform, component8.amount + num8, true);
						this.fuel -= num8;
					}
					else
					{
						if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
						{
							return false;
						}
						if (!component8.isSiphonable)
						{
							return false;
						}
						ushort num9 = (ushort)Mathf.Min((int)component8.amount, (int)(((ItemFuelAsset)base.player.equipment.asset).fuel - this.fuel));
						ObjectManager.updateObjectResource(component8.transform, component8.amount - num9, true);
						this.fuel += num9;
					}
				}
			}
			return true;
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x0018F874 File Offset: 0x0018DA74
		private bool start(UseableFuel.EUseMode mode)
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (this.isUseable && this.fire(mode))
			{
				if (Provider.isServer)
				{
					byte[] bytes = BitConverter.GetBytes(this.fuel);
					base.player.equipment.state[0] = bytes[0];
					base.player.equipment.state[1] = bytes[1];
					base.player.equipment.sendUpdateState();
				}
				base.player.equipment.isBusy = true;
				this.startedUse = Time.realtimeSinceStartup;
				this.isUsing = true;
				this.glug();
				if (Provider.isServer)
				{
					UseableFuel.SendPlayGlug.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
					ItemFuelAsset itemFuelAsset = base.player.equipment.asset as ItemFuelAsset;
					if (mode == UseableFuel.EUseMode.Deposit && itemFuelAsset != null && itemFuelAsset.shouldDeleteAfterFillingTarget)
					{
						this.shouldDeleteAfterUse = true;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x0018F973 File Offset: 0x0018DB73
		public override bool startPrimary()
		{
			return this.start(UseableFuel.EUseMode.Deposit);
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x0018F97C File Offset: 0x0018DB7C
		public override bool startSecondary()
		{
			return this.start(UseableFuel.EUseMode.Withdraw);
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x0018F988 File Offset: 0x0018DB88
		public override void updateState(byte[] newState)
		{
			if (base.channel.IsLocalPlayer)
			{
				this.fuel = BitConverter.ToUInt16(newState, 0);
				PlayerUI.message(EPlayerMessage.FUEL, ((int)((float)this.fuel / (float)((ItemFuelAsset)base.player.equipment.asset).fuel * 100f)).ToString(), 2f);
			}
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x0018F9F0 File Offset: 0x0018DBF0
		public override void equip()
		{
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				if (base.player.equipment.state.Length < 2)
				{
					base.player.equipment.state = ((ItemFuelAsset)base.player.equipment.asset).getState(EItemOrigin.ADMIN);
					base.player.equipment.updateState();
				}
				this.fuel = BitConverter.ToUInt16(base.player.equipment.state, 0);
			}
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
			if (base.channel.IsLocalPlayer)
			{
				PlayerUI.message(EPlayerMessage.FUEL, ((int)((float)this.fuel / (float)((ItemFuelAsset)base.player.equipment.asset).fuel * 100f)).ToString(), 2f);
			}
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x0018FAFC File Offset: 0x0018DCFC
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (Provider.isServer && this.shouldDeleteAfterUse)
				{
					base.player.equipment.use();
				}
			}
		}

		// Token: 0x04002E18 RID: 11800
		private float startedUse;

		// Token: 0x04002E19 RID: 11801
		private float useTime;

		// Token: 0x04002E1A RID: 11802
		private bool isUsing;

		// Token: 0x04002E1B RID: 11803
		private bool shouldDeleteAfterUse;

		// Token: 0x04002E1C RID: 11804
		private ushort fuel;

		// Token: 0x04002E1D RID: 11805
		private static readonly ClientInstanceMethod SendPlayGlug = ClientInstanceMethod.Get(typeof(UseableFuel), "ReceivePlayGlug");

		// Token: 0x02000A15 RID: 2581
		private enum EUseMode
		{
			/// <summary>
			/// Add fuel to target.
			/// </summary>
			// Token: 0x04003517 RID: 13591
			Deposit,
			/// <summary>
			/// Remove fuel from target.
			/// </summary>
			// Token: 0x04003518 RID: 13592
			Withdraw
		}
	}
}
