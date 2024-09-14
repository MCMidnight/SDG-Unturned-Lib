using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000768 RID: 1896
	public class RagdollTool
	{
		// Token: 0x06003DF9 RID: 15865 RVA: 0x0012C2F0 File Offset: 0x0012A4F0
		private static Material getRagdollEffectMaterial(ERagdollEffect effect)
		{
			switch (effect)
			{
			case ERagdollEffect.BRONZE:
				if (RagdollTool.bronzeMaterial == null)
				{
					RagdollTool.bronzeMaterial = Resources.Load<Material>("Characters/RagdollMaterials/Bronze");
				}
				return RagdollTool.bronzeMaterial;
			case ERagdollEffect.SILVER:
				if (RagdollTool.silverMaterial == null)
				{
					RagdollTool.silverMaterial = Resources.Load<Material>("Characters/RagdollMaterials/Silver");
				}
				return RagdollTool.silverMaterial;
			case ERagdollEffect.GOLD:
				if (RagdollTool.goldMaterial == null)
				{
					RagdollTool.goldMaterial = Resources.Load<Material>("Characters/RagdollMaterials/Gold");
				}
				return RagdollTool.goldMaterial;
			case ERagdollEffect.ZERO_KELVIN:
				if (RagdollTool.zeroKelvinMaterial == null)
				{
					RagdollTool.zeroKelvinMaterial = Resources.Load<Material>("Characters/RagdollMaterials/Zero_Kelvin");
				}
				return RagdollTool.zeroKelvinMaterial;
			case ERagdollEffect.JADED:
				if (RagdollTool.jadedMaterial == null)
				{
					RagdollTool.jadedMaterial = Resources.Load<Material>("Characters/RagdollMaterials/Jaded");
				}
				return RagdollTool.jadedMaterial;
			case ERagdollEffect.SOUL_CRYSTAL_GREEN:
				if (RagdollTool.soulCrystalGreenMaterial == null)
				{
					RagdollTool.soulCrystalGreenMaterial = Resources.Load<Material>("Characters/RagdollMaterials/SoulCrystal_Green");
				}
				return RagdollTool.soulCrystalGreenMaterial;
			case ERagdollEffect.SOUL_CRYSTAL_MAGENTA:
				if (RagdollTool.soulCrystalMagentaMaterial == null)
				{
					RagdollTool.soulCrystalMagentaMaterial = Resources.Load<Material>("Characters/RagdollMaterials/SoulCrystal_Magenta");
				}
				return RagdollTool.soulCrystalMagentaMaterial;
			case ERagdollEffect.SOUL_CRYSTAL_RED:
				if (RagdollTool.soulCrystalRedMaterial == null)
				{
					RagdollTool.soulCrystalRedMaterial = Resources.Load<Material>("Characters/RagdollMaterials/SoulCrystal_Red");
				}
				return RagdollTool.soulCrystalRedMaterial;
			case ERagdollEffect.SOUL_CRYSTAL_YELLOW:
				if (RagdollTool.soulCrystalYellowMaterial == null)
				{
					RagdollTool.soulCrystalYellowMaterial = Resources.Load<Material>("Characters/RagdollMaterials/SoulCrystal_Yellow");
				}
				return RagdollTool.soulCrystalYellowMaterial;
			default:
				return null;
			}
		}

		/// <summary>
		/// Find materials in finished ragdoll and replace them with the appropriate effect.
		/// </summary>
		// Token: 0x06003DFA RID: 15866 RVA: 0x0012C464 File Offset: 0x0012A664
		private static void applyRagdollEffect(Transform root, ERagdollEffect effect)
		{
			if (effect == ERagdollEffect.NONE)
			{
				return;
			}
			Material ragdollEffectMaterial = RagdollTool.getRagdollEffectMaterial(effect);
			if (ragdollEffectMaterial == null)
			{
				UnturnedLog.warn("Unable to load ragdoll effect material " + effect.ToString());
				return;
			}
			RagdollTool.tempRenderers.Clear();
			root.GetComponentsInChildren<Renderer>(RagdollTool.tempRenderers);
			foreach (Renderer renderer in RagdollTool.tempRenderers)
			{
				RagdollTool.tempMaterials.Clear();
				renderer.GetSharedMaterials(RagdollTool.tempMaterials);
				if (RagdollTool.tempMaterials.Count > 1)
				{
					for (int i = 0; i < RagdollTool.tempMaterials.Count; i++)
					{
						RagdollTool.tempMaterials[i] = ragdollEffectMaterial;
					}
					renderer.sharedMaterials = RagdollTool.tempMaterials.ToArray();
				}
				else
				{
					renderer.sharedMaterial = ragdollEffectMaterial;
				}
			}
			Rigidbody componentInChildren = root.GetComponentInChildren<Rigidbody>();
			RagdollTool.tempJoints.Clear();
			root.GetComponentsInChildren<CharacterJoint>(RagdollTool.tempJoints);
			foreach (CharacterJoint obj in RagdollTool.tempJoints)
			{
				Object.Destroy(obj);
			}
			RagdollTool.tempRigidbodies.Clear();
			root.GetComponentsInChildren<Rigidbody>(RagdollTool.tempRigidbodies);
			foreach (Rigidbody rigidbody in RagdollTool.tempRigidbodies)
			{
				if (rigidbody != componentInChildren)
				{
					Object.Destroy(rigidbody);
				}
			}
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x0012C614 File Offset: 0x0012A814
		private static void applySkeleton(Transform skeleton_0, Transform skeleton_1)
		{
			if (skeleton_0 == null || skeleton_1 == null)
			{
				return;
			}
			for (int i = 0; i < skeleton_1.childCount; i++)
			{
				Transform child = skeleton_1.GetChild(i);
				Transform transform = skeleton_0.Find(child.name);
				if (transform != null)
				{
					child.localPosition = transform.localPosition;
					child.localRotation = transform.localRotation;
					if (transform.childCount > 0 && child.childCount > 0)
					{
						RagdollTool.applySkeleton(transform, child);
					}
				}
			}
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x0012C694 File Offset: 0x0012A894
		public static void ragdollPlayer(Vector3 point, Quaternion rotation, Transform skeleton, Vector3 ragdoll, PlayerClothing clothes, ERagdollEffect effect)
		{
			if (!GraphicsSettings.ragdolls)
			{
				return;
			}
			ragdoll.y += 8f;
			ragdoll.x += Random.Range(-16f, 16f);
			ragdoll.z += Random.Range(-16f, 16f);
			ragdoll *= (float)((Player.player != null && Player.player.skills.boost == EPlayerBoost.FLIGHT) ? 256 : 32);
			Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Characters/Ragdoll_Player"), point + Vector3.up * 0.1f, rotation * Quaternion.Euler(90f, 0f, 0f))).transform;
			transform.name = "Ragdoll";
			EffectManager.RegisterDebris(transform.gameObject);
			if (skeleton != null)
			{
				RagdollTool.applySkeleton(skeleton, transform.Find("Skeleton"));
			}
			Transform transform2 = transform.Find("Skeleton");
			if (transform2 != null)
			{
				Transform transform3 = transform2.Find("Spine");
				if (transform3 != null)
				{
					Rigidbody component = transform3.GetComponent<Rigidbody>();
					if (component != null)
					{
						component.AddForce(ragdoll);
					}
				}
			}
			Object.Destroy(transform.gameObject, GraphicsSettings.effect);
			if (clothes != null && clothes.thirdClothes != null)
			{
				HumanClothes component2 = transform.GetComponent<HumanClothes>();
				component2.isRagdoll = true;
				component2.skin = clothes.skin;
				component2.color = clothes.color;
				component2.face = clothes.face;
				component2.hair = clothes.hair;
				component2.beard = clothes.beard;
				component2.shirtAsset = clothes.shirtAsset;
				component2.pantsAsset = clothes.pantsAsset;
				component2.hatAsset = clothes.hatAsset;
				component2.backpackAsset = clothes.backpackAsset;
				component2.vestAsset = clothes.vestAsset;
				component2.maskAsset = clothes.maskAsset;
				component2.glassesAsset = clothes.glassesAsset;
				component2.visualShirt = clothes.visualShirt;
				component2.visualPants = clothes.visualPants;
				component2.visualHat = clothes.visualHat;
				component2.visualBackpack = clothes.visualBackpack;
				component2.visualVest = clothes.visualVest;
				component2.visualMask = clothes.visualMask;
				component2.visualGlasses = clothes.visualGlasses;
				component2.isVisual = clothes.isVisual;
				component2.apply();
			}
			RagdollTool.applyRagdollEffect(transform, effect);
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x0012C918 File Offset: 0x0012AB18
		public static Transform ragdollZombie(Vector3 point, Quaternion rotation, Transform skeleton, Vector3 ragdoll, byte type, byte shirt, byte pants, byte hat, byte gear, ushort hatID, ushort gearID, bool isMega, ERagdollEffect effect)
		{
			if (!GraphicsSettings.ragdolls)
			{
				return null;
			}
			ragdoll.y += 8f;
			ragdoll.x += Random.Range(-16f, 16f);
			ragdoll.z += Random.Range(-16f, 16f);
			ragdoll *= (float)((Player.player != null && Player.player.skills.boost == EPlayerBoost.FLIGHT) ? 256 : 32);
			Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Characters/Ragdoll_Zombie"), point + Vector3.up * 0.1f, rotation * Quaternion.Euler(90f, 0f, 0f))).transform;
			transform.name = "Ragdoll";
			EffectManager.RegisterDebris(transform.gameObject);
			if (isMega)
			{
				transform.localScale = Vector3.one * 1.5f;
			}
			else
			{
				transform.localScale = Vector3.one;
			}
			if (skeleton != null)
			{
				RagdollTool.applySkeleton(skeleton, transform.Find("Skeleton"));
			}
			Transform transform2 = transform.Find("Skeleton");
			if (transform2 != null)
			{
				Transform transform3 = transform2.Find("Spine");
				if (transform3 != null)
				{
					Rigidbody component = transform3.GetComponent<Rigidbody>();
					if (component != null)
					{
						component.AddForce(ragdoll);
					}
				}
			}
			Object.Destroy(transform.gameObject, GraphicsSettings.effect);
			ZombieClothing.EApplyFlags eapplyFlags = ZombieClothing.EApplyFlags.Ragdoll;
			if (isMega)
			{
				eapplyFlags |= ZombieClothing.EApplyFlags.Mega;
			}
			Transform transform4;
			Transform transform5;
			ZombieClothing.apply(transform, eapplyFlags, null, transform.Find("Model_1").GetComponent<SkinnedMeshRenderer>(), type, shirt, pants, hat, gear, hatID, gearID, out transform4, out transform5);
			RagdollTool.applyRagdollEffect(transform, effect);
			return transform;
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x0012CAC4 File Offset: 0x0012ACC4
		public static void ragdollAnimal(Vector3 point, Quaternion rotation, Transform skeleton, Vector3 ragdoll, ushort id, ERagdollEffect effect)
		{
			if (!GraphicsSettings.ragdolls)
			{
				return;
			}
			ragdoll.y += 8f;
			ragdoll.x += Random.Range(-16f, 16f);
			ragdoll.z += Random.Range(-16f, 16f);
			ragdoll *= (float)((Player.player != null && Player.player.skills.boost == EPlayerBoost.FLIGHT) ? 256 : 32);
			AnimalAsset animalAsset = Assets.find(EAssetType.ANIMAL, id) as AnimalAsset;
			if (animalAsset == null)
			{
				return;
			}
			Transform transform = Object.Instantiate<GameObject>(animalAsset.ragdoll, point + Vector3.up * 0.1f, rotation * Quaternion.Euler(0f, 90f, 0f)).transform;
			transform.name = "Ragdoll";
			EffectManager.RegisterDebris(transform.gameObject);
			if (skeleton != null)
			{
				RagdollTool.applySkeleton(skeleton, transform.Find("Skeleton"));
			}
			Transform transform2 = transform.Find("Skeleton");
			if (transform2 != null)
			{
				Transform transform3 = transform2.Find("Spine");
				if (transform3 != null)
				{
					Rigidbody component = transform3.GetComponent<Rigidbody>();
					if (component != null)
					{
						component.AddForce(ragdoll);
					}
				}
			}
			Object.Destroy(transform.gameObject, GraphicsSettings.effect);
			RagdollTool.applyRagdollEffect(transform, effect);
		}

		// Token: 0x040026F6 RID: 9974
		private static List<Renderer> tempRenderers = new List<Renderer>();

		// Token: 0x040026F7 RID: 9975
		private static List<Rigidbody> tempRigidbodies = new List<Rigidbody>();

		// Token: 0x040026F8 RID: 9976
		private static List<CharacterJoint> tempJoints = new List<CharacterJoint>();

		// Token: 0x040026F9 RID: 9977
		private static List<Material> tempMaterials = new List<Material>();

		// Token: 0x040026FA RID: 9978
		private static Material bronzeMaterial;

		// Token: 0x040026FB RID: 9979
		private static Material silverMaterial;

		// Token: 0x040026FC RID: 9980
		private static Material goldMaterial;

		// Token: 0x040026FD RID: 9981
		private static Material zeroKelvinMaterial;

		// Token: 0x040026FE RID: 9982
		private static Material jadedMaterial;

		// Token: 0x040026FF RID: 9983
		private static Material soulCrystalGreenMaterial;

		// Token: 0x04002700 RID: 9984
		private static Material soulCrystalMagentaMaterial;

		// Token: 0x04002701 RID: 9985
		private static Material soulCrystalRedMaterial;

		// Token: 0x04002702 RID: 9986
		private static Material soulCrystalYellowMaterial;
	}
}
