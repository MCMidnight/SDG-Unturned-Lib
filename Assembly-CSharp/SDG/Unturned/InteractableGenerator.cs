using System;
using System.Collections.Generic;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000454 RID: 1108
	public class InteractableGenerator : Interactable, IManualOnDestroy
	{
		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x00080D15 File Offset: 0x0007EF15
		public ushort capacity
		{
			get
			{
				return this._capacity;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002167 RID: 8551 RVA: 0x00080D1D File Offset: 0x0007EF1D
		public float wirerange
		{
			get
			{
				return this._wirerange;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x00080D25 File Offset: 0x0007EF25
		public float sqrWirerange
		{
			get
			{
				return this._sqrWirerange;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002169 RID: 8553 RVA: 0x00080D2D File Offset: 0x0007EF2D
		public bool isRefillable
		{
			get
			{
				return this.fuel < this.capacity;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x00080D3D File Offset: 0x0007EF3D
		public bool isSiphonable
		{
			get
			{
				return this.fuel > 0;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600216B RID: 8555 RVA: 0x00080D48 File Offset: 0x0007EF48
		public bool isPowered
		{
			get
			{
				return this._isPowered;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x00080D50 File Offset: 0x0007EF50
		public ushort fuel
		{
			get
			{
				return this._fuel;
			}
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x00080D58 File Offset: 0x0007EF58
		public void askBurn(ushort amount)
		{
			if (amount == 0)
			{
				return;
			}
			if (amount >= this.fuel)
			{
				this._fuel = 0;
			}
			else
			{
				this._fuel -= amount;
			}
			if (Provider.isServer)
			{
				this.updateState();
			}
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x00080D8C File Offset: 0x0007EF8C
		public void askFill(ushort amount)
		{
			if (amount == 0)
			{
				return;
			}
			if (amount >= this.capacity - this.fuel)
			{
				this._fuel = this.capacity;
			}
			else
			{
				this._fuel += amount;
			}
			if (Provider.isServer)
			{
				this.updateState();
			}
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x00080DCC File Offset: 0x0007EFCC
		public void tellFuel(ushort newFuel)
		{
			this._fuel = newFuel;
			this.updateWire();
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x00080DDB File Offset: 0x0007EFDB
		public void updatePowered(bool newPowered)
		{
			this._isPowered = newPowered;
			this.updateWire();
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x00080DEC File Offset: 0x0007EFEC
		public override void updateState(Asset asset, byte[] state)
		{
			this._capacity = ((ItemGeneratorAsset)asset).capacity;
			this._wirerange = ((ItemGeneratorAsset)asset).wirerange;
			this._sqrWirerange = this.wirerange * this.wirerange;
			this.burn = ((ItemGeneratorAsset)asset).burn;
			this._isPowered = (state[0] == 1);
			this._fuel = BitConverter.ToUInt16(state, 1);
			this.engine = base.transform.Find("Engine");
			if (Provider.isServer)
			{
				this.metadata = state;
			}
			this.updateWire();
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x00080E82 File Offset: 0x0007F082
		public override void use()
		{
			this.ClientToggle();
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x00080E8A File Offset: 0x0007F08A
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.isPowered)
			{
				message = EPlayerMessage.GENERATOR_OFF;
			}
			else
			{
				message = EPlayerMessage.GENERATOR_ON;
			}
			text = "";
			color = Color.white;
			return true;
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x00080EB1 File Offset: 0x0007F0B1
		private void updateState()
		{
			if (this.metadata == null)
			{
				return;
			}
			BitConverter.GetBytes(this.fuel).CopyTo(this.metadata, 1);
		}

		/// <summary>
		/// Catch exceptions to prevent a broken powerable from breaking all the other powerable items in the area.
		/// </summary>
		// Token: 0x06002175 RID: 8565 RVA: 0x00080ED4 File Offset: 0x0007F0D4
		private void updatePowerableIsWired(InteractablePower powerable, bool isWired)
		{
			try
			{
				powerable.updateWired(isWired);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Generator caught exception during updateWired for {0}:", new object[]
				{
					powerable.GetSceneHierarchyPath()
				});
			}
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x00080F18 File Offset: 0x0007F118
		private void updateWire()
		{
			if (this.engine != null)
			{
				this.engine.gameObject.SetActive(this.isPowered && this.fuel > 0);
			}
			bool flag = this.isPowered && this.fuel > 0 && !base.IsChildOfVehicle;
			if (this.isWorldCandidate != flag)
			{
				this.isWorldCandidate = flag;
				if (this.isWorldCandidate)
				{
					InteractableGenerator.worldCandidates.Add(this);
				}
				else
				{
					InteractableGenerator.worldCandidates.RemoveFast(this);
				}
			}
			if (Level.info != null && Level.info.configData != null && Level.info.configData.Has_Global_Electricity)
			{
				return;
			}
			ushort maxValue = ushort.MaxValue;
			if (base.IsChildOfVehicle)
			{
				byte b;
				byte b2;
				BarricadeRegion barricadeRegion;
				BarricadeManager.tryGetPlant(base.transform.parent, out b, out b2, out maxValue, out barricadeRegion);
			}
			List<InteractablePower> list = PowerTool.checkPower(base.transform.position, this.wirerange, maxValue);
			for (int i = 0; i < list.Count; i++)
			{
				InteractablePower interactablePower = list[i];
				if (interactablePower.isWired)
				{
					if (!this.isPowered || this.fuel == 0)
					{
						bool flag2;
						if (maxValue == 65535)
						{
							flag2 = InteractableGenerator.IsWorldPositionPowered(interactablePower.transform.position);
						}
						else
						{
							flag2 = false;
							List<InteractableGenerator> list2 = PowerTool.checkGenerators(interactablePower.transform.position, PowerTool.MAX_POWER_RANGE, maxValue);
							for (int j = 0; j < list2.Count; j++)
							{
								if (list2[j] != this && list2[j].isPowered && list2[j].fuel > 0 && (list2[j].transform.position - interactablePower.transform.position).sqrMagnitude < list2[j].sqrWirerange)
								{
									flag2 = true;
									break;
								}
							}
						}
						if (!flag2)
						{
							this.updatePowerableIsWired(interactablePower, false);
						}
					}
				}
				else if (this.isPowered && this.fuel > 0)
				{
					this.updatePowerableIsWired(interactablePower, true);
				}
			}
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x00081140 File Offset: 0x0007F340
		public void ManualOnDestroy()
		{
			this.updatePowered(false);
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x00081149 File Offset: 0x0007F349
		private void OnEnable()
		{
			this.lastBurn = Time.realtimeSinceStartup;
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x00081158 File Offset: 0x0007F358
		private void Update()
		{
			if (Time.realtimeSinceStartup - this.lastBurn > this.burn)
			{
				this.lastBurn = Time.realtimeSinceStartup;
				if (this.isPowered)
				{
					if (this.fuel > 0)
					{
						this.isWiring = true;
						this.askBurn(1);
						return;
					}
					if (this.isWiring)
					{
						this.isWiring = false;
						this.updateWire();
					}
				}
			}
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000811B9 File Offset: 0x0007F3B9
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveFuel(ushort newFuel)
		{
			this.tellFuel(newFuel);
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000811C2 File Offset: 0x0007F3C2
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceivePowered(bool newPowered)
		{
			this.updatePowered(newPowered);
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000811CB File Offset: 0x0007F3CB
		public void ClientToggle()
		{
			InteractableGenerator.SendToggleRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, !this.isPowered);
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000811E8 File Offset: 0x0007F3E8
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2)]
		public void ReceiveToggleRequest(in ServerInvocationContext context, bool desiredPowered)
		{
			if (this.isPowered == desiredPowered)
			{
				return;
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out region))
			{
				return;
			}
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if ((base.transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			BarricadeManager.ServerSetGeneratorPoweredInternal(this, x, y, plant, region, !this.isPowered);
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x00081288 File Offset: 0x0007F488
		internal static bool IsWorldPositionPowered(Vector3 position)
		{
			foreach (InteractableGenerator interactableGenerator in InteractableGenerator.worldCandidates)
			{
				if ((interactableGenerator.transform.position - position).sqrMagnitude < interactableGenerator.sqrWirerange)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x000812FC File Offset: 0x0007F4FC
		private void OnDestroy()
		{
			if (this.isWorldCandidate)
			{
				this.isWorldCandidate = false;
				InteractableGenerator.worldCandidates.RemoveFast(this);
			}
		}

		// Token: 0x04001065 RID: 4197
		private ushort _capacity;

		// Token: 0x04001066 RID: 4198
		private float _wirerange;

		// Token: 0x04001067 RID: 4199
		private float _sqrWirerange;

		// Token: 0x04001068 RID: 4200
		private float burn;

		// Token: 0x04001069 RID: 4201
		private bool _isPowered;

		// Token: 0x0400106A RID: 4202
		private ushort _fuel;

		// Token: 0x0400106B RID: 4203
		private Transform engine;

		// Token: 0x0400106C RID: 4204
		private float lastBurn;

		// Token: 0x0400106D RID: 4205
		private bool isWiring;

		// Token: 0x0400106E RID: 4206
		private byte[] metadata;

		// Token: 0x0400106F RID: 4207
		internal static readonly ClientInstanceMethod<ushort> SendFuel = ClientInstanceMethod<ushort>.Get(typeof(InteractableGenerator), "ReceiveFuel");

		// Token: 0x04001070 RID: 4208
		internal static readonly ClientInstanceMethod<bool> SendPowered = ClientInstanceMethod<bool>.Get(typeof(InteractableGenerator), "ReceivePowered");

		// Token: 0x04001071 RID: 4209
		private static readonly ServerInstanceMethod<bool> SendToggleRequest = ServerInstanceMethod<bool>.Get(typeof(InteractableGenerator), "ReceiveToggleRequest");

		/// <summary>
		/// Unsorted list of world space generators turned-on and fueled.
		/// </summary>
		// Token: 0x04001072 RID: 4210
		private static List<InteractableGenerator> worldCandidates = new List<InteractableGenerator>(40);

		// Token: 0x04001073 RID: 4211
		private bool isWorldCandidate;
	}
}
