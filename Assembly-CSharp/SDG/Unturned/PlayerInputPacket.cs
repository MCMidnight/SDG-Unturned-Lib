using System;
using System.Collections.Generic;
using SDG.NetPak;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200061E RID: 1566
	public class PlayerInputPacket
	{
		// Token: 0x060032A0 RID: 12960 RVA: 0x000E3C6C File Offset: 0x000E1E6C
		public virtual void read(SteamChannel channel, NetPakReader reader)
		{
			SystemNetPakReaderEx.ReadUInt32(reader, ref this.clientSimulationFrameNumber);
			SystemNetPakReaderEx.ReadInt32(reader, ref this.recov);
			SystemNetPakReaderEx.ReadUInt16(reader, ref this.keys);
			uint num;
			reader.ReadBits(2, ref num);
			if ((num & 1U) == 1U)
			{
				this.primaryAttack |= EAttackInputFlags.Start;
			}
			if ((num & 2U) == 2U)
			{
				this.primaryAttack |= EAttackInputFlags.Stop;
			}
			uint num2;
			reader.ReadBits(2, ref num2);
			if ((num2 & 1U) == 1U)
			{
				this.secondaryAttack |= EAttackInputFlags.Start;
			}
			if ((num2 & 2U) == 2U)
			{
				this.secondaryAttack |= EAttackInputFlags.Stop;
			}
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			int num3 = (int)b;
			if (num3 > 0)
			{
				num3 = Mathf.Min(num3, PlayerInputPacket.MAX_CLIENTSIDE_INPUTS);
				this.serversideInputs = new Queue<InputInfo>(num3);
				for (int i = 0; i < num3; i++)
				{
					InputInfo inputInfo = new InputInfo();
					reader.ReadEnum(out inputInfo.type);
					switch (inputInfo.type)
					{
					case ERaycastInfoType.NONE:
						reader.ReadEnum(out inputInfo.usage);
						UnityNetPakReaderEx.ReadClampedVector3(reader, ref inputInfo.point, 13, 7);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.normal, 9);
						reader.ReadPhysicsMaterialName(out inputInfo.materialName);
						inputInfo.material = PhysicsTool.GetLegacyMaterialByName(inputInfo.materialName);
						break;
					case ERaycastInfoType.SKIP:
						inputInfo = null;
						break;
					case ERaycastInfoType.OBJECT:
					{
						reader.ReadEnum(out inputInfo.usage);
						UnityNetPakReaderEx.ReadClampedVector3(reader, ref inputInfo.point, 13, 7);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.direction, 9);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.normal, 9);
						reader.ReadPhysicsMaterialName(out inputInfo.materialName);
						inputInfo.material = PhysicsTool.GetLegacyMaterialByName(inputInfo.materialName);
						SystemNetPakReaderEx.ReadUInt8(reader, ref inputInfo.section);
						byte x;
						SystemNetPakReaderEx.ReadUInt8(reader, ref x);
						byte y;
						SystemNetPakReaderEx.ReadUInt8(reader, ref y);
						ushort index;
						SystemNetPakReaderEx.ReadUInt16(reader, ref index);
						reader.ReadTransform(out inputInfo.colliderTransform);
						LevelObject @object = ObjectManager.getObject(x, y, index);
						if (@object != null && @object.transform != null && (inputInfo.point - @object.transform.position).sqrMagnitude < 256f)
						{
							inputInfo.transform = @object.transform;
						}
						else
						{
							inputInfo.type = ERaycastInfoType.NONE;
						}
						break;
					}
					case ERaycastInfoType.PLAYER:
					{
						reader.ReadEnum(out inputInfo.usage);
						UnityNetPakReaderEx.ReadClampedVector3(reader, ref inputInfo.point, 13, 7);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.direction, 9);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.normal, 9);
						reader.ReadEnum(out inputInfo.limb);
						CSteamID steamID;
						SteamworksNetPakReaderEx.ReadSteamID(reader, ref steamID);
						Player player = PlayerTool.getPlayer(steamID);
						if (player != null)
						{
							float num4 = 256f;
							if (player.movement.getVehicle() != null)
							{
								num4 = 512f;
							}
							if ((inputInfo.point - player.transform.position).sqrMagnitude < num4)
							{
								inputInfo.materialName = "Flesh_Dynamic";
								inputInfo.material = EPhysicsMaterial.FLESH_DYNAMIC;
								inputInfo.player = player;
								inputInfo.transform = player.transform;
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
						break;
					}
					case ERaycastInfoType.ZOMBIE:
					{
						reader.ReadEnum(out inputInfo.usage);
						UnityNetPakReaderEx.ReadClampedVector3(reader, ref inputInfo.point, 13, 7);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.direction, 9);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.normal, 9);
						reader.ReadEnum(out inputInfo.limb);
						ushort id;
						SystemNetPakReaderEx.ReadUInt16(reader, ref id);
						Zombie zombie = ZombieManager.getZombie(inputInfo.point, id);
						if (zombie != null)
						{
							Vector2 vector = new Vector2(inputInfo.point.x - zombie.transform.position.x, inputInfo.point.z - zombie.transform.position.z);
							if (vector.sqrMagnitude < 256f)
							{
								if (zombie.isRadioactive)
								{
									inputInfo.materialName = "Alien_Dynamic";
									inputInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;
								}
								else
								{
									inputInfo.materialName = "Flesh_Dynamic";
									inputInfo.material = EPhysicsMaterial.FLESH_DYNAMIC;
								}
								inputInfo.zombie = zombie;
								inputInfo.transform = zombie.transform;
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
						break;
					}
					case ERaycastInfoType.ANIMAL:
					{
						reader.ReadEnum(out inputInfo.usage);
						UnityNetPakReaderEx.ReadClampedVector3(reader, ref inputInfo.point, 13, 7);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.direction, 9);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.normal, 9);
						reader.ReadEnum(out inputInfo.limb);
						ushort index2;
						SystemNetPakReaderEx.ReadUInt16(reader, ref index2);
						Animal animal = AnimalManager.getAnimal(index2);
						if (animal != null && (inputInfo.point - animal.transform.position).sqrMagnitude < 256f)
						{
							inputInfo.materialName = "Flesh_Dynamic";
							inputInfo.material = EPhysicsMaterial.FLESH_DYNAMIC;
							inputInfo.animal = animal;
							inputInfo.transform = animal.transform;
						}
						else
						{
							inputInfo = null;
						}
						break;
					}
					case ERaycastInfoType.VEHICLE:
					{
						reader.ReadEnum(out inputInfo.usage);
						UnityNetPakReaderEx.ReadClampedVector3(reader, ref inputInfo.point, 13, 7);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.normal, 9);
						reader.ReadPhysicsMaterialName(out inputInfo.materialName);
						inputInfo.material = PhysicsTool.GetLegacyMaterialByName(inputInfo.materialName);
						uint instanceID;
						SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
						reader.ReadTransform(out inputInfo.colliderTransform);
						InteractableVehicle interactableVehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);
						if (interactableVehicle != null && (interactableVehicle == channel.owner.player.movement.getVehicle() || (inputInfo.point - interactableVehicle.transform.position).sqrMagnitude < 4096f))
						{
							inputInfo.vehicle = interactableVehicle;
							inputInfo.transform = interactableVehicle.transform;
						}
						else
						{
							inputInfo = null;
						}
						break;
					}
					case ERaycastInfoType.BARRICADE:
					{
						reader.ReadEnum(out inputInfo.usage);
						UnityNetPakReaderEx.ReadClampedVector3(reader, ref inputInfo.point, 13, 7);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.normal, 9);
						reader.ReadPhysicsMaterialName(out inputInfo.materialName);
						inputInfo.material = PhysicsTool.GetLegacyMaterialByName(inputInfo.materialName);
						NetId key;
						reader.ReadNetId(out key);
						reader.ReadTransform(out inputInfo.colliderTransform);
						BarricadeDrop barricadeDrop = NetIdRegistry.Get<BarricadeDrop>(key);
						if (barricadeDrop != null)
						{
							Transform model = barricadeDrop.model;
							if (model != null && (inputInfo.point - model.position).sqrMagnitude < 256f)
							{
								inputInfo.transform = model;
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
						break;
					}
					case ERaycastInfoType.STRUCTURE:
					{
						reader.ReadEnum(out inputInfo.usage);
						UnityNetPakReaderEx.ReadClampedVector3(reader, ref inputInfo.point, 13, 7);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.direction, 9);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.normal, 9);
						reader.ReadPhysicsMaterialName(out inputInfo.materialName);
						inputInfo.material = PhysicsTool.GetLegacyMaterialByName(inputInfo.materialName);
						NetId key2;
						reader.ReadNetId(out key2);
						reader.ReadTransform(out inputInfo.colliderTransform);
						StructureDrop structureDrop = NetIdRegistry.Get<StructureDrop>(key2);
						if (structureDrop != null)
						{
							Transform model2 = structureDrop.model;
							if (model2 != null && (inputInfo.point - model2.position).sqrMagnitude < 256f)
							{
								inputInfo.transform = model2;
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
						break;
					}
					case ERaycastInfoType.RESOURCE:
					{
						reader.ReadEnum(out inputInfo.usage);
						UnityNetPakReaderEx.ReadClampedVector3(reader, ref inputInfo.point, 13, 7);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.direction, 9);
						UnityNetPakReaderEx.ReadNormalVector3(reader, ref inputInfo.normal, 9);
						reader.ReadPhysicsMaterialName(out inputInfo.materialName);
						inputInfo.material = PhysicsTool.GetLegacyMaterialByName(inputInfo.materialName);
						byte x2;
						SystemNetPakReaderEx.ReadUInt8(reader, ref x2);
						byte y2;
						SystemNetPakReaderEx.ReadUInt8(reader, ref y2);
						ushort index3;
						SystemNetPakReaderEx.ReadUInt16(reader, ref index3);
						reader.ReadTransform(out inputInfo.colliderTransform);
						Transform resource = ResourceManager.getResource(x2, y2, index3);
						if (resource != null && (inputInfo.point - resource.transform.position).sqrMagnitude < 256f)
						{
							inputInfo.transform = resource;
						}
						else
						{
							inputInfo = null;
						}
						break;
					}
					}
					if (inputInfo != null)
					{
						this.serversideInputs.Enqueue(inputInfo);
					}
				}
			}
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x000E453C File Offset: 0x000E273C
		public virtual void write(NetPakWriter writer)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, this.clientSimulationFrameNumber);
			SystemNetPakWriterEx.WriteInt32(writer, this.recov);
			SystemNetPakWriterEx.WriteUInt16(writer, this.keys);
			writer.WriteBits((uint)this.primaryAttack, 2);
			writer.WriteBits((uint)this.secondaryAttack, 2);
			if (this.clientsideInputs == null)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, 0);
				return;
			}
			int num = this.clientsideInputs.Count;
			if (num > PlayerInputPacket.MAX_CLIENTSIDE_INPUTS)
			{
				UnturnedLog.warn("Discarding excessive hit inputs {0}/{1}", new object[]
				{
					num,
					PlayerInputPacket.MAX_CLIENTSIDE_INPUTS
				});
				num = PlayerInputPacket.MAX_CLIENTSIDE_INPUTS;
			}
			SystemNetPakWriterEx.WriteUInt8(writer, (byte)num);
			for (int i = 0; i < num; i++)
			{
				RaycastInfo info = this.clientsideInputs[i].info;
				ERaycastInfoUsage usage = this.clientsideInputs[i].usage;
				if (info.player != null)
				{
					writer.WriteEnum(ERaycastInfoType.PLAYER);
					writer.WriteEnum(usage);
					UnityNetPakWriterEx.WriteClampedVector3(writer, info.point, 13, 7);
					UnityNetPakWriterEx.WriteNormalVector3(writer, info.direction, 9);
					UnityNetPakWriterEx.WriteNormalVector3(writer, info.normal, 9);
					writer.WriteEnum(info.limb);
					SteamworksNetPakWriterEx.WriteSteamID(writer, info.player.channel.owner.playerID.steamID);
				}
				else if (info.zombie != null)
				{
					writer.WriteEnum(ERaycastInfoType.ZOMBIE);
					writer.WriteEnum(usage);
					UnityNetPakWriterEx.WriteClampedVector3(writer, info.point, 13, 7);
					UnityNetPakWriterEx.WriteNormalVector3(writer, info.direction, 9);
					UnityNetPakWriterEx.WriteNormalVector3(writer, info.normal, 9);
					writer.WriteEnum(info.limb);
					SystemNetPakWriterEx.WriteUInt16(writer, info.zombie.id);
				}
				else if (info.animal != null)
				{
					writer.WriteEnum(ERaycastInfoType.ANIMAL);
					writer.WriteEnum(usage);
					UnityNetPakWriterEx.WriteClampedVector3(writer, info.point, 13, 7);
					UnityNetPakWriterEx.WriteNormalVector3(writer, info.direction, 9);
					UnityNetPakWriterEx.WriteNormalVector3(writer, info.normal, 9);
					writer.WriteEnum(info.limb);
					SystemNetPakWriterEx.WriteUInt16(writer, info.animal.index);
				}
				else if (info.vehicle != null)
				{
					writer.WriteEnum(ERaycastInfoType.VEHICLE);
					writer.WriteEnum(usage);
					UnityNetPakWriterEx.WriteClampedVector3(writer, info.point, 13, 7);
					UnityNetPakWriterEx.WriteNormalVector3(writer, info.normal, 9);
					writer.WritePhysicsMaterialName(info.materialName);
					SystemNetPakWriterEx.WriteUInt32(writer, info.vehicle.instanceID);
					Collider collider = info.collider;
					writer.WriteTransform((collider != null) ? collider.transform : null);
				}
				else if (info.transform != null)
				{
					if (info.transform.CompareTag("Barricade"))
					{
						writer.WriteEnum(ERaycastInfoType.BARRICADE);
						writer.WriteEnum(usage);
						info.transform = DamageTool.getBarricadeRootTransform(info.transform);
						BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(info.transform);
						if (barricadeDrop != null)
						{
							UnityNetPakWriterEx.WriteClampedVector3(writer, info.point, 13, 7);
							UnityNetPakWriterEx.WriteNormalVector3(writer, info.normal, 9);
							writer.WritePhysicsMaterialName(info.materialName);
							writer.WriteNetId(barricadeDrop.GetNetId());
						}
						else
						{
							UnityNetPakWriterEx.WriteClampedVector3(writer, Vector3.zero, 13, 7);
							UnityNetPakWriterEx.WriteNormalVector3(writer, Vector3.up, 9);
							writer.WritePhysicsMaterialName(null);
							writer.WriteNetId(NetId.INVALID);
						}
						Collider collider2 = info.collider;
						writer.WriteTransform((collider2 != null) ? collider2.transform : null);
					}
					else if (info.transform.CompareTag("Structure"))
					{
						writer.WriteEnum(ERaycastInfoType.STRUCTURE);
						writer.WriteEnum(usage);
						info.transform = DamageTool.getStructureRootTransform(info.transform);
						StructureDrop structureDrop = StructureManager.FindStructureByRootTransform(info.transform);
						if (structureDrop != null)
						{
							UnityNetPakWriterEx.WriteClampedVector3(writer, info.point, 13, 7);
							UnityNetPakWriterEx.WriteNormalVector3(writer, info.direction, 9);
							UnityNetPakWriterEx.WriteNormalVector3(writer, info.normal, 9);
							writer.WritePhysicsMaterialName(info.materialName);
							writer.WriteNetId(structureDrop.GetNetId());
						}
						else
						{
							UnityNetPakWriterEx.WriteClampedVector3(writer, Vector3.zero, 13, 7);
							UnityNetPakWriterEx.WriteNormalVector3(writer, Vector3.up, 9);
							UnityNetPakWriterEx.WriteNormalVector3(writer, Vector3.up, 9);
							writer.WritePhysicsMaterialName(null);
							writer.WriteNetId(NetId.INVALID);
						}
						Collider collider3 = info.collider;
						writer.WriteTransform((collider3 != null) ? collider3.transform : null);
					}
					else if (info.transform.CompareTag("Resource"))
					{
						writer.WriteEnum(ERaycastInfoType.RESOURCE);
						writer.WriteEnum(usage);
						info.transform = DamageTool.getResourceRootTransform(info.transform);
						byte b;
						byte b2;
						ushort num2;
						if (ResourceManager.tryGetRegion(info.transform, out b, out b2, out num2))
						{
							UnityNetPakWriterEx.WriteClampedVector3(writer, info.point, 13, 7);
							UnityNetPakWriterEx.WriteNormalVector3(writer, info.direction, 9);
							UnityNetPakWriterEx.WriteNormalVector3(writer, info.normal, 9);
							writer.WritePhysicsMaterialName(info.materialName);
							SystemNetPakWriterEx.WriteUInt8(writer, b);
							SystemNetPakWriterEx.WriteUInt8(writer, b2);
							SystemNetPakWriterEx.WriteUInt16(writer, num2);
						}
						else
						{
							UnityNetPakWriterEx.WriteClampedVector3(writer, Vector3.zero, 13, 7);
							UnityNetPakWriterEx.WriteNormalVector3(writer, Vector3.up, 9);
							UnityNetPakWriterEx.WriteNormalVector3(writer, Vector3.up, 9);
							writer.WritePhysicsMaterialName(null);
							SystemNetPakWriterEx.WriteUInt8(writer, 0);
							SystemNetPakWriterEx.WriteUInt8(writer, 0);
							SystemNetPakWriterEx.WriteUInt16(writer, ushort.MaxValue);
						}
						Collider collider4 = info.collider;
						writer.WriteTransform((collider4 != null) ? collider4.transform : null);
					}
					else if (info.transform.CompareTag("Small") || info.transform.CompareTag("Medium") || info.transform.CompareTag("Large"))
					{
						writer.WriteEnum(ERaycastInfoType.OBJECT);
						writer.WriteEnum(usage);
						InteractableObjectRubble componentInParent = info.transform.GetComponentInParent<InteractableObjectRubble>();
						if (componentInParent != null)
						{
							info.transform = componentInParent.transform;
							info.section = componentInParent.getSection(info.collider.transform);
						}
						byte b3;
						byte b4;
						ushort num3;
						if (ObjectManager.tryGetRegion(info.transform, out b3, out b4, out num3))
						{
							UnityNetPakWriterEx.WriteClampedVector3(writer, info.point, 13, 7);
							UnityNetPakWriterEx.WriteNormalVector3(writer, info.direction, 9);
							UnityNetPakWriterEx.WriteNormalVector3(writer, info.normal, 9);
							writer.WritePhysicsMaterialName(info.materialName);
							SystemNetPakWriterEx.WriteUInt8(writer, info.section);
							SystemNetPakWriterEx.WriteUInt8(writer, b3);
							SystemNetPakWriterEx.WriteUInt8(writer, b4);
							SystemNetPakWriterEx.WriteUInt16(writer, num3);
						}
						else
						{
							UnityNetPakWriterEx.WriteClampedVector3(writer, Vector3.zero, 13, 7);
							UnityNetPakWriterEx.WriteNormalVector3(writer, Vector3.up, 9);
							UnityNetPakWriterEx.WriteNormalVector3(writer, Vector3.up, 9);
							writer.WritePhysicsMaterialName(null);
							SystemNetPakWriterEx.WriteUInt8(writer, byte.MaxValue);
							SystemNetPakWriterEx.WriteUInt8(writer, 0);
							SystemNetPakWriterEx.WriteUInt8(writer, 0);
							SystemNetPakWriterEx.WriteUInt16(writer, ushort.MaxValue);
						}
						Collider collider5 = info.collider;
						writer.WriteTransform((collider5 != null) ? collider5.transform : null);
					}
					else if (info.transform.CompareTag("Ground") || info.transform.CompareTag("Environment"))
					{
						writer.WriteEnum(ERaycastInfoType.NONE);
						writer.WriteEnum(usage);
						UnityNetPakWriterEx.WriteClampedVector3(writer, info.point, 13, 7);
						UnityNetPakWriterEx.WriteNormalVector3(writer, info.normal, 9);
						writer.WritePhysicsMaterialName(info.materialName);
					}
					else
					{
						writer.WriteEnum(ERaycastInfoType.SKIP);
					}
				}
				else
				{
					writer.WriteEnum(ERaycastInfoType.SKIP);
				}
			}
		}

		/// <summary>
		/// Worst case scenario, maybe shotgun hit or fast spray SMG.
		/// </summary>
		// Token: 0x04001CE7 RID: 7399
		private static int MAX_CLIENTSIDE_INPUTS = 16;

		// Token: 0x04001CE8 RID: 7400
		public List<PlayerInputPacket.ClientRaycast> clientsideInputs;

		// Token: 0x04001CE9 RID: 7401
		public Queue<InputInfo> serversideInputs;

		// Token: 0x04001CEA RID: 7402
		public uint clientSimulationFrameNumber;

		// Token: 0x04001CEB RID: 7403
		public int recov;

		// Token: 0x04001CEC RID: 7404
		public ushort keys;

		// Token: 0x04001CED RID: 7405
		public EAttackInputFlags primaryAttack;

		// Token: 0x04001CEE RID: 7406
		public EAttackInputFlags secondaryAttack;

		// Token: 0x020009A6 RID: 2470
		public struct ClientRaycast
		{
			// Token: 0x06004BEE RID: 19438 RVA: 0x001B6183 File Offset: 0x001B4383
			public ClientRaycast(RaycastInfo info, ERaycastInfoUsage usage)
			{
				this.info = info;
				this.usage = usage;
			}

			// Token: 0x040033FB RID: 13307
			public RaycastInfo info;

			// Token: 0x040033FC RID: 13308
			public ERaycastInfoUsage usage;
		}
	}
}
