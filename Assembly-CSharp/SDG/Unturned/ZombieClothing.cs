using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200082A RID: 2090
	public class ZombieClothing
	{
		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06004757 RID: 18263 RVA: 0x001ACB9C File Offset: 0x001AAD9C
		// (set) Token: 0x06004758 RID: 18264 RVA: 0x001ACBA3 File Offset: 0x001AADA3
		public static Material ghostMaterial { get; private set; }

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06004759 RID: 18265 RVA: 0x001ACBAB File Offset: 0x001AADAB
		// (set) Token: 0x0600475A RID: 18266 RVA: 0x001ACBB2 File Offset: 0x001AADB2
		public static Material ghostSpiritMaterial { get; private set; }

		// Token: 0x0600475B RID: 18267 RVA: 0x001ACBBC File Offset: 0x001AADBC
		public static Material paint(ushort shirt, ushort pants, bool isMega)
		{
			Material material = new Material(ZombieClothing.clothingShader);
			material.name = string.Concat(new string[]
			{
				"Zombie_",
				isMega ? "Mega" : "Normal",
				"_",
				shirt.ToString(),
				"_",
				pants.ToString()
			});
			material.hideFlags = HideFlags.HideAndDontSave;
			material.SetColor(HumanClothes.skinColorPropertyID, isMega ? new Color32(89, 99, 89, byte.MaxValue) : new Color32(99, 124, 99, byte.MaxValue));
			material.SetTexture(HumanClothes.faceAlbedoTexturePropertyID, ZombieClothing.faceTexture);
			if (shirt != 0)
			{
				ItemShirtAsset itemShirtAsset = Assets.find(EAssetType.ITEM, shirt) as ItemShirtAsset;
				if (itemShirtAsset != null)
				{
					material.SetTexture(HumanClothes.shirtAlbedoTexturePropertyID, itemShirtAsset.shirt);
					material.SetTexture(HumanClothes.shirtEmissionTexturePropertyID, itemShirtAsset.emission);
					material.SetTexture(HumanClothes.shirtMetallicTexturePropertyID, itemShirtAsset.metallic);
				}
			}
			if (pants != 0)
			{
				ItemPantsAsset itemPantsAsset = Assets.find(EAssetType.ITEM, pants) as ItemPantsAsset;
				if (itemPantsAsset != null)
				{
					material.SetTexture(HumanClothes.pantsAlbedoTexturePropertyID, itemPantsAsset.pants);
					material.SetTexture(HumanClothes.pantsEmissionTexturePropertyID, itemPantsAsset.emission);
					material.SetTexture(HumanClothes.pantsMetallicTexturePropertyID, itemPantsAsset.metallic);
				}
			}
			return material;
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x001ACD00 File Offset: 0x001AAF00
		public static void apply(Transform zombie, ZombieClothing.EApplyFlags flags, SkinnedMeshRenderer renderer_0, SkinnedMeshRenderer renderer_1, byte type, byte shirt, byte pants, byte hat, byte gear, ushort hatID, ushort gearID, out Transform attachmentModel_0, out Transform attachmentModel_1)
		{
			bool flag = flags.HasFlag(ZombieClothing.EApplyFlags.Mega);
			bool isRagdoll = flags.HasFlag(ZombieClothing.EApplyFlags.Ragdoll);
			attachmentModel_0 = null;
			attachmentModel_1 = null;
			Transform transform = zombie.Find("Skeleton").Find("Spine");
			Transform transform2 = transform.Find("Skull");
			if ((int)type >= LevelZombies.tables.Count)
			{
				UnturnedLog.warn("Zombie clothes unknown type index {0}, defaulting to zero", new object[]
				{
					type
				});
				type = 0;
			}
			if ((int)type >= LevelZombies.tables.Count)
			{
				UnturnedLog.warn("No valid zombie tables, should not have been spawned");
				return;
			}
			ZombieTable zombieTable = LevelZombies.tables[(int)type];
			if (shirt == 255)
			{
				shirt = (byte)zombieTable.slots[0].table.Count;
			}
			else if ((int)shirt > zombieTable.slots[0].table.Count)
			{
				byte b = (byte)zombieTable.slots[0].table.Count;
				UnturnedLog.warn("Zombie clothes unknown shirt index {0}, defaulting to {1}", new object[]
				{
					shirt,
					b
				});
				shirt = b;
			}
			if (pants == 255)
			{
				pants = (byte)zombieTable.slots[1].table.Count;
			}
			else if ((int)pants > zombieTable.slots[1].table.Count)
			{
				byte b2 = (byte)zombieTable.slots[1].table.Count;
				UnturnedLog.warn("Zombie clothes unknown pants index {0}, defaulting to {1}", new object[]
				{
					pants,
					b2
				});
				pants = b2;
			}
			Material material;
			if ((int)shirt <= zombieTable.slots[0].table.Count && (int)pants <= zombieTable.slots[1].table.Count)
			{
				material = ZombieClothing.clothes[(int)type][(int)shirt, (int)pants];
			}
			else
			{
				material = null;
				UnturnedLog.warn("Zombies clothes type {0} no valid shirt or pants", new object[]
				{
					type
				});
			}
			if (material != null)
			{
				if (renderer_0 != null)
				{
					renderer_0.sharedMesh = (flag ? ZombieClothing.megaMesh_0 : ZombieClothing.zombieMesh_0);
					renderer_0.sharedMaterial = material;
				}
				if (renderer_1 != null)
				{
					renderer_1.sharedMesh = (flag ? ZombieClothing.megaMesh_1 : ZombieClothing.zombieMesh_1);
					renderer_1.sharedMaterial = material;
				}
			}
			Transform transform3 = transform2.Find("Hat");
			if (transform3 != null)
			{
				Object.Destroy(transform3.gameObject);
			}
			Transform transform4 = transform.Find("Backpack");
			if (transform4 != null)
			{
				Object.Destroy(transform4.gameObject);
			}
			Transform transform5 = transform.Find("Vest");
			if (transform5 != null)
			{
				Object.Destroy(transform5.gameObject);
			}
			Transform transform6 = transform2.Find("Mask");
			if (transform6 != null)
			{
				Object.Destroy(transform6.gameObject);
			}
			Transform transform7 = transform2.Find("Glasses");
			if (transform7 != null)
			{
				Object.Destroy(transform7.gameObject);
			}
			if (hatID == 0 && hat != 255 && (int)hat < zombieTable.slots[2].table.Count)
			{
				hatID = zombieTable.slots[2].table[(int)hat].item;
			}
			if (hatID != 0)
			{
				ItemClothingAsset itemClothingAsset = Assets.find(EAssetType.ITEM, hatID) as ItemClothingAsset;
				if (itemClothingAsset != null && itemClothingAsset.shouldBeVisible(isRagdoll))
				{
					if (itemClothingAsset.type == EItemType.HAT)
					{
						transform3 = Object.Instantiate<GameObject>(((ItemHatAsset)itemClothingAsset).hat).transform;
						transform3.name = "Hat";
						transform3.transform.parent = transform2;
						transform3.transform.localPosition = Vector3.zero;
						transform3.transform.localRotation = Quaternion.identity;
						transform3.transform.localScale = Vector3.one;
						if (itemClothingAsset.shouldDestroyClothingColliders)
						{
							PrefabUtil.DestroyCollidersInChildren(transform3.gameObject, true);
						}
						transform3.DestroyRigidbody();
						attachmentModel_0 = transform3.transform;
					}
					else if (itemClothingAsset.type == EItemType.BACKPACK)
					{
						transform4 = Object.Instantiate<GameObject>(((ItemBackpackAsset)itemClothingAsset).backpack).transform;
						transform4.name = "Backpack";
						transform4.transform.parent = transform;
						transform4.transform.localPosition = Vector3.zero;
						transform4.transform.localRotation = Quaternion.identity;
						transform4.transform.localScale = (flag ? new Vector3(1.05f, 1f, 1.1f) : Vector3.one);
						if (itemClothingAsset.shouldDestroyClothingColliders)
						{
							PrefabUtil.DestroyCollidersInChildren(transform4.gameObject, true);
						}
						transform4.DestroyRigidbody();
						attachmentModel_0 = transform4.transform;
					}
					else if (itemClothingAsset.type == EItemType.VEST)
					{
						transform5 = Object.Instantiate<GameObject>(((ItemVestAsset)itemClothingAsset).vest).transform;
						transform5.name = "Vest";
						transform5.transform.parent = transform;
						transform5.transform.localPosition = Vector3.zero;
						transform5.transform.localRotation = Quaternion.identity;
						transform5.transform.localScale = (flag ? new Vector3(1.05f, 1f, 1.1f) : Vector3.one);
						if (itemClothingAsset.shouldDestroyClothingColliders)
						{
							PrefabUtil.DestroyCollidersInChildren(transform5.gameObject, true);
						}
						transform5.DestroyRigidbody();
						attachmentModel_0 = transform5.transform;
					}
					else if (itemClothingAsset.type == EItemType.MASK)
					{
						transform6 = Object.Instantiate<GameObject>(((ItemMaskAsset)itemClothingAsset).mask).transform;
						transform6.name = "Mask";
						transform6.transform.parent = transform2;
						transform6.transform.localPosition = Vector3.zero;
						transform6.transform.localRotation = Quaternion.identity;
						transform6.transform.localScale = Vector3.one;
						if (itemClothingAsset.shouldDestroyClothingColliders)
						{
							PrefabUtil.DestroyCollidersInChildren(transform6.gameObject, true);
						}
						transform6.DestroyRigidbody();
						attachmentModel_0 = transform6.transform;
					}
					else if (itemClothingAsset.type == EItemType.GLASSES)
					{
						transform7 = Object.Instantiate<GameObject>(((ItemGlassesAsset)itemClothingAsset).glasses).transform;
						transform7.name = "Glasses";
						transform7.transform.parent = transform2;
						transform7.transform.localPosition = Vector3.zero;
						transform7.transform.localRotation = Quaternion.identity;
						transform7.transform.localScale = Vector3.one;
						if (itemClothingAsset.shouldDestroyClothingColliders)
						{
							PrefabUtil.DestroyCollidersInChildren(transform7.gameObject, true);
						}
						transform7.DestroyRigidbody();
						attachmentModel_0 = transform7.transform;
					}
				}
			}
			if (gearID == 0 && gear != 255 && (int)gear < zombieTable.slots[3].table.Count)
			{
				gearID = zombieTable.slots[3].table[(int)gear].item;
			}
			if (gearID != 0)
			{
				ItemClothingAsset itemClothingAsset2 = Assets.find(EAssetType.ITEM, gearID) as ItemClothingAsset;
				if (itemClothingAsset2 != null && itemClothingAsset2.shouldBeVisible(isRagdoll))
				{
					if (itemClothingAsset2.type == EItemType.HAT)
					{
						transform3 = Object.Instantiate<GameObject>(((ItemHatAsset)itemClothingAsset2).hat).transform;
						transform3.name = "Hat";
						transform3.transform.parent = transform2;
						transform3.transform.localPosition = Vector3.zero;
						transform3.transform.localRotation = Quaternion.identity;
						transform3.transform.localScale = Vector3.one;
						if (itemClothingAsset2.shouldDestroyClothingColliders)
						{
							PrefabUtil.DestroyCollidersInChildren(transform3.gameObject, true);
						}
						transform3.DestroyRigidbody();
						attachmentModel_1 = transform3.transform;
						return;
					}
					if (itemClothingAsset2.type == EItemType.BACKPACK)
					{
						transform4 = Object.Instantiate<GameObject>(((ItemBackpackAsset)itemClothingAsset2).backpack).transform;
						transform4.name = "Backpack";
						transform4.transform.parent = transform;
						transform4.transform.localPosition = Vector3.zero;
						transform4.transform.localRotation = Quaternion.identity;
						transform4.transform.localScale = (flag ? new Vector3(1.05f, 1f, 1.1f) : Vector3.one);
						if (itemClothingAsset2.shouldDestroyClothingColliders)
						{
							PrefabUtil.DestroyCollidersInChildren(transform4.gameObject, true);
						}
						transform4.DestroyRigidbody();
						attachmentModel_1 = transform4.transform;
						return;
					}
					if (itemClothingAsset2.type == EItemType.VEST)
					{
						transform5 = Object.Instantiate<GameObject>(((ItemVestAsset)itemClothingAsset2).vest).transform;
						transform5.name = "Vest";
						transform5.transform.parent = transform;
						transform5.transform.localPosition = Vector3.zero;
						transform5.transform.localRotation = Quaternion.identity;
						transform5.transform.localScale = (flag ? new Vector3(1.05f, 1f, 1.1f) : Vector3.one);
						if (itemClothingAsset2.shouldDestroyClothingColliders)
						{
							PrefabUtil.DestroyCollidersInChildren(transform5.gameObject, true);
						}
						transform5.DestroyRigidbody();
						attachmentModel_1 = transform5.transform;
						return;
					}
					if (itemClothingAsset2.type == EItemType.MASK)
					{
						transform6 = Object.Instantiate<GameObject>(((ItemMaskAsset)itemClothingAsset2).mask).transform;
						transform6.name = "Mask";
						transform6.transform.parent = transform2;
						transform6.transform.localPosition = Vector3.zero;
						transform6.transform.localRotation = Quaternion.identity;
						transform6.transform.localScale = Vector3.one;
						if (itemClothingAsset2.shouldDestroyClothingColliders)
						{
							PrefabUtil.DestroyCollidersInChildren(transform6.gameObject, true);
						}
						transform6.DestroyRigidbody();
						attachmentModel_1 = transform6.transform;
						return;
					}
					if (itemClothingAsset2.type == EItemType.GLASSES)
					{
						transform7 = Object.Instantiate<GameObject>(((ItemGlassesAsset)itemClothingAsset2).glasses).transform;
						transform7.name = "Glasses";
						transform7.transform.parent = transform2;
						transform7.transform.localPosition = Vector3.zero;
						transform7.transform.localRotation = Quaternion.identity;
						transform7.transform.localScale = Vector3.one;
						if (itemClothingAsset2.shouldDestroyClothingColliders)
						{
							PrefabUtil.DestroyCollidersInChildren(transform7.gameObject, true);
						}
						transform7.DestroyRigidbody();
						attachmentModel_1 = transform7.transform;
					}
				}
			}
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x001AD73C File Offset: 0x001AB93C
		public static void build()
		{
			if (ZombieClothing.ghostMaterial == null)
			{
				ZombieClothing.ghostMaterial = (Material)Resources.Load("Characters/Ghost");
			}
			if (ZombieClothing.ghostSpiritMaterial == null)
			{
				ZombieClothing.ghostSpiritMaterial = (Material)Resources.Load("Characters/Ghost_Spirit");
			}
			if (ZombieClothing.megaMesh_0 == null)
			{
				ZombieClothing.megaMesh_0 = ((GameObject)Resources.Load("Characters/Mega_0")).GetComponent<MeshFilter>().sharedMesh;
			}
			if (ZombieClothing.megaMesh_1 == null)
			{
				ZombieClothing.megaMesh_1 = ((GameObject)Resources.Load("Characters/Mega_1")).GetComponent<MeshFilter>().sharedMesh;
			}
			if (ZombieClothing.zombieMesh_0 == null)
			{
				ZombieClothing.zombieMesh_0 = ((GameObject)Resources.Load("Characters/Zombie_0")).GetComponent<MeshFilter>().sharedMesh;
			}
			if (ZombieClothing.zombieMesh_1 == null)
			{
				ZombieClothing.zombieMesh_1 = ((GameObject)Resources.Load("Characters/Zombie_1")).GetComponent<MeshFilter>().sharedMesh;
			}
			if (ZombieClothing.faceTexture == null)
			{
				ZombieClothing.faceTexture = Resources.Load<Texture2D>("Faces/19/Texture");
			}
			if (ZombieClothing.clothingShader == null)
			{
				ZombieClothing.clothingShader = Shader.Find("Standard/Clothes");
			}
			if (ZombieClothing.clothes != null)
			{
				for (int i = 0; i < ZombieClothing.clothes.GetLength(0); i++)
				{
					for (int j = 0; j < ZombieClothing.clothes[i].GetLength(0); j++)
					{
						for (int k = 0; k < ZombieClothing.clothes[i].GetLength(1); k++)
						{
							if (ZombieClothing.clothes[i][j, k] != null)
							{
								Object.DestroyImmediate(ZombieClothing.clothes[i][j, k]);
								ZombieClothing.clothes[i][j, k] = null;
							}
						}
					}
				}
			}
			if (LevelZombies.tables == null)
			{
				ZombieClothing.clothes = null;
				return;
			}
			ZombieClothing.clothes = new Material[LevelZombies.tables.Count][,];
			byte b = 0;
			while ((int)b < LevelZombies.tables.Count)
			{
				ZombieTable zombieTable = LevelZombies.tables[(int)b];
				ZombieClothing.clothes[(int)b] = new Material[zombieTable.slots[0].table.Count + 1, zombieTable.slots[1].table.Count + 1];
				byte b2 = 0;
				while ((int)b2 < zombieTable.slots[0].table.Count + 1)
				{
					ushort shirt = 0;
					if ((int)b2 < zombieTable.slots[0].table.Count)
					{
						shirt = zombieTable.slots[0].table[(int)b2].item;
					}
					byte b3 = 0;
					while ((int)b3 < zombieTable.slots[1].table.Count + 1)
					{
						ushort pants = 0;
						if ((int)b3 < zombieTable.slots[1].table.Count)
						{
							pants = zombieTable.slots[1].table[(int)b3].item;
						}
						ZombieClothing.clothes[(int)b][(int)b2, (int)b3] = ZombieClothing.paint(shirt, pants, zombieTable.isMega);
						b3 += 1;
					}
					b2 += 1;
				}
				b += 1;
			}
		}

		// Token: 0x040030FD RID: 12541
		private static Mesh megaMesh_0;

		// Token: 0x040030FE RID: 12542
		private static Mesh megaMesh_1;

		// Token: 0x040030FF RID: 12543
		private static Mesh zombieMesh_0;

		// Token: 0x04003100 RID: 12544
		private static Mesh zombieMesh_1;

		// Token: 0x04003101 RID: 12545
		private static Texture2D faceTexture;

		// Token: 0x04003102 RID: 12546
		private static Shader clothingShader;

		// Token: 0x04003103 RID: 12547
		private static Material[][,] clothes;

		// Token: 0x02000A2C RID: 2604
		[Flags]
		public enum EApplyFlags
		{
			// Token: 0x04003556 RID: 13654
			None = 1,
			// Token: 0x04003557 RID: 13655
			Mega = 2,
			// Token: 0x04003558 RID: 13656
			Ragdoll = 4
		}
	}
}
