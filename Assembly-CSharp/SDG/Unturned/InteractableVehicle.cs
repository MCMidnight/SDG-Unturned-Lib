using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Water;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	// Token: 0x02000481 RID: 1153
	public class InteractableVehicle : Interactable
	{
		// Token: 0x14000086 RID: 134
		// (add) Token: 0x0600231B RID: 8987 RVA: 0x00089604 File Offset: 0x00087804
		// (remove) Token: 0x0600231C RID: 8988 RVA: 0x0008963C File Offset: 0x0008783C
		public event VehiclePassengersUpdated onPassengersUpdated;

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x0600231D RID: 8989 RVA: 0x00089674 File Offset: 0x00087874
		// (remove) Token: 0x0600231E RID: 8990 RVA: 0x000896AC File Offset: 0x000878AC
		public event VehicleLockUpdated onLockUpdated;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x0600231F RID: 8991 RVA: 0x000896E4 File Offset: 0x000878E4
		// (remove) Token: 0x06002320 RID: 8992 RVA: 0x0008971C File Offset: 0x0008791C
		public event VehicleHeadlightsUpdated onHeadlightsUpdated;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06002321 RID: 8993 RVA: 0x00089754 File Offset: 0x00087954
		// (remove) Token: 0x06002322 RID: 8994 RVA: 0x0008978C File Offset: 0x0008798C
		public event VehicleTaillightsUpdated onTaillightsUpdated;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06002323 RID: 8995 RVA: 0x000897C4 File Offset: 0x000879C4
		// (remove) Token: 0x06002324 RID: 8996 RVA: 0x000897FC File Offset: 0x000879FC
		public event VehicleSirensUpdated onSirensUpdated;

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06002325 RID: 8997 RVA: 0x00089834 File Offset: 0x00087A34
		// (remove) Token: 0x06002326 RID: 8998 RVA: 0x0008986C File Offset: 0x00087A6C
		public event VehicleBlimpUpdated onBlimpUpdated;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06002327 RID: 8999 RVA: 0x000898A4 File Offset: 0x00087AA4
		// (remove) Token: 0x06002328 RID: 9000 RVA: 0x000898DC File Offset: 0x00087ADC
		public event VehicleBatteryChangedHandler batteryChanged;

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06002329 RID: 9001 RVA: 0x00089914 File Offset: 0x00087B14
		// (remove) Token: 0x0600232A RID: 9002 RVA: 0x0008994C File Offset: 0x00087B4C
		public event VehicleSkinChangedHandler skinChanged;

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x0600232B RID: 9003 RVA: 0x00089984 File Offset: 0x00087B84
		// (remove) Token: 0x0600232C RID: 9004 RVA: 0x000899B8 File Offset: 0x00087BB8
		public static event Action<InteractableVehicle> OnHealthChanged_Global;

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x0600232D RID: 9005 RVA: 0x000899EC File Offset: 0x00087BEC
		// (remove) Token: 0x0600232E RID: 9006 RVA: 0x00089A20 File Offset: 0x00087C20
		public static event Action<InteractableVehicle> OnLockChanged_Global;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x0600232F RID: 9007 RVA: 0x00089A54 File Offset: 0x00087C54
		// (remove) Token: 0x06002330 RID: 9008 RVA: 0x00089A88 File Offset: 0x00087C88
		public static event Action<InteractableVehicle> OnFuelChanged_Global;

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06002331 RID: 9009 RVA: 0x00089ABC File Offset: 0x00087CBC
		// (remove) Token: 0x06002332 RID: 9010 RVA: 0x00089AF0 File Offset: 0x00087CF0
		public static event Action<InteractableVehicle> OnBatteryLevelChanged_Global;

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06002333 RID: 9011 RVA: 0x00089B24 File Offset: 0x00087D24
		// (remove) Token: 0x06002334 RID: 9012 RVA: 0x00089B58 File Offset: 0x00087D58
		public static event Action<InteractableVehicle, int> OnPassengerAdded_Global;

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06002335 RID: 9013 RVA: 0x00089B8C File Offset: 0x00087D8C
		// (remove) Token: 0x06002336 RID: 9014 RVA: 0x00089BC0 File Offset: 0x00087DC0
		public static event Action<InteractableVehicle, int, int> OnPassengerChangedSeats_Global;

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06002337 RID: 9015 RVA: 0x00089BF4 File Offset: 0x00087DF4
		// (remove) Token: 0x06002338 RID: 9016 RVA: 0x00089C28 File Offset: 0x00087E28
		public static event Action<InteractableVehicle, int, Player> OnPassengerRemoved_Global;

		/// <summary>
		/// Unfortunately old netcode sends train position as a Vector3 using the X channel, but new code only supports
		/// [-4096, 4096) so we pack the train position into all three channels. Eventually this should be cleaned up.
		/// </summary>
		// Token: 0x06002339 RID: 9017 RVA: 0x00089C5C File Offset: 0x00087E5C
		internal static Vector3 PackRoadPosition(float roadPosition)
		{
			if (roadPosition >= 16384f)
			{
				return new Vector3(4096f, 4096f, roadPosition - 20480f);
			}
			if (roadPosition >= 8192f)
			{
				return new Vector3(4096f, roadPosition - 12288f, -4096f);
			}
			return new Vector3(roadPosition - 4096f, -4096f, -4096f);
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x00089CBD File Offset: 0x00087EBD
		internal static float UnpackRoadPosition(Vector3 roadPosition)
		{
			return roadPosition.x + roadPosition.y + roadPosition.z + 12288f;
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600233B RID: 9019 RVA: 0x00089CD9 File Offset: 0x00087ED9
		// (set) Token: 0x0600233C RID: 9020 RVA: 0x00089CE1 File Offset: 0x00087EE1
		public Road road { get; protected set; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600233D RID: 9021 RVA: 0x00089CEA File Offset: 0x00087EEA
		// (set) Token: 0x0600233E RID: 9022 RVA: 0x00089CF2 File Offset: 0x00087EF2
		public Color32 PaintColor { get; internal set; }

		/// <summary>
		/// Is this vehicle inside a safezone?
		/// </summary>
		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600233F RID: 9023 RVA: 0x00089CFB File Offset: 0x00087EFB
		// (set) Token: 0x06002340 RID: 9024 RVA: 0x00089D03 File Offset: 0x00087F03
		public bool isInsideSafezone { get; protected set; }

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x00089D0C File Offset: 0x00087F0C
		// (set) Token: 0x06002342 RID: 9026 RVA: 0x00089D14 File Offset: 0x00087F14
		public SafezoneNode insideSafezoneNode { get; protected set; }

		/// <summary>
		/// Duration in seconds since this vehicle entered a safezone,
		/// or -1 if it's not in a safezone.
		/// </summary>
		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002343 RID: 9027 RVA: 0x00089D1D File Offset: 0x00087F1D
		// (set) Token: 0x06002344 RID: 9028 RVA: 0x00089D25 File Offset: 0x00087F25
		public float timeInsideSafezone { get; protected set; }

		/// <summary>
		/// Should askDamage requests currently be ignored because we are inside a safezone?
		/// </summary>
		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x00089D2E File Offset: 0x00087F2E
		public bool isInsideNoDamageZone
		{
			get
			{
				return this.insideSafezoneNode != null && this.insideSafezoneNode.noWeapons;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002346 RID: 9030 RVA: 0x00089D45 File Offset: 0x00087F45
		public bool usesFuel
		{
			get
			{
				return !this.asset.isStaminaPowered && !this.asset.isBatteryPowered;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x00089D64 File Offset: 0x00087F64
		public bool usesBattery
		{
			get
			{
				return !this.asset.isStaminaPowered || this.asset.isBatteryPowered;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x00089D80 File Offset: 0x00087F80
		public bool usesHealth
		{
			get
			{
				return this.asset.engine != EEngine.TRAIN;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x00089D93 File Offset: 0x00087F93
		// (set) Token: 0x0600234A RID: 9034 RVA: 0x00089D9B File Offset: 0x00087F9B
		public bool isBoosting { get; protected set; }

		/// <summary>
		/// Nelson 2024-06-24: This property is confusing, especially with isEnginePowered, but essentially represents
		/// starting the engine ignition when a player enters the driver's seat. If true, there's a driver, there was
		/// sufficient battery to start (or battery not required), and the engine wasn't underwater.
		/// </summary>
		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x0600234B RID: 9035 RVA: 0x00089DA4 File Offset: 0x00087FA4
		// (set) Token: 0x0600234C RID: 9036 RVA: 0x00089DAC File Offset: 0x00087FAC
		public bool isEngineOn { get; protected set; }

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x0600234D RID: 9037 RVA: 0x00089DB5 File Offset: 0x00087FB5
		public bool isEnginePowered
		{
			get
			{
				if (this.asset.isStaminaPowered)
				{
					return true;
				}
				if (this.asset.isBatteryPowered)
				{
					return this.HasBatteryWithCharge;
				}
				return this.fuel > 0 && this.isEngineOn;
			}
		}

		/// <summary>
		/// Doesn't imply the vehicle *uses* batteries, only that it contains a battery item with some charge left.
		/// </summary>
		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x0600234E RID: 9038 RVA: 0x00089DEB File Offset: 0x00087FEB
		public bool HasBatteryWithCharge
		{
			get
			{
				return this.batteryCharge > 1;
			}
		}

		/// <summary>
		/// Doesn't imply the vehicle *uses* batteries, only that it contains a (potentially uncharged) battery item.
		/// </summary>
		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x0600234F RID: 9039 RVA: 0x00089DF6 File Offset: 0x00087FF6
		public bool ContainsBatteryItem
		{
			get
			{
				return this.batteryCharge > 0;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002350 RID: 9040 RVA: 0x00089E01 File Offset: 0x00088001
		public bool isBatteryFull
		{
			get
			{
				return !this.usesBattery || this.batteryCharge >= 10000;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002351 RID: 9041 RVA: 0x00089E1D File Offset: 0x0008801D
		public bool canUseHorn
		{
			get
			{
				return Time.realtimeSinceStartup - this.horned > 0.5f && (!this.usesBattery || this.HasBatteryWithCharge);
			}
		}

		/// <summary>
		/// Whether the player can shoot their equipped turret.
		/// </summary>
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002352 RID: 9042 RVA: 0x00089E44 File Offset: 0x00088044
		public bool canUseTurret
		{
			get
			{
				return !this.isDead;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002353 RID: 9043 RVA: 0x00089E4F File Offset: 0x0008804F
		public bool canTurnOnLights
		{
			get
			{
				return (!this.usesBattery || this.HasBatteryWithCharge) && !this.isUnderwater;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002354 RID: 9044 RVA: 0x00089E6C File Offset: 0x0008806C
		public bool isRefillable
		{
			get
			{
				return this.usesFuel && this.fuel < this.asset.fuel && !this.isDriven && !this.isExploded;
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002355 RID: 9045 RVA: 0x00089E9C File Offset: 0x0008809C
		public bool isSiphonable
		{
			get
			{
				return this.usesFuel && this.fuel > 0 && !this.isDriven && !this.isExploded;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002356 RID: 9046 RVA: 0x00089EC2 File Offset: 0x000880C2
		public bool isRepaired
		{
			get
			{
				return this.health == this.asset.health;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002357 RID: 9047 RVA: 0x00089ED7 File Offset: 0x000880D7
		public bool isDriven
		{
			get
			{
				return this.passengers != null && this.passengers[0].player != null;
			}
		}

		/// <summary>
		/// Do any of the passenger seats have a player?
		/// </summary>
		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002358 RID: 9048 RVA: 0x00089EF4 File Offset: 0x000880F4
		public bool anySeatsOccupied
		{
			get
			{
				if (this.passengers != null)
				{
					Passenger[] passengers = this.passengers;
					for (int i = 0; i < passengers.Length; i++)
					{
						if (passengers[i].player != null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002359 RID: 9049 RVA: 0x00089F2B File Offset: 0x0008812B
		public bool isDriver
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x0600235A RID: 9050 RVA: 0x00089F30 File Offset: 0x00088130
		public bool isEmpty
		{
			get
			{
				byte b = 0;
				while ((int)b < this.passengers.Length)
				{
					if (this.passengers[(int)b].player != null)
					{
						return false;
					}
					b += 1;
				}
				return true;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x00089F63 File Offset: 0x00088163
		public bool isDrowned
		{
			get
			{
				return this._isDrowned;
			}
		}

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x0600235C RID: 9052 RVA: 0x00089F6C File Offset: 0x0008816C
		// (remove) Token: 0x0600235D RID: 9053 RVA: 0x00089FA4 File Offset: 0x000881A4
		public event Action OnIsDrownedChanged;

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x0600235E RID: 9054 RVA: 0x00089FDC File Offset: 0x000881DC
		public bool isUnderwater
		{
			get
			{
				if (this.waterCenterTransform != null)
				{
					return WaterUtility.isPointUnderwater(this.waterCenterTransform.position);
				}
				return WaterUtility.isPointUnderwater(base.transform.position + new Vector3(0f, 1f, 0f));
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x0600235F RID: 9055 RVA: 0x0008A031 File Offset: 0x00088231
		public bool isBatteryReplaceable
		{
			get
			{
				return this.usesBattery && !this.isBatteryFull && !this.isDriven && !this.isExploded;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x0008A056 File Offset: 0x00088256
		public bool isTireReplaceable
		{
			get
			{
				return !this.isDriven && !this.isExploded && this.asset.canTiresBeDamaged;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002361 RID: 9057 RVA: 0x0008A075 File Offset: 0x00088275
		public bool canBeDamaged
		{
			get
			{
				return this.asset.engine != EEngine.TRAIN;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002362 RID: 9058 RVA: 0x0008A088 File Offset: 0x00088288
		public bool isGoingToRespawn
		{
			get
			{
				return this.isExploded || this.isDrowned;
			}
		}

		/// <summary>
		/// When the server saves it doesn't include any cleared vehicles.
		/// </summary>
		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002363 RID: 9059 RVA: 0x0008A09C File Offset: 0x0008829C
		public bool isAutoClearable
		{
			get
			{
				if (this.isExploded)
				{
					return true;
				}
				if (this.isUnderwater && this.buoyancy == null)
				{
					return true;
				}
				if (this.asset == null)
				{
					return false;
				}
				if (this.asset.engine == EEngine.BOAT && this.fuel == 0 && !this.asset.isBatteryPowered)
				{
					return true;
				}
				EEngine engine = this.asset.engine;
				return false;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x0008A109 File Offset: 0x00088309
		public float lastDead
		{
			get
			{
				return this._lastDead;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x0008A111 File Offset: 0x00088311
		public float lastUnderwater
		{
			get
			{
				return this._lastUnderwater;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x0008A119 File Offset: 0x00088319
		public float lastExploded
		{
			get
			{
				return this._lastExploded;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002367 RID: 9063 RVA: 0x0008A121 File Offset: 0x00088321
		public float slip
		{
			get
			{
				return this._slip;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002368 RID: 9064 RVA: 0x0008A129 File Offset: 0x00088329
		public bool isDead
		{
			get
			{
				return this.health == 0;
			}
		}

		/// <summary>
		/// Magnitude of rigidbody velocity, replicated by current simulation owner.
		/// </summary>
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002369 RID: 9065 RVA: 0x0008A134 File Offset: 0x00088334
		// (set) Token: 0x0600236A RID: 9066 RVA: 0x0008A13C File Offset: 0x0008833C
		public float ReplicatedSpeed { get; private set; }

		/// <summary>
		/// Rigidbody velocity along forward axis, replicated by current simulation owner.
		/// </summary>
		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x0008A145 File Offset: 0x00088345
		// (set) Token: 0x0600236C RID: 9068 RVA: 0x0008A14D File Offset: 0x0008834D
		public float ReplicatedForwardVelocity { get; private set; }

		/// <summary>
		/// Replicated by current simulation owner. Target velocity used, e.g., for helicopter engine speed.
		/// </summary>
		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x0600236D RID: 9069 RVA: 0x0008A156 File Offset: 0x00088356
		// (set) Token: 0x0600236E RID: 9070 RVA: 0x0008A15E File Offset: 0x0008835E
		public float ReplicatedVelocityInput { get; private set; }

		/// <summary>
		/// [0, 1] If forward velocity is greater than zero, get normalized by target forward speed. If less than zero,
		/// get normalized by target reverse speed. Result is always positive.
		/// </summary>
		// Token: 0x0600236F RID: 9071 RVA: 0x0008A167 File Offset: 0x00088367
		public float GetReplicatedForwardSpeedPercentageOfTargetSpeed()
		{
			if (this.ReplicatedForwardVelocity > 0f)
			{
				return Mathf.Clamp01(this.ReplicatedForwardVelocity / this.asset.TargetForwardVelocity);
			}
			return Mathf.Clamp01(this.ReplicatedForwardVelocity / this.asset.TargetReverseVelocity);
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0008A1A5 File Offset: 0x000883A5
		public float GetAnimatedForwardSpeedPercentageOfTargetSpeed()
		{
			if (this.AnimatedForwardVelocity > 0f)
			{
				return Mathf.Clamp01(this.AnimatedForwardVelocity / this.asset.TargetForwardVelocity);
			}
			return Mathf.Clamp01(this.AnimatedForwardVelocity / this.asset.TargetReverseVelocity);
		}

		/// <summary>
		/// Animated toward ReplicatedForwardVelocity.
		/// </summary>
		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002371 RID: 9073 RVA: 0x0008A1E3 File Offset: 0x000883E3
		// (set) Token: 0x06002372 RID: 9074 RVA: 0x0008A1EB File Offset: 0x000883EB
		public float AnimatedForwardVelocity { get; private set; }

		/// <summary>
		/// Animated toward ReplicatedVelocityInput.
		/// </summary>
		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002373 RID: 9075 RVA: 0x0008A1F4 File Offset: 0x000883F4
		// (set) Token: 0x06002374 RID: 9076 RVA: 0x0008A1FC File Offset: 0x000883FC
		public float AnimatedVelocityInput { get; private set; }

		/// <summary>
		/// [-1.0, 1.0] Available on both client and server.
		/// </summary>
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002375 RID: 9077 RVA: 0x0008A205 File Offset: 0x00088405
		// (set) Token: 0x06002376 RID: 9078 RVA: 0x0008A20D File Offset: 0x0008840D
		public float ReplicatedSteeringInput { get; private set; }

		/// <summary>
		/// Animated towards replicated steering angle. Used for steering wheel and front steering column.
		/// </summary>
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002377 RID: 9079 RVA: 0x0008A216 File Offset: 0x00088416
		// (set) Token: 0x06002378 RID: 9080 RVA: 0x0008A21E File Offset: 0x0008841E
		public float AnimatedSteeringAngle { get; private set; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002379 RID: 9081 RVA: 0x0008A227 File Offset: 0x00088427
		// (set) Token: 0x0600237A RID: 9082 RVA: 0x0008A22F File Offset: 0x0008842F
		public TrainCar[] trainCars { get; protected set; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x0600237B RID: 9083 RVA: 0x0008A238 File Offset: 0x00088438
		public bool sirensOn
		{
			get
			{
				return this._sirensOn;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x0600237C RID: 9084 RVA: 0x0008A240 File Offset: 0x00088440
		public Transform headlights
		{
			get
			{
				return this._headlights;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x0600237D RID: 9085 RVA: 0x0008A248 File Offset: 0x00088448
		public bool headlightsOn
		{
			get
			{
				return this._headlightsOn;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x0600237E RID: 9086 RVA: 0x0008A250 File Offset: 0x00088450
		public Transform taillights
		{
			get
			{
				return this._taillights;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x0600237F RID: 9087 RVA: 0x0008A258 File Offset: 0x00088458
		public bool taillightsOn
		{
			get
			{
				return this._taillightsOn;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x0008A260 File Offset: 0x00088460
		public CSteamID lockedOwner
		{
			get
			{
				return this._lockedOwner;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x0008A268 File Offset: 0x00088468
		public CSteamID lockedGroup
		{
			get
			{
				return this._lockedGroup;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06002382 RID: 9090 RVA: 0x0008A270 File Offset: 0x00088470
		public bool isLocked
		{
			get
			{
				return this._isLocked;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002383 RID: 9091 RVA: 0x0008A278 File Offset: 0x00088478
		public bool isSkinned
		{
			get
			{
				return this.skinID > 0;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x0008A283 File Offset: 0x00088483
		public VehicleAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002385 RID: 9093 RVA: 0x0008A28B File Offset: 0x0008848B
		public Passenger[] passengers
		{
			get
			{
				return this._passengers;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x0008A293 File Offset: 0x00088493
		public Passenger[] turrets
		{
			get
			{
				return this._turrets;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002387 RID: 9095 RVA: 0x0008A29B File Offset: 0x0008849B
		public Wheel[] tires
		{
			get
			{
				return this._wheels;
			}
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x0008A2A3 File Offset: 0x000884A3
		internal Wheel GetWheelAtIndex(int index)
		{
			if (this._wheels != null && index >= 0 && index <= this._wheels.Length)
			{
				return this._wheels[index];
			}
			return null;
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002389 RID: 9097 RVA: 0x0008A2C6 File Offset: 0x000884C6
		private bool usesGravity
		{
			get
			{
				return this.asset.engine != EEngine.TRAIN;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x0600238A RID: 9098 RVA: 0x0008A2D9 File Offset: 0x000884D9
		private bool isKinematic
		{
			get
			{
				return !this.usesGravity;
			}
		}

		/// <summary>
		/// Primarily for backwards compatibility with plugins. Previously, multiple "updates" could be queued per
		/// vehicle and sent to clients. This list was public, unfortunately, so plugins may rely on submitting vehicle
		/// updates. After making it obsolete each vehicle can only be flagged as needing a replication update, and
		/// this is reset after each server replication update.
		/// </summary>
		// Token: 0x0600238B RID: 9099 RVA: 0x0008A2E4 File Offset: 0x000884E4
		public void MarkForReplicationUpdate()
		{
			this.needsReplicationUpdate = true;
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x0008A2ED File Offset: 0x000884ED
		public void ResetDecayTimer()
		{
			this.decayTimer = 0f;
			this.decayPendingDamage = 0f;
			this.decayLastUpdatePosition = base.transform.position;
		}

		/// <summary>
		/// Is player currently allowed to repair this vehicle?
		/// </summary>
		// Token: 0x0600238D RID: 9101 RVA: 0x0008A316 File Offset: 0x00088516
		public bool canPlayerRepair(Player player)
		{
			return this.asset.canRepairWhileSeated || player.movement.getVehicle() != this;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0008A338 File Offset: 0x00088538
		public void replaceBattery(Player player, byte quality, Guid newBatteryItemGuid)
		{
			if (this.ContainsBatteryItem)
			{
				this.GiveBatteryItem(player);
			}
			this.batteryItemGuid = newBatteryItemGuid;
			int num = Mathf.Clamp(Mathf.RoundToInt((float)(quality * 100)), 1, 10000);
			VehicleManager.sendVehicleBatteryCharge(this, (ushort)num);
			this.ResetDecayTimer();
		}

		/// <summary>
		/// Give battery item to player and set battery charge to zero.
		/// </summary>
		// Token: 0x0600238F RID: 9103 RVA: 0x0008A37F File Offset: 0x0008857F
		public void stealBattery(Player player)
		{
			if (this.ContainsBatteryItem)
			{
				this.GiveBatteryItem(player);
				VehicleManager.sendVehicleBatteryCharge(this, 0);
			}
		}

		/// <summary>
		/// Nelson 2024-06-24: Previously, this wouldn't give an item to the player if the quality was zero. Now it
		/// trusts the caller to validate we have a battery item to give, and respects <see cref="P:SDG.Unturned.ItemAsset.shouldDeleteAtZeroQuality" />.
		/// </summary>
		// Token: 0x06002390 RID: 9104 RVA: 0x0008A398 File Offset: 0x00088598
		private void GiveBatteryItem(Player player)
		{
			byte b = (byte)Mathf.FloorToInt((float)this.batteryCharge / 100f);
			if (this.batteryItemGuid == Guid.Empty)
			{
				this.batteryItemGuid = this.asset.defaultBatteryGuid;
			}
			ItemAsset itemAsset = Assets.find(this.batteryItemGuid) as ItemAsset;
			if (itemAsset != null)
			{
				if (itemAsset.shouldDeleteAtZeroQuality && b < 1)
				{
					return;
				}
				Item item = new Item(itemAsset.id, 1, b);
				player.inventory.forceAddItem(item, false);
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x0008A418 File Offset: 0x00088618
		// (set) Token: 0x06002392 RID: 9106 RVA: 0x0008A460 File Offset: 0x00088660
		public byte tireAliveMask
		{
			get
			{
				int num = 0;
				byte b = 0;
				while ((int)b < Mathf.Min(8, this._wheels.Length))
				{
					if (this._wheels[(int)b].isAlive)
					{
						int num2 = 1 << (int)b;
						num |= num2;
					}
					b += 1;
				}
				return (byte)num;
			}
			set
			{
				byte b = 0;
				while ((int)b < Mathf.Min(8, this._wheels.Length))
				{
					if (!(this._wheels[(int)b].wheel == null))
					{
						int num = 1 << (int)b;
						this._wheels[(int)b].isAlive = (((int)value & num) == num);
					}
					b += 1;
				}
			}
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x0008A4B8 File Offset: 0x000886B8
		public void sendTireAliveMaskUpdate()
		{
			VehicleManager.sendVehicleTireAliveMask(this, this.tireAliveMask);
		}

		/// <summary>
		/// Can a tire item be used with this vehicle?
		/// </summary>
		// Token: 0x06002394 RID: 9108 RVA: 0x0008A4C6 File Offset: 0x000886C6
		public bool isTireCompatible(ushort itemID)
		{
			return this.asset != null && this.asset.tireID == itemID;
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x0008A4E0 File Offset: 0x000886E0
		public void askRepairTire(int index)
		{
			if (index < 0 || index >= this._wheels.Length)
			{
				return;
			}
			this._wheels[index].askRepair();
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0008A4FF File Offset: 0x000886FF
		public void askDamageTire(int index)
		{
			if (this.isInsideNoDamageZone)
			{
				return;
			}
			if (index < 0 || index >= this._wheels.Length)
			{
				return;
			}
			if (this.asset != null && !this.asset.canTiresBeDamaged)
			{
				return;
			}
			this._wheels[index].askDamage();
		}

		/// <summary>
		/// Find the index of the wheel collider that contains this position.
		/// </summary>
		// Token: 0x06002397 RID: 9111 RVA: 0x0008A540 File Offset: 0x00088740
		public int getHitTireIndex(Vector3 position)
		{
			for (int i = 0; i < this._wheels.Length; i++)
			{
				WheelCollider wheel = this._wheels[i].wheel;
				if (!(wheel == null) && (wheel.transform.position - position).sqrMagnitude < wheel.radius * wheel.radius)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Find the index of the wheel collider closest to this position, or -1 if not near any.
		/// </summary>
		// Token: 0x06002398 RID: 9112 RVA: 0x0008A5A4 File Offset: 0x000887A4
		public int getClosestAliveTireIndex(Vector3 position, bool isAlive)
		{
			int result = -1;
			float num = 16f;
			for (int i = 0; i < this._wheels.Length; i++)
			{
				if (this._wheels[i].isAlive == isAlive && !(this._wheels[i].wheel == null))
				{
					float sqrMagnitude = (this._wheels[i].wheel.transform.position - position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						result = i;
						num = sqrMagnitude;
					}
				}
			}
			return result;
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x0008A624 File Offset: 0x00088824
		public void getDisplayFuel(out ushort currentFuel, out ushort maxFuel)
		{
			if (this.usesFuel)
			{
				currentFuel = this.fuel;
				maxFuel = this.asset.fuel;
				return;
			}
			if (this.passengers[0].player != null && this.passengers[0].player.player != null)
			{
				currentFuel = (ushort)this.passengers[0].player.player.life.stamina;
			}
			else if (Player.player != null)
			{
				currentFuel = (ushort)Player.player.life.stamina;
			}
			else
			{
				currentFuel = 0;
			}
			maxFuel = 100;
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0008A6BF File Offset: 0x000888BF
		public void askBurnFuel(ushort amount)
		{
			if (amount == 0 || this.isExploded)
			{
				return;
			}
			if (amount >= this.fuel)
			{
				this.fuel = 0;
				return;
			}
			this.fuel -= amount;
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0008A6F0 File Offset: 0x000888F0
		public void askFillFuel(ushort amount)
		{
			if (amount == 0 || this.isExploded)
			{
				return;
			}
			if (amount >= this.asset.fuel - this.fuel)
			{
				this.fuel = this.asset.fuel;
			}
			else
			{
				this.fuel += amount;
			}
			VehicleManager.sendVehicleFuel(this, this.fuel);
			this.ResetDecayTimer();
		}

		/// <summary>
		/// Called during simulate at fixed rate.
		/// </summary>
		// Token: 0x0600239C RID: 9116 RVA: 0x0008A754 File Offset: 0x00088954
		protected void simulateBurnFuel()
		{
			if (!this.usesFuel || !this.isEngineOn)
			{
				return;
			}
			float rate = PlayerInput.RATE;
			this.fuelBurnBuffer += rate * this.asset.fuelBurnRate;
			ushort num = (ushort)Mathf.FloorToInt(this.fuelBurnBuffer);
			if (num > 0)
			{
				this.fuelBurnBuffer -= (float)num;
				this.askBurnFuel(num);
			}
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0008A7B9 File Offset: 0x000889B9
		public void askBurnBattery(ushort amount)
		{
			if (amount == 0 || this.isExploded || this.batteryCharge < 1)
			{
				return;
			}
			if (amount >= this.batteryCharge - 1)
			{
				this.batteryCharge = 1;
				return;
			}
			this.batteryCharge -= amount;
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x0008A7F2 File Offset: 0x000889F2
		public void askChargeBattery(ushort amount)
		{
			if (amount == 0 || this.isExploded)
			{
				return;
			}
			if (amount >= 10000 - this.batteryCharge)
			{
				this.batteryCharge = 10000;
				return;
			}
			this.batteryCharge += amount;
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x0008A82A File Offset: 0x00088A2A
		public void sendBatteryChargeUpdate()
		{
			VehicleManager.sendVehicleBatteryCharge(this, this.batteryCharge);
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x0008A838 File Offset: 0x00088A38
		public void askDamage(ushort amount, bool canRepair)
		{
			if (this.isInsideNoDamageZone)
			{
				return;
			}
			if (amount == 0)
			{
				return;
			}
			if (this.isDead)
			{
				if (!canRepair)
				{
					this.explode();
				}
				return;
			}
			if (amount >= this.health)
			{
				this.health = 0;
			}
			else
			{
				this.health -= amount;
			}
			VehicleManager.sendVehicleHealth(this, this.health);
			if (this.isDead && !canRepair)
			{
				this.explode();
			}
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x0008A8A4 File Offset: 0x00088AA4
		public void askRepair(ushort amount)
		{
			if (amount == 0 || this.isExploded)
			{
				return;
			}
			if (amount >= this.asset.health - this.health)
			{
				this.health = this.asset.health;
			}
			else
			{
				this.health += amount;
			}
			VehicleManager.sendVehicleHealth(this, this.health);
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x0008A900 File Offset: 0x00088B00
		private void explode()
		{
			Vector3 force = new Vector3(Random.Range(this.asset.minExplosionForce.x, this.asset.maxExplosionForce.x), Random.Range(this.asset.minExplosionForce.y, this.asset.maxExplosionForce.y), Random.Range(this.asset.minExplosionForce.z, this.asset.maxExplosionForce.z));
			this.rootRigidbody.AddForce(force);
			this.rootRigidbody.AddTorque(16f, 0f, 0f);
			this.dropTrunkItems();
			if (this.asset.ShouldExplosionCauseDamage)
			{
				List<EPlayerKill> list;
				DamageTool.explode(base.transform.position, 8f, EDeathCause.VEHICLE, CSteamID.Nil, 200f, 200f, 200f, 0f, 0f, 500f, 2000f, 500f, out list, EExplosionDamageType.CONVENTIONAL, 32f, true, false, EDamageOrigin.Vehicle_Explosion, ERagdollEffect.NONE);
			}
			for (int i = 0; i < this.passengers.Length; i++)
			{
				Passenger passenger = this.passengers[i];
				if (passenger != null)
				{
					SteamPlayer player = passenger.player;
					if (player != null)
					{
						Player player2 = player.player;
						if (!(player2 == null) && !player2.life.isDead)
						{
							if (this.asset.ShouldExplosionCauseDamage)
							{
								EPlayerKill eplayerKill;
								player2.life.askDamage(101, Vector3.up * 101f, EDeathCause.VEHICLE, ELimb.SPINE, CSteamID.Nil, out eplayerKill);
							}
							else
							{
								VehicleManager.forceRemovePlayer(this, player.playerID.steamID);
							}
						}
					}
				}
			}
			this.DropScrapItems();
			VehicleManager.sendVehicleExploded(this);
			EffectAsset effectAsset = this.asset.FindExplosionEffectAsset();
			if (effectAsset != null)
			{
				EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
				{
					position = base.transform.position,
					relevantDistance = EffectManager.LARGE
				});
			}
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x0008AAF4 File Offset: 0x00088CF4
		public bool checkEnter(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			if (this.isHooked)
			{
				return false;
			}
			bool isServer = Provider.isServer;
			return !this.isLocked || enemyPlayer == this.lockedOwner || (this.lockedGroup != CSteamID.Nil && enemyGroup == this.lockedGroup);
		}

		/// <summary>
		/// Is a given player allowed access to this vehicle?
		/// </summary>
		// Token: 0x060023A4 RID: 9124 RVA: 0x0008AB4C File Offset: 0x00088D4C
		public bool checkEnter(Player player)
		{
			if (player == null)
			{
				return false;
			}
			CSteamID steamID = player.channel.owner.playerID.steamID;
			CSteamID groupID = player.quests.groupID;
			return this.checkEnter(steamID, groupID);
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0008AB90 File Offset: 0x00088D90
		public override bool checkUseable()
		{
			return !(Player.player == null) && (base.transform.position - Player.player.transform.position).sqrMagnitude <= 100f && !this.isExploded && this.checkEnter(Provider.client, Player.player.quests.groupID);
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0008ABFE File Offset: 0x00088DFE
		public override void use()
		{
			VehicleManager.enterVehicle(this);
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x0008AC06 File Offset: 0x00088E06
		public override bool checkHighlight(out Color color)
		{
			color = ItemTool.getRarityColorHighlight(this.asset.rarity);
			return true;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0008AC20 File Offset: 0x00088E20
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				message = EPlayerMessage.VEHICLE_ENTER;
				text = this.asset.vehicleName;
				color = ItemTool.getRarityColorUI(this.asset.rarity);
			}
			else
			{
				if (Player.player == null || (base.transform.position - Player.player.transform.position).sqrMagnitude > 100f)
				{
					message = EPlayerMessage.BLOCKED;
				}
				else
				{
					message = EPlayerMessage.LOCKED;
				}
				text = "";
				color = Color.white;
			}
			return !this.isExploded;
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x0008ACC0 File Offset: 0x00088EC0
		public void updateVehicle()
		{
			this.lastUpdatedPos = base.transform.position;
			this.interpTargetPosition = base.transform.position;
			this.interpTargetRotation = base.transform.rotation;
			this.real = base.transform.position;
			this.isRecovering = false;
			this.lastRecover = Time.realtimeSinceStartup;
			this.isFrozen = false;
		}

		/// <summary>
		/// Average vehicle-space position of wheel bases.
		/// </summary>
		// Token: 0x060023AA RID: 9130 RVA: 0x0008AD2C File Offset: 0x00088F2C
		private Vector3? calculateAverageLocalTireContactPosition()
		{
			if (this._wheels == null)
			{
				return default(Vector3?);
			}
			Vector3 a = Vector3.zero;
			int num = 0;
			Wheel[] wheels = this._wheels;
			for (int i = 0; i < wheels.Length; i++)
			{
				WheelCollider wheel = wheels[i].wheel;
				if (!(wheel == null))
				{
					Vector3 position = wheel.transform.TransformPoint(wheel.center - new Vector3(0f, wheel.radius, 0f));
					Vector3 b = base.transform.InverseTransformPoint(position);
					a += b;
					num++;
				}
			}
			if (num > 0)
			{
				return new Vector3?(a / (float)num);
			}
			return default(Vector3?);
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x0008ADE8 File Offset: 0x00088FE8
		public void updatePhysics()
		{
			if (this.checkDriver(Provider.client) || (Provider.isServer && !this.isDriven))
			{
				this.rootRigidbody.useGravity = this.usesGravity;
				this.rootRigidbody.isKinematic = this.isKinematic;
				this.isPhysical = true;
				if (!this.isExploded)
				{
					if (this._wheels != null)
					{
						Wheel[] wheels = this._wheels;
						for (int i = 0; i < wheels.Length; i++)
						{
							wheels[i].isPhysical = true;
						}
					}
					if (this.buoyancy != null)
					{
						this.buoyancy.gameObject.SetActive(true);
					}
				}
			}
			else
			{
				this.rootRigidbody.useGravity = false;
				this.rootRigidbody.isKinematic = true;
				this.isPhysical = false;
				if (this._wheels != null)
				{
					Wheel[] wheels = this._wheels;
					for (int i = 0; i < wheels.Length; i++)
					{
						wheels[i].isPhysical = false;
					}
				}
				if (this.buoyancy != null)
				{
					this.buoyancy.gameObject.SetActive(false);
				}
			}
			if (!this.hasDefaultCenterOfMass)
			{
				this.hasDefaultCenterOfMass = true;
				this.defaultCenterOfMass = this.rootRigidbody.centerOfMass;
			}
			Vector3 centerOfMass;
			if (this.asset.hasCenterOfMassOverride)
			{
				centerOfMass = this.asset.centerOfMass;
			}
			else
			{
				Transform transform = base.transform.Find("Cog");
				if (transform)
				{
					centerOfMass = transform.localPosition;
				}
				else
				{
					centerOfMass = new Vector3(0f, -0.25f, 0f);
					if (this.asset.engine == EEngine.CAR)
					{
						Vector3? vector = this.calculateAverageLocalTireContactPosition();
						if (vector != null)
						{
							centerOfMass = vector.Value;
						}
					}
				}
			}
			this.rootRigidbody.centerOfMass = centerOfMass;
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x0008AF98 File Offset: 0x00089198
		public void updateEngine()
		{
			this.synchronizeTaillights();
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x0008AFAB File Offset: 0x000891AB
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceivePaintColor(Color32 newPaintColor)
		{
			this.PaintColor = newPaintColor;
			this.ApplyPaintColor();
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x0008AFBC File Offset: 0x000891BC
		public void ServerSetPaintColor(Color32 newPaintColor)
		{
			if (!this.PaintColor.Equals(newPaintColor))
			{
				InteractableVehicle.SendPaintColor.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), newPaintColor);
			}
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x0008AFFC File Offset: 0x000891FC
		public void tellLocked(CSteamID owner, CSteamID group, bool locked)
		{
			this._lockedOwner = owner;
			this._lockedGroup = group;
			this._isLocked = locked;
			VehicleLockUpdated vehicleLockUpdated = this.onLockUpdated;
			if (vehicleLockUpdated != null)
			{
				vehicleLockUpdated();
			}
			if (this.eventHook != null)
			{
				if (locked)
				{
					this.eventHook.OnLocked.TryInvoke(this);
				}
				else
				{
					this.eventHook.OnUnlocked.TryInvoke(this);
				}
			}
			InteractableVehicle.OnLockChanged_Global.TryInvoke("OnLockChanged_Global", this);
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x0008B074 File Offset: 0x00089274
		public void tellSkin(ushort newSkinID, ushort newMythicID)
		{
			this.skinID = newSkinID;
			this.mythicID = newMythicID;
			this.updateSkin();
			VehicleSkinChangedHandler vehicleSkinChangedHandler = this.skinChanged;
			if (vehicleSkinChangedHandler == null)
			{
				return;
			}
			vehicleSkinChangedHandler();
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x0008B09C File Offset: 0x0008929C
		public void updateSkin()
		{
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x0008B0AC File Offset: 0x000892AC
		public void tellSirens(bool on)
		{
			this._sirensOn = on;
			VehicleSirensUpdated vehicleSirensUpdated = this.onSirensUpdated;
			if (vehicleSirensUpdated == null)
			{
				return;
			}
			vehicleSirensUpdated();
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x0008B0D0 File Offset: 0x000892D0
		public void tellBlimp(bool on)
		{
			this.isBlimpFloating = on;
			if (this.asset.engine != EEngine.BLIMP)
			{
				return;
			}
			int childCount = this.buoyancy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				this.buoyancy.GetChild(i).GetComponent<Buoyancy>().enabled = this.isBlimpFloating;
			}
			VehicleBlimpUpdated vehicleBlimpUpdated = this.onBlimpUpdated;
			if (vehicleBlimpUpdated == null)
			{
				return;
			}
			vehicleBlimpUpdated();
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x0008B137 File Offset: 0x00089337
		public void tellHeadlights(bool on)
		{
			this._headlightsOn = on;
			VehicleHeadlightsUpdated vehicleHeadlightsUpdated = this.onHeadlightsUpdated;
			if (vehicleHeadlightsUpdated == null)
			{
				return;
			}
			vehicleHeadlightsUpdated();
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x0008B150 File Offset: 0x00089350
		public void tellTaillights(bool on)
		{
			this._taillightsOn = on;
			VehicleTaillightsUpdated vehicleTaillightsUpdated = this.onTaillightsUpdated;
			if (vehicleTaillightsUpdated == null)
			{
				return;
			}
			vehicleTaillightsUpdated();
		}

		/// <summary>
		/// Turn taillights on/off depending on state.
		/// </summary>
		// Token: 0x060023B6 RID: 9142 RVA: 0x0008B174 File Offset: 0x00089374
		private void synchronizeTaillights()
		{
			bool flag = this.isDriven && this.canTurnOnLights;
			if (this.taillightsOn != flag)
			{
				this.tellTaillights(flag);
			}
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x0008B1A3 File Offset: 0x000893A3
		public void tellHorn()
		{
			this.horned = Time.realtimeSinceStartup;
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 32f);
			}
			VehicleEventHook vehicleEventHook = this.eventHook;
			if (vehicleEventHook == null)
			{
				return;
			}
			vehicleEventHook.OnHornUsed.TryInvoke(this);
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x0008B1E2 File Offset: 0x000893E2
		public void tellFuel(ushort newFuel)
		{
			this.fuel = newFuel;
			InteractableVehicle.OnFuelChanged_Global.TryInvoke("OnFuelChanged_Global", this);
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x0008B1FB File Offset: 0x000893FB
		public void tellBatteryCharge(ushort newBatteryCharge)
		{
			this.batteryCharge = newBatteryCharge;
			if (!this.HasBatteryWithCharge)
			{
				this.isEngineOn = false;
			}
			VehicleBatteryChangedHandler vehicleBatteryChangedHandler = this.batteryChanged;
			if (vehicleBatteryChangedHandler != null)
			{
				vehicleBatteryChangedHandler();
			}
			InteractableVehicle.OnBatteryLevelChanged_Global.TryInvoke("OnBatteryLevelChanged_Global", this);
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x0008B234 File Offset: 0x00089434
		public void tellExploded()
		{
			this.clearHooked();
			this.isExploded = true;
			this._lastExploded = Time.realtimeSinceStartup;
			if (this.sirensOn)
			{
				this.tellSirens(false);
			}
			if (this.isBlimpFloating)
			{
				this.tellBlimp(false);
			}
			if (this.headlightsOn)
			{
				this.tellHeadlights(false);
			}
			if (this._wheels != null)
			{
				Wheel[] wheels = this._wheels;
				for (int i = 0; i < wheels.Length; i++)
				{
					wheels[i].isPhysical = false;
				}
			}
			if (this.buoyancy != null)
			{
				this.buoyancy.gameObject.SetActive(false);
			}
			if (this.eventHook != null)
			{
				this.eventHook.OnExploded.TryInvoke(this);
			}
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0008B2EA File Offset: 0x000894EA
		public void updateFires()
		{
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x0008B2EC File Offset: 0x000894EC
		public void tellHealth(ushort newHealth)
		{
			this.health = newHealth;
			if (this.isDead)
			{
				this._lastDead = Time.realtimeSinceStartup;
			}
			this.updateFires();
			InteractableVehicle.OnHealthChanged_Global.TryInvoke("OnHealthChanged_Global", this);
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x0008B320 File Offset: 0x00089520
		public void tellRecov(Vector3 newPosition, int newRecov)
		{
			this.lastTick = Time.realtimeSinceStartup;
			this.rootRigidbody.MovePosition(newPosition);
			this.isFrozen = true;
			this.rootRigidbody.useGravity = false;
			this.rootRigidbody.isKinematic = true;
			if (this.passengers[0] != null && this.passengers[0].player != null && this.passengers[0].player.player != null && this.passengers[0].player.player.input != null)
			{
				this.passengers[0].player.player.input.recov = newRecov;
			}
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x0008B3D4 File Offset: 0x000895D4
		public void tellState(Vector3 newPosition, Quaternion newRotation, float newSpeed, float newForwardVelocity, float newReplicatedSteeringInput, float newReplicatedVelocityInput)
		{
			if (this.isDriver)
			{
				return;
			}
			this.lastTick = Time.realtimeSinceStartup;
			this.lastUpdatedPos = newPosition;
			this.interpTargetPosition = newPosition;
			this.interpTargetRotation = newRotation;
			if (this.asset.engine == EEngine.TRAIN)
			{
				this.roadPosition = InteractableVehicle.UnpackRoadPosition(newPosition);
			}
			this.ReplicatedSpeed = newSpeed;
			this.ReplicatedForwardVelocity = newForwardVelocity;
			this.ReplicatedSteeringInput = newReplicatedSteeringInput;
			this.ReplicatedVelocityInput = newReplicatedVelocityInput;
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x0008B443 File Offset: 0x00089643
		public bool checkDriver(CSteamID steamID)
		{
			return this.isDriven && this.passengers[0].player.playerID.steamID == steamID;
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x0008B46C File Offset: 0x0008966C
		public void grantTrunkAccess(Player player)
		{
			if (Provider.isServer && this.trunkItems != null && this.trunkItems.height > 0)
			{
				player.inventory.openTrunk(this.trunkItems);
			}
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x0008B49C File Offset: 0x0008969C
		public void revokeTrunkAccess(Player player)
		{
			if (Provider.isServer)
			{
				player.inventory.closeTrunk();
			}
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x0008B4B0 File Offset: 0x000896B0
		public void dropTrunkItems()
		{
			if (Provider.isServer && this.trunkItems != null)
			{
				for (byte b = 0; b < this.trunkItems.getItemCount(); b += 1)
				{
					ItemManager.dropItem(this.trunkItems.getItem(b).item, base.transform.position, false, true, true);
				}
				this.trunkItems.clear();
				this.trunkItems = null;
				if (this.passengers[0].player != null && this.passengers[0].player.player != null)
				{
					this.revokeTrunkAccess(this.passengers[0].player.player);
				}
			}
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x0008B560 File Offset: 0x00089760
		private void DropScrapItems()
		{
			if (!this.hasDroppedScrapItemsAlready && this.asset.dropsTableId > 0)
			{
				this.hasDroppedScrapItemsAlready = true;
				int num = Random.Range((int)this.asset.dropsMin, (int)this.asset.dropsMax);
				num = Mathf.Clamp(num, 0, 100);
				for (int i = 0; i < num; i++)
				{
					float f = Random.Range(0f, 6.2831855f);
					ushort num2 = SpawnTableTool.ResolveLegacyId(this.asset.dropsTableId, EAssetType.ITEM, new Func<string>(this.OnGetDropsSpawnTableErrorContext));
					if (num2 != 0)
					{
						ItemManager.dropItem(new Item(num2, EItemOrigin.NATURE), base.transform.position + new Vector3(Mathf.Sin(f) * 3f, 1f, Mathf.Cos(f) * 3f), false, true, true);
					}
				}
			}
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x0008B634 File Offset: 0x00089834
		private string OnGetDropsSpawnTableErrorContext()
		{
			VehicleAsset asset = this.asset;
			return ((asset != null) ? asset.FriendlyName : null) + " explosion drops";
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x0008B654 File Offset: 0x00089854
		public void addPlayer(byte seat, CSteamID steamID)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
			if (steamPlayer != null)
			{
				this.passengers[(int)seat].player = steamPlayer;
				if (steamPlayer.player != null)
				{
					steamPlayer.player.movement.setVehicle(this, seat, this.passengers[(int)seat].seat, Vector3.zero, 0, false);
					if (this.passengers[(int)seat].turret != null)
					{
						steamPlayer.player.equipment.turretEquipClient();
						if (Provider.isServer)
						{
							steamPlayer.player.equipment.turretEquipServer(this.passengers[(int)seat].turret.itemID, this.passengers[(int)seat].state);
						}
					}
				}
				if (this.passengers[(int)seat].collider != null)
				{
					this.passengers[(int)seat].collider.enabled = true;
				}
				this.updatePhysics();
				if (seat == 0)
				{
					this.grantTrunkAccess(steamPlayer.player);
				}
			}
			if (seat == 0)
			{
				this.isEngineOn = ((!this.usesBattery || this.HasBatteryWithCharge) && !this.isUnderwater);
			}
			this.updateEngine();
			if (seat == 0)
			{
				bool isEnginePowered = this.isEnginePowered;
			}
			VehiclePassengersUpdated vehiclePassengersUpdated = this.onPassengersUpdated;
			if (vehiclePassengersUpdated != null)
			{
				vehiclePassengersUpdated();
			}
			bool flag = false;
			if (this.eventHook != null)
			{
				if (seat == 0)
				{
					this.eventHook.OnDriverAdded.TryInvoke(this);
					if (flag)
					{
						this.eventHook.OnLocalDriverAdded.TryInvoke(this);
					}
				}
				if (flag)
				{
					this.eventHook.OnLocalPassengerAdded.TryInvoke(this);
				}
			}
			if (this.passengers[(int)seat].turretEventHook != null)
			{
				this.passengers[(int)seat].turretEventHook.OnPassengerAdded.TryInvoke(this);
				if (flag)
				{
					this.passengers[(int)seat].turretEventHook.OnLocalPassengerAdded.TryInvoke(this);
				}
			}
			InteractableVehicle.OnPassengerAdded_Global.TryInvoke("OnPassengerAdded_Global", this, (int)seat);
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x0008B830 File Offset: 0x00089A30
		public void removePlayer(byte seatIndex, Vector3 point, byte angle, bool forceUpdate)
		{
			SteamPlayer steamPlayer = null;
			if (this.passengers != null && (int)seatIndex < this.passengers.Length)
			{
				Passenger passenger = this.passengers[(int)seatIndex];
				steamPlayer = passenger.player;
				if (steamPlayer != null && steamPlayer.player != null)
				{
					if (passenger.turret != null)
					{
						steamPlayer.player.equipment.turretDequipClient();
						if (Provider.isServer)
						{
							steamPlayer.player.equipment.turretDequipServer();
						}
					}
					steamPlayer.player.movement.setVehicle(null, 0, null, point, angle, forceUpdate);
				}
				if (this.passengers[(int)seatIndex].collider != null)
				{
					this.passengers[(int)seatIndex].collider.enabled = false;
				}
				passenger.player = null;
				this.updatePhysics();
				if (Provider.isServer)
				{
					VehicleManager.sendVehicleFuel(this, this.fuel);
					VehicleManager.sendVehicleBatteryCharge(this, this.batteryCharge);
				}
				if (seatIndex == 0 && steamPlayer != null && steamPlayer.player != null)
				{
					this.revokeTrunkAccess(steamPlayer.player);
				}
			}
			if (seatIndex == 0)
			{
				this.isEngineOn = false;
			}
			this.updateEngine();
			if (seatIndex == 0)
			{
				this.inputTargetVelocity = 0f;
				this.inputEngineVelocity = 0f;
				this.ReplicatedSteeringInput = 0f;
				if (this._wheels != null)
				{
					Wheel[] wheels = this._wheels;
					for (int i = 0; i < wheels.Length; i++)
					{
						wheels[i].Reset();
					}
				}
			}
			VehiclePassengersUpdated vehiclePassengersUpdated = this.onPassengersUpdated;
			if (vehiclePassengersUpdated != null)
			{
				vehiclePassengersUpdated();
			}
			bool flag = false;
			if (this.passengers[(int)seatIndex].turretEventHook != null)
			{
				if (flag)
				{
					this.passengers[(int)seatIndex].turretEventHook.OnLocalPassengerRemoved.TryInvoke(this);
				}
				this.passengers[(int)seatIndex].turretEventHook.OnPassengerRemoved.TryInvoke(this);
			}
			if (this.eventHook != null)
			{
				if (flag)
				{
					this.eventHook.OnLocalPassengerRemoved.TryInvoke(this);
				}
				if (seatIndex == 0)
				{
					if (flag)
					{
						this.eventHook.OnLocalDriverRemoved.TryInvoke(this);
					}
					this.eventHook.OnDriverRemoved.TryInvoke(this);
				}
			}
			InteractableVehicle.OnPassengerRemoved_Global.TryInvoke("OnPassengerRemoved_Global", this, (int)seatIndex, (steamPlayer != null) ? steamPlayer.player : null);
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x0008BA54 File Offset: 0x00089C54
		public void swapPlayer(byte fromSeatIndex, byte toSeatIndex)
		{
			if (this.passengers != null && (int)fromSeatIndex < this.passengers.Length && (int)toSeatIndex < this.passengers.Length)
			{
				Passenger passenger = this.passengers[(int)fromSeatIndex];
				Passenger passenger2 = this.passengers[(int)toSeatIndex];
				SteamPlayer player = passenger.player;
				if (player != null && player.player != null)
				{
					if (passenger.turret != null)
					{
						player.player.equipment.turretDequipClient();
						if (Provider.isServer)
						{
							player.player.equipment.turretDequipServer();
						}
					}
					player.player.movement.setVehicle(this, toSeatIndex, this.passengers[(int)toSeatIndex].seat, Vector3.zero, 0, false);
					if (passenger2.turret != null)
					{
						player.player.equipment.turretEquipClient();
						if (Provider.isServer)
						{
							player.player.equipment.turretEquipServer(this.passengers[(int)toSeatIndex].turret.itemID, this.passengers[(int)toSeatIndex].state);
						}
					}
				}
				if (passenger.collider != null)
				{
					passenger.collider.enabled = false;
				}
				if (passenger2.collider != null)
				{
					passenger2.collider.enabled = true;
				}
				passenger.player = null;
				passenger2.player = player;
				this.updatePhysics();
				if (Provider.isServer)
				{
					VehicleManager.sendVehicleFuel(this, this.fuel);
					VehicleManager.sendVehicleBatteryCharge(this, this.batteryCharge);
				}
				if (fromSeatIndex == 0 && player != null && player.player != null)
				{
					this.revokeTrunkAccess(player.player);
				}
				if (toSeatIndex == 0 && player != null && player.player != null)
				{
					this.grantTrunkAccess(player.player);
				}
			}
			if (toSeatIndex == 0)
			{
				this.isEngineOn = ((!this.usesBattery || this.HasBatteryWithCharge) && !this.isUnderwater);
			}
			if (fromSeatIndex == 0)
			{
				this.isEngineOn = false;
			}
			this.updateEngine();
			if (fromSeatIndex == 0)
			{
				this.inputTargetVelocity = 0f;
				this.inputEngineVelocity = 0f;
				this.ReplicatedSteeringInput = 0f;
				if (this._wheels != null)
				{
					Wheel[] wheels = this._wheels;
					for (int i = 0; i < wheels.Length; i++)
					{
						wheels[i].Reset();
					}
				}
			}
			VehiclePassengersUpdated vehiclePassengersUpdated = this.onPassengersUpdated;
			if (vehiclePassengersUpdated != null)
			{
				vehiclePassengersUpdated();
			}
			bool flag = false;
			if (this.passengers[(int)fromSeatIndex].turretEventHook != null)
			{
				if (flag)
				{
					this.passengers[(int)fromSeatIndex].turretEventHook.OnLocalPassengerRemoved.TryInvoke(this);
				}
				this.passengers[(int)fromSeatIndex].turretEventHook.OnPassengerRemoved.TryInvoke(this);
			}
			if (this.passengers[(int)toSeatIndex].turretEventHook != null)
			{
				this.passengers[(int)toSeatIndex].turretEventHook.OnPassengerAdded.TryInvoke(this);
				if (flag)
				{
					this.passengers[(int)toSeatIndex].turretEventHook.OnLocalPassengerAdded.TryInvoke(this);
				}
			}
			if (this.eventHook != null)
			{
				if (fromSeatIndex == 0)
				{
					if (flag)
					{
						this.eventHook.OnLocalDriverRemoved.TryInvoke(this);
					}
					this.eventHook.OnDriverRemoved.TryInvoke(this);
				}
				if (toSeatIndex == 0)
				{
					this.eventHook.OnDriverAdded.TryInvoke(this);
					if (flag)
					{
						this.eventHook.OnLocalDriverAdded.TryInvoke(this);
					}
				}
			}
			InteractableVehicle.OnPassengerChangedSeats_Global.TryInvoke("OnPassengerChangedSeats_Global", this, (int)fromSeatIndex, (int)toSeatIndex);
		}

		/// <summary>
		/// VehicleManager expects this to only find the seat, not add the player,
		/// because it does a LoS check.
		/// </summary>
		// Token: 0x060023C8 RID: 9160 RVA: 0x0008BDA0 File Offset: 0x00089FA0
		public bool tryAddPlayer(out byte seat, Player player)
		{
			seat = byte.MaxValue;
			if (player == null)
			{
				return false;
			}
			if (this.isExploded)
			{
				return false;
			}
			if (!this.isExitable)
			{
				return false;
			}
			byte b = 0;
			while ((int)b < this.passengers.Length)
			{
				if (this.passengers[(int)b] != null && this.passengers[(int)b].player == player.channel.owner)
				{
					return false;
				}
				b += 1;
			}
			bool flag = player.animator.gesture == EPlayerGesture.ARREST_START;
			byte b2 = flag ? 1 : 0;
			while ((int)b2 < this.passengers.Length)
			{
				if (this.passengers[(int)b2] != null && this.passengers[(int)b2].player == null && (!flag || this.passengers[(int)b2].turret == null))
				{
					seat = b2;
					return true;
				}
				b2 += 1;
			}
			return false;
		}

		/// <summary>
		/// Call on the server to empty the vehicle of passengers.
		/// </summary>
		// Token: 0x060023C9 RID: 9161 RVA: 0x0008BE6C File Offset: 0x0008A06C
		public void forceRemoveAllPlayers()
		{
			for (int i = 0; i < this.passengers.Length; i++)
			{
				Passenger passenger = this.passengers[i];
				if (passenger != null)
				{
					SteamPlayer player = passenger.player;
					if (player != null)
					{
						Player player2 = player.player;
						if (!(player2 == null) && !player2.life.isDead)
						{
							VehicleManager.forceRemovePlayer(this, player.playerID.steamID);
						}
					}
				}
			}
		}

		/// <summary>
		/// Kicks them out even if there isn't a good spot. Used when killing the occupant.
		/// </summary>
		/// <returns>True if player is seated, false otherwise.</returns>
		// Token: 0x060023CA RID: 9162 RVA: 0x0008BED0 File Offset: 0x0008A0D0
		public bool forceRemovePlayer(out byte seat, CSteamID player, out Vector3 point, out byte angle)
		{
			seat = byte.MaxValue;
			point = Vector3.zero;
			angle = 0;
			if (this.findPlayerSeat(player, out seat))
			{
				Passenger passenger = this.passengers[(int)seat];
				Player player2;
				if (passenger == null)
				{
					player2 = null;
				}
				else
				{
					SteamPlayer player3 = passenger.player;
					player2 = ((player3 != null) ? player3.player : null);
				}
				this.forceGetExit(player2, seat, out point, out angle);
				return true;
			}
			return false;
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0008BF2C File Offset: 0x0008A12C
		public bool findPlayerSeat(CSteamID player, out byte seat)
		{
			seat = byte.MaxValue;
			byte b = 0;
			while ((int)b < this.passengers.Length)
			{
				if (this.passengers[(int)b] != null && this.passengers[(int)b].player != null && this.passengers[(int)b].player.playerID.steamID == player)
				{
					seat = b;
					return true;
				}
				b += 1;
			}
			return false;
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x0008BF92 File Offset: 0x0008A192
		public bool findPlayerSeat(Player player, out byte seat)
		{
			return this.findPlayerSeat(player.channel.owner.playerID.steamID, out seat);
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x0008BFB0 File Offset: 0x0008A1B0
		public bool trySwapPlayer(Player player, byte toSeat, out byte fromSeat)
		{
			fromSeat = byte.MaxValue;
			if ((int)toSeat >= this.passengers.Length)
			{
				return false;
			}
			if (player.animator.gesture == EPlayerGesture.ARREST_START)
			{
				if (toSeat < 1)
				{
					return false;
				}
				if (this.passengers[(int)toSeat].turret != null)
				{
					return false;
				}
			}
			byte b = 0;
			while ((int)b < this.passengers.Length)
			{
				if (this.passengers[(int)b] != null && this.passengers[(int)b].player != null && this.passengers[(int)b].player.player == player)
				{
					if (toSeat != b)
					{
						fromSeat = b;
						return this.passengers[(int)toSeat].player == null;
					}
					return false;
				}
				else
				{
					b += 1;
				}
			}
			return false;
		}

		/// <summary>
		/// Can a safe exit point currently be found?
		///
		/// Called when considering to add a new passenger to prevent players from entering
		/// a vehicle that they wouldn't be able to exit properly.
		/// </summary>
		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060023CE RID: 9166 RVA: 0x0008C05C File Offset: 0x0008A25C
		public bool isExitable
		{
			get
			{
				Vector3 vector;
				byte b;
				return this.tryGetExit(0, out vector, out b);
			}
		}

		/// <summary>
		/// Could a player capsule fit in a given exit position?
		/// </summary>
		// Token: 0x060023CF RID: 9167 RVA: 0x0008C074 File Offset: 0x0008A274
		protected bool isExitPositionEmpty(Vector3 position)
		{
			return PlayerStance.hasTeleportClearanceAtPosition(position);
		}

		/// <returns>True if anything was hit.</returns>
		// Token: 0x060023D0 RID: 9168 RVA: 0x0008C07C File Offset: 0x0008A27C
		protected bool raycastIgnoringVehicleAndChildren(Vector3 origin, Vector3 direction, float maxDistance, out float hitDistance)
		{
			hitDistance = maxDistance;
			bool result = false;
			RaycastHit[] array = Physics.RaycastAll(new Ray(origin, direction), maxDistance, RayMasks.BLOCK_EXIT);
			if (array != null && array.Length != 0)
			{
				foreach (RaycastHit raycastHit in array)
				{
					if (raycastHit.transform != null && !raycastHit.transform.IsChildOf(base.transform))
					{
						hitDistance = Mathf.Min(hitDistance, raycastHit.distance);
						result = true;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Raycast along a given direction, penetrating through barricades attached to THIS vehicle.
		/// Returns point at the end of the ray if unblocked, or a safe (radius) distance away from hit.
		/// </summary>
		// Token: 0x060023D1 RID: 9169 RVA: 0x0008C0FC File Offset: 0x0008A2FC
		protected Vector3 getExitDistanceInDirection(Vector3 origin, Vector3 direction, float maxDistance, float extraPadding = 0.1f)
		{
			float num;
			this.raycastIgnoringVehicleAndChildren(origin, direction, maxDistance, out num);
			float num2 = PlayerStance.RADIUS + extraPadding;
			return origin + direction * (num - num2);
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x0008C130 File Offset: 0x0008A330
		protected void findGroundForExitPosition(ref Vector3 exitPosition)
		{
			RaycastHit raycastHit;
			Physics.Raycast(new Ray(exitPosition, Vector3.down), out raycastHit, 3f, RayMasks.BLOCK_EXIT_FIND_GROUND);
			if (raycastHit.transform != null)
			{
				exitPosition = raycastHit.point + new Vector3(0f, 0.25f, 0f);
			}
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x0008C194 File Offset: 0x0008A394
		protected bool getSafeExitInDirection(Vector3 origin, Vector3 direction, float maxDistance, out Vector3 exitPosition)
		{
			exitPosition = this.getExitDistanceInDirection(origin, direction, maxDistance, 0.1f);
			this.findGroundForExitPosition(ref exitPosition);
			return this.isExitPositionEmpty(exitPosition);
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x0008C1C0 File Offset: 0x0008A3C0
		protected bool getExitSidePoint(Vector3 direction, out Vector3 exitPosition)
		{
			float num = PlayerStance.RADIUS + 0.1f;
			float maxDistance = this.asset.exit + Mathf.Abs(this.ReplicatedSpeed) * 0.1f + num;
			Vector3 position = this.center.position;
			return this.getSafeExitInDirection(position, direction, maxDistance, out exitPosition);
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x0008C210 File Offset: 0x0008A410
		protected bool getExitUpwardPoint(out Vector3 exitPosition)
		{
			Vector3 position = this.center.position;
			Vector3 up = this.center.up;
			exitPosition = this.getExitDistanceInDirection(position, up, 6f, PlayerMovement.HEIGHT_STAND);
			this.findGroundForExitPosition(ref exitPosition);
			if (this.isExitPositionEmpty(exitPosition))
			{
				return true;
			}
			exitPosition = this.getExitDistanceInDirection(position, Vector3.up, 6f, PlayerMovement.HEIGHT_STAND);
			this.findGroundForExitPosition(ref exitPosition);
			return this.isExitPositionEmpty(exitPosition);
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x0008C294 File Offset: 0x0008A494
		protected bool getExitDownwardPoint(out Vector3 exitPosition)
		{
			Vector3 position = this.center.position;
			Vector3 direction = -this.center.up;
			return this.getSafeExitInDirection(position, direction, 6f, out exitPosition) || this.getSafeExitInDirection(position, Vector3.down, 6f, out exitPosition);
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x0008C2E4 File Offset: 0x0008A4E4
		protected bool getExitForwardPoint(Vector3 direction, out Vector3 exitPosition)
		{
			float maxDistance = 3f + this.asset.exit * 2f;
			Vector3 position = this.center.position;
			return this.getSafeExitInDirection(position, direction, maxDistance, out exitPosition);
		}

		/// <summary>
		/// Fallback if there are absolutely no good exit points.
		/// Sets point and angle with a normal player spawnpoint.
		///
		/// Once vehicle is completely surrounded there is no nice way to pick an exit point. Finding
		/// a point upwards is abused to teleport upward into bases, finding an empty capsule nearby is
		/// abused to teleport through walls, so if we're sure there isn't a nice exit point we can
		/// fallback to teleporting them to a safe spawnpoint.
		/// </summary>
		// Token: 0x060023D8 RID: 9176 RVA: 0x0008C320 File Offset: 0x0008A520
		protected void getExitSpawnPoint(Player player, ref Vector3 point, ref byte angle)
		{
			PlayerSpawnpoint spawn = LevelPlayers.getSpawn(Level.info != null && Level.info.type == ELevelType.ARENA && LevelManager.isPlayerInArena(player));
			if (spawn != null)
			{
				point = spawn.point;
				angle = MeasurementTool.angleToByte((float)angle);
				return;
			}
			point = new Vector3(0f, 256f, 0f);
			angle = 0;
		}

		/// <returns>True if we can safely exit.</returns>
		// Token: 0x060023D9 RID: 9177 RVA: 0x0008C388 File Offset: 0x0008A588
		internal bool tryGetExit(byte seat, out Vector3 point, out byte angle)
		{
			point = this.center.position;
			angle = MeasurementTool.angleToByte(this.center.rotation.eulerAngles.y);
			Vector3 vector = (seat % 2 == 0) ? (-this.center.right) : this.center.right;
			if (this.getExitSidePoint(vector, out point))
			{
				return true;
			}
			vector = -vector;
			return this.getExitSidePoint(vector, out point) || this.getExitUpwardPoint(out point) || this.getExitDownwardPoint(out point) || this.getExitForwardPoint(-this.center.forward, out point) || this.getExitForwardPoint(this.center.forward, out point);
		}

		/// <summary>
		/// Initially use tryGetExit to find a safe exit, but if one isn't available then fallback to getExitSpawnPoint.
		/// </summary>
		// Token: 0x060023DA RID: 9178 RVA: 0x0008C44F File Offset: 0x0008A64F
		protected void forceGetExit(Player player, byte seat, out Vector3 point, out byte angle)
		{
			if (!this.tryGetExit(seat, out point, out angle))
			{
				this.getExitSpawnPoint(player, ref point, ref angle);
			}
		}

		/// <summary>
		/// Dedicated server simulate driving input.
		/// </summary>
		// Token: 0x060023DB RID: 9179 RVA: 0x0008C468 File Offset: 0x0008A668
		public void simulate(uint simulation, int recov, bool inputStamina, Vector3 point, Quaternion angle, float newSpeed, float newForwardVelocity, float newSteeringInput, float newVelocityInput, float delta)
		{
			if (this.asset.useStaminaBoost)
			{
				bool flag = this.passengers[0].player != null && this.passengers[0].player.player != null && this.passengers[0].player.player.life.stamina > 0;
				if (inputStamina && flag)
				{
					this.isBoosting = true;
				}
				else
				{
					this.isBoosting = false;
				}
			}
			else
			{
				this.isBoosting = false;
			}
			if (this.isRecovering)
			{
				if (recov < this.passengers[0].player.player.input.recov)
				{
					if (Time.realtimeSinceStartup - this.lastRecover > 5f)
					{
						this.lastRecover = Time.realtimeSinceStartup;
						VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
					}
					return;
				}
				this.isRecovering = false;
				this.isFrozen = false;
			}
			if (Dedicator.serverVisibility != ESteamServerVisibility.LAN && !PlayerMovement.forceTrustClient)
			{
				if (this.asset.engine == EEngine.CAR)
				{
					if (MathfEx.HorizontalDistanceSquared(point, this.real) > ((this.usesFuel && this.fuel == 0) ? 0.5f : this.asset.sqrDelta))
					{
						this.isRecovering = true;
						this.lastRecover = Time.realtimeSinceStartup;
						this.passengers[0].player.player.input.recov++;
						VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
						return;
					}
				}
				else if (this.asset.engine == EEngine.BOAT)
				{
					if (MathfEx.HorizontalDistanceSquared(point, this.real) > (WaterUtility.isPointUnderwater(point + new Vector3(0f, -4f, 0f)) ? this.asset.sqrDelta : 0.5f))
					{
						this.isRecovering = true;
						this.lastRecover = Time.realtimeSinceStartup;
						this.passengers[0].player.player.input.recov++;
						VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
						return;
					}
				}
				else if (this.asset.engine != EEngine.TRAIN && MathfEx.HorizontalDistanceSquared(point, this.real) > this.asset.sqrDelta)
				{
					this.isRecovering = true;
					this.lastRecover = Time.realtimeSinceStartup;
					this.passengers[0].player.player.input.recov++;
					VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
					return;
				}
				if (this.asset.engine != EEngine.TRAIN)
				{
					float num = (point.y > this.real.y) ? this.asset.validSpeedUp : this.asset.validSpeedDown;
					if (Mathf.Abs(point.y - this.real.y) / delta > num)
					{
						this.isRecovering = true;
						this.lastRecover = Time.realtimeSinceStartup;
						this.passengers[0].player.player.input.recov++;
						VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
						return;
					}
				}
			}
			if (this.asset.engine != EEngine.TRAIN)
			{
				UndergroundAllowlist.AdjustPosition(ref point, 10f, 2f);
			}
			this.simulateBurnFuel();
			bool flag2 = !MathfEx.IsNearlyEqual(this.ReplicatedSteeringInput, newSteeringInput, 0.5f);
			this.ReplicatedSpeed = newSpeed;
			this.ReplicatedForwardVelocity = newForwardVelocity;
			this.ReplicatedSteeringInput = newSteeringInput;
			this.ReplicatedVelocityInput = newVelocityInput;
			this.real = point;
			if (this.asset.engine == EEngine.TRAIN)
			{
				this.roadPosition = this.clampRoadPosition(InteractableVehicle.UnpackRoadPosition(point));
				this.teleportTrain();
			}
			else
			{
				this.rootRigidbody.MovePosition(point);
				this.rootRigidbody.MoveRotation(angle);
			}
			if (flag2 || Mathf.Abs(this.lastUpdatedPos.x - this.real.x) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.y - this.real.y) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.z - this.real.z) > Provider.UPDATE_DISTANCE)
			{
				this.lastUpdatedPos = this.real;
				this.MarkForReplicationUpdate();
			}
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x0008C93C File Offset: 0x0008AB3C
		public void clearHooked()
		{
			foreach (HookInfo hookInfo in this.hooked)
			{
				if (!(hookInfo.vehicle == null))
				{
					hookInfo.vehicle.isHooked = false;
					this.ignoreCollisionWithVehicle(hookInfo.vehicle, false);
				}
			}
			this.hooked.Clear();
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x0008C9BC File Offset: 0x0008ABBC
		public void useHook()
		{
			if (this.hooked.Count > 0)
			{
				this.clearHooked();
				return;
			}
			int num = Physics.OverlapSphereNonAlloc(this.hook.position, 3f, InteractableVehicle.tempCollidersArray, 67108864);
			for (int i = 0; i < num; i++)
			{
				InteractableVehicle vehicle = DamageTool.getVehicle(InteractableVehicle.tempCollidersArray[i].transform);
				if (!(vehicle == null) && !(vehicle == this) && vehicle.isEmpty && !vehicle.isHooked && !vehicle.isExploded && vehicle.asset.engine != EEngine.TRAIN)
				{
					HookInfo hookInfo = new HookInfo();
					hookInfo.target = vehicle.transform;
					hookInfo.vehicle = vehicle;
					hookInfo.deltaPosition = this.hook.InverseTransformPoint(vehicle.transform.position);
					hookInfo.deltaRotation = Quaternion.FromToRotation(this.hook.forward, vehicle.transform.forward);
					this.hooked.Add(hookInfo);
					vehicle.isHooked = true;
					this.ignoreCollisionWithVehicle(vehicle, true);
				}
			}
		}

		/// <summary>
		/// -1 is reverse.
		/// 0 is neutral.
		/// +1 is index 0 in gear ratios list.
		/// </summary>
		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x0008CAD7 File Offset: 0x0008ACD7
		// (set) Token: 0x060023DF RID: 9183 RVA: 0x0008CADF File Offset: 0x0008ACDF
		public int GearNumber { get; internal set; }

		/// <summary>
		/// Engine RPM replicated by current simulation owner.
		/// </summary>
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060023E0 RID: 9184 RVA: 0x0008CAE8 File Offset: 0x0008ACE8
		// (set) Token: 0x060023E1 RID: 9185 RVA: 0x0008CAF0 File Offset: 0x0008ACF0
		public float ReplicatedEngineRpm { get; internal set; }

		/// <summary>
		/// Animated toward ReplicatedEngineRpm.
		/// </summary>
		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x0008CAF9 File Offset: 0x0008ACF9
		// (set) Token: 0x060023E3 RID: 9187 RVA: 0x0008CB01 File Offset: 0x0008AD01
		public float AnimatedEngineRpm { get; private set; }

		/// <summary>
		/// Called when engine RPM exceeds threshold and there are more gears available.
		/// Purpose is to skip gear numbers that don't bring engine RPM within threshold (if possible).
		/// </summary>
		// Token: 0x060023E4 RID: 9188 RVA: 0x0008CB0C File Offset: 0x0008AD0C
		private int GetShiftUpGearNumber(float averagePoweredWheelRpm)
		{
			for (int i = this.GearNumber; i < this.asset.forwardGearRatios.Length; i++)
			{
				float num = averagePoweredWheelRpm * this.asset.forwardGearRatios[i];
				if (num > this.asset.GearShiftDownThresholdRpm && num < this.asset.GearShiftUpThresholdRpm)
				{
					return i + 1;
				}
			}
			return this.GearNumber + 1;
		}

		/// <summary>
		/// Called when engine RPM is below threshold and there are more lower gears available.
		/// Purpose is to skip gear numbers that don't bring engine RPM within threshold (if possible).
		/// </summary>
		// Token: 0x060023E5 RID: 9189 RVA: 0x0008CB70 File Offset: 0x0008AD70
		private int GetShiftDownGearNumber(float averagePoweredWheelRpm)
		{
			for (int i = this.GearNumber - 2; i >= 0; i--)
			{
				float num = averagePoweredWheelRpm * this.asset.forwardGearRatios[i];
				if (num > this.asset.GearShiftDownThresholdRpm && num < this.asset.GearShiftUpThresholdRpm)
				{
					return i + 1;
				}
			}
			return this.GearNumber - 1;
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x0008CBC8 File Offset: 0x0008ADC8
		private void ChangeGears(int newGearNumber)
		{
			if (this.GearNumber == newGearNumber)
			{
				return;
			}
			this.timeSinceLastGearChange = 0f;
			this.GearNumber = newGearNumber;
		}

		/// <summary>
		/// Client simulate driving input.
		/// </summary>
		// Token: 0x060023E7 RID: 9191 RVA: 0x0008CBE8 File Offset: 0x0008ADE8
		public void simulate(uint simulation, int recov, int input_x, int input_y, float look_x, float look_y, bool inputBrake, bool inputStamina, float delta)
		{
			if (Provider.isServer && this.asset.engine != EEngine.TRAIN)
			{
				Vector3 position = base.transform.position;
				if (UndergroundAllowlist.AdjustPosition(ref position, 10f, 2f))
				{
					this.rootRigidbody.MovePosition(position);
				}
			}
			this.latestGasInput = (float)input_y;
			float num = (float)input_y;
			float num2 = 1f;
			if (this.asset.useStaminaBoost)
			{
				bool flag = this.passengers[0].player != null && this.passengers[0].player.player != null && this.passengers[0].player.player.life.stamina > 0;
				if (inputStamina && flag)
				{
					this.isBoosting = true;
				}
				else
				{
					this.isBoosting = false;
					num *= this.asset.staminaBoost;
					num2 *= this.asset.staminaBoost;
				}
			}
			else
			{
				this.isBoosting = false;
			}
			if (this.isFrozen)
			{
				this.isFrozen = false;
				this.rootRigidbody.useGravity = this.usesGravity;
				this.rootRigidbody.isKinematic = this.isKinematic;
				return;
			}
			if ((this.usesFuel && this.fuel == 0) || this.isUnderwater || this.isDead || !this.isEnginePowered)
			{
				num = 0f;
				num2 = 1f;
			}
			bool flag2 = false;
			if (this.asset.steeringLeaningForceMultiplier > 0f)
			{
				this.rootRigidbody.AddRelativeTorque(0f, 0f, (float)input_x * -this.asset.steeringLeaningForceMultiplier * PlayerInput.SAMPLES);
			}
			if (this._wheels != null)
			{
				foreach (Wheel wheel in this._wheels)
				{
					wheel.ClientSimulate((float)input_x, num, inputBrake, delta);
					flag2 |= wheel.isGrounded;
				}
				if (flag2 && this.asset.wheelBalancingForceMultiplier > 0f)
				{
					this.ApplyWheelBalancingForce(Time.fixedDeltaTime * PlayerInput.SAMPLES);
				}
			}
			if (this.asset.rollAngularVelocityDamping > 0f)
			{
				this.ApplyAngularVelocityDamping(Time.fixedDeltaTime * PlayerInput.SAMPLES);
			}
			switch (this.asset.engine)
			{
			case EEngine.CAR:
			{
				float replicatedForwardSpeedPercentageOfTargetSpeed = this.GetReplicatedForwardSpeedPercentageOfTargetSpeed();
				if (flag2)
				{
					this.rootRigidbody.AddForce(-base.transform.up * replicatedForwardSpeedPercentageOfTargetSpeed * 40f);
				}
				if (this.buoyancy != null)
				{
					float num3 = Mathf.Lerp(this.asset.steerMax, this.asset.steerMin, replicatedForwardSpeedPercentageOfTargetSpeed);
					bool flag3 = WaterUtility.isPointUnderwater(base.transform.position + new Vector3(0f, -1f, 0f));
					this.boatTraction = Mathf.Lerp(this.boatTraction, (float)(flag3 ? 1 : 0), 4f * Time.deltaTime);
					if (!MathfEx.IsNearlyZero(this.boatTraction, 0.01f))
					{
						if (num > 0f)
						{
							this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, this.asset.TargetForwardVelocity, delta / 4f);
						}
						else if (num < 0f)
						{
							this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, this.asset.TargetReverseVelocity, delta / 4f);
						}
						else
						{
							this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, 0f, delta / 8f);
						}
						this.inputEngineVelocity = this.inputTargetVelocity * this.boatTraction;
						Vector3 forward = base.transform.forward;
						forward.y = 0f;
						this.rootRigidbody.AddForce(forward.normalized * this.inputEngineVelocity * 2f * this.boatTraction);
						this.rootRigidbody.AddRelativeTorque((float)input_y * -2.5f * this.boatTraction, (float)input_x * num3 / 8f * this.boatTraction, (float)input_x * -2.5f * this.boatTraction);
					}
				}
				break;
			}
			case EEngine.PLANE:
			{
				float replicatedForwardSpeedPercentageOfTargetSpeed2 = this.GetReplicatedForwardSpeedPercentageOfTargetSpeed();
				float num4 = Mathf.Lerp(this.asset.airSteerMax, this.asset.airSteerMin, replicatedForwardSpeedPercentageOfTargetSpeed2);
				if (num > 0f)
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, this.asset.TargetForwardVelocity * num2, delta);
				}
				else if (num < 0f)
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, 0f, delta / 8f);
				}
				else
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, 0f, delta / 16f);
				}
				this.inputEngineVelocity = this.inputTargetVelocity;
				this.rootRigidbody.AddForce(base.transform.forward * this.inputEngineVelocity * 2f * this.asset.engineForceMultiplier);
				this.rootRigidbody.AddForce(Mathf.Lerp(0f, 1f, base.transform.InverseTransformDirection(this.rootRigidbody.velocity).z / this.asset.TargetForwardVelocity) * this.asset.lift * -Physics.gravity);
				if (this._wheels == null || this._wheels.Length == 0 || (!this._wheels[0].isGrounded && !this._wheels[1].isGrounded))
				{
					this.rootRigidbody.AddRelativeTorque(Mathf.Clamp(look_y, -this.asset.airTurnResponsiveness, this.asset.airTurnResponsiveness) * num4, (float)input_x * this.asset.airTurnResponsiveness * num4 / 4f, Mathf.Clamp(look_x, -this.asset.airTurnResponsiveness, this.asset.airTurnResponsiveness) * -num4 / 2f);
				}
				if ((this._wheels == null || this._wheels.Length == 0) && num < 0f)
				{
					this.rootRigidbody.AddForce(base.transform.forward * this.asset.TargetReverseVelocity * 4f * this.asset.engineForceMultiplier);
				}
				break;
			}
			case EEngine.HELICOPTER:
			{
				float replicatedForwardSpeedPercentageOfTargetSpeed3 = this.GetReplicatedForwardSpeedPercentageOfTargetSpeed();
				float num5 = Mathf.Lerp(this.asset.steerMax, this.asset.steerMin, replicatedForwardSpeedPercentageOfTargetSpeed3);
				if (num > 0f)
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, this.asset.TargetForwardVelocity * num2, delta / 4f);
				}
				else if (num < 0f)
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, 0f, delta / 8f);
				}
				else
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, 0f, delta / 16f);
				}
				this.inputEngineVelocity = this.inputTargetVelocity;
				this.rootRigidbody.AddForce(base.transform.up * this.inputEngineVelocity * 3f);
				this.rootRigidbody.AddRelativeTorque(Mathf.Clamp(look_y, -2f, 2f) * num5, (float)input_x * num5 / 2f, Mathf.Clamp(look_x, -2f, 2f) * -num5 / 4f);
				break;
			}
			case EEngine.BLIMP:
			{
				float replicatedForwardSpeedPercentageOfTargetSpeed4 = this.GetReplicatedForwardSpeedPercentageOfTargetSpeed();
				float num6 = Mathf.Lerp(this.asset.steerMax, this.asset.steerMin, replicatedForwardSpeedPercentageOfTargetSpeed4);
				if (num > 0f)
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, this.asset.TargetForwardVelocity * num2, delta / 4f);
				}
				else if (num < 0f)
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, this.asset.TargetReverseVelocity * num2, delta / 4f);
				}
				else
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, 0f, delta / 8f);
				}
				this.inputEngineVelocity = this.inputTargetVelocity;
				this.rootRigidbody.AddForce(base.transform.forward * this.inputEngineVelocity * 2f);
				if (!this.isBlimpFloating)
				{
					this.rootRigidbody.AddForce(-Physics.gravity * 0.5f);
				}
				this.rootRigidbody.AddRelativeTorque(Mathf.Clamp(look_y, -this.asset.airTurnResponsiveness, this.asset.airTurnResponsiveness) * num6 / 4f, (float)input_x * this.asset.airTurnResponsiveness * num6 * 2f, Mathf.Clamp(look_x, -this.asset.airTurnResponsiveness, this.asset.airTurnResponsiveness) * -num6 / 4f);
				break;
			}
			case EEngine.BOAT:
			{
				float replicatedForwardSpeedPercentageOfTargetSpeed5 = this.GetReplicatedForwardSpeedPercentageOfTargetSpeed();
				float num7 = Mathf.Lerp(this.asset.steerMax, this.asset.steerMin, replicatedForwardSpeedPercentageOfTargetSpeed5);
				this.boatTraction = Mathf.Lerp(this.boatTraction, (float)(WaterUtility.isPointUnderwater(base.transform.position + new Vector3(0f, -1f, 0f)) ? 1 : 0), 4f * Time.deltaTime);
				if (num > 0f)
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, this.asset.TargetForwardVelocity * num2, delta / 4f);
				}
				else if (num < 0f)
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, this.asset.TargetReverseVelocity * num2, delta / 4f);
				}
				else
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, 0f, delta / 8f);
				}
				this.inputEngineVelocity = this.inputTargetVelocity * this.boatTraction;
				Vector3 forward2 = base.transform.forward;
				forward2.y = 0f;
				this.rootRigidbody.AddForce(forward2.normalized * this.inputEngineVelocity * 4f * this.boatTraction);
				if (this._wheels == null || this._wheels.Length == 0 || (!this._wheels[0].isGrounded && !this._wheels[1].isGrounded))
				{
					this.rootRigidbody.AddRelativeTorque(num * -10f * this.boatTraction, (float)input_x * num7 / 2f * this.boatTraction, (float)input_x * -5f * this.boatTraction);
				}
				break;
			}
			case EEngine.TRAIN:
				if (num > 0f)
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, this.asset.TargetForwardVelocity * num2, delta / 8f);
				}
				else if (num < 0f)
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, this.asset.TargetReverseVelocity * num2, delta / 8f);
				}
				else
				{
					this.inputTargetVelocity = Mathf.Lerp(this.inputTargetVelocity, 0f, delta / 8f);
				}
				this.inputEngineVelocity = this.inputTargetVelocity;
				break;
			}
			if (this.asset.engine == EEngine.TRAIN)
			{
				this.ReplicatedSpeed = Mathf.Abs(this.inputEngineVelocity);
				this.ReplicatedForwardVelocity = this.inputEngineVelocity;
				this.ReplicatedVelocityInput = this.inputEngineVelocity;
			}
			else
			{
				Vector3 velocity = this.rootRigidbody.velocity;
				this.ReplicatedSpeed = velocity.magnitude;
				Vector3 vector = base.transform.InverseTransformDirection(velocity);
				if (this.asset.engine == EEngine.HELICOPTER)
				{
					this.ReplicatedForwardVelocity = vector.y;
				}
				else
				{
					this.ReplicatedForwardVelocity = vector.z;
				}
				this.ReplicatedVelocityInput = this.inputEngineVelocity;
			}
			this.ReplicatedSteeringInput = (float)input_x;
			this.simulateBurnFuel();
			this.lastUpdatedPos = base.transform.position;
			this.interpTargetPosition = base.transform.position;
			this.interpTargetRotation = base.transform.rotation;
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x0008D824 File Offset: 0x0008BA24
		private void moveTrain(Vector3 frontPosition, Vector3 frontNormal, Vector3 frontDirection, Vector3 backPosition, Vector3 backNormal, Vector3 backDirection, TrainCar car)
		{
			Vector3 a = (frontPosition + backPosition) / 2f;
			Vector3 vector = Vector3.Lerp(backNormal, frontNormal, 0.5f);
			Vector3 normalized = (frontPosition - backPosition).normalized;
			Quaternion rotation = Quaternion.LookRotation(frontDirection, frontNormal);
			Quaternion rotation2 = Quaternion.LookRotation(backDirection, backNormal);
			Quaternion quaternion = Quaternion.LookRotation(normalized, vector);
			if (car.rootRigidbody != null)
			{
				car.rootRigidbody.MovePosition(a + vector * this.asset.trainTrackOffset);
				car.rootRigidbody.MoveRotation(quaternion);
			}
			if (car.root != null)
			{
				car.root.position = a + vector * this.asset.trainTrackOffset;
				car.root.rotation = quaternion;
			}
			if (car.trackFront != null)
			{
				car.trackFront.position = a + normalized * this.asset.trainWheelOffset;
				car.trackFront.rotation = rotation;
			}
			if (car.trackBack != null)
			{
				car.trackBack.position = a - normalized * this.asset.trainWheelOffset;
				car.trackBack.rotation = rotation2;
			}
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x0008D980 File Offset: 0x0008BB80
		private void teleportTrain()
		{
			foreach (TrainCar trainCar in this.trainCars)
			{
				Vector3 frontPosition;
				Vector3 frontNormal;
				Vector3 frontDirection;
				this.road.getTrackData(this.clampRoadPosition(this.roadPosition + trainCar.trackPositionOffset + this.asset.trainWheelOffset), out frontPosition, out frontNormal, out frontDirection);
				Vector3 backPosition;
				Vector3 backNormal;
				Vector3 backDirection;
				this.road.getTrackData(this.clampRoadPosition(this.roadPosition + trainCar.trackPositionOffset - this.asset.trainWheelOffset), out backPosition, out backNormal, out backDirection);
				this.moveTrain(frontPosition, frontNormal, frontDirection, backPosition, backNormal, backDirection, trainCar);
			}
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x0008DA18 File Offset: 0x0008BC18
		private TrainCar getTrainCar(Transform root)
		{
			Transform transform = root.Find("Objects");
			Transform trackFront = (transform != null) ? transform.Find("Track_Front") : null;
			Transform transform2 = root.Find("Objects");
			Transform trackBack = (transform2 != null) ? transform2.Find("Track_Back") : null;
			return new TrainCar
			{
				root = root,
				trackFront = trackFront,
				trackBack = trackBack,
				rootRigidbody = root.GetComponent<Rigidbody>()
			};
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x0008DA88 File Offset: 0x0008BC88
		private float clampRoadPosition(float newRoadPosition)
		{
			if (!this.road.isLoop)
			{
				return Mathf.Clamp(newRoadPosition, 0.5f + this.asset.trainWheelOffset, this.road.trackSampledLength - (float)(this.trainCars.Length - 1) * this.asset.trainCarLength - this.asset.trainWheelOffset - 0.5f);
			}
			if (newRoadPosition < 0f)
			{
				return this.road.trackSampledLength + newRoadPosition;
			}
			if (newRoadPosition > this.road.trackSampledLength)
			{
				return newRoadPosition - this.road.trackSampledLength;
			}
			return newRoadPosition;
		}

		/// <summary>
		/// 2020-11-26 experimented with dispatching all vehicle updates from C# in VehicleManager because they make up
		/// a significant portion of the MonoBehaviour Update, but the savings on my PC with 24 vehicles on PEI was
		/// minor. Not worth the potential troubles.
		/// </summary>
		// Token: 0x060023EC RID: 9196 RVA: 0x0008DB24 File Offset: 0x0008BD24
		private void Update()
		{
			if (this.asset == null)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			if (Provider.isServer && this.hooked != null)
			{
				for (int i = 0; i < this.hooked.Count; i++)
				{
					HookInfo hookInfo = this.hooked[i];
					if (hookInfo != null && !(hookInfo.target == null))
					{
						hookInfo.target.position = this.hook.TransformPoint(hookInfo.deltaPosition);
						hookInfo.target.rotation = this.hook.rotation * hookInfo.deltaRotation;
					}
				}
			}
			if (this.isPhysical && !this.needsReplicationUpdate && (Mathf.Abs(this.lastUpdatedPos.x - base.transform.position.x) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.y - base.transform.position.y) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.z - base.transform.position.z) > Provider.UPDATE_DISTANCE))
			{
				this.lastUpdatedPos = base.transform.position;
				this.MarkForReplicationUpdate();
			}
			if (!Provider.isServer && !this.isPhysical && this.asset.engine != EEngine.TRAIN)
			{
				Vector3 a;
				Quaternion a2;
				base.transform.GetPositionAndRotation(out a, out a2);
				float t = 1f - Mathf.Pow(2f, -13f * Time.deltaTime);
				Vector3 position = Vector3.Lerp(a, this.interpTargetPosition, t);
				Quaternion rot = Quaternion.Slerp(a2, this.interpTargetRotation, t);
				this.rootRigidbody.MovePosition(position);
				this.rootRigidbody.MoveRotation(rot);
			}
			if (Provider.isServer && this.isPhysical && this.asset.engine != EEngine.TRAIN && !this.isDriven)
			{
				Vector3 position2 = base.transform.position;
				if (UndergroundAllowlist.AdjustPosition(ref position2, 10f, 2f))
				{
					this.rootRigidbody.MovePosition(position2);
				}
			}
			if (this.headlightsOn && !this.canTurnOnLights)
			{
				this.tellHeadlights(false);
			}
			if (this.sirensOn && !this.canTurnOnLights)
			{
				this.tellSirens(false);
			}
			if (this.isUnderwater)
			{
				if (!this.isDrowned)
				{
					this._lastUnderwater = Time.realtimeSinceStartup;
					this._isDrowned = true;
					Action onIsDrownedChanged = this.OnIsDrownedChanged;
					if (onIsDrownedChanged != null)
					{
						onIsDrownedChanged.Invoke();
					}
					this.tellSirens(false);
					this.tellBlimp(false);
					this.tellHeadlights(false);
					this.updateFires();
				}
			}
			else if (this._isDrowned)
			{
				this._isDrowned = false;
				Action onIsDrownedChanged2 = this.OnIsDrownedChanged;
				if (onIsDrownedChanged2 != null)
				{
					onIsDrownedChanged2.Invoke();
				}
				this.updateFires();
			}
			this.synchronizeTaillights();
			if (this.isDriver)
			{
				if (!this.asset.hasTraction)
				{
					bool flag = LevelLighting.isPositionSnowy(base.transform.position);
					if (!flag && Level.info != null && Level.info.configData.Use_Snow_Volumes)
					{
						AmbianceVolume firstOverlappingVolume = VolumeManager<AmbianceVolume, AmbianceVolumeManager>.Get().GetFirstOverlappingVolume(base.transform.position);
						if (firstOverlappingVolume != null)
						{
							flag = ((firstOverlappingVolume.weatherMask & 2U) > 0U);
						}
					}
					flag &= (LevelLighting.snowyness == ELightingSnow.BLIZZARD);
					this._slip = Mathf.Lerp(this._slip, (float)(flag ? 1 : 0), Time.deltaTime * 0.05f);
				}
				else
				{
					this._slip = 0f;
				}
				if (this._wheels != null)
				{
					float num = 0f;
					int num2 = 0;
					if (this.asset.poweredWheelIndices != null)
					{
						float num3 = 0f;
						foreach (int index in this.asset.poweredWheelIndices)
						{
							Wheel wheelAtIndex = this.GetWheelAtIndex(index);
							if (wheelAtIndex != null && !(wheelAtIndex.wheel == null))
							{
								num3 += Mathf.Abs(wheelAtIndex.wheel.rpm);
								num2++;
							}
						}
						if (num2 > 0)
						{
							num = num3 / (float)num2;
						}
					}
					float num4 = num;
					if (this.asset.UsesEngineRpmAndGears)
					{
						this.timeSinceLastGearChange += deltaTime;
						if (this.timeSinceLastGearChange > this.asset.GearShiftInterval)
						{
							if (this.latestGasInput < -0.01f && this.ReplicatedForwardVelocity < 0.05f)
							{
								this.ChangeGears(-1);
							}
							else if (this.GearNumber < 1 && this.ReplicatedForwardVelocity > -0.05f)
							{
								this.ChangeGears(1);
							}
							else if (this.ReplicatedEngineRpm > this.asset.GearShiftUpThresholdRpm && this.GearNumber > 0 && this.GearNumber < this.asset.forwardGearRatios.Length)
							{
								this.ChangeGears(this.GetShiftUpGearNumber(num));
							}
							else if (this.ReplicatedEngineRpm < this.asset.GearShiftDownThresholdRpm && this.GearNumber > 1)
							{
								this.ChangeGears(this.GetShiftDownGearNumber(num));
							}
						}
						if (this.GearNumber == -1)
						{
							num4 *= this.asset.reverseGearRatio;
						}
						else if (this.GearNumber >= 1 && this.GearNumber <= this.asset.forwardGearRatios.Length)
						{
							num4 *= this.asset.forwardGearRatios[this.GearNumber - 1];
						}
						num4 = Mathf.Max(num4, this.asset.EngineIdleRpm);
					}
					if (num4 > this.ReplicatedEngineRpm)
					{
						this.ReplicatedEngineRpm = Mathf.MoveTowards(this.ReplicatedEngineRpm, num4, this.asset.EngineRpmIncreaseRate * deltaTime);
					}
					else if (num4 < this.ReplicatedEngineRpm)
					{
						this.ReplicatedEngineRpm = Mathf.MoveTowards(this.ReplicatedEngineRpm, num4, this.asset.EngineRpmDecreaseRate * deltaTime);
					}
					this.ReplicatedEngineRpm = Mathf.Clamp(this.ReplicatedEngineRpm, this.asset.EngineIdleRpm, this.asset.EngineMaxRpm);
					float num5 = Mathf.InverseLerp(this.asset.EngineIdleRpm, this.asset.EngineMaxRpm, this.ReplicatedEngineRpm);
					float num6 = ((this.engineCurvesComponent != null) ? this.engineCurvesComponent.engineRpmToTorqueCurve.Evaluate(num5) : Mathf.Lerp(0.5f, 1f, num5)) * this.asset.EngineMaxTorque * Mathf.Abs(this.latestGasInput);
					if (this.timeSinceLastGearChange < this.asset.GearShiftDuration)
					{
						num6 = 0f;
					}
					if (this.GearNumber == -1)
					{
						num6 *= this.asset.reverseGearRatio;
					}
					else if (this.asset.UsesEngineRpmAndGears && this.GearNumber >= 1 && this.GearNumber <= this.asset.forwardGearRatios.Length)
					{
						num6 *= this.asset.forwardGearRatios[this.GearNumber - 1];
					}
					if (this.asset.poweredWheelIndices != null && this.asset.poweredWheelIndices.Length != 0)
					{
						num6 /= (float)this.asset.poweredWheelIndices.Length;
					}
					foreach (Wheel wheel in this._wheels)
					{
						if (wheel == null)
						{
							break;
						}
						wheel.UpdateLocallyDriven(deltaTime, num6);
					}
				}
				if (this.asset.engine == EEngine.TRAIN && this.road != null)
				{
					foreach (TrainCar trainCar in this.trainCars)
					{
						Vector3 frontPosition;
						Vector3 frontNormal;
						Vector3 frontDirection;
						this.road.getTrackData(this.clampRoadPosition(this.roadPosition + trainCar.trackPositionOffset + this.asset.trainWheelOffset), out frontPosition, out frontNormal, out frontDirection);
						Vector3 backPosition;
						Vector3 backNormal;
						Vector3 backDirection;
						this.road.getTrackData(this.clampRoadPosition(this.roadPosition + trainCar.trackPositionOffset - this.asset.trainWheelOffset), out backPosition, out backNormal, out backDirection);
						this.moveTrain(frontPosition, frontNormal, frontDirection, backPosition, backNormal, backDirection, trainCar);
					}
					float num7 = this.inputEngineVelocity * deltaTime;
					Transform transform;
					if (this.inputEngineVelocity > 0f)
					{
						transform = this.overlapFront;
					}
					else
					{
						transform = this.overlapBack;
					}
					BoxCollider boxCollider = (transform != null) ? transform.GetComponent<BoxCollider>() : null;
					bool flag2;
					if (boxCollider != null)
					{
						flag2 = false;
						Vector3 vector = transform.position + transform.forward * num7 / 2f;
						Vector3 size = boxCollider.size;
						size.z = num7;
						int num8 = Physics.OverlapBoxNonAlloc(vector, size / 2f, InteractableVehicle.tempCollidersArray, transform.rotation, RayMasks.BLOCK_TRAIN, QueryTriggerInteraction.Ignore);
						for (int k = 0; k < num8; k++)
						{
							bool flag3 = false;
							for (int l = 0; l < this.trainCars.Length; l++)
							{
								if (InteractableVehicle.tempCollidersArray[k].transform.IsChildOf(this.trainCars[l].root) || InteractableVehicle.tempCollidersArray[k].transform == this.trainCars[l].root)
								{
									flag3 = true;
									break;
								}
							}
							if (!flag3)
							{
								if (InteractableVehicle.tempCollidersArray[k].CompareTag("Vehicle"))
								{
									Rigidbody component = InteractableVehicle.tempCollidersArray[k].GetComponent<Rigidbody>();
									if (!component.isKinematic)
									{
										component.AddForce(base.transform.forward * this.inputEngineVelocity, ForceMode.VelocityChange);
									}
								}
								flag2 = true;
								break;
							}
						}
					}
					else
					{
						flag2 = true;
					}
					if (flag2)
					{
						if (this.inputEngineVelocity > 0f)
						{
							if (this.inputTargetVelocity > 0f)
							{
								this.inputTargetVelocity = 0f;
							}
						}
						else if (this.inputTargetVelocity < 0f)
						{
							this.inputTargetVelocity = 0f;
						}
					}
					else
					{
						this.roadPosition += num7;
						this.roadPosition = this.clampRoadPosition(this.roadPosition);
					}
				}
			}
			if (Provider.isServer)
			{
				if (this.isDriven)
				{
					if (this.asset != null && this.asset.canTiresBeDamaged && this._wheels != null)
					{
						foreach (Wheel wheel2 in this._wheels)
						{
							if (!(wheel2.wheel == null) && !wheel2.IsDead)
							{
								wheel2.CheckForTraps();
							}
						}
					}
				}
				else
				{
					this.ReplicatedSpeed = this.rootRigidbody.velocity.magnitude;
					this.ReplicatedForwardVelocity = base.transform.InverseTransformDirection(this.rootRigidbody.velocity).z;
					this.ReplicatedSteeringInput = 0f;
					this.ReplicatedVelocityInput = 0f;
					this.real = base.transform.position;
				}
				if (this.isDead && !this.isExploded && !this.isUnderwater && Time.realtimeSinceStartup - this.lastDead > 4f)
				{
					this.explode();
				}
			}
			if (!Provider.isServer && !this.isPhysical && Time.realtimeSinceStartup - this.lastTick > Provider.UPDATE_TIME * 2f)
			{
				this.lastTick = Time.realtimeSinceStartup;
				this.ReplicatedSpeed = 0f;
				this.ReplicatedForwardVelocity = 0f;
				this.ReplicatedVelocityInput = 0f;
			}
			bool sirensOn = this.sirensOn;
			if (this.usesBattery)
			{
				bool flag4 = false;
				bool flag5 = false;
				if (this.isDriven && this.isEnginePowered)
				{
					EBatteryMode ebatteryMode = this.asset.batteryDriving;
					if (ebatteryMode != EBatteryMode.Burn)
					{
						if (ebatteryMode == EBatteryMode.Charge)
						{
							flag4 = true;
						}
					}
					else
					{
						flag5 = true;
					}
				}
				else
				{
					EBatteryMode ebatteryMode = this.asset.batteryEmpty;
					if (ebatteryMode != EBatteryMode.Burn)
					{
						if (ebatteryMode == EBatteryMode.Charge)
						{
							flag4 = true;
						}
					}
					else
					{
						flag5 = true;
					}
				}
				if (this.headlightsOn)
				{
					EBatteryMode ebatteryMode = this.asset.batteryHeadlights;
					if (ebatteryMode != EBatteryMode.Burn)
					{
						if (ebatteryMode == EBatteryMode.Charge)
						{
							flag4 = true;
						}
					}
					else
					{
						flag5 = true;
					}
				}
				if (this.sirensOn)
				{
					EBatteryMode ebatteryMode = this.asset.batterySirens;
					if (ebatteryMode != EBatteryMode.Burn)
					{
						if (ebatteryMode == EBatteryMode.Charge)
						{
							flag4 = true;
						}
					}
					else
					{
						flag5 = true;
					}
				}
				flag4 &= this.ContainsBatteryItem;
				float num9 = 0f;
				if (flag4)
				{
					num9 = this.asset.batteryChargeRate;
				}
				else if (flag5)
				{
					num9 = this.asset.batteryBurnRate;
				}
				this.batteryBuffer += deltaTime * num9;
				ushort num10 = (ushort)Mathf.FloorToInt(this.batteryBuffer);
				if (num10 > 0)
				{
					this.batteryBuffer -= (float)num10;
					if (flag4)
					{
						this.askChargeBattery(num10);
					}
					else if (flag5)
					{
						this.askBurnBattery(num10);
					}
				}
			}
			if (Provider.isServer)
			{
				this.UpdateSafezoneStatus(deltaTime);
			}
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x0008E790 File Offset: 0x0008C990
		private void FixedUpdate()
		{
			if (!this.isPhysical || this.isDriven || !Provider.isServer)
			{
				return;
			}
			bool flag = false;
			if (this.asset.replicatedWheelIndices != null)
			{
				foreach (int num in this.asset.replicatedWheelIndices)
				{
					Wheel wheelAtIndex = this.GetWheelAtIndex(num);
					if (wheelAtIndex == null)
					{
						UnturnedLog.error(string.Format("\"{0}\" missing wheel for replicated index: {1}", this.asset.FriendlyName, num));
					}
					else
					{
						wheelAtIndex.UpdateServerSuspensionAndPhysicsMaterial();
						flag |= wheelAtIndex.isGrounded;
					}
				}
			}
			if (this._wheels != null && flag && this.asset.wheelBalancingForceMultiplier > 0f)
			{
				this.ApplyWheelBalancingForce(Time.fixedDeltaTime);
			}
			if (this.asset.rollAngularVelocityDamping > 0f)
			{
				this.ApplyAngularVelocityDamping(Time.fixedDeltaTime);
			}
		}

		/// <summary>
		/// Update whether this vehicle is inside a safezone.
		/// If a certain option is enabled, unlock after time threshold is passed.
		/// </summary>
		// Token: 0x060023EE RID: 9198 RVA: 0x0008E868 File Offset: 0x0008CA68
		private void UpdateSafezoneStatus(float deltaSeconds)
		{
			SafezoneNode insideSafezoneNode;
			this.isInsideSafezone = LevelNodes.isPointInsideSafezone(base.transform.position, out insideSafezoneNode);
			this.insideSafezoneNode = insideSafezoneNode;
			if (this.isInsideSafezone)
			{
				this.timeInsideSafezone += deltaSeconds;
				if (Provider.modeConfigData != null && Provider.modeConfigData.Vehicles.Unlocked_After_Seconds_In_Safezone > 0f && this.timeInsideSafezone > Provider.modeConfigData.Vehicles.Unlocked_After_Seconds_In_Safezone && this.isEmpty && this.isLocked)
				{
					VehicleManager.unlockVehicle(this, null);
					return;
				}
			}
			else
			{
				this.timeInsideSafezone = -1f;
			}
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x0008E901 File Offset: 0x0008CB01
		protected virtual void handleTireAliveChanged(Wheel wheel)
		{
			if (this.isPhysical)
			{
				this.rootRigidbody.WakeUp();
			}
		}

		/// <summary>
		/// Can be called without calling init.
		/// </summary>
		// Token: 0x060023F0 RID: 9200 RVA: 0x0008E916 File Offset: 0x0008CB16
		internal void safeInit(VehicleAsset asset)
		{
			this._asset = asset;
			this.ApplyDepthMaskMaterial();
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0008E928 File Offset: 0x0008CB28
		internal void init(VehicleAsset asset)
		{
			this.safeInit(asset);
			this.eventHook = base.gameObject.GetComponent<VehicleEventHook>();
			this.engineCurvesComponent = base.gameObject.GetComponentInChildren<EngineCurvesComponent>(true);
			if (Provider.isServer)
			{
				if (this.fuel == 65535)
				{
					if (Provider.mode == EGameMode.TUTORIAL)
					{
						this.fuel = 0;
					}
					else
					{
						this.fuel = (ushort)Random.Range((int)asset.fuelMin, (int)asset.fuelMax);
					}
				}
				if (this.health == 65535)
				{
					this.health = (ushort)Random.Range((int)asset.healthMin, (int)asset.healthMax);
				}
				if (this.batteryCharge == 65535)
				{
					if (this.usesBattery)
					{
						if (asset.canSpawnWithBattery && Random.value < Provider.modeConfigData.Vehicles.Has_Battery_Chance)
						{
							float num = Random.Range(Provider.modeConfigData.Vehicles.Min_Battery_Charge, Provider.modeConfigData.Vehicles.Max_Battery_Charge);
							num *= asset.batterySpawnChargeMultiplier;
							this.batteryCharge = (ushort)Mathf.Max(1f, 10000f * num);
						}
						else
						{
							this.batteryCharge = 0;
						}
					}
					else
					{
						this.batteryCharge = 10000;
					}
				}
				if (this.PaintColor.a != 255)
				{
					Color32? randomDefaultPaintColor = asset.GetRandomDefaultPaintColor();
					if (randomDefaultPaintColor != null)
					{
						Color32 value = randomDefaultPaintColor.Value;
						value.a = byte.MaxValue;
						this.PaintColor = value;
					}
				}
			}
			this._sirensOn = false;
			this._headlightsOn = false;
			this._taillightsOn = false;
			this.waterCenterTransform = base.transform.Find("Water_Center");
			Transform transform = base.transform.Find("Seats");
			if (transform == null)
			{
				Assets.reportError(asset, "missing 'Seats' Transform");
				transform = new GameObject("Seats").transform;
				transform.parent = base.transform;
			}
			Transform transform2 = base.transform.Find("Objects");
			Transform transform3 = base.transform.Find("Turrets");
			Transform transform4 = base.transform.Find("Train_Cars");
			this._passengers = new Passenger[transform.childCount];
			for (int i = 0; i < this.passengers.Length; i++)
			{
				string text = "Seat_" + i.ToString();
				Transform transform5 = transform.Find(text);
				if (transform5 == null)
				{
					Assets.reportError(asset, "missing '{0}' Transform", text);
					transform5 = new GameObject(text).transform;
					transform5.parent = transform;
				}
				Transform newObj = null;
				if (transform2 != null)
				{
					newObj = transform2.Find("Seat_" + i.ToString());
				}
				Transform transform6 = null;
				Transform transform7 = null;
				Transform newTurretPitch = null;
				Transform newTurretAim = null;
				if (transform3 != null)
				{
					transform6 = transform3.Find("Turret_" + i.ToString());
					if (transform6 != null)
					{
						transform7 = transform6.Find("Yaw");
						if (transform7 != null)
						{
							Transform transform8 = transform7.Find("Seats");
							if (transform8 != null)
							{
								transform5 = transform8.Find("Seat_" + i.ToString());
							}
							Transform transform9 = transform7.Find("Objects");
							if (transform9 != null)
							{
								newObj = transform9.Find("Seat_" + i.ToString());
							}
							newTurretPitch = transform7.Find("Pitch");
						}
						newTurretAim = transform6.FindChildRecursive("Aim");
					}
				}
				if (transform4 != null)
				{
					Transform transform10 = transform4.FindChildRecursive(text);
					if (transform10 != null)
					{
						transform5 = transform10;
					}
				}
				this.passengers[i] = new Passenger(transform5, newObj, transform7, newTurretPitch, newTurretAim);
				if (transform6 != null)
				{
					this.passengers[i].turretEventHook = transform6.GetComponent<VehicleTurretEventHook>();
				}
				if (asset.shouldSpawnSeatCapsules)
				{
					Transform transform11 = new GameObject("Clip")
					{
						layer = 21
					}.transform;
					transform11.parent = transform5;
					transform11.localPosition = Vector3.zero;
					transform11.localRotation = Quaternion.identity;
					transform11.localScale = Vector3.one;
					transform11.parent = base.transform;
					CapsuleCollider orAddComponent = transform11.GetOrAddComponent<CapsuleCollider>();
					orAddComponent.center = new Vector3(0f, PlayerMovement.HEIGHT_STAND * 0.5f, 0f);
					orAddComponent.height = PlayerMovement.HEIGHT_STAND;
					orAddComponent.radius = PlayerStance.RADIUS;
					orAddComponent.enabled = false;
					this.passengers[i].collider = orAddComponent;
				}
			}
			this._turrets = new Passenger[asset.turrets.Length];
			for (int j = 0; j < this.turrets.Length; j++)
			{
				TurretInfo turretInfo = asset.turrets[j];
				if ((int)turretInfo.seatIndex < this.passengers.Length)
				{
					this.passengers[(int)turretInfo.seatIndex].turret = turretInfo;
					this._turrets[j] = this.passengers[(int)turretInfo.seatIndex];
				}
			}
			this.InitializeWheels();
			this.buoyancy = base.transform.Find("Buoyancy");
			if (this.buoyancy != null)
			{
				for (int k = 0; k < this.buoyancy.childCount; k++)
				{
					Transform child = this.buoyancy.GetChild(k);
					child.gameObject.AddComponent<Buoyancy>().density = (float)(this.buoyancy.childCount * 500);
					if (asset.engine == EEngine.BLIMP)
					{
						child.GetComponent<Buoyancy>().overrideSurfaceElevation = Level.info.configData.Blimp_Altitude;
					}
				}
			}
			this.hook = base.transform.Find("Hook");
			this.hooked = new List<HookInfo>();
			if (this.isExploded)
			{
				this.tellExploded();
			}
			if (asset.engine == EEngine.TRAIN)
			{
				int childCount = transform4.childCount;
				this.trainCars = new TrainCar[1 + childCount];
				this.trainCars[0] = this.getTrainCar(base.transform);
				for (int l = 1; l <= childCount; l++)
				{
					Transform transform12 = transform4.Find("Train_Car_" + l.ToString());
					transform12.parent = null;
					transform12.GetOrAddComponent<VehicleRef>().vehicle = this;
					TrainCar trainCar = this.getTrainCar(transform12);
					trainCar.trackPositionOffset = (float)l * -asset.trainCarLength;
					this.trainCars[l] = trainCar;
				}
				foreach (TrainCar trainCar2 in this.trainCars)
				{
					if (this.overlapFront == null)
					{
						this.overlapFront = trainCar2.root.Find("Overlap_Front");
					}
					if (this.overlapBack == null)
					{
						this.overlapBack = trainCar2.root.Find("Overlap_Back");
					}
					if (this.overlapFront != null && this.overlapBack != null)
					{
						break;
					}
				}
				foreach (LevelTrainAssociation levelTrainAssociation in Level.info.configData.Trains)
				{
					if (levelTrainAssociation.VehicleID == this.id)
					{
						this.roadIndex = levelTrainAssociation.RoadIndex;
						break;
					}
				}
				this.road = LevelRoads.getRoad((int)this.roadIndex);
				this.roadPosition = this.clampRoadPosition(this.roadPosition);
				this.teleportTrain();
			}
			if (asset.physicsProfileRef.isValid)
			{
				VehiclePhysicsProfileAsset vehiclePhysicsProfileAsset = asset.physicsProfileRef.Find();
				if (vehiclePhysicsProfileAsset != null)
				{
					vehiclePhysicsProfileAsset.applyTo(this);
				}
			}
			this.decayLastUpdateTime = Time.time;
			this.decayLastUpdatePosition = base.transform.position;
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0008F0F0 File Offset: 0x0008D2F0
		private void Awake()
		{
			this.rootRigidbody = base.GetComponent<Rigidbody>();
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x0008F109 File Offset: 0x0008D309
		private void initBumper(Transform bumper, bool reverse, bool instakill)
		{
			if (bumper == null)
			{
				return;
			}
			if (Provider.isServer)
			{
				Bumper bumper2 = bumper.gameObject.AddComponent<Bumper>();
				bumper2.reverse = reverse;
				bumper2.instakill = instakill;
				bumper2.init(this);
				return;
			}
			Object.Destroy(bumper.gameObject);
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x0008F148 File Offset: 0x0008D348
		private void initBumpers(Transform root)
		{
			Transform transform = root.FindChildRecursive("Nav");
			if (transform != null)
			{
				if (Provider.isServer)
				{
					transform.DestroyRigidbody();
				}
				else
				{
					Object.Destroy(transform.gameObject);
				}
			}
			Transform bumper = root.FindChildRecursive("Bumper");
			this.initBumper(bumper, false, this.asset.engine == EEngine.TRAIN);
			Transform bumper2 = root.FindChildRecursive("Bumper_Front");
			this.initBumper(bumper2, false, this.asset.engine == EEngine.TRAIN);
			Transform bumper3 = root.FindChildRecursive("Bumper_Back");
			this.initBumper(bumper3, true, this.asset.engine == EEngine.TRAIN);
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0008F1EC File Offset: 0x0008D3EC
		private void Start()
		{
			if (this.trainCars != null && this.trainCars.Length != 0)
			{
				foreach (TrainCar trainCar in this.trainCars)
				{
					this.initBumpers(trainCar.root);
				}
			}
			else
			{
				this.initBumpers(base.transform);
			}
			this.updateVehicle();
			this.updatePhysics();
			this.updateEngine();
			this.updates = new List<VehicleStateUpdate>();
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x0008F25C File Offset: 0x0008D45C
		private void OnDestroy()
		{
			this.dropTrunkItems();
			if (this._wheels != null)
			{
				Wheel[] wheels = this._wheels;
				for (int i = 0; i < wheels.Length; i++)
				{
					wheels[i].OnVehicleDestroyed();
				}
			}
			if (this.skinMaterialToDestroy != null)
			{
				Object.Destroy(this.skinMaterialToDestroy);
				this.skinMaterialToDestroy = null;
			}
			if (this.materialsToDestroy != null)
			{
				foreach (Material material in this.materialsToDestroy)
				{
					if (material != null)
					{
						Object.Destroy(material);
					}
				}
				this.materialsToDestroy.Clear();
			}
			if (this.waterSortHandles != null)
			{
				DynamicWaterTransparentSort dynamicWaterTransparentSort = DynamicWaterTransparentSort.Get();
				foreach (object handle in this.waterSortHandles)
				{
					dynamicWaterTransparentSort.Unregister(handle);
				}
				this.waterSortHandles.Clear();
			}
			bool flag = this.isExploded;
			if (this.headlightsMaterial != null)
			{
				Object.DestroyImmediate(this.headlightsMaterial);
			}
			if (this.taillightsMaterial != null)
			{
				Object.DestroyImmediate(this.taillightsMaterial);
			}
			else if (this.taillightMaterials != null)
			{
				for (int j = 0; j < this.taillightMaterials.Length; j++)
				{
					if (this.taillightMaterials[j] != null)
					{
						Object.DestroyImmediate(this.taillightMaterials[j]);
					}
				}
			}
			if (this.sirenMaterials != null)
			{
				for (int k = 0; k < this.sirenMaterials.Length; k++)
				{
					if (this.sirenMaterials[k] != null)
					{
						Object.DestroyImmediate(this.sirenMaterials[k]);
					}
				}
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060023F7 RID: 9207 RVA: 0x0008F42C File Offset: 0x0008D62C
		public IEnumerable<Collider> vehicleColliders
		{
			get
			{
				return this._vehicleColliders;
			}
		}

		/// <summary>
		/// Called after initializing vehicle.
		/// </summary>
		// Token: 0x060023F8 RID: 9208 RVA: 0x0008F434 File Offset: 0x0008D634
		public void gatherVehicleColliders()
		{
			this._vehicleColliders = new List<Collider>();
			base.gameObject.GetComponentsInChildren<Collider>(true, this._vehicleColliders);
			this.initCenterCollider();
			if (this.trainCars != null)
			{
				foreach (TrainCar trainCar in this.trainCars)
				{
					InteractableVehicle._trainCarColliders.Clear();
					trainCar.root.GetComponentsInChildren<Collider>(true, InteractableVehicle._trainCarColliders);
					this._vehicleColliders.AddRange(InteractableVehicle._trainCarColliders);
				}
			}
		}

		/// <summary>
		/// Makes the collision detection system ignore all collisions between this vehicle and the given colliders.
		/// Used to prevent vehicle from colliding with attached items.
		/// </summary>
		// Token: 0x060023F9 RID: 9209 RVA: 0x0008F4B0 File Offset: 0x0008D6B0
		public void ignoreCollisionWith(IEnumerable<Collider> otherColliders, bool shouldIgnore)
		{
			if (this._vehicleColliders == null)
			{
				throw new Exception("gatherVehicleColliders was not called yet");
			}
			for (int i = this._vehicleColliders.Count - 1; i >= 0; i--)
			{
				Collider collider = this._vehicleColliders[i];
				if (collider == null)
				{
					this._vehicleColliders.RemoveAtFast(i);
				}
				else
				{
					foreach (Collider collider2 in otherColliders)
					{
						if (!(collider2 == null))
						{
							Physics.IgnoreCollision(collider, collider2, shouldIgnore);
						}
					}
				}
			}
		}

		/// <summary>
		/// Used to disable collision between skycrane and held vehicle.
		/// </summary>
		// Token: 0x060023FA RID: 9210 RVA: 0x0008F554 File Offset: 0x0008D754
		private void ignoreCollisionWithVehicle(InteractableVehicle otherVehicle, bool shouldIgnore)
		{
			this.ignoreCollisionWith(otherVehicle._vehicleColliders, shouldIgnore);
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x0008F563 File Offset: 0x0008D763
		public Vector3 getClosestPointOnHull(Vector3 position)
		{
			if (this._vehicleColliders == null)
			{
				throw new Exception("gatherVehicleColliders was not called yet");
			}
			return CollisionUtil.ClosestPoint(this._vehicleColliders, position);
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x0008F584 File Offset: 0x0008D784
		public float getSqrDistanceFromHull(Vector3 position)
		{
			return (this.getClosestPointOnHull(position) - position).sqrMagnitude;
		}

		/// <summary>
		/// Find collider with the largest volume to use for exit physics queries.
		/// </summary>
		// Token: 0x060023FD RID: 9213 RVA: 0x0008F5A8 File Offset: 0x0008D7A8
		private void initCenterCollider()
		{
			this.center = base.transform.Find("Center");
			if (this.center != null)
			{
				return;
			}
			this.center = new GameObject("Center").transform;
			this.center.parent = base.transform;
			this.center.localPosition = Vector3.zero;
			this.center.localRotation = Quaternion.identity;
			this.center.localScale = Vector3.one;
			float num = 0.001f;
			foreach (Collider collider in this._vehicleColliders)
			{
				if (!collider.isTrigger)
				{
					BoxCollider boxCollider = collider as BoxCollider;
					if (boxCollider != null)
					{
						float boxVolume = boxCollider.GetBoxVolume();
						if (boxVolume > num)
						{
							num = boxVolume;
							this.center.position = boxCollider.TransformBoxCenter();
						}
					}
					else
					{
						SphereCollider sphereCollider = collider as SphereCollider;
						if (sphereCollider != null)
						{
							float sphereVolume = sphereCollider.GetSphereVolume();
							if (sphereVolume > num)
							{
								num = sphereVolume;
								this.center.position = sphereCollider.TransformSphereCenter();
							}
						}
						else
						{
							CapsuleCollider capsuleCollider = collider as CapsuleCollider;
							if (capsuleCollider != null)
							{
								float capsuleVolume = capsuleCollider.GetCapsuleVolume();
								if (capsuleVolume > num)
								{
									num = capsuleVolume;
									this.center.position = capsuleCollider.TransformCapsuleCenter();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x0008F714 File Offset: 0x0008D914
		private void InitializeWheels()
		{
			if (this.asset.wheelConfiguration != null && this.asset.wheelConfiguration.Length != 0)
			{
				List<Wheel> list = new List<Wheel>(this.asset.wheelConfiguration.Length);
				foreach (VehicleWheelConfiguration vehicleWheelConfiguration in this.asset.wheelConfiguration)
				{
					WheelCollider wheelCollider = null;
					if (!string.IsNullOrEmpty(vehicleWheelConfiguration.wheelColliderPath))
					{
						Transform transform = base.transform.Find(vehicleWheelConfiguration.wheelColliderPath);
						if (transform == null)
						{
							Assets.reportError(this.asset, "missing wheel collider transform at path \"" + vehicleWheelConfiguration.wheelColliderPath + "\"");
						}
						else
						{
							wheelCollider = transform.GetComponent<WheelCollider>();
							if (wheelCollider == null)
							{
								Assets.reportError(this.asset, "missing WheelCollider component at path \"" + vehicleWheelConfiguration.wheelColliderPath + "\"");
							}
							else if (this.asset.wheelColliderMassOverride != null)
							{
								wheelCollider.mass = this.asset.wheelColliderMassOverride.Value;
							}
						}
					}
					Transform transform2 = null;
					if (!string.IsNullOrEmpty(vehicleWheelConfiguration.modelPath))
					{
						transform2 = base.transform.Find(vehicleWheelConfiguration.modelPath);
						if (transform2 == null)
						{
							Assets.reportError(this.asset, "missing wheel model transform at path \"" + vehicleWheelConfiguration.modelPath + "\"");
						}
					}
					if (!(wheelCollider == null) || !(transform2 == null))
					{
						Wheel wheel = new Wheel(this, list.Count, wheelCollider, transform2, vehicleWheelConfiguration);
						wheel.Reset();
						wheel.aliveChanged += this.handleTireAliveChanged;
						list.Add(wheel);
					}
				}
				this._wheels = list.ToArray();
				return;
			}
			this._wheels = new Wheel[0];
		}

		/// <summary>
		/// Set material on DepthMask child renderer responsible for hiding water when interior of vehicle is submerged.
		/// </summary>
		// Token: 0x060023FF RID: 9215 RVA: 0x0008F8E4 File Offset: 0x0008DAE4
		private void ApplyDepthMaskMaterial()
		{
			Transform transform = base.transform.Find("DepthMask");
			if (transform != null)
			{
				Renderer component = transform.GetComponent<Renderer>();
				if (component != null)
				{
					component.sharedMaterial = Resources.Load<Material>("Materials/DepthMask");
				}
			}
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x0008F92C File Offset: 0x0008DB2C
		private void InitializeAdditionalTransparentSections()
		{
			if (this.asset.extraTransparentSections == null || this.asset.extraTransparentSections.Length < 1)
			{
				return;
			}
			DynamicWaterTransparentSort dynamicWaterTransparentSort = DynamicWaterTransparentSort.Get();
			foreach (string text in this.asset.extraTransparentSections)
			{
				Transform transform = base.transform.Find(text);
				if (transform == null)
				{
					Assets.reportError(this.asset, "missing additional transparent section transform \"" + text + "\"");
				}
				else
				{
					Renderer component = transform.GetComponent<Renderer>();
					if (component == null)
					{
						Assets.reportError(this.asset, "additional transparent section \"" + text + "\" missing Renderer component");
					}
					else
					{
						Material material = component.material;
						if (material == null)
						{
							Assets.reportError(this.asset, "additional transparent section \"" + text + "\" missing material");
						}
						else
						{
							this.materialsToDestroy.Add(material);
							object obj = dynamicWaterTransparentSort.Register(transform, material);
							this.waterSortHandles.Add(obj);
						}
					}
				}
			}
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x0008FA40 File Offset: 0x0008DC40
		private void InitializePaintableSections()
		{
			if (!this.asset.SupportsPaintColor)
			{
				return;
			}
			this.paintableMaterials = new List<Material>();
			foreach (PaintableVehicleSection paintableVehicleSection in this.asset.PaintableVehicleSections)
			{
				Transform transform = base.transform.Find(paintableVehicleSection.path);
				if (transform == null)
				{
					Assets.reportError(this.asset, "paintable section missing transform \"" + paintableVehicleSection.path + "\"");
				}
				else
				{
					Renderer component = transform.GetComponent<Renderer>();
					if (component == null)
					{
						Assets.reportError(this.asset, "paintable section missing renderer \"" + paintableVehicleSection.path + "\"");
					}
					else
					{
						InteractableVehicle.tempMaterialsList.Clear();
						component.GetMaterials(InteractableVehicle.tempMaterialsList);
						foreach (Material material in InteractableVehicle.tempMaterialsList)
						{
							this.materialsToDestroy.Add(material);
						}
						if (paintableVehicleSection.materialIndex < 0 || paintableVehicleSection.materialIndex >= InteractableVehicle.tempMaterialsList.Count)
						{
							Assets.reportError(this.asset, string.Format("paintable section \"{0}\" material index out of range (index: {1} length: {2})", paintableVehicleSection.path, paintableVehicleSection.materialIndex, InteractableVehicle.tempMaterialsList.Count));
						}
						else
						{
							this.paintableMaterials.Add(InteractableVehicle.tempMaterialsList[paintableVehicleSection.materialIndex]);
						}
					}
				}
			}
			this.ApplyPaintColor();
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x0008FBDC File Offset: 0x0008DDDC
		private void ApplyPaintColor()
		{
			List<Material> list = this.paintableMaterials;
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x0008FBF0 File Offset: 0x0008DDF0
		private void ApplySkinToRenderer(Renderer renderer, Material skinMaterial, bool shared)
		{
			this.skinOriginalMaterials.Add(new VehicleSkinMaterialChange
			{
				renderer = renderer,
				originalMaterial = (shared ? renderer.sharedMaterial : renderer.material),
				shared = shared
			});
			if (shared)
			{
				renderer.material = skinMaterial;
				return;
			}
			renderer.sharedMaterial = skinMaterial;
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x0008FC4C File Offset: 0x0008DE4C
		private void SetExhaustParticleSystemsRateOverTimeToZero()
		{
			ParticleSystem[] array = this.exhaustParticleSystems;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].emission.rateOverTime = 0f;
			}
			this.isExhaustRateOverTimeZero = true;
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x0008FC90 File Offset: 0x0008DE90
		private void ApplyAngularVelocityDamping(float deltaTime)
		{
			float num = base.transform.InverseTransformDirection(this.rootRigidbody.angularVelocity).z * -this.asset.rollAngularVelocityDamping * deltaTime * 50f;
			if (!MathfEx.IsNearlyZero(num, 0.001f))
			{
				this.rootRigidbody.AddRelativeTorque(0f, 0f, num, ForceMode.Acceleration);
			}
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x0008FCF4 File Offset: 0x0008DEF4
		private void ApplyWheelBalancingForce(float deltaTime)
		{
			Vector3 a = Vector3.zero;
			int num = 0;
			foreach (Wheel wheel in this._wheels)
			{
				if (wheel.isGrounded)
				{
					a += wheel.mostRecentGroundHit.normal;
					num++;
				}
			}
			Vector3 direction = a / (float)num;
			Vector3 vector = base.transform.InverseTransformDirection(direction);
			vector.z = 0f;
			vector = vector.normalized;
			float num2 = Mathf.Clamp01(1f - Vector3.Dot(Vector3.up, vector));
			num2 = Mathf.Pow(num2, this.asset.wheelBalancingUprightExponent);
			float num3 = (vector.x > 0f) ? -1f : 1f;
			num2 *= num3;
			if (!MathfEx.IsNearlyZero(num2 * this.asset.wheelBalancingForceMultiplier * deltaTime * 50f, 0.001f))
			{
				this.rootRigidbody.AddRelativeTorque(0f, 0f, num2 * this.asset.wheelBalancingForceMultiplier * deltaTime * 50f);
			}
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x0008FE10 File Offset: 0x0008E010
		[Obsolete]
		public void tellState(Vector3 newPosition, byte newAngle_X, byte newAngle_Y, byte newAngle_Z, byte newSpeed, byte newPhysicsSpeed, byte newTurn)
		{
			Quaternion newRotation = Quaternion.Euler(MeasurementTool.byteToAngle2(newAngle_X), MeasurementTool.byteToAngle2(newAngle_Y), MeasurementTool.byteToAngle2(newAngle_Z));
			this.tellState(newPosition, newRotation, (float)newSpeed, (float)newPhysicsSpeed, (float)newTurn, 0f);
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0008FE4C File Offset: 0x0008E04C
		[Obsolete("This override uses the vanilla battery item rather than the equipped battery item.")]
		public void replaceBattery(Player player, byte quality)
		{
			this.replaceBattery(player, quality, new Guid("098b13be34a7411db7736b7f866ada69"));
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x0008FE60 File Offset: 0x0008E060
		[Obsolete]
		public void safeInit()
		{
			this.safeInit(Assets.find(EAssetType.VEHICLE, this.id) as VehicleAsset);
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x0008FE79 File Offset: 0x0008E079
		[Obsolete]
		public void init()
		{
			this.init(Assets.find(EAssetType.VEHICLE, this.id) as VehicleAsset);
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600240B RID: 9227 RVA: 0x0008FE92 File Offset: 0x0008E092
		[Obsolete("Replaced by ReplicatedSteeringInput")]
		public int turn
		{
			get
			{
				return Mathf.RoundToInt(this.ReplicatedSteeringInput);
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x0008FE9F File Offset: 0x0008E09F
		[Obsolete("Replaced by AnimatedSteeringAngle")]
		public float steer
		{
			get
			{
				return this.AnimatedSteeringAngle;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x0600240D RID: 9229 RVA: 0x0008FEA7 File Offset: 0x0008E0A7
		[Obsolete("Replaced by ReplicatedSpeed")]
		public float speed
		{
			get
			{
				return this.ReplicatedSpeed * Mathf.Sign(this.ReplicatedForwardVelocity);
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x0008FEBB File Offset: 0x0008E0BB
		[Obsolete("Replaced by ReplicatedForwardVelocity")]
		public float physicsSpeed
		{
			get
			{
				return this.ReplicatedForwardVelocity;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x0008FEC3 File Offset: 0x0008E0C3
		[Obsolete("Replaced by GetReplicatedForwardSpeedPercentageOfTargetSpeed")]
		public float factor
		{
			get
			{
				return this.GetReplicatedForwardSpeedPercentageOfTargetSpeed();
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x0008FECB File Offset: 0x0008E0CB
		[Obsolete("Clarified with HasBatteryWithCharge and ContainsBatteryItem properties.")]
		public bool hasBattery
		{
			get
			{
				return !this.usesBattery || this.batteryCharge > 1;
			}
		}

		/// <summary>
		/// Temporary array for use with physics queries.
		/// </summary>
		// Token: 0x04001181 RID: 4481
		private static Collider[] tempCollidersArray = new Collider[4];

		/// <summary>
		/// Temporary list for gathering materials.
		/// </summary>
		// Token: 0x04001182 RID: 4482
		private static List<Material> tempMaterialsList = new List<Material>();

		// Token: 0x04001183 RID: 4483
		private const float EXPLODE = 4f;

		// Token: 0x04001184 RID: 4484
		private const ushort SMOKE_0_HEALTH_THRESHOLD = 100;

		// Token: 0x04001185 RID: 4485
		private const ushort SMOKE_1_HEALTH_THRESHOLD = 200;

		/// <summary>
		/// Precursor to Net ID. Should eventually become obsolete.
		/// </summary>
		// Token: 0x04001195 RID: 4501
		public uint instanceID;

		/// <summary>
		/// Asset ID. Essentially obsolete at this point.
		/// </summary>
		// Token: 0x04001196 RID: 4502
		public ushort id;

		// Token: 0x04001197 RID: 4503
		public Items trunkItems;

		// Token: 0x04001198 RID: 4504
		public ushort skinID;

		// Token: 0x04001199 RID: 4505
		public ushort mythicID;

		// Token: 0x0400119A RID: 4506
		protected SkinAsset skinAsset;

		// Token: 0x0400119B RID: 4507
		private List<Mesh> tempMesh;

		/// <summary>
		/// Used to restore vehicle materials when changing skin.
		/// </summary>
		// Token: 0x0400119C RID: 4508
		private List<VehicleSkinMaterialChange> skinOriginalMaterials;

		// Token: 0x0400119D RID: 4509
		protected Transform effectSlotsRoot;

		// Token: 0x0400119E RID: 4510
		protected Transform[] effectSlots;

		// Token: 0x0400119F RID: 4511
		protected MythicalEffectController[] effectSystems;

		/// <summary>
		/// Only used by trains. Constrains the train to this path.
		/// </summary>
		// Token: 0x040011A0 RID: 4512
		public ushort roadIndex;

		// Token: 0x040011A1 RID: 4513
		public float roadPosition;

		// Token: 0x040011A3 RID: 4515
		public ushort fuel;

		// Token: 0x040011A4 RID: 4516
		public ushort health;

		/// <summary>
		/// Nelson 2024-06-24: When first implementing batteries there was only the vanilla battery item, and it was
		/// fine to delete it when the charge reached zero. This may not be desirable, however, so zero now represents
		/// no battery item is present, and one represents the battery is completely drained but still there.
		/// </summary>
		// Token: 0x040011A5 RID: 4517
		public ushort batteryCharge;

		// Token: 0x040011A6 RID: 4518
		internal Guid batteryItemGuid;

		// Token: 0x040011AC RID: 4524
		private float horned;

		// Token: 0x040011AE RID: 4526
		protected VehicleEventHook eventHook;

		// Token: 0x040011AF RID: 4527
		private bool _isDrowned;

		// Token: 0x040011B1 RID: 4529
		private float _lastDead;

		// Token: 0x040011B2 RID: 4530
		private float _lastUnderwater;

		// Token: 0x040011B3 RID: 4531
		private float _lastExploded;

		// Token: 0x040011B4 RID: 4532
		private float _slip;

		// Token: 0x040011B5 RID: 4533
		public bool isExploded;

		// Token: 0x040011BD RID: 4541
		private float propellerRotationDegrees;

		// Token: 0x040011BE RID: 4542
		private PropellerModel[] propellerModels;

		// Token: 0x040011BF RID: 4543
		private GameObject exhaustGameObject;

		// Token: 0x040011C0 RID: 4544
		private bool isExhaustGameObjectActive;

		// Token: 0x040011C1 RID: 4545
		private bool isExhaustRateOverTimeZero;

		// Token: 0x040011C2 RID: 4546
		private ParticleSystem[] exhaustParticleSystems;

		// Token: 0x040011C3 RID: 4547
		private Transform steeringWheelModelTransform;

		// Token: 0x040011C5 RID: 4549
		private Transform overlapFront;

		// Token: 0x040011C6 RID: 4550
		private Transform overlapBack;

		// Token: 0x040011C7 RID: 4551
		private Transform pedalLeft;

		// Token: 0x040011C8 RID: 4552
		private Transform pedalRight;

		/// <summary>
		/// Front steering column of bicycles and motorcycles.
		/// </summary>
		// Token: 0x040011C9 RID: 4553
		private Transform frontModelTransform;

		// Token: 0x040011CA RID: 4554
		private Quaternion steeringWheelRestLocalRotation;

		// Token: 0x040011CB RID: 4555
		private Quaternion frontModelRestLocalRotation;

		// Token: 0x040011CC RID: 4556
		private Transform waterCenterTransform;

		// Token: 0x040011CD RID: 4557
		private Transform fire;

		// Token: 0x040011CE RID: 4558
		private Transform smoke_0;

		// Token: 0x040011CF RID: 4559
		private Transform smoke_1;

		// Token: 0x040011D0 RID: 4560
		[Obsolete("Replaced by MarkForReplicationUpdate. Will be removed in a future release.")]
		public List<VehicleStateUpdate> updates;

		/// <summary>
		/// If true, server should replicate latest state to clients.
		/// </summary>
		// Token: 0x040011D1 RID: 4561
		internal bool needsReplicationUpdate;

		// Token: 0x040011D2 RID: 4562
		private Material[] sirenMaterials;

		// Token: 0x040011D3 RID: 4563
		private bool sirenState;

		// Token: 0x040011D4 RID: 4564
		private List<GameObject> sirenGameObjects = new List<GameObject>();

		// Token: 0x040011D5 RID: 4565
		private List<GameObject> sirenGameObjects_0 = new List<GameObject>();

		// Token: 0x040011D6 RID: 4566
		private List<GameObject> sirenGameObjects_1 = new List<GameObject>();

		// Token: 0x040011D7 RID: 4567
		private bool _sirensOn;

		// Token: 0x040011D8 RID: 4568
		private Transform _headlights;

		// Token: 0x040011D9 RID: 4569
		private Material headlightsMaterial;

		// Token: 0x040011DA RID: 4570
		private bool _headlightsOn;

		// Token: 0x040011DB RID: 4571
		private Transform _taillights;

		// Token: 0x040011DC RID: 4572
		private Material taillightsMaterial;

		// Token: 0x040011DD RID: 4573
		private Material[] taillightMaterials;

		// Token: 0x040011DE RID: 4574
		private bool _taillightsOn;

		// Token: 0x040011DF RID: 4575
		private CSteamID _lockedOwner;

		// Token: 0x040011E0 RID: 4576
		private CSteamID _lockedGroup;

		// Token: 0x040011E1 RID: 4577
		private bool _isLocked;

		// Token: 0x040011E2 RID: 4578
		private VehicleAsset _asset;

		// Token: 0x040011E3 RID: 4579
		public float lastSeat;

		// Token: 0x040011E4 RID: 4580
		private Passenger[] _passengers;

		// Token: 0x040011E5 RID: 4581
		private Passenger[] _turrets;

		// Token: 0x040011E6 RID: 4582
		internal Wheel[] _wheels;

		// Token: 0x040011E7 RID: 4583
		public bool isHooked;

		// Token: 0x040011E8 RID: 4584
		private Transform buoyancy;

		// Token: 0x040011E9 RID: 4585
		private Transform hook;

		// Token: 0x040011EA RID: 4586
		private List<HookInfo> hooked;

		// Token: 0x040011EB RID: 4587
		private Vector3 lastUpdatedPos;

		// Token: 0x040011EC RID: 4588
		private Vector3 interpTargetPosition;

		// Token: 0x040011ED RID: 4589
		private Quaternion interpTargetRotation;

		// Token: 0x040011EE RID: 4590
		private Vector3 real;

		// Token: 0x040011EF RID: 4591
		private float lastTick;

		// Token: 0x040011F0 RID: 4592
		private float lastWeeoo;

		// Token: 0x040011F1 RID: 4593
		private AudioSource clipAudioSource;

		// Token: 0x040011F2 RID: 4594
		private WindZone windZone;

		// Token: 0x040011F3 RID: 4595
		private bool isRecovering;

		// Token: 0x040011F4 RID: 4596
		private float lastRecover;

		// Token: 0x040011F5 RID: 4597
		private bool isPhysical;

		// Token: 0x040011F6 RID: 4598
		private bool isFrozen;

		// Token: 0x040011F7 RID: 4599
		public bool isBlimpFloating;

		/// <summary>
		/// Used by several engine modes to represent an interpolated velocity target according to input.
		/// </summary>
		// Token: 0x040011F8 RID: 4600
		private float inputTargetVelocity;

		/// <summary>
		/// Set from inputTargetVelocity then multiplied by any factors which shouldn't affect the player's "target"
		/// speed ike boatTraction.
		/// </summary>
		// Token: 0x040011F9 RID: 4601
		private float inputEngineVelocity;

		/// <summary>
		/// Vehicles with buoyancy interpolate this value according to whether it's in the water, and multiply
		/// boat-related forces by it.
		/// </summary>
		// Token: 0x040011FA RID: 4602
		private float boatTraction;

		// Token: 0x040011FB RID: 4603
		private float batteryBuffer;

		// Token: 0x040011FC RID: 4604
		private float fuelBurnBuffer;

		/// <summary>
		/// Rigidbody on the Vehicle prefab.
		/// (not called "rigidbody" because as of 2024-02-28 the deprecated "rigidbody" property still exists)
		/// </summary>
		// Token: 0x040011FD RID: 4605
		private Rigidbody rootRigidbody;

		// Token: 0x040011FE RID: 4606
		internal static readonly ClientInstanceMethod<Color32> SendPaintColor = ClientInstanceMethod<Color32>.Get(typeof(InteractableVehicle), "ReceivePaintColor");

		/// <summary>
		/// This check should really not be necessary, but somehow it is a recurring issue that servers get slowed down
		/// by something going wrong and the vehicle exploding a billion times leaving items everywhere.
		/// </summary>
		// Token: 0x040011FF RID: 4607
		private bool hasDroppedScrapItemsAlready;

		// Token: 0x04001203 RID: 4611
		private EngineCurvesComponent engineCurvesComponent;

		// Token: 0x04001204 RID: 4612
		private float timeSinceLastGearChange;

		// Token: 0x04001205 RID: 4613
		internal float latestGasInput;

		// Token: 0x04001206 RID: 4614
		public bool hasDefaultCenterOfMass;

		// Token: 0x04001207 RID: 4615
		public Vector3 defaultCenterOfMass;

		// Token: 0x04001208 RID: 4616
		internal List<Collider> _vehicleColliders;

		// Token: 0x04001209 RID: 4617
		private static List<Collider> _trainCarColliders = new List<Collider>(16);

		/// <summary>
		/// Transform used for exit physics queries.
		/// </summary>
		// Token: 0x0400120A RID: 4618
		private Transform center;

		/// <summary>
		/// Skin material does not always need to be destroyed, so this is only valid if it should be destroyed.
		/// </summary>
		// Token: 0x0400120B RID: 4619
		private Material skinMaterialToDestroy;

		/// <summary>
		/// Materials that should be destroyed when this vehicle is destroyed.
		/// </summary>
		// Token: 0x0400120C RID: 4620
		private HashSet<Material> materialsToDestroy = new HashSet<Material>();

		/// <summary>
		/// Handles to unregister from DynamicWaterTransparentSort.
		/// </summary>
		// Token: 0x0400120D RID: 4621
		private List<object> waterSortHandles = new List<object>();

		// Token: 0x0400120E RID: 4622
		private static int PAINT_COLOR_ID = Shader.PropertyToID("_PaintColor");

		/// <summary>
		/// Materials to set _PaintColor on.
		/// </summary>
		// Token: 0x0400120F RID: 4623
		private List<Material> paintableMaterials;

		/// <summary>
		/// Time.time decayTimer was last updated.
		/// </summary>
		// Token: 0x04001210 RID: 4624
		internal float decayLastUpdateTime;

		/// <summary>
		/// Seconds since vehicle was interacted with.
		/// </summary>
		// Token: 0x04001211 RID: 4625
		internal float decayTimer;

		/// <summary>
		/// Fractional damage counter.
		/// </summary>
		// Token: 0x04001212 RID: 4626
		internal float decayPendingDamage;

		/// <summary>
		/// transform.position used to test whether vehicle is moving.
		/// </summary>
		// Token: 0x04001213 RID: 4627
		internal Vector3 decayLastUpdatePosition;

		// Token: 0x04001214 RID: 4628
		[Obsolete]
		public bool isUpdated;
	}
}
