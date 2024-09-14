using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004ED RID: 1261
	public class LevelNodes
	{
		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x000A2D94 File Offset: 0x000A0F94
		[Obsolete("Was the parent of all editor nodes in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelNodes._models == null)
				{
					LevelNodes._models = new GameObject().transform;
					LevelNodes._models.name = "Nodes";
					LevelNodes._models.parent = Level.level;
					LevelNodes._models.tag = "Logic";
					LevelNodes._models.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing LevelNodes.models which has been deprecated.", Array.Empty<object>());
				}
				return LevelNodes._models;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002733 RID: 10035 RVA: 0x000A2E0E File Offset: 0x000A100E
		[Obsolete("All legacy node types have been converted to subclasses of IDevkitHierarchyItem")]
		public static List<Node> nodes
		{
			get
			{
				return LevelNodes._nodes;
			}
		}

		/// <summary>
		/// Hash of nodes file.
		/// Prevents using the level editor to make noLight nodes visible.
		/// </summary>
		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x000A2E15 File Offset: 0x000A1015
		// (set) Token: 0x06002735 RID: 10037 RVA: 0x000A2E1C File Offset: 0x000A101C
		public static byte[] hash { get; private set; }

		// Token: 0x06002736 RID: 10038 RVA: 0x000A2E24 File Offset: 0x000A1024
		internal static void AutoConvertLegacyVolumes()
		{
			UnturnedLog.info("Auto converting legacy volumes");
			foreach (Node node in LevelNodes._nodes)
			{
				ArenaNode arenaNode = node as ArenaNode;
				if (arenaNode != null)
				{
					GameObject gameObject = new GameObject();
					Transform transform = gameObject.transform;
					transform.position = arenaNode.point;
					transform.rotation = Quaternion.identity;
					float sphereRadius = ArenaNode.CalculateRadiusFromNormalizedRadius(arenaNode._normalizedRadius);
					ArenaCompactorVolume arenaCompactorVolume = gameObject.AddComponent<ArenaCompactorVolume>();
					arenaCompactorVolume.Shape = ELevelVolumeShape.Sphere;
					arenaCompactorVolume.SetSphereRadius(sphereRadius);
					LevelHierarchy.AssignInstanceIdAndMarkDirty(arenaCompactorVolume);
				}
				else
				{
					DeadzoneNode deadzoneNode = node as DeadzoneNode;
					if (deadzoneNode != null)
					{
						GameObject gameObject2 = new GameObject();
						Transform transform2 = gameObject2.transform;
						transform2.position = deadzoneNode.point;
						transform2.rotation = Quaternion.identity;
						float sphereRadius2 = DeadzoneNode.CalculateRadiusFromNormalizedRadius(deadzoneNode._normalizedRadius);
						DeadzoneVolume deadzoneVolume = gameObject2.AddComponent<DeadzoneVolume>();
						deadzoneVolume.DeadzoneType = deadzoneNode.DeadzoneType;
						deadzoneVolume.Shape = ELevelVolumeShape.Sphere;
						deadzoneVolume.SetSphereRadius(sphereRadius2);
						LevelHierarchy.AssignInstanceIdAndMarkDirty(deadzoneVolume);
					}
					else
					{
						EffectNode effectNode = node as EffectNode;
						if (effectNode != null)
						{
							GameObject gameObject3 = new GameObject();
							Transform transform3 = gameObject3.transform;
							transform3.position = effectNode.point;
							transform3.rotation = Quaternion.identity;
							AmbianceVolume ambianceVolume = gameObject3.AddComponent<AmbianceVolume>();
							ambianceVolume.id = effectNode.id;
							ambianceVolume.noLighting = effectNode.noLighting;
							ambianceVolume.noWater = effectNode.noWater;
							LevelHierarchy.AssignInstanceIdAndMarkDirty(ambianceVolume);
							if (effectNode.shape == ENodeShape.BOX)
							{
								transform3.localScale = effectNode.bounds * 2f;
							}
							else
							{
								float sphereRadius3 = EffectNode.CalculateRadiusFromNormalizedRadius(effectNode._normalizedRadius);
								ambianceVolume.SetSphereRadius(sphereRadius3);
								ambianceVolume.Shape = ELevelVolumeShape.Sphere;
							}
						}
						else
						{
							PurchaseNode purchaseNode = node as PurchaseNode;
							if (purchaseNode != null)
							{
								GameObject gameObject4 = new GameObject();
								Transform transform4 = gameObject4.transform;
								transform4.position = purchaseNode.point;
								transform4.rotation = Quaternion.identity;
								float sphereRadius4 = PurchaseNode.CalculateRadiusFromNormalizedRadius(purchaseNode._normalizedRadius);
								HordePurchaseVolume hordePurchaseVolume = gameObject4.AddComponent<HordePurchaseVolume>();
								hordePurchaseVolume.Shape = ELevelVolumeShape.Sphere;
								hordePurchaseVolume.SetSphereRadius(sphereRadius4);
								LevelHierarchy.AssignInstanceIdAndMarkDirty(hordePurchaseVolume);
							}
							else
							{
								SafezoneNode safezoneNode = node as SafezoneNode;
								if (safezoneNode != null)
								{
									GameObject gameObject5 = new GameObject();
									Transform transform5 = gameObject5.transform;
									transform5.rotation = Quaternion.identity;
									SafezoneVolume safezoneVolume = gameObject5.AddComponent<SafezoneVolume>();
									safezoneVolume.noWeapons = safezoneNode.noWeapons;
									safezoneVolume.noBuildables = safezoneNode.noBuildables;
									LevelHierarchy.AssignInstanceIdAndMarkDirty(safezoneVolume);
									if (safezoneNode.isHeight)
									{
										transform5.position = node.point + new Vector3(0f, 1000f, 0f);
										transform5.localScale = new Vector3(10000f, 2000f, 10000f);
									}
									else
									{
										transform5.position = node.point;
										float sphereRadius5 = SafezoneNode.CalculateRadiusFromNormalizedRadius(safezoneNode._normalizedRadius);
										safezoneVolume.SetSphereRadius(sphereRadius5);
										safezoneVolume.Shape = ELevelVolumeShape.Sphere;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x000A3124 File Offset: 0x000A1324
		internal static void AutoConvertLegacyNodes()
		{
			UnturnedLog.info("Auto converting legacy nodes");
			foreach (Node node in LevelNodes._nodes)
			{
				AirdropNode airdropNode = node as AirdropNode;
				if (airdropNode != null)
				{
					GameObject gameObject = new GameObject();
					Transform transform = gameObject.transform;
					transform.position = airdropNode.point;
					transform.rotation = Quaternion.identity;
					AirdropDevkitNode airdropDevkitNode = gameObject.AddComponent<AirdropDevkitNode>();
					airdropDevkitNode.id = airdropNode.id;
					LevelHierarchy.AssignInstanceIdAndMarkDirty(airdropDevkitNode);
				}
				else
				{
					LocationNode locationNode = node as LocationNode;
					if (locationNode != null)
					{
						GameObject gameObject2 = new GameObject();
						Transform transform2 = gameObject2.transform;
						transform2.position = locationNode.point;
						transform2.rotation = Quaternion.identity;
						LocationDevkitNode locationDevkitNode = gameObject2.AddComponent<LocationDevkitNode>();
						locationDevkitNode.locationName = locationNode.name;
						locationDevkitNode.isVisibleOnMap = true;
						LevelHierarchy.AssignInstanceIdAndMarkDirty(locationDevkitNode);
					}
				}
			}
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x000A320C File Offset: 0x000A140C
		internal static Node FindLocationNode(string id)
		{
			foreach (Node node in LevelNodes._nodes)
			{
				if (node.type == ENodeType.LOCATION && string.Equals(((LocationNode)node).name, id, 3))
				{
					return node;
				}
			}
			return null;
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000A327C File Offset: 0x000A147C
		public static Transform addNode(Vector3 point, ENodeType type)
		{
			if (type == ENodeType.LOCATION)
			{
				LevelNodes._nodes.Add(new LocationNode(point));
			}
			else if (type == ENodeType.SAFEZONE)
			{
				LevelNodes._nodes.Add(new SafezoneNode(point));
			}
			else if (type == ENodeType.PURCHASE)
			{
				LevelNodes._nodes.Add(new PurchaseNode(point));
			}
			else if (type == ENodeType.ARENA)
			{
				LevelNodes._nodes.Add(new ArenaNode(point));
			}
			else if (type == ENodeType.DEADZONE)
			{
				LevelNodes._nodes.Add(new DeadzoneNode(point));
			}
			else if (type == ENodeType.AIRDROP)
			{
				LevelNodes._nodes.Add(new AirdropNode(point));
			}
			else if (type == ENodeType.EFFECT)
			{
				LevelNodes._nodes.Add(new EffectNode(point));
			}
			return LevelNodes._nodes[LevelNodes._nodes.Count - 1].model;
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000A3340 File Offset: 0x000A1540
		public static bool isPointInsideSafezone(Vector3 point, out SafezoneNode outSafezoneNode)
		{
			SafezoneVolume firstOverlappingVolume = VolumeManager<SafezoneVolume, SafezoneVolumeManager>.Get().GetFirstOverlappingVolume(point);
			outSafezoneNode = ((firstOverlappingVolume != null) ? firstOverlappingVolume.backwardsCompatibilityNode : null);
			return firstOverlappingVolume != null;
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x000A3370 File Offset: 0x000A1570
		public static void removeNode(Transform select)
		{
			for (int i = 0; i < LevelNodes._nodes.Count; i++)
			{
				if (LevelNodes._nodes[i].model == select)
				{
					LevelNodes._nodes[i].remove();
					LevelNodes._nodes.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000A33C8 File Offset: 0x000A15C8
		public static Node getNode(Transform select)
		{
			for (int i = 0; i < LevelNodes._nodes.Count; i++)
			{
				if (LevelNodes._nodes[i].model == select)
				{
					return LevelNodes._nodes[i];
				}
			}
			return null;
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000A3410 File Offset: 0x000A1610
		public static void load()
		{
			LevelNodes._nodes = new List<Node>();
			LevelNodes.hasLegacyVolumesForConversion = false;
			LevelNodes.hasLegacyNodesForConversion = false;
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Nodes.dat", false, false))
			{
				River river = new River(Level.info.path + "/Environment/Nodes.dat", false);
				byte b = river.readByte();
				if (b > 0)
				{
					bool flag = false;
					bool flag2 = false;
					ushort num = (ushort)river.readByte();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						Vector3 vector = river.readSingleVector3();
						ENodeType enodeType = (ENodeType)river.readByte();
						if (enodeType == ENodeType.LOCATION)
						{
							flag2 = true;
							string newName = river.readString();
							LevelNodes._nodes.Add(new LocationNode(vector, newName));
						}
						else if (enodeType == ENodeType.SAFEZONE)
						{
							flag = true;
							float newRadius = river.readSingle();
							bool newHeight = false;
							if (b > 1)
							{
								newHeight = river.readBoolean();
							}
							bool newNoWeapons = true;
							if (b > 4)
							{
								newNoWeapons = river.readBoolean();
							}
							bool newNoBuildables = true;
							if (b > 4)
							{
								newNoBuildables = river.readBoolean();
							}
							LevelNodes._nodes.Add(new SafezoneNode(vector, newRadius, newHeight, newNoWeapons, newNoBuildables));
						}
						else if (enodeType == ENodeType.PURCHASE)
						{
							flag = true;
							float newRadius2 = river.readSingle();
							ushort newID = river.readUInt16();
							uint newCost = river.readUInt32();
							LevelNodes._nodes.Add(new PurchaseNode(vector, newRadius2, newID, newCost));
						}
						else if (enodeType == ENodeType.ARENA)
						{
							flag = true;
							float num3 = river.readSingle();
							if (b < 6)
							{
								num3 *= 0.5f;
							}
							LevelNodes._nodes.Add(new ArenaNode(vector, num3));
						}
						else if (enodeType == ENodeType.DEADZONE)
						{
							flag = true;
							float newRadius3 = river.readSingle();
							EDeadzoneType newDeadzoneType = EDeadzoneType.DefaultRadiation;
							if (b > 6)
							{
								newDeadzoneType = (EDeadzoneType)river.readByte();
							}
							LevelNodes._nodes.Add(new DeadzoneNode(vector, newRadius3, newDeadzoneType));
						}
						else if (enodeType == ENodeType.AIRDROP)
						{
							flag2 = true;
							ushort num4 = river.readUInt16();
							byte b2;
							byte b3;
							if (SpawnTableTool.ResolveLegacyId(num4, EAssetType.ITEM, new Func<string>(LevelNodes.OnGetTestAirdropSpawnTableErrorContext)) == 0 && Assets.shouldLoadAnyAssets && Regions.tryGetCoordinate(vector, out b2, out b3))
							{
								Assets.reportError(string.Concat(new string[]
								{
									Level.info.name,
									" airdrop references invalid spawn table ",
									num4.ToString(),
									" at (",
									b2.ToString(),
									", ",
									b3.ToString(),
									")!"
								}));
							}
							LevelNodes._nodes.Add(new AirdropNode(vector, num4));
						}
						else if (enodeType == ENodeType.EFFECT)
						{
							flag = true;
							byte newShape = 0;
							if (b > 2)
							{
								newShape = river.readByte();
							}
							float newRadius4 = river.readSingle();
							Vector3 newBounds = Vector3.one;
							if (b > 2)
							{
								newBounds = river.readSingleVector3();
							}
							ushort newID2 = river.readUInt16();
							bool newNoWater = river.readBoolean();
							bool newNoLighting = false;
							if (b > 3)
							{
								newNoLighting = river.readBoolean();
							}
							LevelNodes._nodes.Add(new EffectNode(vector, (ENodeShape)newShape, newRadius4, newBounds, newID2, newNoWater, newNoLighting));
						}
					}
					LevelNodes.hasLegacyVolumesForConversion = (flag && b < 8);
					LevelNodes.hasLegacyNodesForConversion = (flag2 && b < 9);
				}
				LevelNodes.hash = river.getHash();
				river.closeRiver();
				return;
			}
			LevelNodes.hash = new byte[20];
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000A3734 File Offset: 0x000A1934
		public static void save()
		{
			River river = new River(Level.info.path + "/Environment/Nodes.dat", false);
			river.writeByte(9);
			byte b = 0;
			ushort num = 0;
			while ((int)num < LevelNodes._nodes.Count)
			{
				if (LevelNodes._nodes[(int)num].type != ENodeType.LOCATION || ((LocationNode)LevelNodes._nodes[(int)num]).name.Length > 0)
				{
					b += 1;
				}
				num += 1;
			}
			river.writeByte(b);
			byte b2 = 0;
			while ((int)b2 < LevelNodes._nodes.Count)
			{
				if (LevelNodes._nodes[(int)b2].type != ENodeType.LOCATION || ((LocationNode)LevelNodes._nodes[(int)b2]).name.Length > 0)
				{
					river.writeSingleVector3(LevelNodes._nodes[(int)b2].point);
					river.writeByte((byte)LevelNodes._nodes[(int)b2].type);
					if (LevelNodes._nodes[(int)b2].type == ENodeType.LOCATION)
					{
						river.writeString(((LocationNode)LevelNodes._nodes[(int)b2]).name);
					}
					else if (LevelNodes._nodes[(int)b2].type == ENodeType.SAFEZONE)
					{
						river.writeSingle(((SafezoneNode)LevelNodes._nodes[(int)b2]).radius);
						river.writeBoolean(((SafezoneNode)LevelNodes._nodes[(int)b2]).isHeight);
						river.writeBoolean(((SafezoneNode)LevelNodes._nodes[(int)b2]).noWeapons);
						river.writeBoolean(((SafezoneNode)LevelNodes._nodes[(int)b2]).noBuildables);
					}
					else if (LevelNodes._nodes[(int)b2].type == ENodeType.PURCHASE)
					{
						river.writeSingle(((PurchaseNode)LevelNodes._nodes[(int)b2]).radius);
						river.writeUInt16(((PurchaseNode)LevelNodes._nodes[(int)b2]).id);
						river.writeUInt32(((PurchaseNode)LevelNodes._nodes[(int)b2]).cost);
					}
					else if (LevelNodes._nodes[(int)b2].type == ENodeType.ARENA)
					{
						river.writeSingle(((ArenaNode)LevelNodes._nodes[(int)b2]).radius);
					}
					else if (LevelNodes._nodes[(int)b2].type == ENodeType.DEADZONE)
					{
						river.writeSingle(((DeadzoneNode)LevelNodes._nodes[(int)b2]).radius);
						river.writeByte((byte)((DeadzoneNode)LevelNodes._nodes[(int)b2]).DeadzoneType);
					}
					else if (LevelNodes._nodes[(int)b2].type == ENodeType.AIRDROP)
					{
						river.writeUInt16(((AirdropNode)LevelNodes._nodes[(int)b2]).id);
					}
					else if (LevelNodes._nodes[(int)b2].type == ENodeType.EFFECT)
					{
						river.writeByte((byte)((EffectNode)LevelNodes._nodes[(int)b2]).shape);
						river.writeSingle(((EffectNode)LevelNodes._nodes[(int)b2]).radius);
						river.writeSingleVector3(((EffectNode)LevelNodes._nodes[(int)b2]).bounds);
						river.writeUInt16(((EffectNode)LevelNodes._nodes[(int)b2]).id);
						river.writeBoolean(((EffectNode)LevelNodes._nodes[(int)b2]).noWater);
						river.writeBoolean(((EffectNode)LevelNodes._nodes[(int)b2]).noLighting);
					}
				}
				b2 += 1;
			}
			river.closeRiver();
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000A3AC5 File Offset: 0x000A1CC5
		private static string OnGetTestAirdropSpawnTableErrorContext()
		{
			return "level nodes airdrop test";
		}

		// Token: 0x040014C9 RID: 5321
		private const byte SAVEDATA_VERSION_CONVERTED_NODE_VOLUMES = 8;

		// Token: 0x040014CA RID: 5322
		private const byte SAVEDATA_VERSION_FINISHED_CONVERTING_ALL_NODES = 9;

		// Token: 0x040014CB RID: 5323
		private const byte SAVEDATA_VERSION_NEWEST = 9;

		// Token: 0x040014CC RID: 5324
		public static readonly byte SAVEDATA_VERSION = 9;

		// Token: 0x040014CD RID: 5325
		private static Transform _models;

		// Token: 0x040014CE RID: 5326
		private static List<Node> _nodes;

		/// <summary>
		/// If true then level should convert old node types to volumes.
		/// </summary>
		// Token: 0x040014CF RID: 5327
		internal static bool hasLegacyVolumesForConversion;

		/// <summary>
		/// If true then level should convert old non-volumes types to devkit objects.
		/// </summary>
		// Token: 0x040014D0 RID: 5328
		internal static bool hasLegacyNodesForConversion;
	}
}
