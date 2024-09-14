using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000624 RID: 1572
	public class PlayerInput : PlayerCaller
	{
		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060032AA RID: 12970 RVA: 0x000E50A4 File Offset: 0x000E32A4
		public float tick
		{
			get
			{
				return this._tick;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x000E50AC File Offset: 0x000E32AC
		public uint simulation
		{
			get
			{
				return this._simulation;
			}
		}

		/// <summary>
		/// Whether client is currently penalized for potentially using a lag switch. False positives are relatively
		/// likely when client framerate hitches (e.g. loading dense region), so we only modify their stats (e.g. reduce
		/// player damage) for a corresponding duration.
		/// </summary>
		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060032AC RID: 12972 RVA: 0x000E50B4 File Offset: 0x000E32B4
		public bool IsUnderFakeLagPenalty
		{
			get
			{
				return this.fakeLagPenaltyFrames > 0;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060032AD RID: 12973 RVA: 0x000E50BF File Offset: 0x000E32BF
		public uint clock
		{
			get
			{
				return this._clock;
			}
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x000E50C7 File Offset: 0x000E32C7
		public bool IsPluginKeyHeld(int index)
		{
			return this.keys[this.keys.Length - (int)ControlsSettings.NUM_PLUGIN_KEYS + index];
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x060032AF RID: 12975 RVA: 0x000E50E0 File Offset: 0x000E32E0
		// (set) Token: 0x060032B0 RID: 12976 RVA: 0x000E50E8 File Offset: 0x000E32E8
		public bool[] keys { get; protected set; }

		// Token: 0x060032B1 RID: 12977 RVA: 0x000E50F1 File Offset: 0x000E32F1
		public bool hasInputs()
		{
			return this.inputs != null && this.inputs.Count > 0;
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x000E510B File Offset: 0x000E330B
		public int getInputCount()
		{
			if (this.inputs == null)
			{
				return 0;
			}
			return this.inputs.Count;
		}

		/// <summary>
		/// Get the hit result of a raycast on the server. Until a generic way to address net objects is implemented
		/// this is how legacy features specify which player/animal/zombie/vehicle/etc they want to interact with.
		/// </summary>
		// Token: 0x060032B3 RID: 12979 RVA: 0x000E5124 File Offset: 0x000E3324
		public InputInfo getInput(bool doOcclusionCheck, ERaycastInfoUsage usage)
		{
			if (this.inputs == null)
			{
				return null;
			}
			while (this.inputs.Count > 0)
			{
				PlayerInput.<>c__DisplayClass31_0 CS$<>8__locals1;
				CS$<>8__locals1.inputInfo = this.inputs.Dequeue();
				if (CS$<>8__locals1.inputInfo == null)
				{
					return null;
				}
				if (CS$<>8__locals1.inputInfo.usage == usage)
				{
					if (doOcclusionCheck && !this.hasDoneOcclusionCheck)
					{
						this.hasDoneOcclusionCheck = true;
						if (CS$<>8__locals1.inputInfo != null)
						{
							Vector3 a = CS$<>8__locals1.inputInfo.point - base.player.look.aim.position;
							float magnitude = a.magnitude;
							Vector3 vector = a / magnitude;
							if (magnitude > 0.025f)
							{
								Physics.Raycast(new Ray(base.player.look.aim.position, vector), out this.obstruction, magnitude - 0.025f, RayMasks.DAMAGE_SERVER);
								if (this.obstruction.transform != null && !this.<getInput>g__IsObstructionHitValid|31_0(ref CS$<>8__locals1))
								{
									return null;
								}
								Physics.Raycast(new Ray(base.player.look.aim.position + vector * (magnitude - 0.025f), -vector), out this.obstruction, magnitude - 0.025f, RayMasks.DAMAGE_SERVER);
								if (this.obstruction.transform != null && !this.<getInput>g__IsObstructionHitValid|31_0(ref CS$<>8__locals1))
								{
									return null;
								}
							}
						}
					}
					return CS$<>8__locals1.inputInfo;
				}
			}
			return null;
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x000E52A3 File Offset: 0x000E34A3
		[Obsolete("Use the overload of getInput that takes an expected usage parameter instead.")]
		public InputInfo getInput(bool doOcclusionCheck)
		{
			return null;
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x000E52A8 File Offset: 0x000E34A8
		public bool isRaycastInvalid(RaycastInfo info)
		{
			return info.player == null && info.zombie == null && info.animal == null && info.vehicle == null && info.transform == null;
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x000E52FC File Offset: 0x000E34FC
		public void sendRaycast(RaycastInfo info, ERaycastInfoUsage usage)
		{
			if (this.isRaycastInvalid(info))
			{
				return;
			}
			if (Provider.isServer)
			{
				InputInfo inputInfo = new InputInfo();
				inputInfo.usage = usage;
				inputInfo.animal = info.animal;
				inputInfo.direction = info.direction;
				inputInfo.limb = info.limb;
				inputInfo.materialName = info.materialName;
				inputInfo.material = info.material;
				inputInfo.normal = info.normal;
				inputInfo.player = info.player;
				inputInfo.point = info.point;
				inputInfo.transform = info.transform;
				InputInfo inputInfo2 = inputInfo;
				Collider collider = info.collider;
				inputInfo2.colliderTransform = ((collider != null) ? collider.transform : null);
				inputInfo.vehicle = info.vehicle;
				inputInfo.zombie = info.zombie;
				inputInfo.section = info.section;
				if (inputInfo.player != null)
				{
					inputInfo.type = ERaycastInfoType.PLAYER;
				}
				else if (inputInfo.zombie != null)
				{
					inputInfo.type = ERaycastInfoType.ZOMBIE;
				}
				else if (inputInfo.animal != null)
				{
					inputInfo.type = ERaycastInfoType.ANIMAL;
				}
				else if (inputInfo.vehicle != null)
				{
					inputInfo.type = ERaycastInfoType.VEHICLE;
				}
				else if (inputInfo.transform != null)
				{
					if (inputInfo.transform.CompareTag("Barricade"))
					{
						inputInfo.type = ERaycastInfoType.BARRICADE;
					}
					else if (info.transform.CompareTag("Structure"))
					{
						inputInfo.type = ERaycastInfoType.STRUCTURE;
					}
					else if (info.transform.CompareTag("Resource"))
					{
						inputInfo.type = ERaycastInfoType.RESOURCE;
					}
					else if (inputInfo.transform.CompareTag("Small") || inputInfo.transform.CompareTag("Medium") || inputInfo.transform.CompareTag("Large"))
					{
						inputInfo.type = ERaycastInfoType.OBJECT;
					}
					else if (info.transform.CompareTag("Ground") || info.transform.CompareTag("Environment"))
					{
						inputInfo.type = ERaycastInfoType.NONE;
					}
					else
					{
						inputInfo = null;
					}
				}
				else
				{
					inputInfo = null;
				}
				if (inputInfo != null)
				{
					this.inputs.Enqueue(inputInfo);
					return;
				}
			}
			else
			{
				if (this.clientPendingInput.clientsideInputs == null)
				{
					this.clientPendingInput.clientsideInputs = new List<PlayerInputPacket.ClientRaycast>();
				}
				this.clientPendingInput.clientsideInputs.Add(new PlayerInputPacket.ClientRaycast(info, usage));
			}
		}

		/// <summary>
		/// Set rollingWindowIndex to newIndex, zeroing all input counts along the way.
		/// Important to zero the intermediary indexes in-case server stalled for more than one second.
		/// </summary>
		// Token: 0x060032B7 RID: 12983 RVA: 0x000E5559 File Offset: 0x000E3759
		private void advanceRollingWindowIndex(int newIndex)
		{
			do
			{
				this.rollingWindowIndex++;
				if (this.rollingWindowIndex >= this.rollingWindow.Length)
				{
					this.rollingWindowIndex = 0;
				}
				this.rollingWindow[this.rollingWindowIndex] = 0;
			}
			while (this.rollingWindowIndex != newIndex);
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x000E5597 File Offset: 0x000E3797
		private void internalDismiss()
		{
			this.isDismissed = true;
			Provider.dismiss(base.channel.owner.playerID.steamID);
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x000E55BC File Offset: 0x000E37BC
		private void ClientRemoveInputHistory(uint frameNumber)
		{
			if (this.clientInputHistory.IsEmpty<ClientMovementInput>())
			{
				return;
			}
			if (this.clientInputHistory[0].frameNumber <= frameNumber)
			{
				int num = 1;
				while (num < this.clientInputHistory.Count && this.clientInputHistory[num].frameNumber <= frameNumber)
				{
					num++;
				}
				this.clientInputHistory.RemoveRange(0, num);
			}
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x000E5624 File Offset: 0x000E3824
		private void ClientResimulate()
		{
			this.ClientRemoveInputHistory(this.clientResimulationFrameNumber);
			if (base.player.movement.getVehicle() != null)
			{
				return;
			}
			if (base.player.movement.hasPendingVehicleChange)
			{
				return;
			}
			if (!base.player.movement.controller.enabled)
			{
				return;
			}
			this.isResimulating = true;
			base.player.stance.internalSetStance(this.clientResimulationStance);
			Quaternion rotation = base.transform.rotation;
			Quaternion rotation2 = base.player.look.aim.rotation;
			base.player.movement.controller.enabled = false;
			base.transform.position = this.clientResimulationPosition;
			base.player.movement.controller.enabled = true;
			base.player.movement.velocity = this.clientResimulationVelocity;
			base.player.life.internalSetStamina(this.clientResimulationStamina);
			base.player.life.lastTire = MathfEx.ClampLongToUInt((long)((ulong)this.clientResimulationFrameNumber - (ulong)((long)this.clientResimulationLastTireOffset)));
			base.player.life.lastRest = MathfEx.ClampLongToUInt((long)((ulong)this.clientResimulationFrameNumber - (ulong)((long)this.clientResimulationLastRestOffset)));
			foreach (ClientMovementInput clientMovementInput in this.clientInputHistory)
			{
				base.transform.rotation = clientMovementInput.rotation;
				base.player.look.aim.rotation = clientMovementInput.aimRotation;
				base.player.life.SimulateStaminaFrame(clientMovementInput.frameNumber);
				base.player.stance.simulate(clientMovementInput.frameNumber, clientMovementInput.crouch, clientMovementInput.prone, clientMovementInput.sprint);
				base.player.movement.simulate(clientMovementInput.frameNumber, 0, clientMovementInput.input_x, clientMovementInput.input_y, 0f, 0f, clientMovementInput.jump, false, PlayerInput.RATE);
			}
			base.transform.rotation = rotation;
			base.player.look.aim.rotation = rotation2;
			this.isResimulating = false;
		}

		/// <summary>
		/// Notify client there has been a prediction error, so movement needs to be re-simulated.
		/// </summary>
		// Token: 0x060032BB RID: 12987 RVA: 0x000E5884 File Offset: 0x000E3A84
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveSimulateMispredictedInputs(uint frameNumber, EPlayerStance stance, Vector3 position, Vector3 velocity, byte stamina, int lastTireOffset, int lastRestOffset)
		{
			this.clientHasPendingResimulation = true;
			this.clientResimulationFrameNumber = frameNumber;
			this.clientResimulationStance = stance;
			this.clientResimulationPosition = position;
			this.clientResimulationVelocity = velocity;
			this.clientResimulationStamina = stamina;
			this.clientResimulationLastTireOffset = lastTireOffset;
			this.clientResimulationLastRestOffset = lastRestOffset;
		}

		/// <summary>
		/// Notify client old inputs can be discarded because they were predicted correctly.
		/// </summary>
		// Token: 0x060032BC RID: 12988 RVA: 0x000E58C2 File Offset: 0x000E3AC2
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveAckGoodInputs(uint frameNumber)
		{
			if (this.clientHasPendingResimulation)
			{
				if (frameNumber <= this.clientResimulationFrameNumber)
				{
					return;
				}
				this.clientHasPendingResimulation = false;
			}
			this.ClientRemoveInputHistory(frameNumber);
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x000E58E6 File Offset: 0x000E3AE6
		[Obsolete]
		public void askInput(CSteamID steamID)
		{
		}

		/// <summary>
		/// Not using rate limit attribute because it internally keeps a rolling window limit.
		/// </summary>
		// Token: 0x060032BE RID: 12990 RVA: 0x000E58E8 File Offset: 0x000E3AE8
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER)]
		public void ReceiveInputs(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			if (this.isDismissed)
			{
				return;
			}
			if (this.serversideAskInputCount == 0)
			{
				this.initialServersideAskInputTime = Time.realtimeSinceStartup;
			}
			this.serversideAskInputCount++;
			float num = Time.realtimeSinceStartup - this.initialServersideAskInputTime;
			int num2 = (int)num % PlayerInput.ASKINPUT_WINDOW_LENGTH;
			if (num2 != this.rollingWindowIndex)
			{
				this.advanceRollingWindowIndex(num2);
				if (Provider.configData.Server.Enable_Kick_Input_Spam && num > (float)PlayerInput.ASKINPUT_WINDOW_LENGTH)
				{
					if (Time.realtimeSinceStartup - this.latestAskInputDismissTestTime < (float)(PlayerInput.ASKINPUT_WINDOW_LENGTH / 2))
					{
						int num3 = 0;
						foreach (int num4 in this.rollingWindow)
						{
							num3 += num4;
						}
						float num5 = (float)(num3 / PlayerInput.ASKINPUT_WINDOW_LENGTH);
						if (num5 > (float)PlayerInput.KICK_ASKINPUT_PER_SECOND)
						{
							string text = Mathf.RoundToInt(num5 / PlayerInput.EXPECTED_ASKINPUT_PER_SECOND * 100f).ToString();
							UnturnedLog.warn("Received {0}% of expected input packets from {1} over the past {2} seconds, so we're dismissing them", new object[]
							{
								text,
								base.channel.owner.playerID.steamID,
								PlayerInput.ASKINPUT_WINDOW_LENGTH
							});
							this.internalDismiss();
							return;
						}
					}
					this.latestAskInputDismissTestTime = Time.realtimeSinceStartup;
				}
			}
			this.rollingWindow[this.rollingWindowIndex]++;
			if (this.rollingWindow[this.rollingWindowIndex] > PlayerInput.MAX_ASKINPUT_PER_SECOND)
			{
				return;
			}
			bool flag;
			reader.ReadBit(ref flag);
			PlayerInputPacket playerInputPacket;
			if (flag)
			{
				playerInputPacket = new DrivingPlayerInputPacket(base.player.movement.getVehicle());
			}
			else
			{
				playerInputPacket = new WalkingPlayerInputPacket();
			}
			playerInputPacket.read(base.channel, reader);
			if (this.serverLastReceivedSimulationFrameNumber != 4294967295U && playerInputPacket.clientSimulationFrameNumber <= this.serverLastReceivedSimulationFrameNumber)
			{
				return;
			}
			this.serverLastReceivedSimulationFrameNumber = playerInputPacket.clientSimulationFrameNumber;
			this.serversidePackets.Enqueue(playerInputPacket);
			float num6 = Time.realtimeSinceStartup - this.lastInputed;
			if (this.hasInputed && num6 > 1f && num6 > Provider.configData.Server.Fake_Lag_Threshold_Seconds)
			{
				if (Provider.configData.Server.Fake_Lag_Log_Warnings)
				{
					CommandWindow.LogWarning(string.Format("{0} seconds between inputs from \"{1}\" steamid: {2}", num6, base.channel.owner.playerID.playerName, base.channel.owner.playerID.steamID));
				}
				float num7 = num6 - 1f;
				this.fakeLagPenaltyFrames += Mathf.CeilToInt(num7 / PlayerInput.RATE);
			}
			this.lastInputed = Time.realtimeSinceStartup;
			this.hasInputed = true;
		}

		/// <summary>
		/// Only bound on dedicated server.
		/// When dieing in a vehicle this prevents delay handling packets.
		/// </summary>
		// Token: 0x060032BF RID: 12991 RVA: 0x000E5B97 File Offset: 0x000E3D97
		private void onLifeUpdated(bool isDead)
		{
			this.serversidePackets.Clear();
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x000E5BA4 File Offset: 0x000E3DA4
		private void FixedUpdate()
		{
			if (this.isDismissed)
			{
				return;
			}
			if (base.channel.IsLocalPlayer)
			{
				if (this.count % PlayerInput.SAMPLES == 0U)
				{
					this._tick = Time.realtimeSinceStartup;
					if (this.clientHasPendingResimulation)
					{
						this.clientHasPendingResimulation = false;
						this.ClientResimulate();
					}
					this.keys[0] = base.player.movement.jump;
					this.keys[1] = false;
					this.keys[2] = false;
					this.keys[3] = base.player.stance.crouch;
					this.keys[4] = base.player.stance.prone;
					this.keys[5] = base.player.stance.sprint;
					this.keys[6] = base.player.animator.leanLeft;
					this.keys[7] = base.player.animator.leanRight;
					this.keys[8] = false;
					this.keys[9] = base.player.stance.localWantsToSteadyAim;
					bool flag = MenuConfigurationControlsUI.binding == byte.MaxValue;
					for (int i = 0; i < (int)ControlsSettings.NUM_PLUGIN_KEYS; i++)
					{
						int num = this.keys.Length - (int)ControlsSettings.NUM_PLUGIN_KEYS + i;
						this.keys[num] = (flag && InputEx.GetKey(ControlsSettings.getPluginKeyCode(i)));
					}
					base.player.equipment.CaptureAttackInputs(out this.pendingPrimaryAttackInput, out this.pendingSecondaryAttackInput);
					base.player.life.simulate(this.simulation);
					bool crouch = base.player.stance.crouch;
					bool prone = base.player.stance.prone;
					bool sprint = base.player.stance.sprint;
					base.player.stance.simulate(this.simulation, crouch, prone, sprint);
					int input_x = (int)(base.player.movement.horizontal - 1);
					int input_y = (int)(base.player.movement.vertical - 1);
					bool jump = base.player.movement.jump;
					base.player.movement.simulate(this.simulation, 0, input_x, input_y, base.player.look.look_x, base.player.look.look_y, jump, sprint, PlayerInput.RATE);
					if (Provider.isServer)
					{
						this.inputs.Clear();
					}
					else
					{
						if (base.player.stance.stance == EPlayerStance.DRIVING)
						{
							this.clientPendingInput = new DrivingPlayerInputPacket(base.player.movement.getVehicle());
						}
						else
						{
							this.clientPendingInput = new WalkingPlayerInputPacket
							{
								analog = (byte)((int)base.player.movement.horizontal << 4 | (int)base.player.movement.vertical),
								clientPosition = base.transform.position,
								pitch = base.player.look.pitch,
								yaw = base.player.look.yaw
							};
							ClientMovementInput clientMovementInput = default(ClientMovementInput);
							clientMovementInput.frameNumber = this.simulation;
							clientMovementInput.crouch = crouch;
							clientMovementInput.prone = prone;
							clientMovementInput.input_x = input_x;
							clientMovementInput.input_y = input_y;
							clientMovementInput.jump = jump;
							clientMovementInput.sprint = sprint;
							clientMovementInput.rotation = base.transform.rotation;
							clientMovementInput.aimRotation = base.player.look.aim.rotation;
							this.clientInputHistory.Add(clientMovementInput);
						}
						this.clientPendingInput.clientSimulationFrameNumber = this.simulation;
						this.clientPendingInput.recov = this.recov;
					}
					base.player.equipment.simulate(this.simulation, this.pendingPrimaryAttackInput, this.pendingSecondaryAttackInput, base.player.stance.localWantsToSteadyAim);
					base.player.animator.simulate(this.simulation, base.player.animator.leanLeft, base.player.animator.leanRight);
					this.buffer += PlayerInput.SAMPLES;
					this._simulation += 1U;
				}
				if (this.consumed < this.buffer)
				{
					this.consumed += 1U;
					base.player.equipment.tock(this.clock);
					this._clock += 1U;
				}
				if (this.consumed == this.buffer && this.clientPendingInput != null && !Provider.isServer)
				{
					ushort num2 = 0;
					byte b = 0;
					while ((int)b < this.keys.Length)
					{
						if (this.keys[(int)b])
						{
							num2 |= this.flags[(int)b];
						}
						b += 1;
					}
					this.clientPendingInput.keys = num2;
					this.clientPendingInput.primaryAttack = this.pendingPrimaryAttackInput;
					this.clientPendingInput.secondaryAttack = this.pendingSecondaryAttackInput;
					if (this.clientPendingInput is DrivingPlayerInputPacket)
					{
						DrivingPlayerInputPacket drivingPlayerInputPacket = this.clientPendingInput as DrivingPlayerInputPacket;
						InteractableVehicle vehicle = base.player.movement.getVehicle();
						if (vehicle != null)
						{
							drivingPlayerInputPacket.vehicle = vehicle;
							Transform transform = vehicle.transform;
							if (vehicle.asset.engine == EEngine.TRAIN)
							{
								drivingPlayerInputPacket.position = InteractableVehicle.PackRoadPosition(vehicle.roadPosition);
							}
							else
							{
								drivingPlayerInputPacket.position = transform.position;
							}
							drivingPlayerInputPacket.rotation = transform.rotation;
							drivingPlayerInputPacket.speed = vehicle.ReplicatedSpeed;
							drivingPlayerInputPacket.forwardVelocity = vehicle.ReplicatedForwardVelocity;
							drivingPlayerInputPacket.steeringInput = vehicle.ReplicatedSteeringInput;
							drivingPlayerInputPacket.velocityInput = vehicle.ReplicatedVelocityInput;
						}
					}
					if (true & Provider.isConnected)
					{
						PlayerInput.SendInputs.Invoke(base.GetNetId(), ENetReliability.Reliable, delegate(NetPakWriter writer)
						{
							if (this.clientPendingInput is DrivingPlayerInputPacket)
							{
								writer.WriteBit(true);
							}
							else
							{
								writer.WriteBit(false);
							}
							this.clientPendingInput.write(writer);
						});
					}
				}
				this.count += 1U;
				return;
			}
			if (Provider.isServer)
			{
				if (this.serversidePackets.Count > 0)
				{
					PlayerInputPacket playerInputPacket = this.serversidePackets.Peek();
					if (playerInputPacket is WalkingPlayerInputPacket || this.count % PlayerInput.SAMPLES == 0U)
					{
						if (this.simulation > (uint)((Time.realtimeSinceStartup + 5f - this.tick) / PlayerInput.RATE))
						{
							return;
						}
						playerInputPacket = this.serversidePackets.Dequeue();
						if (playerInputPacket == null)
						{
							return;
						}
						this.hasDoneOcclusionCheck = false;
						this.inputs = playerInputPacket.serversideInputs;
						byte b2 = 0;
						while ((int)b2 < this.keys.Length)
						{
							this.keys[(int)b2] = ((playerInputPacket.keys & this.flags[(int)b2]) == this.flags[(int)b2]);
							b2 += 1;
						}
						this.pendingPrimaryAttackInput = playerInputPacket.primaryAttack;
						this.pendingSecondaryAttackInput = playerInputPacket.secondaryAttack;
						if (playerInputPacket is DrivingPlayerInputPacket)
						{
							DrivingPlayerInputPacket drivingPlayerInputPacket2 = playerInputPacket as DrivingPlayerInputPacket;
							if (base.player.life.IsAlive)
							{
								base.player.life.simulate(this.simulation);
								base.player.look.simulate(0f, 0f, PlayerInput.RATE);
								base.player.stance.simulate(this.simulation, false, false, false);
								base.player.movement.simulate(this.simulation, drivingPlayerInputPacket2.recov, this.keys[0], this.keys[5], drivingPlayerInputPacket2.position, drivingPlayerInputPacket2.rotation, drivingPlayerInputPacket2.speed, drivingPlayerInputPacket2.forwardVelocity, drivingPlayerInputPacket2.steeringInput, drivingPlayerInputPacket2.velocityInput, PlayerInput.RATE);
								base.player.equipment.simulate(this.simulation, this.pendingPrimaryAttackInput, this.pendingSecondaryAttackInput, this.keys[9]);
								base.player.animator.simulate(this.simulation, false, false);
							}
						}
						else
						{
							WalkingPlayerInputPacket walkingPlayerInputPacket = playerInputPacket as WalkingPlayerInputPacket;
							byte analog = walkingPlayerInputPacket.analog;
							if (base.player.life.IsAlive)
							{
								base.player.life.simulate(this.simulation);
								base.player.look.simulate(walkingPlayerInputPacket.yaw, walkingPlayerInputPacket.pitch, PlayerInput.RATE);
								base.player.stance.simulate(this.simulation, this.keys[3], this.keys[4], this.keys[5]);
								int input_x2 = (analog >> 4 & 15) - 1;
								int input_y2 = (int)((analog & 15) - 1);
								bool inputJump = this.keys[0];
								bool inputSprint = this.keys[5];
								base.player.movement.simulate(this.simulation, walkingPlayerInputPacket.recov, input_x2, input_y2, 0f, 0f, inputJump, inputSprint, PlayerInput.RATE);
								base.player.equipment.simulate(this.simulation, this.pendingPrimaryAttackInput, this.pendingSecondaryAttackInput, this.keys[9]);
								base.player.animator.simulate(this.simulation, this.keys[6], this.keys[7]);
								if (!base.player.movement.hasPendingVehicleChange && base.player.stance.stance != EPlayerStance.DRIVING && base.player.stance.stance != EPlayerStance.SITTING)
								{
									Vector3 position = base.transform.position;
									if ((walkingPlayerInputPacket.clientPosition - position).sqrMagnitude > 0.0004f)
									{
										int arg = (int)(this.simulation - base.player.life.lastTire);
										int arg2 = (int)(this.simulation - base.player.life.lastRest);
										PlayerInput.SendSimulateMispredictedInputs.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GetOwnerTransportConnection(), walkingPlayerInputPacket.clientSimulationFrameNumber, base.player.stance.stance, position, base.player.movement.velocity, base.player.life.stamina, arg, arg2);
									}
									else
									{
										PlayerInput.SendAckGoodInputs.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GetOwnerTransportConnection(), walkingPlayerInputPacket.clientSimulationFrameNumber);
									}
								}
							}
						}
						if (PlayerInput.onPluginKeyTick != null)
						{
							for (byte b3 = 0; b3 < ControlsSettings.NUM_PLUGIN_KEYS; b3 += 1)
							{
								int num3 = this.keys.Length - (int)ControlsSettings.NUM_PLUGIN_KEYS + (int)b3;
								PlayerInput.onPluginKeyTick(base.player, this.simulation, b3, this.keys[num3]);
							}
						}
						this.buffer += PlayerInput.SAMPLES;
						this._simulation += 1U;
						while (this.consumed < this.buffer)
						{
							this.consumed += 1U;
							if (base.player.life.IsAlive)
							{
								base.player.equipment.tock(this.clock);
							}
							this._clock += 1U;
						}
						this.fakeLagPenaltyFrames = Mathf.Max(0, this.fakeLagPenaltyFrames - 1);
					}
					this.count += 1U;
					return;
				}
				base.player.movement.simulate();
				if (this.hasInputed && Time.realtimeSinceStartup - this.lastInputed > 20f && Provider.configData.Server.Enable_Kick_Input_Timeout)
				{
					UnturnedLog.warn("Haven't received input from {0} for the past 20 seconds, so we're dismissing them", new object[]
					{
						base.channel.owner.playerID.steamID
					});
					this.internalDismiss();
				}
			}
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x000E676C File Offset: 0x000E496C
		internal void InitializePlayer()
		{
			this._tick = Time.realtimeSinceStartup;
			this._simulation = 0U;
			this._clock = 0U;
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				this.keys = new bool[(int)(10 + ControlsSettings.NUM_PLUGIN_KEYS)];
				this.flags = new ushort[(int)(10 + ControlsSettings.NUM_PLUGIN_KEYS)];
				byte b = 0;
				while ((int)b < this.keys.Length)
				{
					this.flags[(int)b] = (ushort)(1 << (int)b);
					b += 1;
				}
			}
			if (base.channel.IsLocalPlayer && Provider.isServer)
			{
				this.hasDoneOcclusionCheck = false;
				this.inputs = new Queue<InputInfo>();
			}
			if (base.channel.IsLocalPlayer)
			{
				this.clientPendingInput = null;
				this.clientInputHistory = new List<ClientMovementInput>();
			}
			else if (Provider.isServer)
			{
				this.serversidePackets = new Queue<PlayerInputPacket>();
				PlayerLife life = base.player.life;
				life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			}
			this.recov = -1;
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x000E6958 File Offset: 0x000E4B58
		[CompilerGenerated]
		private bool <getInput>g__IsObstructionHitValid|31_0(ref PlayerInput.<>c__DisplayClass31_0 A_1)
		{
			if (A_1.inputInfo.transform == null)
			{
				return this.obstruction.transform.CompareTag("Ground") || this.obstruction.transform.CompareTag("Environment");
			}
			return this.obstruction.transform.IsChildOf(A_1.inputInfo.transform);
		}

		// Token: 0x04001D1F RID: 7455
		public static readonly uint SAMPLES = 4U;

		// Token: 0x04001D20 RID: 7456
		public static readonly float RATE = 0.08f;

		/// <summary>
		/// Calls to UseableGun.tock per second.
		/// </summary>
		// Token: 0x04001D21 RID: 7457
		public static readonly uint TOCK_PER_SECOND = 50U;

		// Token: 0x04001D22 RID: 7458
		private const int VANILLA_DIGITAL_KEYS = 10;

		/// <summary>
		/// Called for every input packet received allowing plugins to listen for a few special
		/// keys they can display in chat/effect UIs.
		/// </summary>
		// Token: 0x04001D23 RID: 7459
		public static PluginKeyTickHandler onPluginKeyTick;

		// Token: 0x04001D24 RID: 7460
		private float _tick;

		// Token: 0x04001D25 RID: 7461
		private uint buffer;

		// Token: 0x04001D26 RID: 7462
		private uint consumed;

		// Token: 0x04001D27 RID: 7463
		private uint count;

		// Token: 0x04001D28 RID: 7464
		private uint _simulation;

		// Token: 0x04001D29 RID: 7465
		private uint _clock;

		// Token: 0x04001D2B RID: 7467
		private EAttackInputFlags pendingPrimaryAttackInput;

		// Token: 0x04001D2C RID: 7468
		private EAttackInputFlags pendingSecondaryAttackInput;

		// Token: 0x04001D2D RID: 7469
		private ushort[] flags;

		// Token: 0x04001D2E RID: 7470
		private bool hasDoneOcclusionCheck;

		// Token: 0x04001D2F RID: 7471
		private Queue<InputInfo> inputs;

		// Token: 0x04001D30 RID: 7472
		private PlayerInputPacket clientPendingInput;

		// Token: 0x04001D31 RID: 7473
		private List<ClientMovementInput> clientInputHistory;

		// Token: 0x04001D32 RID: 7474
		private Queue<PlayerInputPacket> serversidePackets;

		/// <summary>
		/// Ideally simulation frame number would be signed, but there is a lot of code expecting unsigned.
		/// </summary>
		// Token: 0x04001D33 RID: 7475
		private uint serverLastReceivedSimulationFrameNumber = uint.MaxValue;

		// Token: 0x04001D34 RID: 7476
		public int recov;

		// Token: 0x04001D35 RID: 7477
		private RaycastHit obstruction;

		// Token: 0x04001D36 RID: 7478
		private float lastInputed;

		// Token: 0x04001D37 RID: 7479
		private bool hasInputed;

		// Token: 0x04001D38 RID: 7480
		private bool isDismissed;

		/// <summary>
		/// askInput is always called the same number of times per second because it's run from FixedUpdate,
		/// but the spacing between calls can vary depending on network and whether client FPS is low.
		/// </summary>
		// Token: 0x04001D39 RID: 7481
		private static readonly float EXPECTED_ASKINPUT_PER_SECOND = 1f / PlayerInput.RATE;

		/// <summary>
		/// If average askInput calls per second exceeds this, we either ignore their request or flat-out kick them.
		/// </summary>
		// Token: 0x04001D3A RID: 7482
		private static readonly int MAX_ASKINPUT_PER_SECOND = (int)(PlayerInput.EXPECTED_ASKINPUT_PER_SECOND + 3f);

		/// <summary>
		/// If average askInput calls per second exceeds this we silently kick them.
		/// </summary>
		// Token: 0x04001D3B RID: 7483
		private static readonly int KICK_ASKINPUT_PER_SECOND = (int)(PlayerInput.EXPECTED_ASKINPUT_PER_SECOND * 5f);

		/// <summary>
		/// Number of times askInput has been called by client.
		/// Even with huge packet loss, we know that 
		/// </summary>
		// Token: 0x04001D3C RID: 7484
		private int serversideAskInputCount;

		/// <summary>
		/// Realtime that the first call to askInput was made by the client.
		/// </summary>
		// Token: 0x04001D3D RID: 7485
		private float initialServersideAskInputTime = -1f;

		/// <summary>
		/// Realtime that the previous askInput kick test was performed.
		/// </summary>
		// Token: 0x04001D3E RID: 7486
		private float latestAskInputDismissTestTime = -1f;

		// Token: 0x04001D3F RID: 7487
		private static readonly int ASKINPUT_WINDOW_LENGTH = 10;

		// Token: 0x04001D40 RID: 7488
		private int[] rollingWindow = new int[PlayerInput.ASKINPUT_WINDOW_LENGTH];

		// Token: 0x04001D41 RID: 7489
		private int rollingWindowIndex;

		// Token: 0x04001D42 RID: 7490
		private bool clientHasPendingResimulation;

		// Token: 0x04001D43 RID: 7491
		private uint clientResimulationFrameNumber;

		// Token: 0x04001D44 RID: 7492
		private EPlayerStance clientResimulationStance;

		// Token: 0x04001D45 RID: 7493
		private Vector3 clientResimulationPosition;

		// Token: 0x04001D46 RID: 7494
		private Vector3 clientResimulationVelocity;

		// Token: 0x04001D47 RID: 7495
		private byte clientResimulationStamina;

		// Token: 0x04001D48 RID: 7496
		private int clientResimulationLastTireOffset;

		// Token: 0x04001D49 RID: 7497
		private int clientResimulationLastRestOffset;

		// Token: 0x04001D4A RID: 7498
		internal bool isResimulating;

		// Token: 0x04001D4B RID: 7499
		private static readonly ClientInstanceMethod<uint, EPlayerStance, Vector3, Vector3, byte, int, int> SendSimulateMispredictedInputs = ClientInstanceMethod<uint, EPlayerStance, Vector3, Vector3, byte, int, int>.Get(typeof(PlayerInput), "ReceiveSimulateMispredictedInputs");

		// Token: 0x04001D4C RID: 7500
		private static readonly ClientInstanceMethod<uint> SendAckGoodInputs = ClientInstanceMethod<uint>.Get(typeof(PlayerInput), "ReceiveAckGoodInputs");

		// Token: 0x04001D4D RID: 7501
		private static readonly ServerInstanceMethod SendInputs = ServerInstanceMethod.Get(typeof(PlayerInput), "ReceiveInputs");

		// Token: 0x04001D4E RID: 7502
		private const float MIN_FAKE_LAG_THRESHOLD_SECONDS = 1f;

		/// <summary>
		/// Counter of simulation frames before fake lag penalty is disabled.
		/// </summary>
		// Token: 0x04001D4F RID: 7503
		private int fakeLagPenaltyFrames;

		/// <summary>
		/// Player damage multiplier while under penalty for fake lag. (10%)
		/// </summary>
		// Token: 0x04001D50 RID: 7504
		internal const float FAKE_LAG_PENALTY_DAMAGE = 0.1f;
	}
}
